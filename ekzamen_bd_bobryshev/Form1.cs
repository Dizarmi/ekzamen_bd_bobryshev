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
using System.Drawing.Text;

namespace ekzamen_bd_bobryshev
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;

        private SqlCommandBuilder sqlBuilder = null;

        private SqlDataAdapter sqlDataAdapter = null;

        private DataSet dataSet = null;

        private bool newRowAdding = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT * , 'Delete' AS [Delete] FROM Users", sqlConnection);
                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                sqlBuilder.GetInsertCommand();
                sqlBuilder.GetUpdateCommand();
                sqlBuilder.GetDeleteCommand();

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Users");

                dataGridView1.DataSource = dataSet.Tables["Users"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[7, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReloadData()
        {
            try
            {
                dataSet.Tables["Users"].Clear();

                sqlDataAdapter.Fill(dataSet, "Users");

                dataGridView1.DataSource = dataSet.Tables["Users"];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();
                    dataGridView1[6, i] = linkCell;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\artom\source\repos\ekzamen_bd_bobryshev\ekzamen_bd_bobryshev\Database1.mdf;Integrated Security=True");

            sqlConnection.Open();

            LoadData();
        }

        private void вИToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 7)
                {
                    string task = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();

                    if (task == "Delete")
                    {
                        if (MessageBox.Show("Удалить эту строку?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            int rowIndex = e.RowIndex;
                            dataGridView1.Rows.RemoveAt(rowIndex);

                            sqlDataAdapter.Update(dataSet, "Users");
                        }
                    }
                    else if (task == "Insert")
                    {
                        int rowIndex = dataGridView1.Rows.Count - 2;

                        DataRow row = dataSet.Tables["Users"].NewRow();

                        row["Order number"] = dataGridView1.Rows[rowIndex].Cells["Order number"].Value;
                        row["Сustomer contacts"] = dataGridView1.Rows[rowIndex].Cells["Сustomer contacts"].Value;
                        row["Type"] = dataGridView1.Rows[rowIndex].Cells["Type"].Value;
                        row["Project start date"] = dataGridView1.Rows[rowIndex].Cells["Project start date"].Value;
                        row["Project end date"] = dataGridView1.Rows[rowIndex].Cells["Project end date"].Value;
                        row["Price"] = dataGridView1.Rows[rowIndex].Cells["Price"].Value;

                        dataSet.Tables["Users"].Rows.Add(row);
                        dataSet.Tables["Users"].Rows.RemoveAt(dataSet.Tables["Users"].Rows.Count - 1);
                        dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 2);
                        dataGridView1.Rows[e.RowIndex].Cells[7].Value = "Delete";

                        sqlDataAdapter.Update(dataSet, "Users");

                        newRowAdding = false;
                    }
                    else if (task == "Update")
                    {
                        int r = e.RowIndex;

                        dataSet.Tables["Users"].Rows[r]["Order number"] = dataGridView1.Rows[r].Cells["Order number"].Value;
                        dataSet.Tables["Users"].Rows[r]["Сustomer contacts"] = dataGridView1.Rows[r].Cells["Сustomer contacts"].Value;
                        dataSet.Tables["Users"].Rows[r]["Type"] = dataGridView1.Rows[r].Cells["Type"].Value;
                        dataSet.Tables["Users"].Rows[r]["Project start date"] = dataGridView1.Rows[r].Cells["Project start date"].Value;
                        dataSet.Tables["Users"].Rows[r]["Project end date"] = dataGridView1.Rows[r].Cells["Project end date"].Value;
                        dataSet.Tables["Users"].Rows[r]["Price"] = dataGridView1.Rows[r].Cells["Price"].Value;

                        sqlDataAdapter.Update(dataSet, "Users");

                        dataGridView1.Rows[e.RowIndex].Cells[7].Value = "Delete";
                    }

                    ReloadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    newRowAdding = true;

                    int lastRow = dataGridView1.Rows.Count - 2;

                    DataGridViewRow row = dataGridView1.Rows[lastRow];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[7, lastRow] = linkCell;

                    row.Cells["Delete"].Value = "Insert";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (newRowAdding == false)
                {
                    int rowIndex = dataGridView1.SelectedCells[0].RowIndex;

                    DataGridViewRow editingRow = dataGridView1.Rows[rowIndex];

                    DataGridViewLinkCell linkCell = new DataGridViewLinkCell();

                    dataGridView1[7, rowIndex] = linkCell;

                    editingRow.Cells["Delete"].Value = "Update";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteCascade()
        {
            
            sqlConnection.Open();

            
            SqlTransaction transaction = sqlConnection.BeginTransaction();

            try
            {
                
                SqlCommand command1 = new SqlCommand("DELETE FROM Sales WHERE OrderID IN (SELECT OrderID FROM Orders)", sqlConnection, transaction);
                command1.ExecuteNonQuery();

                
                SqlCommand command2 = new SqlCommand("DELETE FROM Orders", sqlConnection, transaction);
                command2.ExecuteNonQuery();

               
                transaction.Commit();

                MessageBox.Show("Удаление выполнено успешно.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                
                transaction.Rollback();

                MessageBox.Show("Ошибка при выполнении удаления: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                
                sqlConnection.Close();
            }
        }

        private void btnDeleteCascade_Click(object sender, EventArgs e)
        {
            DeleteCascade();
            ReloadData();
        }
    }
}