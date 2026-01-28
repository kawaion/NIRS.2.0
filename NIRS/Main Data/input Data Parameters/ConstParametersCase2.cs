using NIRS.Helpers;
using NIRS.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace NIRS.Main_Data.Input_Data_Parameters
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ConstParametersCase2 : IConstParameters, INotifyPropertyChanged, ICloneable
    {
        #region Приватные поля

        private double _tau = 1e-6;
        private double _chamberLength = 0.5;
        private int _countDivideChamber = 100;

        // Теплофизические свойства
        private double _cp = 1838.8;
        private double _cv = 1497.4;

        // Параметры пороха
        private double _powderDelta = 1520.0;
        private double _f = 900000.0;
        private double _qPropellant = 46.0;
        private double _covolume = 9.5e-4;

        // Геометрия порохового зерна
        private double _d0 = 0.0115;
        private double _d0Inner = 0.0009;
        private double _l0 = 0.019;

        // Теплофизические свойства
        private double _lambda0 = 0.45;
        private double _mu0 = 1.0;

        // Конструктивные параметры
        private double _e1 = 0.0011;
        private double _u1 = 1.022e-9;
        private double _omegaV = 0.1;
        private double _forcingPressure = 30e6;

        #endregion

        #region Конструкторы

        public ConstParametersCase2()
        {
            // Конструктор по умолчанию
        }

        public ConstParametersCase2(double tau, int countDivideChamber, Point2D chamber)
        {
            _tau = tau;
            _countDivideChamber = countDivideChamber;
            _chamberLength = chamber.X;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void OnMultiplePropertiesChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                OnPropertyChanged(propertyName);
            }
        }

        #endregion

        #region 1. Дискретизация и геометрия

        [Category("1. Дискретизация и геометрия")]
        [DisplayName("Шаг по времени (τ), с")]
        [Description("Шаг интегрирования по времени")]
        [DefaultValue(1e-6)]
        public double Tau
        {
            get => _tau;
            set
            {
                if (SetProperty(ref _tau, value, nameof(Tau)))
                {
                    OnMultiplePropertiesChanged(nameof(CourantNumber), nameof(CharacteristicTime));
                }
            }
        }

        [Category("1. Дискретизация и геометрия")]
        [DisplayName("Длина камеры (L), м")]
        [Description("Длина камеры сгорания")]
        [DefaultValue(0.5)]
        public double ChamberLength
        {
            get => _chamberLength;
            set
            {
                if (SetProperty(ref _chamberLength, value, nameof(ChamberLength)))
                {
                    OnMultiplePropertiesChanged(nameof(H), nameof(ChamberVolume));
                }
            }
        }

        [Category("1. Дискретизация и геометрия")]
        [DisplayName("Количество разбиений (N)")]
        [Description("Число ячеек по длине камеры")]
        [DefaultValue(100)]
        public int CountDivideChamber
        {
            get => _countDivideChamber;
            set
            {
                if (SetProperty(ref _countDivideChamber, value, nameof(CountDivideChamber)))
                {
                    OnMultiplePropertiesChanged(nameof(H), nameof(ChamberVolume));
                }
            }
        }

        [Category("1. Дискретизация и геометрия")]
        [DisplayName("Шаг по пространству (h), м")]
        [Description("Вычисляемый шаг сетки")]
        [ReadOnly(true)]
        public double H => _chamberLength / _countDivideChamber;

        #endregion

        #region 2. Теплофизические свойства газа

        [Category("2. Теплофизические свойства газа")]
        [DisplayName("Теплоемкость Cp, Дж/(кг·К)")]
        [Description("Теплоемкость при постоянном давлении")]
        [DefaultValue(1838.8)]
        public double Cp
        {
            get => _cp;
            set
            {
                if (SetProperty(ref _cp, value, nameof(Cp)))
                {
                    OnMultiplePropertiesChanged(
                        nameof(Gamma), nameof(GammaMinusOne),
                        nameof(GasConstant), nameof(Q));
                }
            }
        }

        [Category("2. Теплофизические свойства газа")]
        [DisplayName("Теплоемкость Cv, Дж/(кг·К)")]
        [Description("Теплоемкость при постоянном объеме")]
        [DefaultValue(1497.4)]
        public double Cv
        {
            get => _cv;
            set
            {
                if (SetProperty(ref _cv, value, nameof(Cv)))
                {
                    OnMultiplePropertiesChanged(
                        nameof(Gamma), nameof(GammaMinusOne),
                        nameof(GasConstant), nameof(Q));
                }
            }
        }

        [Category("2. Теплофизические свойства газа")]
        [DisplayName("Показатель адиабаты (γ)")]
        [Description("Отношение Cp/Cv")]
        [ReadOnly(true)]
        public double Gamma => _cp / _cv;

        [Category("2. Теплофизические свойства газа")]
        [DisplayName("(γ - 1)")]
        [Description("Показатель адиабаты минус единица")]
        [ReadOnly(true)]
        public double GammaMinusOne => Gamma - 1;

        [Category("2. Теплофизические свойства газа")]
        [DisplayName("Газовая постоянная (R), Дж/(кг·К)")]
        [Description("R = Cp - Cv")]
        [ReadOnly(true)]
        public double GasConstant => _cp - _cv;

        #endregion

        #region 3. Параметры пороха

        [Category("3. Параметры пороха")]
        [DisplayName("Плотность пороха (Δ), кг/м³")]
        [Description("Начальная плотность пороха")]
        [DefaultValue(1520.0)]
        public double PowderDelta
        {
            get => _powderDelta;
            set => SetProperty(ref _powderDelta, value, nameof(PowderDelta));
        }

        [Category("3. Параметры пороха")]
        [DisplayName("Сила пороха (f), Дж/кг")]
        [Description("Удельная энергия пороха")]
        [DefaultValue(900000.0)]
        public double F
        {
            get => _f;
            set
            {
                if (SetProperty(ref _f, value, nameof(F)))
                {
                    OnPropertyChanged(nameof(Q));
                }
            }
        }

        [Category("3. Параметры пороха")]
        [DisplayName("Теплота сгорания (Q), Дж/кг")]
        [Description("Q = f / (γ - 1)")]
        [ReadOnly(true)]
        public double Q => _f / GammaMinusOne;

        [Category("3. Параметры пороха")]
        [DisplayName("Коволюм (α), м³/кг")]
        [Description("Собственный объем молекул газа")]
        [DefaultValue(9.5e-4)]
        public double Covolume
        {
            get => _covolume;
            set => SetProperty(ref _covolume, value, nameof(Covolume));
        }

        [Category("3. Параметры пороха")]
        [DisplayName("Удельный вес газов (q)")]
        [Description("Массовая доля газообразных продуктов")]
        [DefaultValue(46.0)]
        public double QPropellant
        {
            get => _qPropellant;
            set => SetProperty(ref _qPropellant, value, nameof(QPropellant));
        }

        #endregion

        #region 4. Геометрия порохового зерна

        [Category("4. Геометрия порохового зерна")]
        [DisplayName("Наружный диаметр (D₀), м")]
        [Description("Начальный наружный диаметр")]
        [DefaultValue(0.0115)]
        public double D0
        {
            get => _d0;
            set
            {
                if (SetProperty(ref _d0, value, nameof(D0)))
                {
                    OnMultiplePropertiesChanged(
                        nameof(RelativeLength), nameof(CrossSectionalArea),
                        nameof(ChamberVolume));
                }
            }
        }

        [Category("4. Геометрия порохового зерна")]
        [DisplayName("Внутренний диаметр (d₀), м")]
        [Description("Диаметр канала в зерне")]
        [DefaultValue(0.0009)]
        public double d0
        {
            get => _d0Inner;
            set => SetProperty(ref _d0Inner, value, nameof(d0));
        }

        [Category("4. Геометрия порохового зерна")]
        [DisplayName("Длина зерна (L₀), м")]
        [Description("Длина порохового зерна")]
        [DefaultValue(0.019)]
        public double L0
        {
            get => _l0;
            set
            {
                if (SetProperty(ref _l0, value, nameof(L0)))
                {
                    OnPropertyChanged(nameof(RelativeLength));
                }
            }
        }

        [Category("4. Геометрия порохового зерна")]
        [DisplayName("Относительная длина")]
        [Description("L₀ / D₀")]
        [ReadOnly(true)]
        public double RelativeLength => _l0 / _d0;

        [Category("4. Геометрия порохового зерна")]
        [DisplayName("Толщина свода (e₁), м")]
        [Description("Толщина горящего свода")]
        [DefaultValue(0.0011)]
        public double E1
        {
            get => _e1;
            set => SetProperty(ref _e1, value, nameof(E1));
        }

        #endregion

        #region 5. Транспортные свойства

        [Category("5. Транспортные свойства")]
        [DisplayName("Вязкость (μ₀), Па·с")]
        [Description("Динамическая вязкость при н.у.")]
        [DefaultValue(1.0)]
        public double Mu0
        {
            get => _mu0;
            set => SetProperty(ref _mu0, value, nameof(Mu0));
        }

        [Category("5. Транспортные свойства")]
        [DisplayName("Теплопроводность (λ₀), Вт/(м·К)")]
        [Description("Коэффициент теплопроводности")]
        [DefaultValue(0.45)]
        public double Lambda0
        {
            get => _lambda0;
            set => SetProperty(ref _lambda0, value, nameof(Lambda0));
        }

        [Category("5. Транспортные свойства")]
        [DisplayName("Коэф. теплопередачи (u₁), м²/с")]
        [Description("Коэффициент теплообмена")]
        [DefaultValue(1.022e-9)]
        public double U1
        {
            get => _u1;
            set => SetProperty(ref _u1, value, nameof(U1));
        }

        #endregion

        #region 6. Дополнительные параметры

        [Category("6. Дополнительные параметры")]
        [DisplayName("Удельный объем (ω_v), м³/кг")]
        [Description("Удельный объем продуктов сгорания")]
        [DefaultValue(0.1)]
        public double OmegaV
        {
            get => _omegaV;
            set => SetProperty(ref _omegaV, value, nameof(OmegaV));
        }

        [Category("6. Дополнительные параметры")]
        [DisplayName("Давление форсирования, Па")]
        [Description("Начальное давление форсирования")]
        [DefaultValue(30e6)]
        public double ForcingPressure
        {
            get => _forcingPressure;
            set => SetProperty(ref _forcingPressure, value, nameof(ForcingPressure));
        }

        #endregion

        #region 7. Расчетные параметры (только чтение)

        [Category("7. Расчетные параметры")]
        [DisplayName("Площадь сечения, м²")]
        [Description("Площадь поперечного сечения канала")]
        [ReadOnly(true)]
        public double CrossSectionalArea => Math.PI * Math.Pow(_d0 / 2, 2);

        [Category("7. Расчетные параметры")]
        [DisplayName("Объем камеры, м³")]
        [Description("Объем камеры сгорания")]
        [ReadOnly(true)]
        public double ChamberVolume => CrossSectionalArea * H * _countDivideChamber;

        [Category("7. Расчетные параметры")]
        [DisplayName("Характерное время, с")]
        [Description("Характерное время процесса")]
        [ReadOnly(true)]
        public double CharacteristicTime => H / Math.Sqrt(Gamma * GasConstant * 300);

        [Category("7. Расчетные параметры")]
        [DisplayName("Число Куранта")]
        [Description("Критерий устойчивости схемы")]
        [ReadOnly(true)]
        public double CourantNumber => _tau * Math.Sqrt(Gamma * GasConstant * 300) / H;

        [Category("7. Расчетные параметры")]
        [DisplayName("Скорость звука, м/с")]
        [Description("Скорость звука при 300 К")]
        [ReadOnly(true)]
        public double SpeedOfSound => Math.Sqrt(Gamma * GasConstant * 300);

        #endregion

        #region 8. Информация и валидация

        [Category("8. Информация и валидация")]
        [DisplayName("Статус")]
        [Description("Статус корректности параметров")]
        [ReadOnly(true)]
        public string ValidationStatus
        {
            get
            {
                if (!Validate()) return "✗ Ошибки";
                return CourantNumber <= 1.0 ? "✓ Корректно и устойчиво" : "⚠ Корректно, но Cu > 1";
            }
        }

        [Category("8. Информация и валидация")]
        [DisplayName("Устойчивость")]
        [Description("Удовлетворяет ли условию Куранта")]
        [ReadOnly(true)]
        public bool IsStable => CourantNumber <= 1.0;

        [Category("8. Информация и валидация")]
        [DisplayName("Число Куранта")]
        [Description("Cu = τ·a/h")]
        [ReadOnly(true)]
        public double CuValue => CourantNumber;

        [Category("8. Информация и валидация")]
        [DisplayName("Дата создания")]
        [Description("Время создания объекта")]
        [ReadOnly(true)]
        public DateTime CreationTime { get; private set; } = DateTime.Now;

        [Category("8. Информация и валидация")]
        [DisplayName("Версия")]
        [Description("Версия параметров")]
        [ReadOnly(true)]
        public string Version => "1.0";

        #endregion

        #region Явная реализация IConstParameters

        double IConstParameters.cp => _cp;
        double IConstParameters.cv => _cv;
        double IConstParameters.teta => GammaMinusOne;
        double IConstParameters.alpha => _covolume;
        double IConstParameters.PowderDelta => _powderDelta;
        double IConstParameters.D0 => _d0;
        double IConstParameters.d0 => _d0Inner;
        double IConstParameters.L0 => _l0;
        double IConstParameters.mu0 => _mu0;
        double IConstParameters.lamda0 => _lambda0;
        double IConstParameters.Q => Q;
        double IConstParameters.e1 => _e1;
        double IConstParameters.u1 => _u1;
        double IConstParameters.omegaV => _omegaV;
        double IConstParameters.f => _f;
        double IConstParameters.q => _qPropellant;
        double IConstParameters.forcingPressure => _forcingPressure;
        int IConstParameters.countDivideChamber => _countDivideChamber;
        double IConstParameters.tau => _tau;
        double IConstParameters.h => H;

        #endregion

        #region Вспомогательные методы

        private bool SetProperty<T>(ref T field, T value, string propertyName)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private bool SetProperty<T>(ref T field, T value, string propertyName, Action onChanged)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            onChanged?.Invoke();
            return true;
        }

        public bool Validate()
        {
            return _tau > 0 &&
                   _chamberLength > 0 &&
                   _countDivideChamber > 0 &&
                   _cp > 0 &&
                   _cv > 0 &&
                   _powderDelta > 0 &&
                   _f > 0 &&
                   _covolume >= 0 &&
                   _d0 > 0 &&
                   _d0Inner >= 0 &&
                   _l0 > 0 &&
                   _lambda0 >= 0 &&
                   _mu0 >= 0;
        }

        public string GetValidationErrors()
        {
            var errors = new System.Collections.Generic.List<string>();

            if (_tau <= 0) errors.Add("Шаг по времени должен быть > 0");
            if (_chamberLength <= 0) errors.Add("Длина камеры должна быть > 0");
            if (_countDivideChamber <= 0) errors.Add("Количество разбиений должно быть > 0");
            if (_cp <= 0) errors.Add("Теплоемкость Cp должна быть > 0");
            if (_cv <= 0) errors.Add("Теплоемкость Cv должна быть > 0");
            if (_cp <= _cv) errors.Add("Cp должна быть больше Cv");
            if (_powderDelta <= 0) errors.Add("Плотность пороха должна быть > 0");
            if (_f <= 0) errors.Add("Сила пороха должна быть > 0");
            if (_covolume < 0) errors.Add("Коволюм не может быть отрицательным");
            if (_d0 <= 0) errors.Add("Наружный диаметр должен быть > 0");
            if (_d0Inner < 0) errors.Add("Внутренний диаметр не может быть отрицательным");
            if (_l0 <= 0) errors.Add("Длина зерна должна быть > 0");
            if (_lambda0 < 0) errors.Add("Теплопроводность не может быть отрицательной");
            if (_mu0 < 0) errors.Add("Вязкость не может быть отрицательной");

            if (CourantNumber > 1.0)
                errors.Add($"Число Куранта {CourantNumber:F3} > 1 (схема может быть неустойчивой)");

            return errors.Count > 0 ? string.Join("\n", errors) : null;
        }

        public void ResetToDefaults()
        {
            _tau = 1e-6;
            _chamberLength = 0.5;
            _countDivideChamber = 100;
            _cp = 1838.8;
            _cv = 1497.4;
            _powderDelta = 1520.0;
            _f = 900000.0;
            _covolume = 9.5e-4;
            _qPropellant = 46.0;
            _d0 = 0.0115;
            _d0Inner = 0.0009;
            _l0 = 0.019;
            _lambda0 = 0.45;
            _mu0 = 1.0;
            _e1 = 0.0011;
            _u1 = 1.022e-9;
            _omegaV = 0.1;
            _forcingPressure = 30e6;

            OnPropertyChanged(null); // Обновить все свойства
        }

        public ConstParametersCase2 Clone()
        {
            var clone = new ConstParametersCase2
            {
                _tau = _tau,
                _chamberLength = _chamberLength,
                _countDivideChamber = _countDivideChamber,
                _cp = _cp,
                _cv = _cv,
                _powderDelta = _powderDelta,
                _f = _f,
                _covolume = _covolume,
                _qPropellant = _qPropellant,
                _d0 = _d0,
                _d0Inner = _d0Inner,
                _l0 = _l0,
                _lambda0 = _lambda0,
                _mu0 = _mu0,
                _e1 = _e1,
                _u1 = _u1,
                _omegaV = _omegaV,
                _forcingPressure = _forcingPressure
            };
            return clone;
        }

        object ICloneable.Clone() => Clone();

        #endregion

        #region Форматированный вывод и сериализация

        public override string ToString()
        {
            return $"Параметры: τ={_tau:E2} с, L={_chamberLength:F3} м, N={_countDivideChamber}, Cu={CourantNumber:F3}";
        }

        public string ToDetailedString()
        {
            return $"Теплофизика: Cp={_cp:F1}, Cv={_cv:F1}, γ={Gamma:F3}\n" +
                   $"Порох: f={_f:E3}, α={_covolume:E3}, Δ={_powderDelta:F0}\n" +
                   $"Геометрия: D₀={_d0:F4}, d₀={_d0Inner:F4}, L₀={_l0:F4}\n" +
                   $"Дискретизация: h={H:E3}, Cu={CourantNumber:F3}";
        }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                ["Tau"] = _tau,
                ["ChamberLength"] = _chamberLength,
                ["CountDivideChamber"] = _countDivideChamber,
                ["Cp"] = _cp,
                ["Cv"] = _cv,
                ["PowderDelta"] = _powderDelta,
                ["F"] = _f,
                ["Covolume"] = _covolume,
                ["QPropellant"] = _qPropellant,
                ["D0"] = _d0,
                ["d0"] = _d0Inner,
                ["L0"] = _l0,
                ["Lambda0"] = _lambda0,
                ["Mu0"] = _mu0,
                ["E1"] = _e1,
                ["U1"] = _u1,
                ["OmegaV"] = _omegaV,
                ["ForcingPressure"] = _forcingPressure
            };
        }

        #endregion
    }
}