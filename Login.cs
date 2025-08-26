using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace PrivateGradeDeliverySystem
{
    public partial class Login : Form
    {

        private DBConnection db;

        public Login(DBConnection db)
        {
            InitializeComponent();
            this.db = db;
            // جلب الأدوار من قاعدة البيانات لملء ComboBox
            LoadRoles();
        }

        private void LoadRoles()
        {
            try
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT RoleID, RoleName FROM Roles";
                    using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        cmbRole.DataSource = dt;
                        cmbRole.DisplayMember = "RoleName"; // يظهر للمستخدم
                        cmbRole.ValueMember = "RoleID";     // القيمة (ID)
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading roles: " + ex.Message);
            }

        }


        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            int roleId = Convert.ToInt32(cmbRole.SelectedValue);
            string roleName = cmbRole.Text; // الاسم المعروض

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(roleName))
            {
                MessageBox.Show("Please fill all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT u.UserID 
                                     FROM Users u
                                     INNER JOIN Roles r ON u.RoleID = r.RoleID
                                     WHERE u.Username = @username AND u.PasswordHash = @password AND r.RoleID = @roleId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        cmd.Parameters.AddWithValue("@roleId", roleId) ;

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                            // فتح الفورم المناسب حسب الدور
                            OpenFormByRole(roleName);

                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Invalid credentials!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }


        }




        private void OpenFormByRole(string role)
        {
            switch (role)
            {
                case "Student Affairs":
                    frmStudentAffairs studentForm = new frmStudentAffairs();
                    studentForm.Show();
                    break;
                case "dean":
                    frmDean deanForm = new frmDean();
                    deanForm.Show();
                    break;
                default:
                    MessageBox.Show("Role form not defined.");
                    break;
            }
        }


        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
        {














        }
    }
}