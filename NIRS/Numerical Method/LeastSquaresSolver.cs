using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Numerical_Method
{
    public sealed class LeastSquaresSolver
    {
        private readonly double[] _xValues;
        private readonly double[] _yValues;

        public LeastSquaresSolver(double[] xValues, double[] yValues)
        {
            if (xValues == null) throw new ArgumentNullException(nameof(xValues));
            if (yValues == null) throw new ArgumentNullException(nameof(yValues));
            if (xValues.Length != yValues.Length)
                throw new ArgumentException("Массивы X и Y должны иметь одинаковую длину");
            if (xValues.Length < 2)
                throw new ArgumentException("Для регрессии нужно минимум 2 точки");

            _xValues = xValues;
            _yValues = yValues;
        }

        public (double Slope, double Intercept, double RSquared) CalculateRegression()
        {
            int n = _xValues.Length;

            // Вычисляем суммы
            double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0, sumY2 = 0;

            for (int i = 0; i < n; i++)
            {
                sumX += _xValues[i];
                sumY += _yValues[i];
                sumXY += _xValues[i] * _yValues[i];
                sumX2 += _xValues[i] * _xValues[i];
                sumY2 += _yValues[i] * _yValues[i];
            }

            // Вычисляем коэффициенты линейной регрессии y = a*x + b
            double denominator = n * sumX2 - sumX * sumX;
            if (Math.Abs(denominator) < 1e-15)
                throw new InvalidOperationException("Данные вырождены (все X одинаковы)");

            double p = (n * sumXY - sumX * sumY) / denominator;     // a
            double a = (sumY - p * sumX) / n;                       // b

            // Вычисляем коэффициент детерминации R²
            double ssTotal = sumY2 - sumY * sumY / n;
            double ssResidual = 0;

            for (int i = 0; i < n; i++)
            {
                double predictedY = a + p * _xValues[i];
                ssResidual += Math.Pow(_yValues[i] - predictedY, 2);
            }

            double rSquared = (ssTotal > 0) ? 1 - ssResidual / ssTotal : 0;

            return (p, a, rSquared);
        }

        public double Predict(double x)
        {
            var (slope, intercept, _) = CalculateRegression();
            return intercept + slope * x;
        }

        public double[] GetPredictions()
        {
            var (slope, intercept, _) = CalculateRegression();
            double[] predictions = new double[_xValues.Length];

            for (int i = 0; i < _xValues.Length; i++)
            {
                predictions[i] = intercept + slope * _xValues[i];
            }

            return predictions;
        }

        public double CalculateMSE()
        {
            var predictions = GetPredictions();
            double sumSquaredError = 0;

            for (int i = 0; i < _yValues.Length; i++)
            {
                double error = _yValues[i] - predictions[i];
                sumSquaredError += error * error;
            }

            return sumSquaredError / _yValues.Length;
        }
    }
}
