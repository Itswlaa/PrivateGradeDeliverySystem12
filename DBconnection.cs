using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;

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
        public DataTable GetData(string query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }

    }
    }
    





