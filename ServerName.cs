using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrivateGradeDeliverySystem
{
    public partial class ServerName : Form
    {


        public ServerName()
        {
            InitializeComponent();
        }

        private void Guna2Button1_Click(object sender, EventArgs e)
        {
            string server = txt_servername.Text;
            DBConnection db = new DBConnection(server);

            if (db.TestConnection())
            {
                MessageBox.Show("تم الاتصال بنجاح بالخادم: " + server);
                this.Hide();
                Login login = new Login(db); // تمرير الاتصال إلى شاشة تسجيل الدخول
                login.ShowDialog();
            }
            else
            {
                MessageBox.Show("فشل الاتصال. تحقق من اسم الخادم.");
            }
        }
    }
}
