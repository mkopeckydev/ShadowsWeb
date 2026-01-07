using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace Shadows.Data.Excel
{
    public class NamedValuesManager
    {
        public List<NamedValue> ReadNamedValues(string prefix, string filePath)
        {
            List<NamedValue> l = new List<NamedValue>();

            using (FileStream fs = File.OpenRead(filePath))
            {
                try
                {
                    using (ExcelPackage excel = new ExcelPackage(fs))
                    {
                        foreach (ExcelNamedRange r in excel.Workbook.Names)
                        {
                            if (r.Name.StartsWith(prefix))
                            {
                                try
                                {
                                    NamedValue d = new NamedValue();
                                    d.Name = r.Name;
                                    d.SheetName = r.Worksheet.Name;
                                    d.StartCellColumn = r.Start.Column;
                                    d.StartCellRow = r.Start.Row;
                                    d.EndCellColumn = r.End.Column;
                                    d.EndCellRow = r.End.Row;

                                    ExcelWorksheet ws = excel.Workbook.Worksheets[d.SheetName];

                                    for (int row = d.StartCellRow; row <= d.EndCellRow; row++)
                                    {
                                        for (int col = d.StartCellColumn; col <= d.EndCellColumn; col++)
                                        {
                                            object v = ws.Cells[row, col].Value;
                                            d.Data.Add(v);
                                        }
                                    }

                                    l.Add(d);
                                }
                                catch
                                { }
                            }
                        }
                    }
                }
                catch
                { }
            }
            return l;
        }

        public MemoryStream CopyNamedValues(string prefix, string filePath, List<NamedValue> loadedVaues)
        {
            try
            {
                List<NamedValue> namedValues = ReadNamedValues(prefix, filePath);
                MemoryStream ms = new MemoryStream();

                using (FileStream fs = File.OpenRead(filePath))
                {
                    using (ExcelPackage excel = new ExcelPackage(fs))
                    {
                        foreach (NamedValue nv in namedValues)
                        {
                            NamedValue loadedValue = loadedVaues.Find(x => x.Name == nv.Name);
                            if (loadedValue != null)
                            {
                                ExcelWorksheet ws = excel.Workbook.Worksheets[nv.SheetName];

                                int valIndex = 0;
                                for (int row = nv.StartCellRow; row <= nv.EndCellRow; row++)
                                {
                                    for (int col = nv.StartCellColumn; col <= nv.EndCellColumn; col++)
                                    {
                                        if (loadedValue.Data.Count > valIndex)
                                        {
                                            ws.Cells[row, col].Value = loadedValue.Data[valIndex];
                                        }
                                        valIndex++;
                                    }
                                }
                            }
                        }

                        excel.SaveAs(ms);
                        excel.Save();

                        excel.Dispose();
                    }
                }
                return ms;
            }
            catch (Exception ex)
            {
                throw new Exception("Chyba:" + ex.Message);
            }
        }
    }
}
