using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EmployeesApp.AutomatedUITests
{
    public class AutomatedUITests : IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly EmployeePage _employeePage;

        public AutomatedUITests() 
        {
            _driver = new ChromeDriver();
            _employeePage = new EmployeePage(_driver);
            _employeePage.Navigate();
        }

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Fact]
        public void Create_WhenExecuted_ReturnsCreateView()
        {
            _employeePage.Title.Should().Be("Create - EmployeesApp");
            _employeePage.Source.Should().Contain("Please provide a new employee data");
        }

        [Fact]
        public void Create_WrongModelData_ReturnsErrorMessage()
        {
            _employeePage.PopulateName("New Name");
            _employeePage.PopulateAge("34");
            _employeePage.ClickCreate();
            _employeePage.AccountNumberErrorMessage.Should().Be("Account number is required");
        }

        [Fact]
        public void Create_WhenSuccessfullyExecuted_ReturnsIndexViewWithNewEmployee()
        {
            _employeePage.PopulateName("Another Test Employee");
            _employeePage.PopulateAge("34");
            _employeePage.PopulateAccountNumber("123-1234567890-12");
            _employeePage.ClickCreate();
            _employeePage.Title.Should().Be("Index - EmployeesApp");
            _employeePage.Source.Should().ContainAll("Another Test Employee", "34", "123-1234567890-12");
        }
    }
}