using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ВКР
{
    public partial class Товар : Form
    {

        private DatabaseHelper dbHelper;
        public string Username { get; set; } // Передача значення користувача login
        private DataTable originalDataTable; // Поле для зберігання вихідних даних
        private bool searchTextEmpty = true; // Прапор для відстеження стану тексту у searchTextBox
        private DataView originalDataView; // Поле для зберігання вихідних даних
        private DataView tovgroupaDataView; // Для фільтрації даних у dataGridView2
        private DataView porurokuDataView; // Для фільтрації даних у dataGridView3
        private DataView kategoriyaDataView; // Для фільтрації даних у dataGridView4
        private DataView materialDataView; // Для фільтрації даних у dataGridView5

        public Товар()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Fixed3D; // Розміри форми незмінні
            this.StartPosition = FormStartPosition.CenterScreen; // Форма по центру екрану
            dbHelper = new DatabaseHelper();
        }

        private void LoadData()
        {
            string query = "SELECT [ID_товару] as ID, [Назва], [Ціна], [Розмір], [ID_товарної_групи], [ID_пори_року], [ID_категорії], [ID_матеріалу] FROM Товар";

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

        private void LoadTovGroupData()
        {
            string query = "SELECT * FROM Товарна_група";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable tovgroupDataTable = new DataTable();
                    adapter.Fill(tovgroupDataTable);
                    dataGridView2.DataSource = tovgroupDataTable;
                    tovgroupaDataView = tovgroupDataTable.DefaultView;
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

        private void LoadPoraRokuData()
        {
            string query = "SELECT * FROM Товар_по_порам_року";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable porarokuDataTable = new DataTable();
                    adapter.Fill(porarokuDataTable);
                    dataGridView3.DataSource = porarokuDataTable;
                    porurokuDataView = porarokuDataTable.DefaultView;
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

        private void LoadCategoryData()
        {
            string query = "SELECT * FROM Товар_по_категоріям";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable categoryDataTable = new DataTable();
                    adapter.Fill(categoryDataTable);
                    dataGridView4.DataSource = categoryDataTable;
                    kategoriyaDataView = categoryDataTable.DefaultView;
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

        private void LoadMaterialData()
        {
            string query = "SELECT * FROM Матеріали";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable materialDataTable = new DataTable();
                    adapter.Fill(materialDataTable);
                    dataGridView5.DataSource = materialDataTable;
                    materialDataView = materialDataTable.DefaultView;
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

        private void Товар_FormClosing(object sender, FormClosingEventArgs e)
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

        private void Товар_FormClosed(object sender, FormClosedEventArgs e)
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

        private void Товар_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadTovGroupData();
            LoadPoraRokuData();
            LoadCategoryData();
            LoadMaterialData();
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
            // Встановлюємо властивості для автоматичної зміни розміру
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToDeleteRows = false;            
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            // Встановлюємо властивості для автоматичної зміни розміру
            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.ReadOnly = true;
            dataGridView3.AllowUserToDeleteRows = false;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            // Встановлюємо властивості для автоматичної зміни розміру
            dataGridView4.AllowUserToAddRows = false;
            dataGridView4.ReadOnly = true;
            dataGridView4.AllowUserToDeleteRows = false;
            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView4.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            // Встановлюємо властивості для автоматичної зміни розміру
            dataGridView5.AllowUserToAddRows = false;
            dataGridView5.ReadOnly = true;
            dataGridView5.AllowUserToDeleteRows = false;           
            dataGridView5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView5.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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

        private void searchtovgrTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchtovgrTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText) && !searchTextEmpty)
            {
                LoadTovGroupData();
                searchTextEmpty = true;
                return;
            }
            else if (!string.IsNullOrWhiteSpace(searchText))
            {
                tovgroupaDataView.RowFilter = $"Convert([ID_товар_групи], 'System.String') LIKE '%{searchText}%'";
                // Оновлюємо дані, що відображаються
                dataGridView2.DataSource = tovgroupaDataView.ToTable();
                searchTextEmpty = false;
            }
            else
            {
                LoadTovGroupData();
                searchTextEmpty = true;
            }
        }

        private void searchporurokuTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchporurokuTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText) && !searchTextEmpty)
            {
                LoadPoraRokuData();
                searchTextEmpty = true;
                return;
            }
            else if (!string.IsNullOrWhiteSpace(searchText))
            {
                porurokuDataView.RowFilter = $"Convert([ID_пори_року], 'System.String') LIKE '%{searchText}%'";
                // Оновлюємо дані, що відображаються
                dataGridView3.DataSource = porurokuDataView.ToTable();
                searchTextEmpty = false;
            }
            else
            {
                LoadPoraRokuData();
                searchTextEmpty = true;
            }
        }

        private void searchkatTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchkatTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText) && !searchTextEmpty)
            {
                LoadCategoryData();
                searchTextEmpty = true;
                return;
            }
            else if (!string.IsNullOrWhiteSpace(searchText))
            {
                kategoriyaDataView.RowFilter = $"Convert([ID_категорії], 'System.String') LIKE '%{searchText}%'";
                // Оновлюємо дані, що відображаються
                dataGridView4.DataSource = kategoriyaDataView.ToTable();
                searchTextEmpty = false;
            }
            else
            {
                LoadCategoryData();
                searchTextEmpty = true;
            }
        }

        private void searchmaterialTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchmaterialTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText) && !searchTextEmpty)
            {
                LoadMaterialData();
                searchTextEmpty = true;
                return;
            }
            else if (!string.IsNullOrWhiteSpace(searchText))
            {
                materialDataView.RowFilter = $"Convert([ID_матеріалу], 'System.String') LIKE '%{searchText}%'";
                // Оновлюємо дані, що відображаються
                dataGridView5.DataSource = materialDataView.ToTable();
                searchTextEmpty = false;
            }
            else
            {
                LoadMaterialData();
                searchTextEmpty = true;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Перетворимо числове значення в рядок для фільтрації
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                nameTextBox.Text = row.Cells["Назва"].Value.ToString();
                priceTextBox.Text = row.Cells["Ціна"].Value.ToString();
                sizeTextBox.Text = row.Cells["Розмір"].Value.ToString();
                idtovgrTextBox.Text = row.Cells["ID_товарної_групи"].Value.ToString();
                idporurokuTextBox.Text = row.Cells["ID_пори_року"].Value.ToString();
                idkatTextBox.Text = row.Cells["ID_категорії"].Value.ToString();
                idmatTextBox.Text = row.Cells["ID_матеріалу"].Value.ToString();
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

        private void searchtovgrTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Перевіряємо, чи є введений символ цифрою (0-9) або клавішею Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Якщо символ не є цифрою або Backspace, скасовуємо його введення
                e.Handled = true;
                MessageBox.Show("Дозволено вводити тільки цифри.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void searchporurokuTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Перевіряємо, чи є введений символ цифрою (0-9) або клавішею Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Якщо символ не є цифрою або Backspace, скасовуємо його введення
                e.Handled = true;
                MessageBox.Show("Дозволено вводити тільки цифри.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void searchkatTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Перевіряємо, чи є введений символ цифрою (0-9) або клавішею Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Якщо символ не є цифрою або Backspace, скасовуємо його введення
                e.Handled = true;
                MessageBox.Show("Дозволено вводити тільки цифри.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void searchmaterialTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Перевіряємо, чи є введений символ цифрою (0-9) або клавішею Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Якщо символ не є цифрою або Backspace, скасовуємо його введення
                e.Handled = true;
                MessageBox.Show("Дозволено вводити тільки цифри.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private bool TovGrExist(int idTovGr)
        {
            // Виконуємо запит до таблиці, щоб перевірити існування зазначеного ID
            string query = "SELECT COUNT(*) FROM Товарна_група WHERE [ID_товар_групи] = @ID_товгр";

            using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID_товгр", idTovGr);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private bool PoruRokuExist(int idPoruRoku)
        {
            // Виконуємо запит до таблиці, щоб перевірити існування зазначеного ID
            string query = "SELECT COUNT(*) FROM Товар_по_порам_року WHERE [ID_пори_року] = @ID_порроку";

            using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID_порроку", idPoruRoku);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private bool KatExist(int idKat)
        {
            // Виконуємо запит до таблиці, щоб перевірити існування зазначеного ID
            string query = "SELECT COUNT(*) FROM Товар_по_категоріям WHERE [ID_категорії] = @ID_кат";

            using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID_кат", idKat);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private bool MatExist(int idMat)
        {
            // Виконуємо запит до таблиці, щоб перевірити існування зазначеного ID
            string query = "SELECT COUNT(*) FROM Матеріали WHERE [ID_матеріалу] = @ID_мат";

            using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID_мат", idMat);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            string priceText = priceTextBox.Text;
            string name = nameTextBox.Text;
            string size = sizeTextBox.Text;
            string idtovgr= idtovgrTextBox.Text;
            string idporuroku = idporurokuTextBox.Text;
            string idkat = idkatTextBox.Text;
            string idmat = idmatTextBox.Text;
            int price;

            // Перевіряємо наявність даних
            if (string.IsNullOrWhiteSpace(priceText) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(size) || string.IsNullOrWhiteSpace(idtovgr) || string.IsNullOrWhiteSpace(idporuroku) || string.IsNullOrWhiteSpace(idmat) || string.IsNullOrWhiteSpace(idkat))
            {
                MessageBox.Show("Будь ласка, коректно заповніть всі поля.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string checknameQuery = "SELECT COUNT(*) FROM Товар WHERE [Назва] = @name";
            int userCount = 0;

            try
            {
                dbHelper.OpenConnection();

                // Перевіряємо, чи існує нове значення ID 
                if (!TovGrExist(int.Parse(idtovgrTextBox.Text.Trim())) || !PoruRokuExist(int.Parse(idporurokuTextBox.Text.Trim())) || !MatExist(int.Parse(idmatTextBox.Text.Trim())) || !KatExist(int.Parse(idkatTextBox.Text.Trim())))
                {
                    MessageBox.Show("Запис з вказаними ID в таблицях не існує. Перевірте правильність вводу!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Перевірка чи введена ціна є цілим числом
                if (!int.TryParse(priceText, out price))
                {
                    MessageBox.Show("Ціна повинна бути цілим числом.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(checknameQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        userCount = (int)cmd.ExecuteScalar();
                    }
                }

                if (userCount > 0)
                {
                    MessageBox.Show("Запис з такою назвою вже існує вже існує.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Якщо назва унікальна, виконуємо вставку
                string query = "INSERT INTO Товар ([Назва], [Ціна], [Розмір], [ID_товарної_групи],[ID_пори_року],[ID_категорії] ,[ID_матеріалу]) VALUES (@name, @price, @size, @idtovgr, @idporuroku, @idkat, @idmat)";

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@price", priceText);
                        cmd.Parameters.AddWithValue("@size", size);
                        cmd.Parameters.AddWithValue("@idtovgr", idtovgr);
                        cmd.Parameters.AddWithValue("@idporuroku", idporuroku);
                        cmd.Parameters.AddWithValue("@idkat", idkat);
                        cmd.Parameters.AddWithValue("@idmat", idmat);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запис успішно додано.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                nameTextBox.Text = string.Empty;
                sizeTextBox.Text = string.Empty;
                priceTextBox.Text = string.Empty;
                idtovgrTextBox.Text = string.Empty;
                idmatTextBox.Text = string.Empty;
                idkatTextBox.Text = string.Empty;
                idporurokuTextBox.Text = string.Empty;
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

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            // Перевіряємо, чи обраний запис і чи не є виділений осередок порожнім
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].Cells["Назва"].Value == null || dataGridView1.SelectedRows[0].Cells["Ціна"].Value == null || dataGridView1.SelectedRows[0].Cells["Розмір"].Value == null || dataGridView1.SelectedRows[0].Cells["ID_товарної_групи"].Value == null || dataGridView1.SelectedRows[0].Cells["ID_пори_року"].Value == null || dataGridView1.SelectedRows[0].Cells["ID_категорії"].Value == null || dataGridView1.SelectedRows[0].Cells["ID_матеріалу"].Value == null)
            {
                MessageBox.Show("Будь ласка, оберіть запис для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int tovId = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;

            string query = "DELETE FROM Товар WHERE [ID_товару] = @TovID";

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
                        cmd.Parameters.AddWithValue("@TovID", tovId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Запис успішно видалено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    nameTextBox.Text = string.Empty;
                    sizeTextBox.Text = string.Empty;
                    priceTextBox.Text = string.Empty;
                    idtovgrTextBox.Text = string.Empty;
                    idmatTextBox.Text = string.Empty;
                    idkatTextBox.Text = string.Empty;
                    idporurokuTextBox.Text = string.Empty;
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

        private void EditButton_Click(object sender, EventArgs e)
        {
            // Перевіряємо, чи обраний запис і чи не є виділений осередок порожнім
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows[0].Cells["Назва"].Value == null || dataGridView1.SelectedRows[0].Cells["Ціна"].Value == null || dataGridView1.SelectedRows[0].Cells["Розмір"].Value == null || dataGridView1.SelectedRows[0].Cells["ID_товарної_групи"].Value == null || dataGridView1.SelectedRows[0].Cells["ID_пори_року"].Value == null || dataGridView1.SelectedRows[0].Cells["ID_категорії"].Value == null || dataGridView1.SelectedRows[0].Cells["ID_матеріалу"].Value == null)
            {
                MessageBox.Show("Будь ласка, оберіть запис для видалення.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int tovarId = (int)dataGridView1.SelectedRows[0].Cells["ID"].Value;
            string priceText = priceTextBox.Text;
            string name = nameTextBox.Text;
            string size = sizeTextBox.Text;
            string idtovgr = idtovgrTextBox.Text;
            string idporuroku = idporurokuTextBox.Text;
            string idkat = idkatTextBox.Text;
            string idmat = idmatTextBox.Text;
            int price;

            // Перевірка на коректність даних в полях
            if (string.IsNullOrWhiteSpace(priceText) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(size) || string.IsNullOrWhiteSpace(idtovgr) || string.IsNullOrWhiteSpace(idporuroku) || string.IsNullOrWhiteSpace(idmat) || string.IsNullOrWhiteSpace(idkat))
            {
                MessageBox.Show("Будь ласка, введіть коректні дані для редагування.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Отримуємо поточні дані з DataGridView
            string currentpriceText = dataGridView1.SelectedRows[0].Cells["Ціна"].Value.ToString();
            string currentname = dataGridView1.SelectedRows[0].Cells["Назва"].Value.ToString();
            string currentsize = dataGridView1.SelectedRows[0].Cells["Розмір"].Value.ToString();
            string currentidtovgr = dataGridView1.SelectedRows[0].Cells["ID_товарної_групи"].Value.ToString();
            string currentidporuroku = dataGridView1.SelectedRows[0].Cells["ID_пори_року"].Value.ToString();
            string currentidkat = dataGridView1.SelectedRows[0].Cells["ID_категорії"].Value.ToString();
            string currentidmat = dataGridView1.SelectedRows[0].Cells["ID_матеріалу"].Value.ToString();


            // Перевіряємо, чи були внесені зміни
            if (priceText == currentpriceText && name == currentname && size == currentsize && idtovgr == currentidtovgr && idporuroku == currentidporuroku && idkat == currentidkat && idmat == currentidmat)
            {
                MessageBox.Show("Ви не внесли зміни в дані.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Якщо зміни були внесені, оновлюємо дані
            string query = "UPDATE Товар SET [Назва] = @name, [Ціна] = @priceText, [Розмір] = @size, [ID_товарної_групи] = @idtovgr, [ID_пори_року] = @idporuroku, [ID_категорії] = @idkat, [ID_матеріалу] = @idmat WHERE [ID_товару] = @tovarId";

            try
            {
                dbHelper.OpenConnection();

                // Перевіряємо, чи існує нове значення ID 
                if (!TovGrExist(int.Parse(idtovgrTextBox.Text.Trim())) || !PoruRokuExist(int.Parse(idporurokuTextBox.Text.Trim())) || !MatExist(int.Parse(idmatTextBox.Text.Trim())) || !KatExist(int.Parse(idkatTextBox.Text.Trim())))
                {
                    MessageBox.Show("Запис з вказаними ID в таблицях не існує. Перевірте правильність вводу!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Перевірка чи введена ціна є цілим числом
                if (!int.TryParse(priceText, out price))
                {
                    MessageBox.Show("Ціна повинна бути цілим числом.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@priceText", priceText);
                        cmd.Parameters.AddWithValue("@size", size);
                        cmd.Parameters.AddWithValue("@idtovgr", idtovgr);
                        cmd.Parameters.AddWithValue("@idporuroku", idporuroku);
                        cmd.Parameters.AddWithValue("@idkat", idkat);
                        cmd.Parameters.AddWithValue("@idmat", idmat);
                        cmd.Parameters.AddWithValue("@tovarId", tovarId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Запис успішно видалено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                nameTextBox.Text = string.Empty;
                sizeTextBox.Text = string.Empty;
                priceTextBox.Text = string.Empty;
                idtovgrTextBox.Text = string.Empty;
                idmatTextBox.Text = string.Empty;
                idkatTextBox.Text = string.Empty;
                idporurokuTextBox.Text = string.Empty;
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
    }
}
