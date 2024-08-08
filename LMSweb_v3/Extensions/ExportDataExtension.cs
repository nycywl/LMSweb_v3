using ClosedXML.Excel;
using System.Text;

namespace LMSweb_v3.Extensions;

public static class ExportDataExtension
{
   public static XLWorkbook ToExcel<T>(this ICollection<T> collection, List<string> headers)
    {
        var workbook = new XLWorkbook();
        workbook.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        workbook.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
        IXLWorksheet worksheet = workbook.Worksheets.Add("User QA Logs");
        for (int i = 0; i < headers.Count; i++)
        {
            worksheet.Cell(1, i + 1).Value = headers[i];
        }

        // Add data to the Excel file
        for (int i = 0; i < collection.Count; i++)
        {
            var item = collection.ElementAt(i);
            var properties = item?.GetType().GetProperties();
            for (int j = 0; j < properties.Length; j++)
            {
                worksheet.Cell(i + 2, j + 1).Value = properties[j].GetValue(item)?.ToString();
            }
        }
        return workbook;
    }

    public static string ToCSV<T>(this ICollection<T> collection, List<string> headers)
    {
        var csv = new StringBuilder();
        csv.AppendLine(string.Join(",", headers));
        foreach (var item in collection)
        {
            var properties = item?.GetType().GetProperties();
            var line = new List<string>();
            foreach (var property in properties)
            {
                line.Add(property.GetValue(item)?.ToString());
            }
            csv.AppendLine(string.Join(",", line));
        }
        return csv.ToString();
    }
}
