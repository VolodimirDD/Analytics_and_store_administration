using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ВКР
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoginForm loginForm = new LoginForm();
            loginForm.Shown += (sender, e) => CheckLocalDBInstallation(loginForm);

            Application.Run(loginForm);         
        }

        private static void CheckLocalDBInstallation(LoginForm loginForm)
        {
            Task.Run(() =>
            {
                bool isLocalDBInstalled = LocalDBInstaller.EnsureLocalDBInstalled();
                loginForm.Invoke(new Action(() =>
                {
                    if (!isLocalDBInstalled)
                    {
                        MessageBox.Show("Для початку використання програми потрібно встановити файли LocalDB.", "Попередження", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        loginForm.Close();
                    }
                }));
            });
        }
    }
}
