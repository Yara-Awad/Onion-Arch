using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using Shared.RequestFeatures;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository 
{
        public EmployeeRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
        {
        }
            public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId,  EmployeeParameters employeeParameters, bool trackChanges) 
           {

            var employees = await FindByCondition(e => e.CompanyId.Equals(companyId),
            trackChanges)
            .FilterEmployees(employeeParameters.MinAge, employeeParameters.MaxAge)
            .Search(employeeParameters.SearchTerm)
            .OrderBy(e => e.Name)
            .ToListAsync();
            return PagedList<Employee>
            .ToPagedList(employees, employeeParameters.PageNumber,
            employeeParameters.PageSize); 
            } 
        public  void CreateEmployeeForCompanyAsync(Guid companyId, Employee employee)
        {

            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployeeAsync(Employee employee)
        {
            Delete(employee);
        }
       public async Task<Employee> GetEmployeeAsyncById(Guid companyId, Guid id, bool trackChanges) =>
    FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id),
trackChanges)
    .SingleOrDefault();

    
    }
}
