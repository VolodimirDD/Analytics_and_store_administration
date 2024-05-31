using Microsoft.Data.Tools.Schema.Sql.SchemaModel.Parameterization;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ВКР
{
    public partial class Магазини : Form
    {

        private DatabaseHelper dbHelper;
        public string Username { get; set; } // Передача значення користувача login
        private DataTable originalDataTable; // Поле для зберігання вихідних даних
        private bool searchTextEmpty = true; // Прапор для відстеження стану тексту у searchTextBox

        public Магазини()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Fixed3D; // Розміри форми незмінні
            this.StartPosition = FormStartPosition.CenterScreen; // Форма по центру екрану
            dbHelper = new DatabaseHelper();
        }

        private void LoadData()
        {
            string query = "SELECT [ID_магазину] as ID, [Адреса], [Телефон] FROM Магазини";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    originalDataTable = new DataTable();
                    adapter.Fill(originalDataTable);
                    dataGridView1.DataSource = originalDataTable; // Прив'язуємо дані до dataGridView1                  
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

        private bool isClosing = false;
        private void Магазини_FormClosing(object sender, FormClosingEventArgs e)
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

        private void Магазини_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form loginForm = Application.OpenForms["LoginForm"];
            if (loginForm != null)
            {
                loginForm.Close();
            }
        }

        private void Магазини_Load(object sender, EventArgs e)
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
            phoneMaskedTextBox.Mask = "0 000 00-00-00";
            phoneMaskedTextBox.PromptChar = ' ';
            phoneMaskedTextBox.SkipLiterals = true;
            phoneMaskedTextBox.BeepOnError = false;
            phoneMaskedTextBox.AllowPromptAsInput = true;
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
                dv.RowFilter = $"Адреса LIKE '%{searchText}%'"; // Фільтруємо 

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

        private void backButton_Click(object sender, EventArgs e)
        {
            AdminForm admForm = new AdminForm();
            admForm.Username = Username;
            admForm.Show();
            this.Hide();
        }

        // Перевірка формату номера телефону
        private bool IsPhoneNumberValid(string phoneNumber)
        {
            // Очищаємо номер від зайвих символів
            string cleanedNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());


            return cleanedNumber.Length == 10; // Перевіряємо, що номер містить 10 цифр
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Перевіряємо, чи обраний запис і чи не є виділений осередок порожнім
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].Cells["Адреса"].Value == null || dataGridView1.SelectedRows[0].Cells["Телефон"].Value == null)
            {
                MessageBox.Show("Будь ласка, оберіть запис для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int magId = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;
            string newPhone = phoneMaskedTextBox.Text;
            string newAdress = adressTextBox.Text;

            // Перевірка на коректність даних в полях
            if (string.IsNullOrWhiteSpace(phoneMaskedTextBox.Text) || string.IsNullOrWhiteSpace(adressTextBox.Text) || !IsPhoneNumberValid(newPhone))
            {
                MessageBox.Show("Будь ласка, введіть коректні дані для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Отримуємо поточні дані з DataGridView
            string currentAdress = dataGridView1.SelectedRows[0].Cells["Адреса"].Value.ToString();
            string currentPhone = dataGridView1.SelectedRows[0].Cells["Телефон"].Value.ToString();

            // Перевіряємо, чи були внесені зміни
            if (newPhone == currentPhone && newAdress == currentAdress)
            {
                MessageBox.Show("Ви не внесли зміни в дані.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Якщо зміни були внесені, оновлюємо дані
            string query = "UPDATE Магазини SET [Адреса] = @NewAdress, [Телефон] = @NewPhone WHERE [ID_магазину] = @MagID";
            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@NewAdress", newAdress);
                        cmd.Parameters.AddWithValue("@NewPhone", newPhone);
                        cmd.Parameters.AddWithValue("@MagID", magId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запис успішно оновлено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                adressTextBox.Text = string.Empty;
                phoneMaskedTextBox.Text = string.Empty;
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
            // Перевіряємо, чи обраний запис і чи не є обраний осередок порожнім
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].Cells["Адреса"].Value == null || dataGridView1.SelectedRows[0].Cells["Телефон"].Value == null)
            {
                MessageBox.Show("Будь ласка, оберіть запис для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int magId = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();

                    bool hasRelatedRecords = false;

                    // Перевіряємо наявність пов'язаних записів у таблиці "Реалізація"
                    using (SqlCommand checkRealizatsiyaRecordsCmd = new SqlCommand("SELECT COUNT(*) FROM Реалізація WHERE ID_магазину = @magID", connection))
                    {
                        checkRealizatsiyaRecordsCmd.Parameters.AddWithValue("@magID", magId);
                        int realizatsiyaRecordsCount = (int)checkRealizatsiyaRecordsCmd.ExecuteScalar();
                        if (realizatsiyaRecordsCount > 0)
                        {
                            hasRelatedRecords = true;
                        }
                    }

                    // Перевіряємо наявність пов'язаних записів у таблиці "Зміст_реалізації"
                    if (!hasRelatedRecords)
                    {
                        using (SqlCommand checkZmistRealizatsiyiRecordsCmd = new SqlCommand("SELECT COUNT(*) FROM Зміст_реалізації WHERE ID_покупки IN (SELECT ID_покупки FROM Реалізація WHERE ID_магазину = @magID)", connection))
                        {
                            checkZmistRealizatsiyiRecordsCmd.Parameters.AddWithValue("@magID", magId);
                            int zmistRealizatsiyiRecordsCount = (int)checkZmistRealizatsiyiRecordsCmd.ExecuteScalar();
                            if (zmistRealizatsiyiRecordsCount > 0)
                            {
                                hasRelatedRecords = true;
                            }
                        }
                    }

                    string deleteMessage = hasRelatedRecords ? "Ви точно хочете видалити цей запис? Якщо ви видалити цей запис, всі зв'язані записи теж будуть видалені!" : "Ви дійсно хочете видалити цей запис?";

                    DialogResult result = MessageBox.Show(deleteMessage, "Підтвердження видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Якщо є пов'язані записи та користувач згоден видалити їх
                        if (hasRelatedRecords)
                        {
                            // Видалення пов'язаних записів та самого магазину
                            string deleteRealizatsiyaQuery = "DELETE FROM Зміст_реалізації WHERE ID_покупки IN (SELECT ID_покупки FROM Реалізація WHERE ID_магазину = @magID)";
                            using (SqlCommand deleteRealizatsiyaCmd = new SqlCommand(deleteRealizatsiyaQuery, connection))
                            {
                                deleteRealizatsiyaCmd.Parameters.AddWithValue("@magID", magId);
                                deleteRealizatsiyaCmd.ExecuteNonQuery();
                            }

                            string deleteRealizatsiyaRecordQuery = "DELETE FROM Реалізація WHERE ID_магазину = @magID";
                            using (SqlCommand deleteRealizatsiyaRecordCmd = new SqlCommand(deleteRealizatsiyaRecordQuery, connection))
                            {
                                deleteRealizatsiyaRecordCmd.Parameters.AddWithValue("@magID", magId);
                                deleteRealizatsiyaRecordCmd.ExecuteNonQuery();
                            }
                        }

                        // Видалення запису з таблиці "Магазини"
                        string deleteMagazynQuery = "DELETE FROM Магазини WHERE ID_магазину = @magID";
                        using (SqlCommand deleteMagazynCmd = new SqlCommand(deleteMagazynQuery, connection))
                        {
                            deleteMagazynCmd.Parameters.AddWithValue("@magID", magId);
                            deleteMagazynCmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Запис успішно видалено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        adressTextBox.Text = string.Empty;
                        phoneMaskedTextBox.Text = string.Empty;
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

        private void AddButton_Click(object sender, EventArgs e)
        {
            string adress = adressTextBox.Text;
            string phone = phoneMaskedTextBox.Text;

            // Перевіряємо наявність даних
            if (string.IsNullOrWhiteSpace(adress) || string.IsNullOrWhiteSpace(phone) || !IsPhoneNumberValid(phone))
            {
                MessageBox.Show("Будь ласка, коректно заповніть всі поля.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string checkLoginQuery = "SELECT COUNT(*) FROM Магазини WHERE [Адреса] = @Adress";
            int userCount = 0;

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(checkLoginQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Adress", adress);
                        userCount = (int)cmd.ExecuteScalar();
                    }
                }

                if (userCount > 0)
                {
                    MessageBox.Show("Запис з такою адресою вже існує.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Якщо логін унікальний, виконуємо вставку
                string query = "INSERT INTO Магазини ([Адреса], [Телефон]) VALUES (@Adress, @Phone)";

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Adress", adress);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запис успішно додано.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                adressTextBox.Text = string.Empty;
                phoneMaskedTextBox.Text = string.Empty;
                dataGridView1.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при додаванні запису: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbHelper.CloseConnection();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Заповнюємо текстові поля при виборі запису DataGridView
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                adressTextBox.Text = dataGridView1.SelectedRows[0].Cells["Адреса"].Value.ToString();
                phoneMaskedTextBox.Text = dataGridView1.SelectedRows[0].Cells["Телефон"].Value.ToString();
            }
        }
    }
}