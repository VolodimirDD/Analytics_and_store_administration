using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;

namespace ВКР
{
    public partial class LoginForm : Form
    {

        private DatabaseHelper dbHelper;       

        public LoginForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Fixed3D;// Розміри форми незмінні
            this.StartPosition = FormStartPosition.CenterScreen;// Форма по центру екрану
            dbHelper = new DatabaseHelper();
            passwordTextBox.PasswordChar = '●'; // Використовуємо '●' як PasswordChar
            passwordTextBox.Font = new Font(passwordTextBox.Font.FontFamily, 10, FontStyle.Bold);
            loginButton.TabStop = false;// Встановлюємо TabStop для кнопки значення false
            loginTextBox.TextAlign = HorizontalAlignment.Center;
            passwordTextBox.TextAlign = HorizontalAlignment.Center;
            loginButton.Cursor = Cursors.Hand;
        }

        AdminForm admForm = new AdminForm();
        
        private void loginButton_Click(object sender, EventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordTextBox.Text;

            // Перевіряємо, що login та password не є порожніми або містять лише прогалини
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Будь ласка, заповність всі поля!", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                dbHelper.OpenConnection();

                // SQL-запит для перервірки введених даних
                string query = "SELECT COUNT(*) FROM Користувачі WHERE Логін = @Login AND Захешований_пароль = @Password";

                using (SqlCommand command = new SqlCommand(query, dbHelper.GetConnection()))
                {
                    command.Parameters.AddWithValue("@Login", login);
                    command.Parameters.AddWithValue("@Password", password); 
                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        admForm.Username = login;                     
                        admForm.Show(); 
                        this.Hide(); 
                    }
                    else
                    {
                        MessageBox.Show("Вхід не виконано. Невірний логін чи пароль.", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        passwordTextBox.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при вході: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);               
            }
            finally
            {
                dbHelper.CloseConnection();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            label1.AutoSize = true; // Встановлюємо автоматичну зміну розміру тексту
            int x = (this.ClientSize.Width - label1.Width) / 2; // Обчислюємо X-координату для центрування
            int y = label1.Location.Y; // Y-координата залишається без змін (якщо потрібно центрувати і по вертикалі, то встановіть потрібне значення)
            label1.Location = new Point(x, y); // Встановлюємо нову позицію для Label
            this.MaximizeBox = false; 
        }       
    }
}
