using EmployeesApp.Contracts;
using EmployeesApp.Models;
using EmployeesApp.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EmployeesApp.Controllers
{
    public class EmployeesController(IEmployeeRepository employeeRepository, AccountNumberValidation accountNumberValidation) : Controller
    {

        // GET: EmployeeController
        public ActionResult Index()
        {
            var employees = employeeRepository.GetAll();
            return View(employees);
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(Guid id)
        {
            var employee = employeeRepository.GetEmployeeById(id);
            return View(employee);
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Name,AccountNumber,Age")] Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {

                    return View(employee);
                }
                if (!accountNumberValidation.IsValid(employee.AccountNumber))
                {
                    ModelState.AddModelError("AccountNumber", "Account number is invalid");
                    return View(employee);
                }
                employeeRepository.CreateEmployee(employee);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(Guid id)
        {
            var employee = employeeRepository.GetEmployeeById(id);
            return View(employee);
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind("Id,Name,AccountNumber,Age")] Employee employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(employee);
                }
                employeeRepository.UpdateEmployee(employee);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeesController/Delete/5
        public ActionResult Delete(Guid id)
        {
            var employee = employeeRepository.GetEmployeeById(id);
            return View(employee);
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind("Id")]Employee employee)
        {
            try
            {
                employeeRepository.DeleteEmployee(employee.Id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}
