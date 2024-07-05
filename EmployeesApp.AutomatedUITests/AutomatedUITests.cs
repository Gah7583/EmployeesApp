using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EmployeesApp.AutomatedUITests
{
    public class AutomatedUITests : IDisposable
    {
        private readonly IWebDriver _driver;

        public AutomatedUITests() => _driver = new ChromeDriver();

        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        [Fact]
        public void Create_WhenExecuted_ReturnsCreateView()
        {
            _driver.Navigate().GoToUrl("https://localhost:7019/Employees/Create");

            _driver.Title.Should().Be("Create - EmployeesApp");
            _driver.PageSource.Should().Contain("Please provide a new employee data");
        }

        [Fact]
        public void Create_WrongModelData_ReturnsErrorMessage()
        {
            _driver.Navigate().GoToUrl("https://localhost:7019/Employees/Create");

            _driver.FindElement(By.Id("Name")).SendKeys("Test Employee");

            _driver.FindElement(By.Id("Age")).SendKeys("34");

            _driver.FindElement(By.Id("Create")).Click();

            var errorMessage = _driver.FindElement(By.Id("AccountNumber-error")).Text;

            errorMessage.Should().Be("Account number is required");
        }

        [Fact]
        public void Create_WhenSuccessfullyExecuted_ReturnsIndexViewWithNewEmployee()
        {
            _driver.Navigate().GoToUrl("https://localhost:7019/Employees/Create");

            _driver.FindElement(By.Id("Name")).SendKeys("Another Test Employee");

            _driver.FindElement(By.Id("Age")).SendKeys("34");

            _driver.FindElement(By.Id("AccountNumber")).SendKeys("123-1234567890-12");

            _driver.FindElement(By.Id("Create")).Click();

            _driver.Title.Should().Be("Index - EmployeesApp");
            _driver.PageSource.Should().ContainAll("Another Test Employee", "34", "123-1234567890-12");
        }
    }
}