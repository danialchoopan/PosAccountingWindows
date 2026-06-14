using System.Data;
using System.IO;
using System.Windows;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Xlsx = DocumentFormat.OpenXml;
using XlsxSp = DocumentFormat.OpenXml.Spreadsheet;
using XlsxPk = DocumentFormat.OpenXml.Packaging;

namespace PosAccountingApp.Data;

public static class ExportHelper
{
    public static void ExportToExcel(DataTable table, string title)
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            Filter = "Excel|*.xlsx",
            FileName = $"{title}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
        };
        if (dialog.ShowDialog() != true) return;

        using var doc = XlsxPk.SpreadsheetDocument.Create(dialog.FileName, Xlsx.SpreadsheetDocumentType.Workbook);
        var wb = doc.AddWorkbookPart();
        wb.Workbook = new XlsxSp.Workbook();
        var wsPart = wb.AddNewPart<XlsxPk.WorksheetPart>();
        wb.Workbook.Append(new XlsxSp.Sheet { Name = title, Id = wb.GetIdOfPart(wsPart) });

        var sstPart = wb.AddNewPart<XlsxPk.SharedStringTablePart>();
        sstPart.SharedStringTable = new XlsxSp.SharedStringTable();

        var rows = new List<XlsxSp.Row>();

        // Header
        var headerRow = new XlsxSp.Row { RowIndex = 1 };
        for (int i = 0; i < table.Columns.Count; i++)
        {
            var text = table.Columns[i].ColumnName;
            sstPart.SharedStringTable.AppendChild(new XlsxSp.SharedStringItem(new XlsxSp.Text(text)));
            var cell = new XlsxSp.Cell
            {
                CellReference = $"{(char)('A' + i)}1",
                DataType = XlsxSp.CellValues.SharedString
            };
            cell.AppendChild(new XlsxSp.CellValue((sstPart.SharedStringTable.Elements<XlsxSp.SharedStringItem>().Count() - 1).ToString()));
            headerRow.Append(cell);
        }
        rows.Add(headerRow);

        // Data
        for (int r = 0; r < table.Rows.Count; r++)
        {
            var row = new XlsxSp.Row { RowIndex = (uint)(r + 2) };
            for (int c = 0; c < table.Columns.Count; c++)
            {
                var text = table.Rows[r][c]?.ToString() ?? "";
                sstPart.SharedStringTable.AppendChild(new XlsxSp.SharedStringItem(new XlsxSp.Text(text)));
                var cell = new XlsxSp.Cell
                {
                    CellReference = $"{(char)('A' + c)}{r + 2}",
                    DataType = XlsxSp.CellValues.SharedString
                };
                cell.AppendChild(new XlsxSp.CellValue((sstPart.SharedStringTable.Elements<XlsxSp.SharedStringItem>().Count() - 1).ToString()));
                row.Append(cell);
            }
            rows.Add(row);
        }

        var worksheet = new XlsxSp.Worksheet();
        worksheet.AppendChild(new XlsxSp.SheetData(rows));
        wsPart.Worksheet = worksheet;
        wsPart.Worksheet.Save();
        wb.Workbook.Save();

        MessageBox.Show($"فایل اکسل ذخیره شد:\n{dialog.FileName}", "خروجی", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public static void ExportToPdf(DataTable table, string title)
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            Filter = "PDF|*.pdf",
            FileName = $"{title}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
        };
        if (dialog.ShowDialog() != true) return;

        var doc = new Document(PageSize.A4.Rotate(), 20, 20, 30, 30);
        PdfWriter.GetInstance(doc, new FileStream(dialog.FileName, FileMode.Create));
        doc.Open();

        var bf = GetBaseFont();
        var titleFont = new iTextSharp.text.Font(bf, 18, Font.BOLD);
        var headerFont = new iTextSharp.text.Font(bf, 10, Font.BOLD, BaseColor.White);
        var cellFont = new iTextSharp.text.Font(bf, 9, Font.NORMAL);
        var headerBg = new BaseColor(0, 120, 212);

        var titlePara = new Paragraph(title, titleFont) { Alignment = Element.ALIGN_RIGHT };
        doc.Add(titlePara);
        doc.Add(new Paragraph($"تاریخ چاپ: {DateTime.Now:yyyy/MM/dd HH:mm}", new iTextSharp.text.Font(bf, 9)) { Alignment = Element.ALIGN_RIGHT });
        doc.Add(new Paragraph(" "));

        var pdfTable = new PdfPTable(table.Columns.Count)
        {
            RunDirection = PdfWriter.RUN_DIRECTION_RTL,
            WidthPercentage = 100
        };

        foreach (DataColumn col in table.Columns)
        {
            pdfTable.AddCell(new PdfPCell(new Phrase(col.ColumnName, headerFont))
            { BackgroundColor = headerBg, HorizontalAlignment = Element.ALIGN_CENTER, Padding = 6 });
        }

        foreach (DataRow row in table.Rows)
        {
            foreach (var item in row.ItemArray)
            {
                pdfTable.AddCell(new PdfPCell(new Phrase(item?.ToString() ?? "", cellFont))
                { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 5 });
            }
        }

        doc.Add(pdfTable);
        doc.Close();

        MessageBox.Show($"فایل PDF ذخیره شد:\n{dialog.FileName}", "خروجی", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public static void PrintTable(DataTable table, string title)
    {
        var doc = new Document(PageSize.A4.Rotate(), 20, 20, 30, 30);
        var stream = new MemoryStream();
        PdfWriter.GetInstance(doc, stream);
        doc.Open();

        var bf = GetBaseFont();
        var titleFont = new iTextSharp.text.Font(bf, 16, Font.BOLD);
        var headerFont = new iTextSharp.text.Font(bf, 9, Font.BOLD, BaseColor.White);
        var cellFont = new iTextSharp.text.Font(bf, 8, Font.NORMAL);
        var headerBg = new BaseColor(0, 120, 212);

        doc.Add(new Paragraph(title, titleFont) { Alignment = Element.ALIGN_RIGHT });
        doc.Add(new Paragraph($"تاریخ: {DateTime.Now:yyyy/MM/dd HH:mm}", new iTextSharp.text.Font(bf, 8)) { Alignment = Element.ALIGN_RIGHT });
        doc.Add(new Paragraph(" "));

        var pdfTable = new PdfPTable(table.Columns.Count)
        {
            RunDirection = PdfWriter.RUN_DIRECTION_RTL,
            WidthPercentage = 100
        };

        foreach (DataColumn col in table.Columns)
            pdfTable.AddCell(new PdfPCell(new Phrase(col.ColumnName, headerFont))
            { BackgroundColor = headerBg, HorizontalAlignment = Element.ALIGN_CENTER, Padding = 5 });

        foreach (DataRow row in table.Rows)
            foreach (var item in row.ItemArray)
                pdfTable.AddCell(new PdfPCell(new Phrase(item?.ToString() ?? "", cellFont))
                { HorizontalAlignment = Element.ALIGN_RIGHT, Padding = 4 });

        doc.Add(pdfTable);
        doc.Close();

        var tempPath = Path.Combine(Path.GetTempPath(), $"print_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
        File.WriteAllBytes(tempPath, stream.ToArray());
        stream.Close();
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(tempPath) { UseShellExecute = true });
    }

    private static BaseFont GetBaseFont()
    {
        var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "tahoma.ttf");
        if (File.Exists(fontPath))
            return BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        return BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
    }
}
