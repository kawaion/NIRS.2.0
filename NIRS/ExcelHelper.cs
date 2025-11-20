using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using System.Drawing;

namespace NIRS
{
    public static class ExcelHelper
    {
        private static readonly string TargetDirectory = @"F:\NIRS_";

        // Создание файла с несколькими страницами
        public static void CreateExcelFileWithSheets(Dictionary<string, double[,]> dataSheets, string fileName)
        {
            string fullPath = GetFullPath(fileName);

            using (var workbook = new XLWorkbook())
            {
                foreach (var sheetData in dataSheets)
                {
                    AddOrUpdateWorksheet(workbook, sheetData.Key, sheetData.Value, isNew: true);
                }

                workbook.SaveAs(fullPath);
            }
        }

        // Обновление файла с несколькими страницами
        public static void UpdateExcelFileWithSheets(Dictionary<string, double[,]> dataSheets, string fileName, bool compareMode = true)
        {
            string fullPath = GetFullPath(fileName);

            if (!File.Exists(fullPath))
            {
                CreateExcelFileWithSheets(dataSheets, fileName);
                return;
            }

            using (var workbook = new XLWorkbook(fullPath))
            {
                foreach (var sheetData in dataSheets)
                {
                    AddOrUpdateWorksheet(workbook, sheetData.Key, sheetData.Value, isNew: false, compareMode: compareMode);
                }

                workbook.Save();
            }
        }

        // Получение полного пути
        private static string GetFullPath(string fileName)
        {
            return Path.IsPathRooted(fileName) ? fileName : Path.Combine(TargetDirectory, fileName);
        }

        // Универсальный метод для добавления/обновления листа
        private static void AddOrUpdateWorksheet(XLWorkbook workbook, string sheetName, double[,] array, bool isNew, bool compareMode = false)
        {
            string validSheetName = GetValidSheetName(sheetName);
            var worksheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == validSheetName);

            if (worksheet == null)
            {
                worksheet = workbook.Worksheets.Add(validSheetName);
                FillWorksheetWithData(worksheet, array);
            }
            else if (compareMode && !isNew)
            {
                UpdateWorksheetWithComparison(worksheet, array);
            }
            else
            {
                ClearAndFillWorksheet(worksheet, array);
            }
        }

        // Оптимизированное заполнение нового листа данными
        private static void FillWorksheetWithData(IXLWorksheet worksheet, double[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            // Батч-обработка заголовков столбцов
            if (cols > 0)
            {
                var columnRange = worksheet.Range(1, 2, 1, cols + 1);
                columnRange.Style.Font.Bold = true;
                columnRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                for (int col = 0; col < cols; col++)
                {
                    worksheet.Cell(1, col + 2).Value = col;
                }
            }

            // Батч-обработка заголовков строк
            if (rows > 0)
            {
                var rowRange = worksheet.Range(2, 1, rows + 1, 1);
                rowRange.Style.Font.Bold = true;
                rowRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                for (int row = 0; row < rows; row++)
                {
                    worksheet.Cell(row + 2, 1).Value = row;
                }
            }

            // Оптимизированное заполнение данных
            FillDataRange(worksheet, array, 2, 2);
            worksheet.Columns().AdjustToContents();
        }

        // Оптимизированное заполнение данных
        private static void FillDataRange(IXLWorksheet worksheet, double[,] array, int startRow, int startCol)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            if (rows == 0 || cols == 0) return;

            // Батч-стилизация
            var dataRange = worksheet.Range(startRow, startCol, startRow + rows - 1, startCol + cols - 1);
            dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            // Прямое заполнение значений
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    worksheet.Cell(startRow + row, startCol + col).Value = array[row, col];
                }
            }
        }

        // Очистка и заполнение листа
        private static void ClearAndFillWorksheet(IXLWorksheet worksheet, double[,] array)
        {
            worksheet.Clear();
            FillWorksheetWithData(worksheet, array);
        }

        // Оптимизированное обновление с сравнением
        private static void UpdateWorksheetWithComparison(IXLWorksheet worksheet, double[,] newArray)
        {
            // Определяем размеры существующих данных по фактическому содержимому
            var lastCell = worksheet.LastCellUsed();
            if (lastCell == null)
            {
                FillWorksheetWithData(worksheet, newArray);
                return;
            }

            int oldRows = GetLastUsedRow(worksheet) - 1; // -1 для учета заголовка
            int oldCols = GetLastUsedColumn(worksheet) - 1; // -1 для учета заголовка

            oldRows = Math.Max(0, oldRows);
            oldCols = Math.Max(0, oldCols);

            int newRows = newArray.GetLength(0);
            int newCols = newArray.GetLength(1);

            int minRows = Math.Min(oldRows, newRows);
            int minCols = Math.Min(oldCols, newCols);

            // Обновляем общую область
            int matches = UpdateCommonArea(worksheet, newArray, minRows, minCols);

            // Добавляем новые строки и столбцы
            AddNewRows(worksheet, newArray, oldRows, newRows, newCols);
            AddNewColumns(worksheet, newArray, oldCols, newCols, newRows, oldRows);

            worksheet.Columns().AdjustToContents();
        }

        // Обновление общей области с сравнением
        private static int UpdateCommonArea(IXLWorksheet worksheet, double[,] newArray, int minRows, int minCols)
        {
            int matches = 0;
            const double tolerance = 0.000001;

            for (int row = 0; row < minRows; row++)
            {
                for (int col = 0; col < minCols; col++)
                {
                    int excelRow = row + 2;
                    int excelCol = col + 2;

                    var cell = worksheet.Cell(excelRow, excelCol);
                    double oldValue = cell.Value.GetNumber();
                    double newValue = newArray[row, col];

                    cell.Value = $"{oldValue:F6}/{newValue:F6}";

                    if (Math.Abs(oldValue - newValue) < tolerance)
                    {
                        cell.Style.Fill.BackgroundColor = XLColor.LightGreen;
                        matches++;
                    }
                    else
                    {
                        cell.Style.Fill.BackgroundColor = XLColor.LightCoral;
                    }

                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
            }

            return matches;
        }

        // Добавление новых строк
        private static void AddNewRows(IXLWorksheet worksheet, double[,] newArray, int oldRows, int newRows, int newCols)
        {
            if (newRows <= oldRows) return;

            for (int row = oldRows; row < newRows; row++)
            {
                // Заголовок строки
                worksheet.Cell(row + 2, 1).Value = row;
                worksheet.Cell(row + 2, 1).Style.Font.Bold = true;
                worksheet.Cell(row + 2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Данные новой строки
                for (int col = 0; col < newCols; col++)
                {
                    int excelRow = row + 2;
                    int excelCol = col + 2;

                    worksheet.Cell(excelRow, excelCol).Value = $"–/{newArray[row, col]:F6}";
                    worksheet.Cell(excelRow, excelCol).Style.Fill.BackgroundColor = XLColor.LightYellow;
                    worksheet.Cell(excelRow, excelCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
            }
        }

        // Добавление новых столбцов
        private static void AddNewColumns(IXLWorksheet worksheet, double[,] newArray, int oldCols, int newCols, int newRows, int oldRows)
        {
            if (newCols <= oldCols) return;

            for (int col = oldCols; col < newCols; col++)
            {
                // Заголовок столбца
                worksheet.Cell(1, col + 2).Value = col;
                worksheet.Cell(1, col + 2).Style.Font.Bold = true;
                worksheet.Cell(1, col + 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Данные нового столбца
                for (int row = 0; row < newRows; row++)
                {
                    int excelRow = row + 2;
                    int excelCol = col + 2;

                    worksheet.Cell(excelRow, excelCol).Value = $"–/{newArray[row, col]:F6}";
                    worksheet.Cell(excelRow, excelCol).Style.Fill.BackgroundColor = XLColor.LightYellow;
                    worksheet.Cell(excelRow, excelCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
            }
        }

        // Вспомогательные методы для определения границ данных
        private static int GetLastUsedRow(IXLWorksheet worksheet)
        {
            return worksheet.LastRowUsed()?.RowNumber() ?? 0;
        }

        private static int GetLastUsedColumn(IXLWorksheet worksheet)
        {
            return worksheet.LastColumnUsed()?.ColumnNumber() ?? 0;
        }

        // Валидация имени листа
        private static string GetValidSheetName(string originalName)
        {
            if (string.IsNullOrEmpty(originalName))
                return "Sheet1";

            string validName = originalName
                .Replace(':', '_')
                .Replace('\\', '_')
                .Replace('/', '_')
                .Replace('?', '_')
                .Replace('*', '_')
                .Replace('[', '_')
                .Replace(']', '_');

            return validName.Length > 31 ? validName.Substring(0, 31) : validName;
        }
    }
}
