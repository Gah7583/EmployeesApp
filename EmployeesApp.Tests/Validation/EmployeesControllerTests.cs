using EmployeesApp.Contracts;
using EmployeesApp.Controllers;
using EmployeesApp.Models;
using EmployeesApp.Validation;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

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
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void Index_ActionExecutes_ReturnsTypeIsEmployeeList()
        {
            _employeeRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Employee>());

            var result = _employeesController.Index();

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            viewResult.Model.Should().BeOfType<List<Employee>>();
        }

        [Fact]
        public void Index_ActionExecutes_ReturnsExactNumberOfEmployees()
        {
            _employeeRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Employee> { new(), new() });

            var result = _employeesController.Index();

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var employees = viewResult.Model.Should().BeAssignableTo<List<Employee>>().Subject;
            employees.Should().HaveCount(2);
        }

        [Fact]
        public void Create_ActionExecutes_ReturnsViewForCreate()
        {
            var result = _employeesController.Create();

            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void Create_InvalidModelState_ReturnsView()
        {
            _employeesController.ModelState.AddModelError("Name", "Name is required");

            var employee = new Employee { Age = 25, AccountNumber = "255-1234567890-12" };

            var result = _employeesController.Create(employee);

            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var testEmployee = viewResult.Model.Should().BeOfType<Employee>().Subject;

            employee.AccountNumber.Should().Be(testEmployee.AccountNumber);
            employee.Age.Should().Be(testEmployee.Age);
        }

        [Fact]
        public void Create_InvalidModelState_CreateEmployeeNeverExecutes()
        {
            _employeesController.ModelState.AddModelError("Name", "Name is required");

            var employee = new Employee { Age = 24 };

            _employeesController.Create(employee);

            _employeeRepositoryMock.Invocations.Should().NotContain(x => x.Method.Name == nameof(IEmployeeRepository.CreateEmployee) && x.Arguments.Any(a => a is Employee));
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
            _employeeRepositoryMock.Invocations.Should().Contain(x => x.Method.Name == nameof(IEmployeeRepository.CreateEmployee) && x.Arguments.Any(a => a is Employee)).And.HaveCount(1);

            emp.Name.Should().Be(employee.Name);
            emp.Age.Should().Be(employee.Age);
            emp.AccountNumber.Should().Be(employee.AccountNumber);
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

            var redirectToActionResult = result.Should().BeOfType<RedirectToActionResult>().Subject;

            redirectToActionResult.ActionName.Should().Be("Index");
        }
    }
}
