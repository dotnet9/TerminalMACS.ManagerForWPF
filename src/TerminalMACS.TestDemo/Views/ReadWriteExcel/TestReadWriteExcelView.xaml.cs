using System;
using System.IO;
using System.Windows;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using TerminalMACS.Infrastructure;

namespace TerminalMACS.TestDemo.Views.ReadWriteExcel;

/// <summary>
///     TestReadWriteExcelView.xaml 的交互逻辑
///     https://www.cnblogs.com/zqyw/p/7462561.html
/// </summary>
public partial class TestReadWriteExcelView : Window
{
    public TestReadWriteExcelView()
    {
        InitializeComponent();
    }

    private void OpenWriteExcel_Click(object sender, RoutedEventArgs e)
    {
        var filePath = $"{AppDomain.CurrentDomain.BaseDirectory}{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xls";

        var wb = new HSSFWorkbook();

        var sheet = wb.CreateSheet("first page");

        var currentRow = 0;

        var cellForeground = wb.CreateCellStyle();
        cellForeground.FillForegroundColor = GetColourByRGB(wb, 82, 238, 172);
        cellForeground.BottomBorderColor = GetColourByRGB(wb, 222, 0, 0);
        cellForeground.FillPattern = FillPattern.SolidForeground;

        var row = sheet.CreateRow(currentRow++);
        row.CreateCell(0).SetCellValue("row 1");
        row.GetCell(0).CellStyle = cellForeground;


        var cellBackground = wb.CreateCellStyle();
        cellBackground.FillPattern = FillPattern.SolidForeground;
        cellBackground.FillBackgroundColor = GetColourByRGB(wb, 98, 33, 239);
        row = sheet.CreateRow(currentRow++);
        row.CreateCell(0).SetCellValue("row 2");
        row.GetCell(0).CellStyle = cellBackground;


        var fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);

        wb.Write(fs);
        fs.Close();

        FileHelper.OpenFolderAndSelectFile(filePath);
    }

    private short GetColourByRGB(HSSFWorkbook workbook, byte r, byte g, byte b)
    {
        var palette = workbook.GetCustomPalette();
        var hssfColor = palette.FindColor(r, g, b);
        if (hssfColor == null)
        {
            palette.SetColorAtIndex(HSSFColor.Lavender.Index, r, g, b);
            hssfColor = palette.GetColor(HSSFColor.Lavender.Index);
        }

        if (hssfColor != null)
            return hssfColor.Indexed;
        return short.MinValue;
    }
}