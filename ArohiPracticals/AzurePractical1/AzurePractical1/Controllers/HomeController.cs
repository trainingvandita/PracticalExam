using AzurePractical1.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzurePractical1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                // Get particular student info  
                BlobManager BManagerObj = new BlobManager("ArohiEmployeeTable"); // 'ArohiEmployeeTable' is the name of the table  
                                                                                     // pass query where RowKey eq value of id
                List<Employee> EmpListObj = BManagerObj.RetrieveEntity<Employee>("RowKey eq '" + id + "'");
                Employee StudentObj = EmpListObj.FirstOrDefault();
                return View(StudentObj);
            }
            return View(new Employee());
        }
        [HttpPost]
        public ActionResult Index(FormCollection formData, HttpPostedFileBase uploadFile)
        {
            foreach (string file in Request.Files)
            {
                uploadFile = Request.Files[file];
            }
            

            string EmpFName = formData["EmpFName"];
            string EmpLName = formData["EmpLName"];

            if(!String.IsNullOrEmpty(EmpFName) && !String.IsNullOrEmpty(EmpLName))
            {
                Employee StudentObj = new Employee(EmpFName,EmpLName);
                StudentObj.Email = formData["Email"] == "" ? null : formData["Email"];
                StudentObj.Mobile = formData["Mobile"] == "" ? null : formData["Mobile"];
                StudentObj.Address = formData["Address"] == "" ? null : formData["Address"];
                FileManager fileManagerObj = new FileManager("trainingblobcontainer");
                string AbsoluteUri = fileManagerObj.UploadFile(uploadFile);
                StudentObj.FileUpload = AbsoluteUri;
                BlobManager BManagerObj = new BlobManager("ArohiEmployeeTable");
                BManagerObj.InsertEntity<Employee>(StudentObj, true);
            }

           

            return RedirectToAction("List");
        }
        public ActionResult List()
        {
            BlobManager BManagerObj = new BlobManager("ArohiEmployeeTable");
            List<Employee> EmpListObj = BManagerObj.RetrieveEntity<Employee>();
            return View(EmpListObj);
        }
        
        [HttpGet]
        public ActionResult Download(string name)
        {
            FileManager BlobManagerObj = new FileManager("trainingblobcontainer");
            CloudBlockBlob cloudBlockBlob = BlobManagerObj.Download(name);
            MemoryStream memStream = new MemoryStream();
            cloudBlockBlob.DownloadToStream(memStream);
            Response.ContentType = cloudBlockBlob.Properties.ContentType.ToString();
            Response.AddHeader("Content-Disposition", "Attachment; filename=" + name);
            Response.AddHeader("Content-Length", cloudBlockBlob.Properties.Length.ToString());
            Response.BinaryWrite(memStream.ToArray());
            Response.Flush();
            Response.Close();
            return RedirectToAction("List");
        }
    }
}