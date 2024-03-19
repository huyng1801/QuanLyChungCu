using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class DataProvider
    {
        private static string connectionString = @"Data Source=.;Initial Catalog=QuanLyChungCu;Integrated Security=True;";

        public static DataTable ExecuteQuery(string query, object[] parameters = null)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        string[] parameterNames = query.Split(' ');
                        int index = 0;

                        foreach (string parameterName in parameterNames)
                        {
                            if (parameterName.Contains('@'))
                            {
                                command.Parameters.AddWithValue(parameterName, parameters[index]);
                                index++;
                            }
                        }
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(data);
                }
            }

            return data;
        }
        public static string ReplaceParameterName(string parameterName)
        {
            return parameterName.Replace("(", "").Replace(")", "").Replace(",", "");
        }

        public static int ExecuteNonQuery(string query, object[] parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        string[] parameterNames = query.Split(' ');
                        int index = 0;

                        foreach (string parameterName in parameterNames)
                        {
                            if (parameterName.Contains('@'))
                            {
                                command.Parameters.AddWithValue(ReplaceParameterName(parameterName), parameters[index]);
                                index++;
                            }
                        }
                    }

                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
