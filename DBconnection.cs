using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PrivateGradeDeliverySystem
{
   
        public class DBConnection
        {
            private string serverName;
            private string databaseName;
            private string connectionString;

            public DBConnection(string server, string database = "GradeMailDB")
            {
                serverName = server;
                databaseName = database;
                connectionString = $"Data Source={serverName};Initial Catalog={databaseName};Integrated Security=True";
            }

            // دالة لإرجاع SqlConnection جاهز
            public SqlConnection GetConnection()
            {
                return new SqlConnection(connectionString);
            }

            // دالة لإختبار الاتصال
            public bool TestConnection()
            {
                try
                {
                    using (SqlConnection conn = GetConnection())
                    {
                        conn.Open();
                        conn.Close();
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
    





