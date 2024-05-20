using OfficeOpenXml;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ВКР
{
    public partial class Analitics : Form
    {

        private DatabaseHelper dbHelper;
        public string Username { get; set; } // Передача значення користувача login
        private DataTable originalDataTable; // Поле для зберігання вихідних даних
        private DataView originalDataView; // Поле для зберігання вихідних даних
        private DataView shopDataView; // Для фільтрації даних 
        private DataView categoryDataView; // Для фільтрації даних 
        private bool searchTextEmpty = true; // Прапор для відстеження стану тексту у searchTextBox


        public Analitics()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Fixed3D; // Розміри форми незмінні
            this.StartPosition = FormStartPosition.CenterScreen; // Форма по центру екрану
            dbHelper = new DatabaseHelper();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private bool isClosing = false;

        private void Analitics_FormClosing(object sender, FormClosingEventArgs e)
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

        private void Analitics_FormClosed(object sender, FormClosedEventArgs e)
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
            admForm.Username = Username; // Передача значения на AdminForm 
            admForm.Show();
            this.Hide();
        }

        private void LoadTovData()
        {
            string query = @"
    SELECT Товар.[ID_товару], Товар.[Назва], Товар.[Ціна],
           COALESCE(SUM(Зміст_реалізації.[Кількість]), 0) AS [Реалізована кількість],
           COALESCE(SUM(Зміст_реалізації.[Кількість] * Товар.[Ціна]), 0) AS [Загальний прибуток]
    FROM Товар
    LEFT JOIN Зміст_реалізації ON Товар.[ID_товару] = Зміст_реалізації.[ID_товару]
    GROUP BY Товар.[ID_товару], Товар.[Назва], Товар.[Ціна]";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    originalDataTable = new DataTable();
                    adapter.Fill(originalDataTable);
                    dataGridView4.DataSource = originalDataTable;
                    originalDataView = originalDataTable.DefaultView;
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

        private void LoadMagData()
        {
            string query = @"
        WITH ShopSummary AS (
    SELECT
        M.[ID_магазину],
        M.[Адреса],
        M.[Телефон],
        NULL AS [ID_товару],
        NULL AS [Назва],
        NULL AS [Ціна],
        NULL AS [Реалізована кількість],
        NULL AS [Прибуток товару],
        SUM(ZR.[Кількість] * T.[Ціна]) AS [Загальний прибуток]
    FROM
        Магазини M
    LEFT JOIN
        Реалізація R ON M.[ID_магазину] = R.[ID_магазину]
    LEFT JOIN
        Зміст_реалізації ZR ON R.[ID_покупки] = ZR.[ID_покупки]
    LEFT JOIN
        Товар T ON ZR.[ID_товару] = T.[ID_товару]
    GROUP BY
        M.[ID_магазину], M.[Адреса], M.[Телефон]

    UNION ALL

    SELECT
        M.[ID_магазину],
        NULL AS [Адреса],
        NULL AS [Телефон],
        T.[ID_товару],
        T.[Назва],
        T.[Ціна],
        SUM(ZR.[Кількість]) AS [Реалізована кількість],
        SUM(ZR.[Кількість] * T.[Ціна]) AS [Прибуток товару],
        NULL
    FROM
        Магазини M
    LEFT JOIN
        Реалізація R ON M.[ID_магазину] = R.[ID_магазину]
    LEFT JOIN
        Зміст_реалізації ZR ON R.[ID_покупки] = ZR.[ID_покупки]
    LEFT JOIN
        Товар T ON ZR.[ID_товару] = T.[ID_товару]
    GROUP BY
        M.[ID_магазину], T.[ID_товару], T.[Назва], T.[Ціна]
)
SELECT
    [ID_магазину],
    [Адреса],
    [Телефон],
    [ID_товару],
    [Назва],
    [Ціна],
    [Реалізована кількість],
    [Прибуток товару],
    [Загальний прибуток]
FROM
    ShopSummary
ORDER BY
    [ID_магазину] ASC,
    [ID_товару] ASC,
    [Реалізована кількість] DESC;";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable shopDataTable = new DataTable();
                    adapter.Fill(shopDataTable);
                    dataGridView2.DataSource = shopDataTable;
                    shopDataView = shopDataTable.DefaultView;                   
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

        private void LoadCatData()
        {
            string query = @"
             WITH CategorySummary AS (
    SELECT
        C.[ID_категорії],
        C.[Найменування] AS [Найменування_категорії],
        NULL AS [ID_товару],
        NULL AS [Назва],
        NULL AS [Ціна],
        NULL AS [Реалізована кількість],
        NULL AS [Прибуток товару],
        SUM(ZR.[Кількість] * T.[Ціна]) AS [Загальний прибуток]
    FROM
        Товар_по_категоріям C
    LEFT JOIN
        Товар T ON C.[ID_категорії] = T.[ID_категорії]
    LEFT JOIN
        Зміст_реалізації ZR ON T.[ID_товару] = ZR.[ID_товару]
    WHERE
        ZR.[Кількість] IS NOT NULL 
    GROUP BY
        C.[ID_категорії], C.[Найменування]

    UNION ALL

    SELECT
        C.[ID_категорії],
        NULL AS [Найменування_категорії],
        T.[ID_товару],
        T.[Назва],
        T.[Ціна],
        SUM(ZR.[Кількість]) AS [Реалізована кількість],
        SUM(ZR.[Кількість] * T.[Ціна]) AS [Прибуток товару],
        NULL
    FROM
        Товар_по_категоріям C
    LEFT JOIN
        Товар T ON C.[ID_категорії] = T.[ID_категорії]
    LEFT JOIN
        Зміст_реалізації ZR ON T.[ID_товару] = ZR.[ID_товару]
    WHERE
        ZR.[Кількість] IS NOT NULL 
    GROUP BY
        C.[ID_категорії], T.[ID_товару], T.[Назва], T.[Ціна]
)
SELECT
    [ID_категорії],
    [Найменування_категорії],
    [ID_товару],
    [Назва],
    [Ціна],
    [Реалізована кількість],
    [Прибуток товару],
    [Загальний прибуток]
FROM
    CategorySummary
ORDER BY
    [ID_категорії] ASC,
    [ID_товару] ASC,
    [Реалізована кількість] DESC;";

            try
            {
                dbHelper.OpenConnection();

                using (SqlConnection connection = new SqlConnection(dbHelper.GetConnection().ConnectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable catDataTable = new DataTable();
                    adapter.Fill(catDataTable);
                    dataGridView3.DataSource = catDataTable;
                    categoryDataView = catDataTable.DefaultView;
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

        private void LoadPrubutokData()
        {
            string query = @"
    SELECT REPLACE(FORMAT(SUM(ZR.[Кількість] * T.[Ціна]), 'N0'), ',', ' ') AS [Загальний прибуток, грн]
FROM Реалізація R
JOIN Зміст_реалізації ZR ON R.[ID_покупки] = ZR.[ID_покупки]
JOIN Товар T ON ZR.[ID_товару] = T.[ID_товару];";

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

        private void Analitics_Load(object sender, EventArgs e)
        {
            LoadTovData();
            LoadMagData();
            LoadCatData();
            LoadPrubutokData();
            user.Text = "Користувач : " + Username;
            user.Anchor = AnchorStyles.Top | AnchorStyles.Right; // Закріпляємо в верхній правій частині
            user.AutoSize = true; // Автоматично встановлюємо під довжину тексту
            user.TextAlign = ContentAlignment.TopRight; // Вирівнюємо текст в правій частині
            user.Location = new Point(this.ClientSize.Width - user.Width, 0);
            backButton.BackgroundImageLayout = ImageLayout.Zoom;
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Back.png");
            backButton.Image = Image.FromFile(imagePath);
            backButton.Cursor = Cursors.Hand;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView3.AllowUserToAddRows = false;
            dataGridView3.ReadOnly = true;
            dataGridView3.AllowUserToDeleteRows = false;
            dataGridView4.AllowUserToAddRows = false;
            dataGridView4.ReadOnly = true;
            dataGridView4.AllowUserToDeleteRows = false;
            int centerX = (this.Width - label5.Width) / 2;
            int centerY = label5.Location.Y;
            label5.Location = new System.Drawing.Point(centerX, centerY);
            this.MaximizeBox = false;
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            // Отримуємо введений користувачем текст
            string searchText = searchTextBox.Text.Trim();

            // Якщо текст порожній та попередній стан searchText було не порожнім
            if (string.IsNullOrWhiteSpace(searchText) && !searchTextEmpty)
            {
                LoadTovData();
                searchTextEmpty = true;
                return;
            }
            // Якщо текст не порожній
            else if (!string.IsNullOrWhiteSpace(searchText))
            {
                originalDataView.RowFilter = $"Convert([ID_товару], 'System.String') LIKE '%{searchText}%'"; // Фільтруємо з використанням originalDataView

                // Оновлюємо дані, що відображаються
                dataGridView4.DataSource = originalDataView.ToTable();
                searchTextEmpty = false; // Встановлюємо прапор у false, тому що текст не порожній
            }
            else
            {
                LoadTovData();
                searchTextEmpty = true; // Встановлюємо прапор назад у true
            }
        }

        private void searchcatTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchcatTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText) && !searchTextEmpty)
            {
                LoadCatData();
                searchTextEmpty = true;
                return;
            }
            else if (!string.IsNullOrWhiteSpace(searchText))
            {
                categoryDataView.RowFilter = $"Convert([ID_категорії], 'System.String') LIKE '%{searchText}%'";
                // Оновлюємо дані, що відображаються
                dataGridView3.DataSource = categoryDataView.ToTable();
                searchTextEmpty = false;
            }
            else
            {
                LoadCatData();
                searchTextEmpty = true;
            }
        }

        private void searchshopTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchshopTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText) && !searchTextEmpty)
            {
                LoadMagData();
                searchTextEmpty = true;
                return;
            }
            else if (!string.IsNullOrWhiteSpace(searchText))
            {
                shopDataView.RowFilter = $"Convert([ID_магазину], 'System.String') LIKE '%{searchText}%'";
                // Оновлюємо дані, що відображаються
                dataGridView2.DataSource = shopDataView.ToTable();
                searchTextEmpty = false;
            }
            else
            {
                LoadMagData();
                searchTextEmpty = true;
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

        private void searchshopTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Перевіряємо, чи є введений символ цифрою (0-9) або клавішею Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Якщо символ не є цифрою або Backspace, скасовуємо його введення
                e.Handled = true;
                MessageBox.Show("Дозволено вводити тільки цифри.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void searchcatTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Перевіряємо, чи є введений символ цифрою (0-9) або клавішею Backspace
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                // Якщо символ не є цифрою або Backspace, скасовуємо його введення
                e.Handled = true;
                MessageBox.Show("Дозволено вводити тільки цифри.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DataGridView[] dataGridViews = new DataGridView[]
    {
        dataGridView1,
        dataGridView2,
        dataGridView3,
        dataGridView4
    };

            string timestamp = DateTime.Now.ToString("Дата yyyy-MM-dd, Час HH-mm-ss");
            string fileName = $"Результати {Username} ({timestamp}).xlsx";

            string[] sheetNames = new string[]
            {
        "Загальний прибуток",
        "Реалізація та прибуток магазинів",
        "Реалізація та прибуток товарів по категоріям",
        "Реалізація та прибуток товарів"
            };

            ExcelHelper.SaveToExcelOnDesktop(dataGridViews, sheetNames, fileName);
            MessageBox.Show("Дані збережені на робочому столі у файлі " + fileName, "Успішно", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
