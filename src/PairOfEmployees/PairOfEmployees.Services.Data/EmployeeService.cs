using PairOfEmployees.Data.Models;
using PairOfEmployees.Services.Data.Contracts;

namespace PairOfEmployees.Services.Data
{
    public class EmployeeService : IEmployeeService
    {
        public List<EmployeesPair> GetLongestWorkingPair(List<EmployeeProject> data)
        {
            List<EmployeesPair> employeesPairsOnCommonProjects = data.GroupBy(p => p.ProjectId)
                                                                     .SelectMany(g => EmployeesPairsOnCommonProjects(g.ToList()))
                                                                     .OrderByDescending(p => p.DaysWorked)
                                                                     .ToList();

            var longestWorkingPairs = FindLongestWorkingPair(employeesPairsOnCommonProjects);

            return longestWorkingPairs;
        }

        private List<EmployeesPair> FindLongestWorkingPair(IEnumerable<EmployeesPair> employeesPairsOnCommonProjects) 
        {
            var groupByPair = employeesPairsOnCommonProjects.GroupBy(p => new { p.FirstEmployeeId, p.SecondEmployeeId });
            var longestWorkingPairList = new List<EmployeesPair>();
            var longestDuration = 0;

            foreach (var pair in groupByPair)
            {
                int pairWorkingDaysSum = pair.Sum(p => p.DaysWorked);

                if(pairWorkingDaysSum > longestDuration)
                {
                    longestDuration = pairWorkingDaysSum;
                    longestWorkingPairList = pair.Select(x => x)
                                                 .ToList();
                }
            }

            return longestWorkingPairList;
        }

        private List<EmployeesPair> EmployeesPairsOnCommonProjects(List<EmployeeProject> employees)
        {
            var list = new List<EmployeesPair>();

            for (int i = 0; i < employees.Count; i++)
            {
                for (int j = i + 1; j < employees.Count; j++)
                {
                    EmployeeProject commonProject = FindCommonProject(employees[i], employees[j]);

                    if (commonProject != null)
                    {
                        int daysWorked = commonProject.DateTo.Value.Subtract(commonProject.DateFrom).Days;

                        var longestPair = new EmployeesPair
                        {
                            FirstEmployeeId = employees[i].EmployeeId,
                            SecondEmployeeId = employees[j].EmployeeId,
                            ProjectId = commonProject.ProjectId,
                            DaysWorked = daysWorked
                        };

                        list.Add(longestPair);
                    }
                }
            }

            return list;
        }

        private EmployeeProject FindCommonProject(EmployeeProject emp1, EmployeeProject emp2)
        {
            if (emp1.ProjectId == emp2.ProjectId &&
               ((emp1.DateFrom >= emp2.DateFrom && emp1.DateFrom <= emp2.DateTo.Value) ||
                (emp2.DateFrom >= emp1.DateFrom && emp2.DateFrom <= emp1.DateTo.Value)))
            {
                return new EmployeeProject
                {
                    EmployeeId = emp1.EmployeeId,
                    ProjectId = emp1.ProjectId,
                    DateFrom = emp1.DateFrom > emp2.DateFrom ? emp1.DateFrom : emp2.DateFrom,
                    DateTo = emp1.DateTo < emp2.DateTo ? emp1.DateTo : emp2.DateTo
                };
            }

            return null;
        }
    }
}
