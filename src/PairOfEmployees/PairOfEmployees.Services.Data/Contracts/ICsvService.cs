using Microsoft.AspNetCore.Http;
using PairOfEmployees.Data.Models;

namespace PairOfEmployees.Services.Data.Contracts
{
    public interface ICsvService
    {
        List<EmployeeProject> LoadData(IFormFile file);
    }
}
