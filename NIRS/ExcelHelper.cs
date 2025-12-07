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

            if (worksheet == null)
            {
                worksheet = workbook.Worksheets.Add(validSheetName);
                FillWorksheetWithData(worksheet, array, skipRows);
            }
            else if (compareMode && !isNew)
            {
                UpdateWorksheetWithComparison(worksheet, array, skipRows);
            }
            else
            {
                ClearAndFillWorksheet(worksheet, array, skipRows);
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

        // Преобразование double в строку для записи в Excel
        private static string DoubleToString(double value)
        {
            // Проверка специальных значений
            if (double.IsNaN(value))
                return "NaN";
            if (double.IsPositiveInfinity(value))
                return "∞";
            if (double.IsNegativeInfinity(value))
                return "-∞";

            //// Для целых чисел убираем десятичную часть
            //if (Math.Abs(value - Math.Round(value)) < 1e-10)
            //{
            //    // Для больших чисел используем научную нотацию
            //    if (Math.Abs(value) > 1e10 || (Math.Abs(value) < 1e-10 && value != 0))
            //        return value.ToString("G6");

            //    return ((long)Math.Round(value)).ToString();
            //}

            // Для обычных чисел используем фиксированный формат
            // с достаточной точностью для сравнения
            return value.ToString("R", System.Globalization.CultureInfo.InvariantCulture);
        }

        // Оптимизированное заполнение нового листа данными с учетом пропущенных строк
        private static void FillWorksheetWithData(IXLWorksheet worksheet, double[,] array, int skipRows)
        {
            // Обрезаем массив если нужно
            double[,] dataArray = TrimRows(array, skipRows);

            int rows = dataArray.GetLength(0);
            int cols = dataArray.GetLength(1);

            // Батч-обработка заголовков столбцов
            if (cols > 0)
            {
                var columnRange = worksheet.Range(1, 2, 1, cols + 1);
                columnRange.Style.Font.Bold = true;
                columnRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                for (int col = 0; col < cols; col++)
                {
                    worksheet.Cell(1, col + 2).Value = $"Column {col}";
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

            // Оптимизированное заполнение данных (записываем как текст)
            FillDataRange(worksheet, dataArray, 2, 2);
            worksheet.Columns().AdjustToContents();
        }

        // Оптимизированное заполнение данных (записываем double как текст)
        private static void FillDataRange(IXLWorksheet worksheet, double[,] array, int startRow, int startCol)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            if (rows == 0 || cols == 0) return;

            // Батч-стилизация - для текста используем текстовый формат
            var dataRange = worksheet.Range(startRow, startCol, startRow + rows - 1, startCol + cols - 1);
            dataRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            dataRange.Style.NumberFormat.Format = "@"; // Текстовый формат

            // Заполняем значения, преобразуя double в строку
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    string stringValue = DoubleToString(array[row, col]);
                    worksheet.Cell(startRow + row, startCol + col).Value = stringValue;
                }
            }
        }

        // Очистка и заполнение листа с учетом пропущенных строк
        private static void ClearAndFillWorksheet(IXLWorksheet worksheet, double[,] array, int skipRows)
        {
            worksheet.Clear();
            FillWorksheetWithData(worksheet, array, skipRows);
        }

        // Оптимизированное обновление с сравнением и учетом пропущенных строк
        private static void UpdateWorksheetWithComparison(IXLWorksheet worksheet, double[,] newArray, int skipRows)
        {
            // Обрезаем новый массив если нужно
            double[,] trimmedNewArray = TrimRows(newArray, skipRows);

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

            int newRows = trimmedNewArray.GetLength(0);
            int newCols = trimmedNewArray.GetLength(1);

            int minRows = Math.Min(oldRows, newRows);
            int minCols = Math.Min(oldCols, newCols);

            // Обновляем общую область (сравниваем строковые представления)
            int matches = UpdateCommonArea(worksheet, trimmedNewArray, minRows, minCols, skipRows);

            // Добавляем новые строки и столбцы с учетом пропущенных строк
            AddNewRows(worksheet, trimmedNewArray, oldRows, newRows, newCols, skipRows);
            AddNewColumns(worksheet, trimmedNewArray, oldCols, newCols, newRows, oldRows, skipRows);

            worksheet.Columns().AdjustToContents();
        }

        // Обновление общей области с сравнением строковых представлений
        private static int UpdateCommonArea(IXLWorksheet worksheet, double[,] newArray, int minRows, int minCols, int skipRows)
        {
            int matches = 0;

            for (int row = 0; row < minRows; row++)
            {
                for (int col = 0; col < minCols; col++)
                {
                    int excelRow = row + 2;
                    int excelCol = col + 2;

                    var cell = worksheet.Cell(excelRow, excelCol);
                    string oldValueString = cell.GetValue<string>();
                    string newValueString = DoubleToString(newArray[row, col]);

                    // Сравниваем строковые представления
                    bool areEqual = string.Equals(oldValueString, newValueString, StringComparison.Ordinal);

                    // Записываем оба значения через разделитель
                    cell.Value = $"{oldValueString}/{newValueString}";

                    if (areEqual)
                    {
                        cell.Style.Fill.BackgroundColor = XLColor.LightGreen;
                        matches++;
                    }
                    else
                    {
                        cell.Style.Fill.BackgroundColor = XLColor.LightCoral;
                    }

                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    cell.Style.NumberFormat.Format = "@"; // Текстовый формат
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

                    string newValueString = DoubleToString(newArray[row, col]);
                    worksheet.Cell(excelRow, excelCol).Value = $"→ {newValueString}";
                    worksheet.Cell(excelRow, excelCol).Style.Fill.BackgroundColor = XLColor.LightYellow;
                    worksheet.Cell(excelRow, excelCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell(excelRow, excelCol).Style.NumberFormat.Format = "@"; // Текстовый формат
                }
            }
        }

        // Добавление новых столбцов с учетом пропущенных строк
        private static void AddNewColumns(IXLWorksheet worksheet, double[,] newArray, int oldCols, int newCols, int newRows, int oldRows, int skipRows)
        {
            if (newCols <= oldCols) return;

            for (int col = oldCols; col < newCols; col++)
            {
                // Заголовок столбца
                worksheet.Cell(1, col + 2).Value = $"Column {col}";
                worksheet.Cell(1, col + 2).Style.Font.Bold = true;
                worksheet.Cell(1, col + 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Данные нового столбца
                for (int row = 0; row < newRows; row++)
                {
                    int excelRow = row + 2;
                    int excelCol = col + 2;

                    string newValueString = DoubleToString(newArray[row, col]);
                    worksheet.Cell(excelRow, excelCol).Value = $"→ {newValueString}";
                    worksheet.Cell(excelRow, excelCol).Style.Fill.BackgroundColor = XLColor.LightYellow;
                    worksheet.Cell(excelRow, excelCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell(excelRow, excelCol).Style.NumberFormat.Format = "@"; // Текстовый формат
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

        // Метод для чтения строковых значений из Excel (для другой программы)
        public static string[,] ReadStringDataFromExcel(string fileName, string sheetName, int skipRows = 0)
        {
            string fullPath = GetFullPath(fileName);

            if (!File.Exists(fullPath))
                return new string[0, 0];

            using (var workbook = new XLWorkbook(fullPath))
            {
                var worksheet = workbook.Worksheet(sheetName);
                if (worksheet == null)
                    return new string[0, 0];

                var lastCell = worksheet.LastCellUsed();
                if (lastCell == null)
                    return new string[0, 0];

                int totalRows = worksheet.LastRowUsed().RowNumber();
                int totalCols = worksheet.LastColumnUsed().ColumnNumber();

                // Пропускаем строки заголовков (1 строка - заголовки столбцов)
                int startRow = 2; // Первая строка с данными
                int dataRows = Math.Max(0, totalRows - startRow + 1);

                // Пропускаем столбец с номерами строк
                int startCol = 2;
                int dataCols = Math.Max(0, totalCols - startCol + 1);

                string[,] data = new string[dataRows, dataCols];

                for (int row = 0; row < dataRows; row++)
                {
                    for (int col = 0; col < dataCols; col++)
                    {
                        var cell = worksheet.Cell(startRow + row, startCol + col);
                        data[row, col] = cell.GetValue<string>();
                    }
                }

                return data;
            }
        }
    }
}
