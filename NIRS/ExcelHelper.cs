using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using ClosedXML.Excel;
using System.Drawing;
using System.Globalization;

namespace NIRS
{
    public static class ExcelHelper
    {
        private static readonly string TargetDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

        // Создание файла с несколькими страницами с обрезкой первых n строк
        public static void CreateExcelFileWithSheets(Dictionary<string, double[,]> dataSheets, string fileName, int skipRows = 0)
        {
            string fullPath = GetFullPath(fileName);

            using (var workbook = new XLWorkbook())
            {
                foreach (var sheetData in dataSheets)
                {
                    AddOrUpdateWorksheet(workbook, sheetData.Key, sheetData.Value, isNew: true, skipRows: skipRows);
                }

                workbook.SaveAs(fullPath);
            }
        }

        // Обновление файла с несколькими страницами с обрезкой первых n строк
        public static void UpdateExcelFileWithSheets(Dictionary<string, double[,]> dataSheets, string fileName, bool compareMode = true, int skipRows = 0)
        {
            string fullPath = GetFullPath(fileName);

            if (!File.Exists(fullPath))
            {
                CreateExcelFileWithSheets(dataSheets, fileName, skipRows);
                return;
            }

            using (var workbook = new XLWorkbook(fullPath))
            {
                foreach (var sheetData in dataSheets)
                {
                    AddOrUpdateWorksheet(workbook, sheetData.Key, sheetData.Value, isNew: false, compareMode: compareMode, skipRows: skipRows);
                }

                workbook.Save();
            }
        }

        // Получение полного пути
        private static string GetFullPath(string fileName)
        {
            return Path.IsPathRooted(fileName) ? fileName : Path.Combine(TargetDirectory, fileName);
        }

        // Универсальный метод для добавления/обновления листа с обрезкой
        private static void AddOrUpdateWorksheet(XLWorkbook workbook, string sheetName, double[,] array, bool isNew, bool compareMode = false, int skipRows = 0)
        {
            string validSheetName = GetValidSheetName(sheetName);
            var worksheet = workbook.Worksheets.FirstOrDefault(ws => ws.Name == validSheetName);

            // Получаем обрезанный массив
            double[,] trimmedArray = TrimRows(array, skipRows);

            if (worksheet == null)
            {
                worksheet = workbook.Worksheets.Add(validSheetName);
                FillWorksheetWithData(worksheet, trimmedArray, skipRows);
            }
            else if (compareMode && !isNew)
            {
                UpdateWorksheetWithComparison(worksheet, trimmedArray, skipRows);
            }
            else
            {
                ClearAndFillWorksheet(worksheet, trimmedArray, skipRows);
            }
        }

        // Обрезка первых n строк массива
        private static double[,] TrimRows(double[,] array, int skipRows)
        {
            if (skipRows <= 0 || skipRows >= array.GetLength(0))
            {
                return array;
            }

            int originalRows = array.GetLength(0);
            int cols = array.GetLength(1);
            int newRows = originalRows - skipRows;

            double[,] trimmedArray = new double[newRows, cols];

            for (int row = 0; row < newRows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    trimmedArray[row, col] = array[row + skipRows, col];
                }
            }

            return trimmedArray;
        }

        // Оптимизированное заполнение нового листа данными с учетом пропущенных строк
        private static void FillWorksheetWithData(IXLWorksheet worksheet, double[,] array, int skipRows)
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

            // Батч-обработка заголовков строк с учетом пропущенных строк
            if (rows > 0)
            {
                var rowRange = worksheet.Range(2, 1, rows + 1, 1);
                rowRange.Style.Font.Bold = true;
                rowRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                for (int row = 0; row < rows; row++)
                {
                    worksheet.Cell(row + 2, 1).Value = row + skipRows; // Учитываем пропущенные строки
                }
            }

            // Оптимизированное заполнение данных
            FillDataRange(worksheet, array, 2, 2);
            worksheet.Columns().AdjustToContents();
        }

        // Перегрузка для обратной совместимости
        private static void FillWorksheetWithData(IXLWorksheet worksheet, double[,] array)
        {
            FillWorksheetWithData(worksheet, array, 0);
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
            dataRange.Style.NumberFormat.Format = "@";

            // Прямое заполнение значений
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    worksheet.Cell(startRow + row, startCol + col).Value = array[row, col];
                }
            }
        }

        // Очистка и заполнение листа с учетом пропущенных строк
        private static void ClearAndFillWorksheet(IXLWorksheet worksheet, double[,] array, int skipRows)
        {
            worksheet.Clear();
            FillWorksheetWithData(worksheet, array, skipRows);
        }

        // Перегрузка для обратной совместимости
        private static void ClearAndFillWorksheet(IXLWorksheet worksheet, double[,] array)
        {
            ClearAndFillWorksheet(worksheet, array, 0);
        }

        // Оптимизированное обновление с сравнением и учетом пропущенных строк
        private static void UpdateWorksheetWithComparison(IXLWorksheet worksheet, double[,] newArray, int skipRows)
        {
            // Определяем размеры существующих данных по фактическому содержимому
            var lastCell = worksheet.LastCellUsed();
            if (lastCell == null)
            {
                FillWorksheetWithData(worksheet, newArray, skipRows);
                return;
            }

            int oldRows = GetLastUsedRow(worksheet) - 1;
            int oldCols = GetLastUsedColumn(worksheet) - 1;

            oldRows = Math.Max(0, oldRows);
            oldCols = Math.Max(0, oldCols);

            int newRows = newArray.GetLength(0);
            int newCols = newArray.GetLength(1);

            int minRows = Math.Min(oldRows, newRows);
            int minCols = Math.Min(oldCols, newCols);

            // Обновляем общую область
            int matches = UpdateCommonArea(worksheet, newArray, minRows, minCols, skipRows);

            // Добавляем новые строки и столбцы с учетом пропущенных строк
            AddNewRows(worksheet, newArray, oldRows, newRows, newCols, skipRows);
            AddNewColumns(worksheet, newArray, oldCols, newCols, newRows, oldRows, skipRows);

            worksheet.Columns().AdjustToContents();
        }

        // Перегрузка для обратной совместимости
        private static void UpdateWorksheetWithComparison(IXLWorksheet worksheet, double[,] newArray)
        {
            UpdateWorksheetWithComparison(worksheet, newArray, 0);
        }

        // Обновление общей области с сравнением и учетом пропущенных строк
        private static int UpdateCommonArea(IXLWorksheet worksheet, double[,] newArray, int minRows, int minCols, int skipRows)
        {
            int matches = 0;
            const double tolerance = 0.0000000001;

            for (int row = 0; row < minRows; row++)
            {
                for (int col = 0; col < minCols; col++)
                {
                    int excelRow = row + 2;
                    int excelCol = col + 2;

                    var cell = worksheet.Cell(excelRow, excelCol);
                    double oldValue = cell.Value.GetNumber();
                    double newValue = newArray[row, col];

                    cell.Value = $"{oldValue}/{newValue}";

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

        // Добавление новых строк с учетом пропущенных строк
        private static void AddNewRows(IXLWorksheet worksheet, double[,] newArray, int oldRows, int newRows, int newCols, int skipRows)
        {
            if (newRows <= oldRows) return;

            for (int row = oldRows; row < newRows; row++)
            {
                // Заголовок строки с учетом пропущенных строк
                worksheet.Cell(row + 2, 1).Value = row + skipRows;
                worksheet.Cell(row + 2, 1).Style.Font.Bold = true;
                worksheet.Cell(row + 2, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Данные новой строки
                for (int col = 0; col < newCols; col++)
                {
                    int excelRow = row + 2;
                    int excelCol = col + 2;

                    worksheet.Cell(excelRow, excelCol).Value = $"–/{newArray[row, col]}";
                    worksheet.Cell(excelRow, excelCol).Style.Fill.BackgroundColor = XLColor.LightYellow;
                    worksheet.Cell(excelRow, excelCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
            }
        }

        // Перегрузка для обратной совместимости
        private static void AddNewRows(IXLWorksheet worksheet, double[,] newArray, int oldRows, int newRows, int newCols)
        {
            AddNewRows(worksheet, newArray, oldRows, newRows, newCols, 0);
        }

        // Добавление новых столбцов с учетом пропущенных строк
        private static void AddNewColumns(IXLWorksheet worksheet, double[,] newArray, int oldCols, int newCols, int newRows, int oldRows, int skipRows)
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

                    worksheet.Cell(excelRow, excelCol).Value = $"–/{newArray[row, col]}";
                    worksheet.Cell(excelRow, excelCol).Style.Fill.BackgroundColor = XLColor.LightYellow;
                    worksheet.Cell(excelRow, excelCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }
            }
        }

        // Перегрузка для обратной совместимости
        private static void AddNewColumns(IXLWorksheet worksheet, double[,] newArray, int oldCols, int newCols, int newRows, int oldRows)
        {
            AddNewColumns(worksheet, newArray, oldCols, newCols, newRows, oldRows, 0);
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
