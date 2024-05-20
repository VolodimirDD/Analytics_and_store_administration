using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ВКР
{
    public partial class Реалізація : Form
    {

        private DatabaseHelper dbHelper;
        public string Username { get; set; } // Передача значення користувача login
        private DataTable originalDataTable; // Поле для зберігання вихідних даних
        private DataTable filteredDataTable;
        private DataTable ShopDataTable; // Поле для зберігання вихідних даних таблиці "Магазини"
        private DataTable filteredShopDataTable;

        public Реалізація()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Fixed3D; // Розміри форми незмінні
            this.StartPosition = FormStartPosition.CenterScreen; // Форма по центру екрану
            dbHelper = new DatabaseHelper();
        }

        private void LoadData()
        {
            string query = "SELECT [ID_покупки] as ID, [Дата], [ID_магазину] FROM Реалізація";

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

        private void Реалізація_FormClosing(object sender, FormClosingEventArgs e)
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

        private void Реалізація_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form loginForm = Application.OpenForms["LoginForm"];
            if (loginForm != null)
            {
                loginForm.Close();
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            AdminForm admForm = new AdminForm();
            admForm.Username = Username; 
            admForm.Show();
            this.Hide();
        }

        private void Реалізація_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadShopData();
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
            dataGridView1.Columns["Дата"].DefaultCellStyle.Format = "yyyy-MM-dd";
            dataMaskedTextBox.Mask = "0000-00-00";
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToDeleteRows = false;
            this.MaximizeBox = false;
        }

        private void LoadShopData()
        {
            string query = "SELECT [ID_магазину], [Адреса], [Телефон] FROM Магазини";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    ShopDataTable = new DataTable();
                    adapter.Fill(ShopDataTable);
                    dataGridView2.DataSource = ShopDataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при завантаженні даних магазинів: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbHelper.CloseConnection();
            }
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Якщо текст для пошуку порожній, відображаємо всі дані
                dataGridView1.DataSource = originalDataTable;
                filteredDataTable = null;
                return;
            }

            // Створюємо фільтровану таблицю
            filteredDataTable = new DataTable();

            foreach (DataColumn column in originalDataTable.Columns)
            {
                filteredDataTable.Columns.Add(column.ColumnName, column.DataType);
            }

            foreach (DataRow row in originalDataTable.Rows)
            {
                // Перетворимо дату в рядок з форматом "yyyy-MM-dd" для порівняння
                string dateText = ((DateTime)row["Дата"]).ToString("yyyy-MM-dd");

                if (dateText.Contains(searchText))
                {
                    filteredDataTable.ImportRow(row);
                }
            }

            dataGridView1.DataSource = filteredDataTable;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Заповнюємо текстові поля при виборі запису DataGridView
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                if (DateTime.TryParse(row.Cells["Дата"].Value.ToString(), out DateTime dateValue))
                {
                    dataMaskedTextBox.Text = dateValue.ToString("yyyy-MM-dd");
                }
                else
                {
                    dataMaskedTextBox.Text = string.Empty;
                }
                idshopTextBox.Text = row.Cells["ID_магазину"].Value.ToString();
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string data = dataMaskedTextBox.Text.Trim(); // Отримуємо дату з dataMaskedTextBox
            string idshop = idshopTextBox.Text.Trim();

            // Перевіряємо наявність даних та їх коректність
            if (string.IsNullOrWhiteSpace(data) || string.IsNullOrWhiteSpace(idshop) || data.Length != 10)
            {
                MessageBox.Show("Будь ласка, коректно заповніть всі поля.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Перевірка коректності дати
            if (!DateTime.TryParse(data, out DateTime parsedDate))
            {
                MessageBox.Show("Введена некоректна дата.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Перевіряємо, чи існує ID магазину
            if (!ShopExists(idshop))
            {
                MessageBox.Show("Магазин з вказаним ID не існує.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "INSERT INTO Реалізація ([Дата], [ID_магазину]) VALUES (@Data, @idShop)";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Data", parsedDate); // Використовуємо коректну дату
                        cmd.Parameters.AddWithValue("@idShop", idshop);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запис успішно додано.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                dataMaskedTextBox.Text = string.Empty; // Очищаємо dataMaskedTextBox
                idshopTextBox.Text = string.Empty;
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

        private bool ShopExists(string idShop)
        {
            // Виконуємо запит до таблиці Магазини, щоб перевірити існування магазину із зазначеним ID
            string query = "SELECT COUNT(*) FROM Магазини WHERE [ID_магазину] = @ID_магазину";

            using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID_магазину", idShop);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Перевіряємо, чи обраний запис і чи не є виділений осередок порожнім
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].Cells["Дата"].Value == null || dataGridView1.SelectedRows[0].Cells["ID_магазину"].Value == null)
            {
                MessageBox.Show("Будь ласка, оберіть запис для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int RealId = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;
            string newdata = dataMaskedTextBox.Text; // Получаем дату из dataMaskedTextBox
            string newidshop = idshopTextBox.Text;

            // Перевіряємо наявність даних та їх коректність
            if (string.IsNullOrWhiteSpace(newdata) || string.IsNullOrWhiteSpace(newidshop) || newdata.Length != 10)
            {
                MessageBox.Show("Будь ласка, коректно заповніть всі поля.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Перевірка коректності дати
            if (!DateTime.TryParse(newdata, out DateTime parsedDate))
            {
                MessageBox.Show("Введена некоректна дата.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Отримуємо поточні дані з DataGridView
            string currentData = dataGridView1.SelectedRows[0].Cells["Дата"].Value.ToString();
            string currentIdShop = dataGridView1.SelectedRows[0].Cells["ID_магазину"].Value.ToString();

            // Перевіряємо, чи були внесені зміни
            if (parsedDate == DateTime.Parse(currentData) && newidshop == currentIdShop)
            {
                MessageBox.Show("Ви не внесли зміни в дані.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Перевіряємо, чи існує ID магазину
            if (!ShopExists(newidshop))
            {
                MessageBox.Show("Магазин з вказаним ID не існує.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Якщо зміни були внесені, оновлюємо дані
            string query = "UPDATE Реалізація SET [Дата] = @NewData, [ID_магазину] = @NewID WHERE [ID_покупки] = @RealID";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@NewData", newdata);
                        cmd.Parameters.AddWithValue("@NewID", newidshop);
                        cmd.Parameters.AddWithValue("@RealID", RealId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запис успішно оновлено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                dataMaskedTextBox.Text = string.Empty; // Очищаємо dataMaskedTextBox
                idshopTextBox.Text = string.Empty;
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
            // Перевіряємо, чи обраний запис і чи не є виділений осередок порожнім
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].Cells["Дата"].Value == null || dataGridView1.SelectedRows[0].Cells["ID_магазину"].Value == null)
            {
                MessageBox.Show("Будь ласка, оберіть запис для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int realId = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

            string query = "DELETE FROM Реалізація WHERE [ID_покупки] = @PokupkaID";

            DialogResult result = MessageBox.Show("Ви дійсно хочете видалити цей запис?", "Підтвердження видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    dbHelper.OpenConnection();

                    using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                    {
                        connection.Open();
                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@PokupkaID", realId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Запис успішно видалено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    dataMaskedTextBox.Text = string.Empty;
                    idshopTextBox.Text = string.Empty;
                    dataGridView1.ClearSelection();
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
        }

        private void searchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Перевіряємо, чи введений символ є цифрою, клавішею Backspace або символом "-"
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back && e.KeyChar != '-')
            {
                // Якщо введений символ інший, скасовуємо його введення
                e.Handled = true;
                MessageBox.Show("Дозволено ввід цифр та символу '-'", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void magsearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = magsearchTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                // Если текст для поиска пуст, отображаем все данные
                dataGridView2.DataSource = ShopDataTable;
                filteredShopDataTable = null;
                return;
            }

            // Создаем фильтрованную таблицу
            filteredShopDataTable = new DataTable();

            foreach (DataColumn column in ShopDataTable.Columns)
            {
                filteredShopDataTable.Columns.Add(column.ColumnName, column.DataType);
            }

            foreach (DataRow row in ShopDataTable.Rows)
            {
                // Преобразуем ID_магазину в строку для сравнения
                string idMagazinuText = row["ID_магазину"].ToString();

                if (idMagazinuText.Contains(searchText))
                {
                    filteredShopDataTable.ImportRow(row);
                }
            }

            dataGridView2.DataSource = filteredShopDataTable;
        }

        private void magsearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Перевіряємо, чи є введений символ цифрою (0-9) або клавішею Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Якщо символ не є цифрою або Backspace, скасовуємо його введення
                e.Handled = true;
                MessageBox.Show("Дозволено вводити тільки цифри.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
