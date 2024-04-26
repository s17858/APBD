using System.Data.SqlClient;

namespace APBD.Connection;

public class SqlConnection
{
string connectionString = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s17858;User ID=s17858;Password=Warszawa2025";
SqlConnection connection = new SqlConnection(connectionString);
}