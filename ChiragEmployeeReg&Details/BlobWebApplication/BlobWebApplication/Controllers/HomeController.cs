using BlobWebApplication.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlobWebApplication.Controllers
{
    public class HomeController : Controller
    {
    
       public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection, HttpPostedFileBase file)
        {
            foreach (string files in Request.Files)
            {
                file = Request.Files[files];
            }
            string Firstname = collection[Constants.FirstName];
            string LastName = collection[Constants.LastName];
            string Address = collection[Constants.Address];
            if(!String.IsNullOrEmpty(Firstname) && !String.IsNullOrEmpty(LastName))
            {
                BlobManager blobManager = new BlobManager(Constants.ContainerName);
                string FileAbsoluteUri = blobManager.UploadFile(file);
                ChiragInfo employee = new ChiragInfo(Firstname, LastName);
                employee.Email = collection[Constants.Email]; 
                employee.Number = collection[Constants.PhoneNumber];
                employee.Address = Address;
                employee.ResumeUrl = FileAbsoluteUri;
                TableManager tableManager = new TableManager(Constants.TableName);
                ChiragInfo emp = tableManager.InsertOrMergeEntity(employee);
                if (emp == null)
                {
                    ViewBag.TableError = Constants.TableError;
                    return View();
                }
                if (String.IsNullOrEmpty(FileAbsoluteUri))
                {
                    ViewBag.BlobError = Constants.BlobError;
                    return View();
                }
                TempData["SuccessMsg"] = "Details of " + emp.PartitionKey +" " + emp.RowKey + " Uploaded Successfully";
                return RedirectToAction("GetMyData");
            }
            ViewBag.RequiredError = Constants.RequiredError;
            return View();
        }

        [HttpGet]
        public ActionResult GetMyData()
        {
            TableManager tableManager = new TableManager(Constants.TableName);
            List<ChiragInfo> employees = tableManager.employees();
            return View(employees);
        }

        [HttpGet]
        public ActionResult Download(string name)
        {
            BlobManager BlobManagerObj = new BlobManager(Constants.ContainerName);
            CloudBlockBlob cloudBlockBlob = BlobManagerObj.Download(name);
            MemoryStream memStream = new MemoryStream();
            cloudBlockBlob.DownloadToStream(memStream);
            Response.ContentType = cloudBlockBlob.Properties.ContentType.ToString();
            Response.AddHeader("Content-Disposition", "Attachment; filename=" + name);
            Response.AddHeader("Content-Length", cloudBlockBlob.Properties.Length.ToString());
            Response.BinaryWrite(memStream.ToArray());
            Response.Flush();
            Response.Close();
            return RedirectToAction("GetMyData");
        }
    }
}