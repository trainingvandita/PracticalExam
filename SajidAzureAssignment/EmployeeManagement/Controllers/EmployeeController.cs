using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Common;

using EmployeeManagement.Helper;
using Microsoft.Azure.Cosmos.Table;
using EmployeeManagement.Models;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private CloudTable table;
        private FileManager fileManager;
        public EmployeeController()
        {
            string tableName = "SajidAliEmployee";
            table = CreateTable.CreateTableAsync(tableName).Result;
            fileManager = new FileManager();
        }
        // GET: Employee
        public ActionResult Index()
        {
            List<Employee> employees = CRUDOperation.GetEmployeesAsync(table).Result;
            //employees[0].Resume
            return View(employees);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Employee employee, HttpPostedFileBase resume)
        {
            employee.Resume = fileManager.Upload(resume);
            bool res = CRUDOperation.InsertOrMergeEmployeeAsync(table, employee).Result;
            if(res)
                return RedirectToAction("Index");
            return View(employee);
        }
        
        public ActionResult Download(string file)
        {
            fileManager.Download(file);
            return RedirectToAction("Index");
        }
    }
}