using Microsoft.WindowsAzure.Storage.Blob;
using PoojagWebapplication.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace PoojagWebapplication
{
    public class HomeController : Controller
    {
        public ActionResult Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                // Get particular student info  
                Registration RegistrationManagerObj = new Registration("Poojag"); // 'person' is the name of the table  
                                                                           // pass query where RowKey eq value of id
                List<User> UsertListObj = RegistrationManagerObj.RetrieveEntity<User>("RowKey eq '" + id + "'");
                User UserObj = UsertListObj.FirstOrDefault();
                return View(UserObj);
            }
            return View(new User());
        }
        [HttpPost]
        public ActionResult Index(string id, FormCollection formData, HttpPostedFileBase uploadFile)
        {
            BlobManager BlobManagerObj = new BlobManager("trainingblobcontainer");
            foreach (string file in Request.Files)
            {
                uploadFile = Request.Files[file];
            }
            string FileAbsoluteUri = BlobManagerObj.UploadFile(uploadFile);
            User UserObj = new User();
            UserObj.FirstName = formData["firstname"] == "" ? null : formData["firstname"];
            UserObj.LastName = formData["lastname"] == "" ? null : formData["lastname"];
            UserObj.Email = formData["email"] == "" ? null : formData["email"];
            UserObj.Mobile = formData["mobile"] == "" ? null : formData["mobile"];
            UserObj.Address = formData["address"] == "" ? null : formData["address"];
            UserObj.Resume = FileAbsoluteUri;
            // Insert  
            if (string.IsNullOrEmpty(id))
            {
                UserObj.PartitionKey = "User";
                UserObj.RowKey = Guid.NewGuid().ToString();

                Registration RegistrationManagerObj = new Registration("Poojag");
                RegistrationManagerObj.InsertEntity<User>(UserObj, true);
            }
            // Update  
            else
            {
                UserObj.PartitionKey = "User";
                UserObj.RowKey = id;

                Registration RegistrationManagerObj = new Registration("Poojag");
                RegistrationManagerObj.InsertEntity<User>(UserObj, false);
            }
            return RedirectToAction("Get");
        }
        //public ActionResult Get()
        //{
        //    Registration RegistrationManagerObj = new Registration("Poojag");
        //    List<User> TableManagerObj = TableManagerObj.RetrieveEntity<User>();
        //    return View(RegistrationManagerObj);
        //}
        public ActionResult Get()
        {
            Registration RegistrationManagerObj = new Registration("Poojag");
            List<User> UserListObj = RegistrationManagerObj.RetrieveEntity<User>();
            return View(UserListObj);
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}