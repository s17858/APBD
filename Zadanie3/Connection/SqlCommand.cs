using System.Data.SqlClient;

namespace APBD.Connection;

public class SqlCommand
{
    string queryString = "SELECT * FROM Animals";
    SqlCommand command = new SqlCommand(queryString, connection);
}