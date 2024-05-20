using System;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;

public class ExcelHelper
{
    public static void SaveToExcelOnDesktop(DataGridView[] dataGridViewArray, string[] sheetNames, string fileName)
    {
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string filePath = Path.Combine(desktopPath, fileName);

        
        if (File.Exists(filePath))
        {
            DialogResult result = MessageBox.Show("Файл Excel з такою назвою вже існує. Замінити його?", "Підтвердження заміни", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return; 
            }
        }

        using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
        {
            for (int i = 0; i < dataGridViewArray.Length; i++)
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetNames[i]);

                for (int col = 0; col < dataGridViewArray[i].Columns.Count; col++)
                {
                    worksheet.Cells[1, col + 1].Value = dataGridViewArray[i].Columns[col].HeaderText;
                }

                for (int row = 0; row < dataGridViewArray[i].Rows.Count; row++)
                {
                    for (int col = 0; col < dataGridViewArray[i].Columns.Count; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = dataGridViewArray[i].Rows[row].Cells[col].Value;
                    }
                }
            }

            package.Save();
        }
    }
}