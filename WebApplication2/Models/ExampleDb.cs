using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication2.Models
{
    public class ExampleDb
    {
        private String connstring = "Data Source=ra1.anystream.eu,1010;Initial Catalog=example_database;User ID=sa;Password=aDifficultPassword$";
        public string DbError;

        public List<Employee> Employees()
        {
            // READ demo
            List<Employee> employees = new List<Employee>();

            TryCommand("SELECT Id, Name, Location FROM Employees;", (command) =>
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Employee employee = new Employee()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Location = reader.GetString(2)
                        };
                        employees.Add(employee);
                    }
                }
            });

            return employees;
        }
        public int Insert(Employee employee)
        {
            // INSERT demo
            int rowsAffected = 0;

            TryCommand("INSERT Employees (Name, Location)VALUES (@name, @location); ", (command) =>
            {
                command.Parameters.AddWithValue("@name", employee.Name);
                command.Parameters.AddWithValue("@location", employee.Location);
                rowsAffected = command.ExecuteNonQuery();
            });

            return rowsAffected;
        }
        public int Update(String userToUpdate)
        {
            // UPDATE demo
            int rowsAffected = 0;

            TryCommand("UPDATE Employees SET Location = N'United States' WHERE Name = @name", (command) =>
            {
                command.Parameters.AddWithValue("@name", userToUpdate);
                rowsAffected = command.ExecuteNonQuery();
            });

            return rowsAffected;
        }
        public int Delete(string userToDelete)
        {
            // DELETE demo
            int rowsAffected = 0;
            TryCommand("DELETE FROM Employees WHERE Name = @name;", (command) =>
            {
                command.Parameters.AddWithValue("@name", userToDelete);
                rowsAffected = command.ExecuteNonQuery();
            });

            return rowsAffected;
        }
        public void Create()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("USE example_database; ");
            sb.Append("DROP TABLE IF EXISTS Employees;");
            sb.Append("CREATE TABLE Employees ( ");
            sb.Append(" Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY, ");
            sb.Append(" Name NVARCHAR(50), ");
            sb.Append(" Location NVARCHAR(50) ");
            sb.Append("); ");
            sb.Append("INSERT INTO Employees (Name, Location) VALUES ");
            sb.Append("(N'Jared', N'Australia'), ");
            sb.Append("(N'Nikita', N'India'), ");
            sb.Append("(N'Tom', N'Germany'); ");
            string cmdText = sb.ToString();

            TryCommand(cmdText, (command) =>
            {
                command.ExecuteNonQuery();
            });

        }

        private void TryCommand(string cmdText, Action<SqlCommand> execute)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connstring))
                {
                    using (SqlCommand command = new SqlCommand(cmdText, connection))
                    {
                        connection.Open();
                        execute(command);
                        DbError = null;
                    }
                }
            }
            catch (SqlException e)
            {
                DbError = e.Message;
            }
        }
    }
}