using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TableCrud.Configuration;
using TableCrud.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace TableCrud.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                // Get particular student info  
                TableManager TableManagerObj = new TableManager("AmiEmployeeDetails"); // 'person' is the name of the table  
                                                                           // pass query where RowKey eq value of id
                List<Employee> employees = TableManagerObj.RetrieveEntity<Employee>("RowKey eq '" + id + "'");
                Employee employee = employees.FirstOrDefault();
                return View(employee);
            }
            return View(new Employee());
        }
        [HttpPost]
        public ActionResult Index(string id, FormCollection formData, HttpPostedFileBase file)
        {
            BlobManager BlobManagerObj = new BlobManager("trainingblobcontainer");
            foreach (string files in Request.Files)
            {
                file = Request.Files[files];
            }
            string FileAbsoluteUri = BlobManagerObj.UploadFile(file);
            Employee employee = new Employee();
            employee.FirstName = formData["FirstName"] == "" ? null : formData["FirstName"];
            employee.LastName = formData["LastName"] == "" ? null : formData["LastName"];
            employee.Email = formData["Email"] == "" ? null : formData["Email"];
            employee.Mobile= formData["Mobile"] == "" ? null : formData["Mobile"];
            employee.Address = formData["Address"] == "" ? null : formData["Address"];

            employee.Path = FileAbsoluteUri;

            // Insert  
            if (string.IsNullOrEmpty(id))
            {
                employee.PartitionKey = "Employee";
                employee.RowKey = Guid.NewGuid().ToString();
                TableManager TableManagerObj = new TableManager("AmiEmployeeDetails");
                //BlobManager BlobManagerObj = new BlobManager("trainingblobcontainer");
                TableManagerObj.InsertEntity<Employee>(employee, true);
            }
            // Update  
            else
            {
                employee.PartitionKey = "Employee";
                employee.RowKey = id;

                TableManager TableManagerObj = new TableManager("AmiEmployeeDetails");
                TableManagerObj.InsertEntity<Employee>(employee, false);
            }
            return RedirectToAction("Get");
        }
        public ActionResult Get()
        {
            TableManager TableManagerObj = new TableManager("AmiEmployeeDetails");
            List<Employee> employees = TableManagerObj.RetrieveEntity<Employee>();
            return View(employees);
        }
        
        public ActionResult Delete(string id)
        {
            TableManager TableManagerObj = new TableManager("AmiEmployeeDetails");
            List<Employee> employees = TableManagerObj.RetrieveEntity<Employee>("RowKey eq '" + id + "'");
            Employee employee = employees.FirstOrDefault();
            TableManagerObj.DeleteEntity<Employee>(employee);
            return RedirectToAction("Get");
        }
        
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