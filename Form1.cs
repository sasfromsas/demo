using System;
using System.Data;
using System.Data.SqlClient; // Заменяем MySql.Data.MySqlClient
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp22
{
    public partial class Form1 : Form
    {
        private SqlConnection connection; // Заменяем MySqlConnection на SqlConnection
        private string connectionString = "Server=DESKTOP-8Q0EEIR\\SQLEXPRESS;Database=ReadyToKMS;Integrated Security=True;";
        //private string connectionString = "Server=DESKTOP-80EQEIR\\SQLEXPRESS;Database=ReadyToKMS;User ID=myUser;Password=myPassword;";
        private DataTable dataTable;
        private SqlDataAdapter adapter; // Заменяем MySqlDataAdapter на SqlDataAdapter

        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();
            SetupForm();
        }

        private void InitializeDatabase()
        {
            try
            {
                connection = new SqlConnection(connectionString); 
                connection.Open();
                LoadTables();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message);
                Application.Exit();
            }
        }

        private void SetupForm()
        {
            tableSelector.SelectedIndexChanged += TableSelector_SelectedIndexChanged;
            btnAdd.Click += BtnAdd_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnDelete.Click += BtnDelete_Click;
        }

        private void LoadTables()
        {
            try
            {
                DataTable schema = connection.GetSchema("Tables");
                tableSelector.Items.Clear();

                foreach (DataRow row in schema.Rows)
                {
                    // Фильтруем только таблицы из текущей базы данных (игнорируем системные таблицы)
                    if (row["TABLE_SCHEMA"].ToString() == "dbo")
                    {
                        string tableName = row["TABLE_NAME"].ToString();
                        tableSelector.Items.Add(tableName);
                    }
                }

                if (tableSelector.Items.Count > 0)
                {
                    tableSelector.SelectedIndex = 0;
                    LoadTableData(tableSelector.SelectedItem.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки таблиц: " + ex.Message);
            }
        }

        private void LoadTableData(string tableName)
        {
            try
            {
                string query = $"SELECT * FROM {tableName}";
                adapter = new SqlDataAdapter(query, connection); // Используем SqlDataAdapter
                dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки данных: " + ex.Message);
            }
        }

        private void TableSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTableData(tableSelector.SelectedItem.ToString());
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string tableName = tableSelector.SelectedItem.ToString();

                using (Form inputForm = new Form())
                {
                    inputForm.Size = new System.Drawing.Size(300, 400);
                    inputForm.Text = "Добавить запись";
                    int y = 10;

                    TextBox[] textBoxes = new TextBox[dataTable.Columns.Count - 1]; // Исключаем ID
                    var columns = dataTable.Columns.Cast<DataColumn>().Where(c => c.ColumnName != "ID").ToList();
                    for (int i = 0; i < columns.Count; i++)
                    {
                        Label label = new Label
                        {
                            Text = columns[i].ColumnName,
                            Location = new System.Drawing.Point(10, y),
                            Width = 100
                        };
                        textBoxes[i] = new TextBox
                        {
                            Location = new System.Drawing.Point(120, y),
                            Width = 150
                        };
                        inputForm.Controls.Add(label);
                        inputForm.Controls.Add(textBoxes[i]);
                        y += 30;
                    }

                    Button btnConfirm = new Button
                    {
                        Text = "Добавить",
                        Location = new System.Drawing.Point(10, y),
                        Width = 100
                    };
                    btnConfirm.Click += (s, ev) =>
                    {
                        try
                        {
                            string columnsStr = string.Join(",", columns.Select(c => c.ColumnName));
                            string values = string.Join(",", textBoxes.Select(tb => $"'{tb.Text}'"));
                            string query = $"INSERT INTO {tableName} ({columnsStr}) VALUES ({values})";

                            SqlCommand cmd = new SqlCommand(query, connection);
                            cmd.ExecuteNonQuery();
                            inputForm.Close();
                            LoadTableData(tableName);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка добавления: " + ex.Message);
                        }
                    };
                    inputForm.Controls.Add(btnConfirm);
                    inputForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter); // Используем SqlCommandBuilder
                adapter.UpdateCommand = builder.GetUpdateCommand();
                adapter.InsertCommand = builder.GetInsertCommand();
                adapter.DeleteCommand = builder.GetDeleteCommand();

                int rowsAffected = adapter.Update(dataTable);
                MessageBox.Show($"Обновлено строк: {rowsAffected}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка обновления: " + ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string tableName = tableSelector.SelectedItem.ToString();

                if (dataGridView.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow row in dataGridView.SelectedRows)
                    {
                        string primaryKey = dataTable.Columns[0].ColumnName;
                        string value = row.Cells[0].Value.ToString();
                        string query = $"DELETE FROM {tableName} WHERE {primaryKey} = '{value}'";

                        SqlCommand cmd = new SqlCommand(query, connection); // Используем SqlCommand
                        cmd.ExecuteNonQuery();
                    }
                    LoadTableData(tableName);
                    MessageBox.Show("Записи удалены");
                }
                else
                {
                    MessageBox.Show("Выберите строки для удаления");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка удаления: " + ex.Message);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            connection?.Close();
            base.OnFormClosing(e);
        }
    }
}