using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ВКР
{
    public partial class Зміст_реалізації : Form
    {

        private DatabaseHelper dbHelper;
        public string Username { get; set; } // Передача значення користувача login
        private DataTable originalDataTable; // Поле для зберігання вихідних даних
        private bool searchTextEmpty = true; // Прапор для відстеження стану тексту у searchTextBox
        private DataView originalDataView; // Поле для зберігання вихідних даних
        private DataView realDataView; // Для фільтрації даних у dataGridView2
        private DataView tovarDataView; // Для фільтрації даних у dataGridView3

        public Зміст_реалізації()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Fixed3D; // Розміри форми незмінні
            this.StartPosition = FormStartPosition.CenterScreen; // Форма по центру екрану
            dbHelper = new DatabaseHelper();
        }

        private void LoadData()
        {
            string query = "SELECT * FROM Зміст_реалізації";

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
                    originalDataView = originalDataTable.DefaultView; // Заповнюємо originalDataView
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

        private void Зміст_реалізації_FormClosing(object sender, FormClosingEventArgs e)
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

        private void Зміст_реалізації_FormClosed(object sender, FormClosedEventArgs e)
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

        private void LoadRealData()
        {
            string query = "SELECT Р.ID_покупки, Р.Дата, М.Адреса AS 'Адреса', М.Телефон AS 'Номер телефона' " +
                   "FROM Реалізація Р " +
                   "JOIN Магазини М ON Р.ID_магазину = М.ID_магазину";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable realDataTable = new DataTable();
                    adapter.Fill(realDataTable);
                    dataGridView2.DataSource = realDataTable;                  
                    realDataView = realDataTable.DefaultView;
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

        private void LoadTovarData()
        {
            string query = "SELECT Товар.ID_товару, Товар.Назва, Товар.Ціна, Товар.Розмір, " +
                   "Товарна_група.Найменування AS [Товарна група], " +
                   "Товар_по_порам_року.Найменування AS [Пора року], " +
                   "Товар_по_категоріям.Найменування AS [Категорія], " +
                   "Матеріали.Найменування AS [Матеріал] " +
                   "FROM Товар " +
                   "INNER JOIN Товарна_група ON Товар.ID_товарної_групи = Товарна_група.ID_товар_групи " +
                   "INNER JOIN Товар_по_порам_року ON Товар.ID_пори_року = Товар_по_порам_року.ID_пори_року " +
                   "INNER JOIN Товар_по_категоріям ON Товар.ID_категорії = Товар_по_категоріям.ID_категорії " +
                   "INNER JOIN Матеріали ON Товар.ID_матеріалу = Матеріали.ID_матеріалу";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable tovarDataTable = new DataTable();
                    adapter.Fill(tovarDataTable);
                    dataGridView3.DataSource = tovarDataTable;
                    tovarDataView = tovarDataTable.DefaultView;
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

        private void Зміст_реалізації_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadRealData();
            LoadTovarData();
            user.Text = "Користувач : " + Username;
            user.Anchor = AnchorStyles.Top | AnchorStyles.Right; // Закріпляємо в верхній правій частині
            user.AutoSize = true; // Автоматично встановлюємо під довжину тексту
            user.TextAlign = ContentAlignment.TopRight; // Вирівнюємо текст в правій частині
            user.Location = new Point(this.ClientSize.Width - user.Width, 0);
            label1.AutoSize = true;          
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
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.ReadOnly = true;
            dataGridView3.AllowUserToDeleteRows = false;
            this.MaximizeBox = false;
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            // Отримуємо введений користувачем текст
            string searchText = searchTextBox.Text.Trim();

            // Якщо текст порожній та попередній стан searchText було не порожнім
            if (string.IsNullOrWhiteSpace(searchText) && !searchTextEmpty)
            {
                LoadData();
                searchTextEmpty = true;
                return;
            }
            // Якщо текст не порожній
            else if (!string.IsNullOrWhiteSpace(searchText))
            {
                originalDataView.RowFilter = $"Convert([ID_товару], 'System.String') LIKE '%{searchText}%'"; // Фільтруємо з використанням originalDataView

                // Оновлюємо дані, що відображаються
                dataGridView1.DataSource = originalDataView.ToTable();
                searchTextEmpty = false; // Встановлюємо прапор у false, тому що текст не порожній
            }
            else
            {
                LoadData();
                searchTextEmpty = true; // Встановлюємо прапор назад у true
            }
        }

        private void searchrealTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchrealTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText) && !searchTextEmpty)
            {
                LoadRealData();
                searchTextEmpty = true;
                return;
            }
            else if (!string.IsNullOrWhiteSpace(searchText))
            {
                realDataView.RowFilter = $"Convert([ID_покупки], 'System.String') LIKE '%{searchText}%'";
                // Оновлюємо дані, що відображаються
                dataGridView2.DataSource = realDataView.ToTable();
                searchTextEmpty = false;
            }
            else
            {
                LoadRealData();
                searchTextEmpty = true;
            }
        }

        private void searchtovarTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchtovarTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText) && !searchTextEmpty)
            {
                LoadTovarData();
                searchTextEmpty = true;
                return;
            }
            else if (!string.IsNullOrWhiteSpace(searchText))
            {
                tovarDataView.RowFilter = $"Convert([ID_товару], 'System.String') LIKE '%{searchText}%'";
                // Оновлюємо дані, що відображаються
                dataGridView3.DataSource = tovarDataView.ToTable();
                searchTextEmpty = false;
            }
            else
            {
                LoadTovarData();
                searchTextEmpty = true;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Перетворимо числове значення в рядок для фільтрації
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];              
                idtovarTextBox.Text = row.Cells["ID_товару"].Value.ToString();
                idpokupkaTextBox.Text = row.Cells["ID_покупки"].Value.ToString();
                kvoTextBox.Text = row.Cells["Кількість"].Value.ToString();
            }
        }

        private void searchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Перевіряємо, чи є введений символ цифрою (0-9) або клавішею Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Якщо символ не є цифрою або Backspace, скасовуємо його введення
                e.Handled = true;
                MessageBox.Show("Дозволено вводити тільки цифри.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void searchrealTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Перевіряємо, чи є введений символ цифрою (0-9) або клавішею Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Якщо символ не є цифрою або Backspace, скасовуємо його введення
                e.Handled = true;
                MessageBox.Show("Дозволено вводити тільки цифри.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void searchtovarTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Перевіряємо, чи є введений символ цифрою (0-9) або клавішею Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Якщо символ не є цифрою або Backspace, скасовуємо його введення
                e.Handled = true;
                MessageBox.Show("Дозволено вводити тільки цифри.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool TovarExist(int idTovaru)
        {
            // Виконуємо запит до таблиці Товар, щоб перевірити існування товару із зазначеним ID
            string query = "SELECT COUNT(*) FROM Товар WHERE [ID_товару] = @ID_товару";

            using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID_товару", idTovaru);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private bool PokupkaExist(int idPokupki)
        {
            // Виконуємо запит до таблиці Реалізація, щоб перевірити існування покупки із зазначеним ID
            string query = "SELECT COUNT(*) FROM Реалізація WHERE [ID_покупки] = @ID_покупки";

            using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID_покупки", idPokupki);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string idTovaru = idtovarTextBox.Text.Trim();
            string idPokupky = idpokupkaTextBox.Text.Trim();
            string kvo = kvoTextBox.Text.Trim();

            // Перевіряємо наявність та коректність даних
            if (string.IsNullOrWhiteSpace(idTovaru) || string.IsNullOrWhiteSpace(idPokupky) || string.IsNullOrWhiteSpace(kvo))
            {
                MessageBox.Show("Будь ласка, коректно заповніть всі поля.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }           

            string query = "INSERT INTO Зміст_реалізації (ID_товару, ID_покупки, Кількість) VALUES (@ID_товару, @ID_покупки, @Кількість)";

            try
            {
                dbHelper.OpenConnection();

                // Перевіряємо, чи існує нове значення ID товару та ID покупки
                if (!TovarExist(int.Parse(idtovarTextBox.Text.Trim())) || !PokupkaExist(int.Parse(idpokupkaTextBox.Text.Trim())))
                {
                    MessageBox.Show("Запис з вказаним ID в таблиці Товари або Реалізація не існує. Перевірте правильність вводу!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_товару", idTovaru);
                    command.Parameters.AddWithValue("@ID_покупки", idPokupky);
                    command.Parameters.AddWithValue("@Кількість", kvo);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Запис успішно додано.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                idtovarTextBox.Text = string.Empty;
                idpokupkaTextBox.Text = string.Empty;
                kvoTextBox.Text = string.Empty;
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

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Будь ласка, оберіть запис для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Перевірка на порожні текстові поля
            if (string.IsNullOrWhiteSpace(idtovarTextBox.Text) ||
                string.IsNullOrWhiteSpace(idpokupkaTextBox.Text) ||
                string.IsNullOrWhiteSpace(kvoTextBox.Text))
            {
                MessageBox.Show("Будь ласка, введіть коректні дані для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int idTovaru = (int)dataGridView1.SelectedRows[0].Cells["ID_товару"].Value;
            int idPokupky = (int)dataGridView1.SelectedRows[0].Cells["ID_покупки"].Value;
            string newKvo = kvoTextBox.Text.Trim();

            // Перевіряємо на збіг даних
            if (idTovaru.ToString() == idtovarTextBox.Text.Trim() && idPokupky.ToString() == idpokupkaTextBox.Text.Trim() && newKvo == dataGridView1.SelectedRows[0].Cells["Кількість"].Value.ToString())
            {
                MessageBox.Show("Ви не внесли зміни в дані.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string query = "UPDATE Зміст_реалізації SET Кількість = @NewKvo, ID_товару = @ID_товару, ID_покупки = @ID_покупки WHERE ID_товару = @ID_товару_original AND ID_покупки = @ID_покупки_original";

            try
            {
                dbHelper.OpenConnection();

                // Перевіряємо, чи існує нове значення ID товару та ID покупки
                if (!TovarExist(int.Parse(idtovarTextBox.Text.Trim())) || !PokupkaExist(int.Parse(idpokupkaTextBox.Text.Trim())))
                {
                    MessageBox.Show("Запис з вказаним ID в таблиці Товари або Реалізація не існує. Перевірте правильність вводу!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewKvo", kvoTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@ID_товару", idtovarTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@ID_покупки", idpokupkaTextBox.Text.Trim());
                    command.Parameters.AddWithValue("@ID_товару_original", idTovaru);
                    command.Parameters.AddWithValue("@ID_покупки_original", idPokupky);

                    connection.Open();
                    int rowsUpdated = command.ExecuteNonQuery();

                    if (rowsUpdated > 0)
                    {
                        MessageBox.Show("Запис успішно оновлено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        idpokupkaTextBox.Text = string.Empty;
                        idtovarTextBox.Text = string.Empty;
                        kvoTextBox.Text = string.Empty;
                        dataGridView1.ClearSelection();
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося оновити запис. Перевірте ID товару і ID покупки.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при редагуванні запису: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbHelper.CloseConnection();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].Cells["ID_зміст_реал"].Value == null)
            {
                MessageBox.Show("Будь ласка, оберіть запис для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int zmistRealId = (int)dataGridView1.SelectedRows[0].Cells["ID_зміст_реал"].Value;

            string query = "DELETE FROM Зміст_реалізації WHERE [ID_зміст_реал] = @ZmistRealId";

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
                        cmd.Parameters.AddWithValue("@ZmistRealId", zmistRealId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Запис успішно видалено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
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
    }
}
