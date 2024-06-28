using EmployeesApp.Contracts;
using EmployeesApp.Models;

namespace EmployeesApp.Repository
{
    public class EmployeeRepository(EmployeeContext context) : IEmployeeRepository
    {
        public void CreateEmployee(Employee employee)
        {
            try
            {
                context.Add(employee);
                context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteEmployee(Guid id)
        {
           var result = context.Employees.SingleOrDefault(e => e.Id.Equals(id));
            if (result != null)
            {
                try
                {
                    context.Employees.Remove(result);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public IEnumerable<Employee> GetAll() => [.. context.Employees];

        public Employee GetEmployeeById(Guid id) => context.Employees.SingleOrDefault(e => e.Id.Equals(id));

        public void UpdateEmployee(Employee employee)
        {
            if (Exists(employee.Id))
            {
                var result = GetEmployeeById(employee.Id);
                if (result != null)
                {
                    try
                    {
                        context.Entry(result).CurrentValues.SetValues(employee);
                        context.SaveChanges();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public bool Exists(Guid id) => context.Employees.Any(e => e.Id.Equals(id));
    }
}
