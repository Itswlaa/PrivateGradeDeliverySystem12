using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PrivateGradeDeliverySystem
{
    public partial class frmDean : Form
    {
        private DBConnection db;
        public frmDean(DBConnection db)
        {
            InitializeComponent();
            this.db = db;
            LoadTree();
        }
        private void LoadTree()
        {
            treeView1.Nodes.Clear();

            // جلب كل الأعوام الدراسية
            DataTable years = db.GetData("SELECT YearID, YearName FROM AcademicYear ORDER BY YearName");
            foreach (DataRow year in years.Rows)
            {
                TreeNode yearNode = new TreeNode(year["YearName"].ToString());
                yearNode.Tag = new { Type = "Year", ID = year["YearID"] };

                // جلب كل الكليات المرتبطة بالسنة (لو مستقلة تترك بدون شرط YearID)
                DataTable colleges = db.GetData("SELECT CollegeID, CollegeName FROM College ORDER BY CollegeName");
                foreach (DataRow college in colleges.Rows)
                {
                    TreeNode collegeNode = new TreeNode(college["CollegeName"].ToString());
                    collegeNode.Tag = new { Type = "College", ID = college["CollegeID"] };

                    // جلب التخصصات لكل كلية
                    DataTable majors = db.GetData("SELECT MajorID, MajorName FROM Major WHERE CollegeID=" + college["CollegeID"]);
                    foreach (DataRow major in majors.Rows)
                    {
                        TreeNode majorNode = new TreeNode(major["MajorName"].ToString());
                        majorNode.Tag = new { Type = "Major", ID = major["MajorID"] };

                        // جلب المواد لكل تخصص
                        DataTable subjects = db.GetData("SELECT SubjectID, SubjectName FROM Subject WHERE MajorID=" + major["MajorID"]);
                        foreach (DataRow subject in subjects.Rows)
                        {
                            TreeNode subjectNode = new TreeNode(subject["SubjectName"].ToString());
                            subjectNode.Tag = new { Type = "Subject", ID = subject["SubjectID"] };

                            majorNode.Nodes.Add(subjectNode);
                        }

                        collegeNode.Nodes.Add(majorNode);
                    }

                    yearNode.Nodes.Add(collegeNode);
                }

                treeView1.Nodes.Add(yearNode);
            }

            //treeView1.ExpandAll();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            dynamic tag = e.Node.Tag;
            // لو العقدة مادة
            if (tag.Type == "Subject")
            {
                int subjectId =tag.ID;

                string query = @"
            SELECT 
                s.StudentName,
                subj.SubjectName,
                m.Score
            FROM Marks m
            INNER JOIN Student s ON m.StudentID = s.StudentID
            INNER JOIN Subject subj ON m.SubjectID = subj.SubjectID
            WHERE subj.SubjectID = " + subjectId + @"
            ORDER BY s.StudentName, subj.SubjectName";

                DataTable dtMarks = db.GetData(query);
                dataGridView1.DataSource = dtMarks;

                // تعديل عناوين الأعمدة
                if (dataGridView1.Columns.Contains("StudentName"))
                    dataGridView1.Columns["StudentName"].HeaderText = "Student Name";
                if (dataGridView1.Columns.Contains("SubjectName"))
                    dataGridView1.Columns["SubjectName"].HeaderText = "Subject";
                if (dataGridView1.Columns.Contains("Score"))
                    dataGridView1.Columns["Score"].HeaderText = "Mark";
            }
        }
    }
}