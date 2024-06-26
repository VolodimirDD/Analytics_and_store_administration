﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;


namespace ВКР
{
    public partial class Товар_по_порам_року : Form
    {

        private DatabaseHelper dbHelper;
        public string Username { get; set; } // Передача значення користувача login
        private DataTable originalDataTable; // Поле для зберігання вихідних даних
        private bool searchTextEmpty = true; // Прапор для відстеження стану тексту у searchTextBox

        public Товар_по_порам_року()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Fixed3D; // Розміри форми незмінні
            this.StartPosition = FormStartPosition.CenterScreen; // Форма по центру екрану
            dbHelper = new DatabaseHelper();
        }

        private void LoadData()
        {
            string query = "SELECT [ID_пори_року] as ID, [Найменування] FROM Товар_по_порам_року";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    originalDataTable = new DataTable();
                    adapter.Fill(originalDataTable);
                    dataGridView1.DataSource = originalDataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при завантаженні даних: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbHelper.CloseConnection();
            }
        }

        private void Товар_по_порам_року_Load(object sender, EventArgs e)
        {
            LoadData();
            user.Text = "Користувач : " + Username;
            user.Anchor = AnchorStyles.Top | AnchorStyles.Right; // Закріпляємо в верхній правій частині
            user.AutoSize = true; // Автоматично встановлюємо під довжину тексту
            user.TextAlign = ContentAlignment.TopRight; // Вирівнюємо текст в правій частині
            user.Location = new Point(this.ClientSize.Width - user.Width, 0);
            label1.AutoSize = true;
            int x = (this.ClientSize.Width - label1.Width) / 2;
            int y = label1.Location.Y;
            label1.Location = new Point(x, y);
            // Встановлюємо властивості для автоматичної зміни розміру
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            // Встановлюємо режим виділення рядків
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            // Встановлюємо комірки лише для читання
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.MultiSelect = false;
            // Додаємо обробник події TextChanged для TextBox
            searchTextBox.TextChanged += SearchTextBox_TextChanged;
            backButton.BackgroundImageLayout = ImageLayout.Zoom;
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Back.png");
            backButton.Image = Image.FromFile(imagePath);
            backButton.Cursor = Cursors.Hand;
            AddButton.Cursor = Cursors.Hand;
            EditButton.Cursor = Cursors.Hand;
            DeleteButton.Cursor = Cursors.Hand;
            this.MaximizeBox = false;
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            // Отримуємо введений користувачем текст
            string searchText = searchTextBox.Text.Trim();

            // Якщо текст порожній та попередній стан searchText було не порожнім
            if (string.IsNullOrWhiteSpace(searchText) && !searchTextEmpty)
            {
                LoadData(); // Завантажуємо всі дані
                searchTextEmpty = true; // Встановлюємо прапор назад у true
                return;
            }
            // Якщо текст не порожній
            else if (!string.IsNullOrWhiteSpace(searchText))
            {
                // Інакше виконуємо фільтрацію
                DataTable dt = originalDataTable; // Використовуємо вихідний DataTable
                DataView dv = dt.DefaultView;
                dv.RowFilter = $"Найменування LIKE '%{searchText}%'"; // Фільтруємо 

                // Оновлюємо дані, що відображаються
                dataGridView1.DataSource = dv.ToTable();
                searchTextEmpty = false; // Встановлюємо прапор у false, тому що текст не порожній
            }
            else
            {
                LoadData(); // Якщо searchText порожній, завантажуємо всі дані
                searchTextEmpty = true; // Встановлюємо прапор назад у true
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Заповнюємо текстові поля при виборі запису DataGridView
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                porarokuTextBox.Text = dataGridView1.SelectedRows[0].Cells["Найменування"].Value.ToString();
            }
        }

        private bool isClosing = false;

        private void Товар_по_порам_року_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isClosing)
                return;

            // Закриття програми з підтвердженням користувача
            DialogResult result = MessageBox.Show("Ви впевнені, що хочете закрити програму?", "Підтвердження закриття", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                isClosing = true;
                Close();
            }
        }

        private void Товар_по_порам_року_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form loginForm = Application.OpenForms["LoginForm"];
            if (loginForm != null)
            {
                loginForm.Close();
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string найменування = porarokuTextBox.Text;

            if (string.IsNullOrWhiteSpace(найменування))
            {
                MessageBox.Show("Будь ласка, коректно заповніть всі поля.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            string checkporarokuQuery = "SELECT COUNT(*) FROM Товар_по_порам_року WHERE [Найменування] = @Найменування";
            int katCount = 0;

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(checkporarokuQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Найменування", найменування);
                        katCount = (int)cmd.ExecuteScalar();
                    }
                }

                if (katCount > 0)
                {
                    MessageBox.Show("Запис з таким найменуванням вже існує.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                string query = "INSERT INTO Товар_по_порам_року ([Найменування]) VALUES (@Найменування)";

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Найменування", найменування);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запис успішно додано.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                porarokuTextBox.Text = string.Empty;
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при додаванні запису : " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbHelper.CloseConnection();
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].Cells["Найменування"].Value == null)
            {
                MessageBox.Show("Будь ласка, оберіть запис для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int porarokulId = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;
            string новеНайменування = porarokuTextBox.Text;


            if (string.IsNullOrWhiteSpace(новеНайменування))
            {
                MessageBox.Show("Будь ласка, введіть коректні дані для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Отримуємо поточні дані з DataGridView
            string currentName = dataGridView1.SelectedRows[0].Cells["Найменування"].Value.ToString();

            // Перевіряємо, чи були внесені зміни
            if (новеНайменування == currentName)
            {
                MessageBox.Show("Ви не внесли зміни в дані.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string query = "UPDATE Товар_по_порам_року SET [Найменування] = @НовеНайменування WHERE [ID_пори_року] = @PoraRokuID";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@НовеНайменування", новеНайменування);
                        cmd.Parameters.AddWithValue("@PoraRokuID", porarokulId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запис успішно оновлено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                porarokuTextBox.Text = string.Empty;
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при редагуванні запису : " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbHelper.CloseConnection();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].Cells["Найменування"].Value == null)
            {
                MessageBox.Show("Будь ласка, оберіть запис для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int porarokuId = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();

                    bool hasRelatedRecords = false;

                    using (SqlCommand checkZmistRealizatsiyiRecordsCmd = new SqlCommand("SELECT COUNT(*) FROM Зміст_реалізації WHERE ID_товару IN (SELECT ID_товару FROM Товар WHERE ID_пори_року = @PoraRokuID)", connection))
                    {
                        checkZmistRealizatsiyiRecordsCmd.Parameters.AddWithValue("@PoraRokuID", porarokuId);
                        int zmistRealizatsiyiRecordsCount = (int)checkZmistRealizatsiyiRecordsCmd.ExecuteScalar();
                        if (zmistRealizatsiyiRecordsCount > 0)
                        {
                            hasRelatedRecords = true;
                        }
                    }

                    if (!hasRelatedRecords)
                    {
                        using (SqlCommand checkRealizatsiyaRecordsCmd = new SqlCommand("SELECT COUNT(*) FROM Реалізація WHERE ID_покупки IN (SELECT ID_покупки FROM Зміст_реалізації WHERE ID_товару IN (SELECT ID_товару FROM Товар WHERE ID_пори_року = @PoraRokuID))", connection))
                        {
                            checkRealizatsiyaRecordsCmd.Parameters.AddWithValue("@PoraRokuID", porarokuId);
                            int realizatsiyaRecordsCount = (int)checkRealizatsiyaRecordsCmd.ExecuteScalar();
                            if (realizatsiyaRecordsCount > 0)
                            {
                                hasRelatedRecords = true;
                            }
                        }
                    }

                    if (!hasRelatedRecords)
                    {
                        using (SqlCommand checkTovarRecordsCmd = new SqlCommand("SELECT COUNT(*) FROM Товар WHERE ID_пори_року = @PoraRokuID", connection))
                        {
                            checkTovarRecordsCmd.Parameters.AddWithValue("@PoraRokuID", porarokuId);
                            int tovarRecordsCount = (int)checkTovarRecordsCmd.ExecuteScalar();
                            if (tovarRecordsCount > 0)
                            {
                                hasRelatedRecords = true;
                            }
                        }
                    }

                    string deleteMessage = hasRelatedRecords ? "Ви точно хочете видалити цей запис? Якщо ви видалите цей запис, всі зв'язані записи теж будуть видалені!" : "Ви дійсно хочете видалити цей запис?";

                    DialogResult result = MessageBox.Show(deleteMessage, "Підтвердження видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        if (hasRelatedRecords)
                        {
                            string deleteZmistRealizatsiyiQuery = "DELETE FROM Зміст_реалізації WHERE ID_товару IN (SELECT ID_товару FROM Товар WHERE ID_пори_року = @PoraRokuID)";
                            using (SqlCommand deleteZmistRealizatsiyiCmd = new SqlCommand(deleteZmistRealizatsiyiQuery, connection))
                            {
                                deleteZmistRealizatsiyiCmd.Parameters.AddWithValue("@PoraRokuID", porarokuId);
                                deleteZmistRealizatsiyiCmd.ExecuteNonQuery();
                            }

                            string deleteTovarQuery = "DELETE FROM Товар WHERE ID_пори_року = @PoraRokuID";
                            using (SqlCommand deleteTovarCmd = new SqlCommand(deleteTovarQuery, connection))
                            {
                                deleteTovarCmd.Parameters.AddWithValue("@PoraRokuID", porarokuId);
                                deleteTovarCmd.ExecuteNonQuery();
                            }

                            string deleteRealizatsiyaQuery = "DELETE FROM Реалізація WHERE ID_покупки NOT IN (SELECT ID_покупки FROM Зміст_реалізації)";
                            using (SqlCommand deleteRealizatsiyaCmd = new SqlCommand(deleteRealizatsiyaQuery, connection))
                            {
                                deleteRealizatsiyaCmd.Parameters.AddWithValue("@PoraRokuID", porarokuId);
                                deleteRealizatsiyaCmd.ExecuteNonQuery();
                            }
                        }

                        string deletePoraRokuQuery = "DELETE FROM Товар_по_порам_року WHERE ID_пори_року = @PoraRokuID";
                        using (SqlCommand deletePoraRokuCmd = new SqlCommand(deletePoraRokuQuery, connection))
                        {
                            deletePoraRokuCmd.Parameters.AddWithValue("@PoraRokuID", porarokuId);
                            deletePoraRokuCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Запис успішно видалено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        porarokuTextBox.Text = string.Empty;
                        dataGridView1.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при видаленні запису : " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbHelper.CloseConnection();
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            AdminForm admForm = new AdminForm();
            admForm.Username = Username; 
            admForm.Show();
            this.Hide();
        }
    }
}
