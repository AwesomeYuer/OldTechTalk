
namespace Microshaoft.DataAccess {
    using System;
    using System.Data;
    using System.Data.SqlClient;
    
    
    public class Class1 {
        
        public static string _ConnectionString = @"application name=testdb;User ID=sa;Password=sa@ehome;Initial Catalog=testdb;Data Source=192.168.1.83";
        
        public static DataTable ExecDataTable
					()
		{
				SqlConnection connection = new SqlConnection(_ConnectionString);
				SqlCommand command = new SqlCommand("Select * from hangdown", connection);
				//command.CommandType = CommandType.StoredProcedure;


				connection.Open();
				SqlDataAdapter sda = new SqlDataAdapter(command);
				DataSet ds = new DataSet();
				sda.Fill(ds);
				connection.Close();
				//command.ExecuteNonQuery();
				return ds.Tables[0];
				//return p_UserID;
        }
        

    }
}
