using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ВКР
{
    public static class LocalDBInstaller
    {
        public static bool EnsureLocalDBInstalled()
        {
            if (!IsLocalDBInstalled())
            {
                return InstallLocalDB();
            }
            return true;
        }

        private static bool IsLocalDBInstalled()
        {
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions\15.0"))
                {
                    return key != null;
                }
            }
            catch
            {
                return false;
            }
        }

        private static bool InstallLocalDB()
        {
            string installerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "SqlLocalDB.MSI");

            if (File.Exists(installerPath))
            {
                try
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = installerPath,
                        Verb = "open",
                        UseShellExecute = true
                    };

                    Process installProcess = Process.Start(psi);
                    if (installProcess == null)
                    {
                        return false;
                    }

                    installProcess.WaitForExit();
                    return IsLocalDBInstalled();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при інсталяції LocalDB: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Не знайдено інсталяційний файл SqlLocalDB.MSI у директорії Resources.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}