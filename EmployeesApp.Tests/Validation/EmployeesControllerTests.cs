using EmployeesApp.Contracts;
using EmployeesApp.Controllers;
using EmployeesApp.Validation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesApp.Tests.Validation
{
    internal class EmployeesControllerTests
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
    }
}
