using AzureAssignment.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureAssignment.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
           
            return View();
        }


        [HttpPost]
        public ActionResult Index(FormCollection formData, HttpPostedFileBase uploadFile)
        {
           

            foreach (string file in Request.Files)
            {
                uploadFile = Request.Files[file];
            }
            string Firstname = formData["Firstname"];
            string LastName = formData["Lastname"];
            // Insert  
            if (!String.IsNullOrEmpty(Firstname) && !String.IsNullOrEmpty(LastName))
            {
                Employee EmployeeObj = new Employee(Firstname, LastName);
                EmployeeObj.Email = formData["Email"] == "" ? null : formData["Email"];
                EmployeeObj.Address = formData["Address"] == "" ? null : formData["Address"];
                EmployeeObj.Mobile = formData["Mobile"] == "" ? null : formData["Mobile"];
                BlobManager BlobManagerObj = new BlobManager("trainingblobcontainer");
                string FileAbsoluteUri = BlobManagerObj.UploadFile(uploadFile);
                EmployeeObj.Url = FileAbsoluteUri;
                TableManager TableManagerObj = new TableManager("DhartiAssignmentEmployeeTable");
                TableManagerObj.InsertEntity<Employee>(EmployeeObj, true);
            }
            return RedirectToAction("Get");

        }

        public ActionResult Get()
        {
            TableManager TableManagerObj = new TableManager("DhartiAssignmentEmployeeTable");
            List<Employee> EmployeeListObj = TableManagerObj.RetrieveEntity<Employee>();
            return View(EmployeeListObj);
        }

        //public ActionResult Download(string uri)
        //{
        //    BlobManager BlobManagerObj = new BlobManager("trainingblobcontainer");
        //    BlobManagerObj.Download(uri);
        //    return RedirectToAction("Get");
        //}

        [HttpGet]
        public ActionResult Download(string name)
        {
            BlobManager BlobManagerObj = new BlobManager("trainingblobcontainer");
            CloudBlockBlob cloudBlockBlob = BlobManagerObj.Download(name);
            MemoryStream memStream = new MemoryStream();
            cloudBlockBlob.DownloadToStream(memStream);
            Response.ContentType = cloudBlockBlob.Properties.ContentType.ToString();
            Response.AddHeader("Content-Disposition", "Attachment; filename=" + name);
            Response.AddHeader("Content-Length", cloudBlockBlob.Properties.Length.ToString());
            Response.BinaryWrite(memStream.ToArray());
            Response.Flush();
            Response.Close();
            return RedirectToAction("Get");
        }
    }
}