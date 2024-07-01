using EmployeesApp.Contracts;
using EmployeesApp.Controllers;
using EmployeesApp.Models;
using EmployeesApp.Validation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesApp.Tests.Validation
{
    public class EmployeesControllerTests
    {
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock;
        private readonly Mock<AccountNumberValidation> _accountNumberValidationMock;
        private readonly EmployeesController _employeesController;

        public EmployeesControllerTests()
        {
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();
            _accountNumberValidationMock = new Mock<AccountNumberValidation>();
            _employeesController = new EmployeesController(_employeeRepositoryMock.Object, _accountNumberValidationMock.Object);
        }

        [Fact]
        public void Index_ActionExecutes_ReturnsViewForIndex()
        {
            var result = _employeesController.Index();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_ActionExecutes_ReturnsTypeIsEmployeeList()
        {
            _employeeRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Employee>());

            var result = _employeesController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<List<Employee>>(viewResult.Model);
        }

        [Fact]
        public void Index_ActionExecutes_ReturnsExactNumberOfEmployees()
        {
            _employeeRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Employee> { new(), new() });

            var result = _employeesController.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var employees = Assert.IsType<List<Employee>>(viewResult.Model);
            Assert.Equal(2, employees.Count);
        }

        [Fact]
        public void Create_ActionExecutes_ReturnsViewForCreate()
        {
            var result = _employeesController.Create();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            _employeesController.ModelState.AddModelError("Name", "Name is required");

            var employee = new Employee { Age = 25, AccountNumber = "255-1234567890-12" };

            var result = _employeesController.Create(employee);

            var viewResult = Assert.IsType<ViewResult>(result);
            var testEmployee = Assert.IsType<Employee>(viewResult.Model);

            Assert.Equal(employee.AccountNumber, testEmployee.AccountNumber);
            Assert.Equal(employee.Age, testEmployee.Age);
        }

        [Fact]
        public void Create_InvalidModelState_CreateEmployeeNeverExecutes()
        {
            _employeesController.ModelState.AddModelError("Name", "Name is required");

            var employee = new Employee { Age = 24 };

            _employeesController.Create(employee);

            _employeeRepositoryMock.Verify(x => x.CreateEmployee(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public void Create_ModelStateValid_CreateEmployeeCalledOnce()
        {
            Employee? emp = null;

            _employeeRepositoryMock.Setup(repo => repo.CreateEmployee(It.IsAny<Employee>())).Callback<Employee>(x => emp = x);

            var employee = new Employee
            { 
                Name = "Test Employee", 
                Age = 32, 
                AccountNumber = "123-1234567890-12" 
            };

            _employeesController.Create(employee);
            _employeeRepositoryMock.Verify(x => x.CreateEmployee(It.IsAny<Employee>()), Times.Once);

            Assert.Equal(emp.Name, employee.Name);
            Assert.Equal(emp.Age, employee.Age);
            Assert.Equal(emp.AccountNumber, employee.AccountNumber);
        }

        [Fact]
        public void Create_ActionExecuted_RedirectsToIndexAction()
        {
            var employee = new Employee
            {
                Name = "Test Employee",
                Age = 45,
                AccountNumber = "123-1234567890-12"
            };

            var result = _employeesController.Create(employee);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
    }
}
