using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using PairOfEmployees.Data.Models;
using PairOfEmployees.Services.Data.Contracts;
using System.Globalization;

namespace PairOfEmployees.Services.Data
{
    public class CsvService : ICsvService
    {
        public List<EmployeeProject> LoadData(IFormFile file)
        {
            var data = new List<EmployeeProject>();

            if (file.ContentType != "text/csv")
            {
                return data;
            }

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csvReader.Read();
                csvReader.ReadHeader();

                while (csvReader.Read())
                {
                    var record = csvReader.GetRecord<EmployeesProjectCsvRecord>();

                    if(record != null) 
                    {
                        data.Add(
                        new EmployeeProject
                        {
                            EmployeeId = record.EmpID,
                            ProjectId = record.ProjectID,
                            DateFrom = DateTime.Parse(record.DateFrom, CultureInfo.InvariantCulture),
                            DateTo = record.DateTo == "NULL" ? DateTime.Now : DateTime.Parse(record.DateTo, CultureInfo.InvariantCulture),
                        });
                    }
                }
            }

            return data;
        }
    }
}
