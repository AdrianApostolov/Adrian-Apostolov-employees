using PairOfEmployees.Data.Models;

namespace PairOfEmployees.Services.Data.Contracts
{
    public interface IEmployeeService
    {
        List<EmployeesPair> GetLongestWorkingPair(List<EmployeeProject> data);
    }
}
