using System;
using System.Windows.Forms;
using System.Drawing;

namespace ВКР
{
    public partial class AdminForm : Form
    {

        private DatabaseHelper dbHelper;
        public string Username { get; set; } // Передача значення користувача login

        public AdminForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.Fixed3D; // Розміри форми незмінні
            this.StartPosition = FormStartPosition.CenterScreen; // Форма по центру екрану
            dbHelper = new DatabaseHelper();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            //Додаємо елементи в ComboBox
            comboBox1.Items.Add("Користувачі");
            comboBox1.Items.Add("Товар");
            comboBox1.Items.Add("Магазини");
            comboBox1.Items.Add("Товар по порам року");
            comboBox1.Items.Add("Товар по категоріям");
            comboBox1.Items.Add("Матеріали");
            comboBox1.Items.Add("Товарна група");
            comboBox1.Items.Add("Реалізація");
            comboBox1.Items.Add("Зміст реалізації");
            user.Text = "Вітаємо, " + Username;
            user.Anchor = AnchorStyles.Top | AnchorStyles.Right; // Закріпляємо в верхній правій частині
            user.AutoSize = true; // Автоматично встановлюємо під довжину тексту
            user.TextAlign = ContentAlignment.TopRight; // Вирівнюємо текст в правій частині
            // Устанавливаем позицию метки в правом верхнем углу формы
            user.Location = new Point(this.ClientSize.Width - user.Width, 0);
            label1.AutoSize = true; // Встановлюємо автоматичну зміну розміру тексту
            int x = (this.ClientSize.Width - label1.Width) / 2; // Обчислюємо X-координату для центрування
            int y = label1.Location.Y; // Y-координата залишається без змін (якщо потрібно центрувати і по вертикалі, то встановіть потрібне значення)
            label1.Location = new Point(x, y); // Встановлюємо нову позицію для Label
            chooseButton.Cursor = Cursors.Hand;
            this.MaximizeBox = false;
        }

        private void chooseButton_Click(object sender, EventArgs e)
        {

            // Отримуємо вибраний елемент із ComboBox для перевірки
            var selectedFormItem = comboBox1.SelectedItem;

            if (selectedFormItem == null)
            {
                MessageBox.Show("Виберіть один з елементів!", "Повідомлення", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Завершаємо метод, щоб форма не відкрилась
            }


            // Отримуємо вибраний елемент із ComboBox
            string selectedForm = comboBox1.SelectedItem.ToString(); 
            
            
            this.Hide();

            
            switch (selectedForm)
            {
                case "Користувачі":
                    Користувачі користувачіForm = new Користувачі();
                    користувачіForm.Username = this.Username; 
                    користувачіForm.Show();
                    break;
                case "Товар":
                    Товар товарForm = new Товар();
                    товарForm.Username = this.Username;
                    товарForm.Show();
                    break;
                case "Магазини":
                    Магазини магазиниForm = new Магазини();
                    магазиниForm.Username = this.Username;
                    магазиниForm.Show();
                    break;
                case "Товар по порам року":
                    Товар_по_порам_року товарПоПорамРокуForm = new Товар_по_порам_року();
                    товарПоПорамРокуForm.Username = this.Username;
                    товарПоПорамРокуForm.Show();
                    break;
                case "Товар по категоріям":
                    Товар_по_категоріям товарПоКатегоріямForm = new Товар_по_категоріям();
                    товарПоКатегоріямForm.Username = this.Username;
                    товарПоКатегоріямForm.Show();
                    break;
                case "Матеріали":
                    Матеріали матеріалиForm = new Матеріали();
                    матеріалиForm.Username = this.Username;
                    матеріалиForm.Show();
                    break;
                case "Товарна група":
                    Товарна_група товарнаГрупаForm = new Товарна_група();
                    товарнаГрупаForm.Username = this.Username;
                    товарнаГрупаForm.Show();
                    break;
                case "Реалізація":
                    Реалізація реалізаціяForm = new Реалізація();
                    реалізаціяForm.Username = this.Username;
                    реалізаціяForm.Show();
                    break;
                case "Зміст реалізації":
                    Зміст_реалізації змістРеалізаціїForm = new Зміст_реалізації();
                    змістРеалізаціїForm.Username = this.Username;
                    змістРеалізаціїForm.Show();
                    break;              
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {          
            Analitics analitics = new Analitics();
            analitics.Username = this.Username;
            analitics.Show();
            this.Hide();
        }

        private bool isClosing = false;

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form loginForm = Application.OpenForms["LoginForm"];
            if (loginForm != null)
            {
                loginForm.Close();
            }
        }
    }
}
