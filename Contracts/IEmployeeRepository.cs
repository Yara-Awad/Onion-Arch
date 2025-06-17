using Entities.Models;
using Shared.RequestFeatures;

namespace Contracts
{
    public interface IEmployeeRepository
    {
         Task< PagedList<Employee>>GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
         void CreateEmployeeForCompanyAsync(Guid companyId, Employee employee);
         void DeleteEmployeeAsync(Employee employee);
        Task<Employee> GetEmployeeAsyncById(Guid companyId, Guid id, bool trackChanges);
    }
}
