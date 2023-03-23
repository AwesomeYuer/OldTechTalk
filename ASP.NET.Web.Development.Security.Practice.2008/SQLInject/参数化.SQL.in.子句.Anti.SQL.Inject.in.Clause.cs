//参数化 SQL in 子句
namespace Microshaoft.Test
{
	using System;
	using System.Data;
	using System.Data.SqlClient;
	using Microshaoft.ParameterizedSqlTest;

	/// <summary>
	/// Class1 的摘要说明。
	/// </summary>
	public class Class1
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		//[STAThread]
		static void Main(string[] args)
		{
			//
			// TODO: 在此处添加代码以启动应用程序
			//
			Console.WriteLine("Hello World");
			Console.WriteLine(Environment.Version.ToString());

			DataTable dt = DataAccess.Execute_SQL
								(
									"abc"
									, "d"
									, "a,b,c,d"
									, SqlDbType.VarChar
									, 3
									, "sqlPara"
								);
			Console.WriteLine(dt.Rows.Count);
			Console.ReadLine();
		}
	}
}

namespace Microshaoft.ParameterizedSqlTest
{
	using System;
	using System.Data;
	using System.Data.SqlClient;

	using System.Text;

	class DataAccess
	{

		public static string _ConnectionString = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=Northwind;Data Source=.\sqlexpress";//ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
		public static DataTable Execute_SQL
								(
									string Parameter1					//普通参数
									, string Parameter2					//普通参数
									, string inClause					//in 子句字段值列表
									, SqlDbType inClauseSqlDbType		//in 子句字段类型
									, int inClauseSqlDbTypeSize			//in 子句字段大小
									, string prefixSqlParameterName		//参数前缀
								)
		{
			SqlConnection connection = new SqlConnection(_ConnectionString);

			StringBuilder sql = new StringBuilder("select 1 where 'abc' = @Parameter1");

			SqlCommand command = new SqlCommand();
			command.CommandType = CommandType.Text;

			SqlParameter sqlParameter1 = command.Parameters.Add("@Parameter1", SqlDbType.VarChar, 3);
			sqlParameter1.Direction = ParameterDirection.Input;
			sqlParameter1.Value = Parameter1;

			SqlParameter sqlParameter2 = command.Parameters.Add("@Parameter2", SqlDbType.VarChar, 3);
			sqlParameter2.Direction = ParameterDirection.Input;
			sqlParameter2.Value = Parameter2;

			string[] a = inClause.Split(',');
			if (a.Length > 0)
			{
				sql.Append(" and @Parameter2 in (");
				int i = 0;

				foreach (string var in a)
				{
					i ++;
					if (i > 1)
					{
						sql.Append(",");
					}
					string sqlParameterName = "@" + prefixSqlParameterName + "_" + i;
					sql.Append(sqlParameterName);
					SqlParameter parameter;
					if (inClauseSqlDbTypeSize > 0)
					{
						parameter = command.Parameters.Add(sqlParameterName, inClauseSqlDbType, inClauseSqlDbTypeSize);
					}
					else
					{
						parameter = command.Parameters.Add(sqlParameterName, inClauseSqlDbType);
					}
					parameter.Direction = ParameterDirection.Input;
					parameter.Value = var;

				}
				sql.Append(")");
			}
			command.CommandText = sql.ToString();
			command.Connection = connection;

			Console.WriteLine(command.CommandText);
			SqlDataAdapter sda = new SqlDataAdapter(command);
			DataSet ds = new DataSet();
			sda.Fill(ds);
			connection.Close();
			return ds.Tables[0];
			
		}

	}
}