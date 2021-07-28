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
using System.IO;

namespace ex5_3
{
    public partial class frmAuthors : Form
    {
        public frmAuthors()
        {
            InitializeComponent();
        }

        SqlConnection booksConnection;
        SqlCommand authorsCommand;
        SqlDataAdapter authorsAdapter;
        DataTable authorsTable;
        CurrencyManager authorsManager;
 
        private void frmAuthors_Load(object sender, EventArgs e)
        {
            string path = Path.GetFullPath("SQLBooksDB.mdf");
            MessageBox.Show(path);
            // connect to books database
            booksConnection = new
                SqlConnection("Data Source=.\\SQLEXPRESS; AttachDBFilename=" + path + ";" +
                "Integrated Security=True; Connect Timeout=30; User Instance=True");
            booksConnection.Open();
            // establish command object
            authorsCommand = new SqlCommand("Select * from Authors ORDER BY Author", booksConnection);
            // establish data adapter/data table
            authorsAdapter = new SqlDataAdapter();
            authorsAdapter.SelectCommand = authorsCommand;
            authorsTable = new DataTable();
            authorsAdapter.Fill(authorsTable);
            // bind controls to data table
            txtAuthorID.DataBindings.Add("Text", authorsTable, "Au_ID");
            txtAuthorName.DataBindings.Add("Text", authorsTable, "Author");
            txtYearBorn.DataBindings.Add("Text", authorsTable, "Year_Born");
            // estalish currency manager
            authorsManager = (CurrencyManager)
                this.BindingContext[authorsTable];
            this.Show();
            SetState("View");
        }

        private void frmAuthors_FormClosing(object sender, FormClosingEventArgs e)
        {
            // close the connection
            booksConnection.Close();
            // dispose of the objects
            booksConnection.Dispose();
            authorsCommand.Dispose();
            authorsAdapter.Dispose();
            authorsTable.Dispose();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (authorsManager.Position == 0)
            {
                Console.Beep();
            }
            authorsManager.Position--;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (authorsManager.Position == authorsManager.Count - 1)
            {
                Console.Beep();
            }
            authorsManager.Position++;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Record saved.", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SetState("View");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult response;
            response = MessageBox.Show("Are you sure you want to delete this record?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if(response == DialogResult.No)
            {
                return;
            }
        }

        private void SetState(string appState)
        {
            switch (appState)
            {
                case "View":
                    txtAuthorID.BackColor = Color.White;
                    txtAuthorID.ForeColor = Color.Black;
                    txtAuthorName.ReadOnly = true;
                    txtYearBorn.ReadOnly = true;
                    btnPrevious.Enabled = true;
                    btnNext.Enabled = true;
                    btnAddNew.Enabled = true;
                    btnSave.Enabled = false;
                    btnCancel.Enabled = false;
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                    btnDelete.Enabled = true;
                    txtAuthorName.Focus();
                    break;
                default: // Add or Edit if not View
                    txtAuthorID.BackColor = Color.Red;
                    txtAuthorID.ForeColor = Color.White;
                    txtAuthorName.ReadOnly = false;
                    txtYearBorn.ReadOnly = false;
                    btnPrevious.Enabled = false;
                    btnNext.Enabled = false;
                    btnAddNew.Enabled = false;
                    btnSave.Enabled = true;
                    btnCancel.Enabled = true;
                    btnEdit.Enabled = false;
                    btnDelete.Enabled = false;
                    btnDelete.Enabled = false;
                    txtAuthorName.Focus();
                    break;
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            SetState("Add");
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            SetState("Edit");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetState("View");
        }

        private void txtYearBorn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (int) e.KeyChar == 8)
            {
                // Acceptable keystrokes
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
                Console.Beep();
            }
        }
    }
}

