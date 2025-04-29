using MyDouble;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using NIRS.Parameter_Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Grid_Folder
{
    public class SpaceCell : IGridCell
    {
        private double? dynamic_mValue = null;
        private double? MValue = null;
        private double? vValue = null;
        private double? wValue = null;

        private double? rValue = null;
        private double? eValue = null;
        private double? psiValue = null;
        private double? zValue = null;
        private double? aValue = null;
        private double? mValue = null;
        private double? pValue = null;
        private double? roValue = null;

        public double dynamic_m { get { return dynamic_mValue.Value; } set { dynamic_mValue = value; } } 
        public double M { get { return MValue.Value; } set { MValue = value; } }
        public double v { get { return vValue.Value; } set { vValue = value; } }
        public double w { get { return wValue.Value; } set { wValue = value; } }

        public double r { get { return rValue.Value; } set { rValue = value; } }
        public double e { get { return eValue.Value; } set { eValue = value; } }
        public double psi { get { return psiValue.Value; } set { psiValue = value; } }
        public double z { get { return zValue.Value; } set { zValue = value; } }
        public double a { get { return aValue.Value; } set { aValue = value; } }
        public double m { get { return mValue.Value; } set { mValue = value; } }
        public double p { get { return pValue.Value; } set { pValue = value; } }
        public double ro { get { return roValue.Value; } set { roValue = value; } }

        public double One_minus_m 
        { 
            get {return 1 - m;} 
        }

        public double this[PN pn]
        {
            get
            {
                return (double)GetType().GetProperty(pn.ToString()).GetValue(this);
            }
            set
            {
                GetType().GetProperty(pn.ToString()).SetValue(this, value);
            }
        }
        public bool isNull(PN pn)
        {
            var name = pn.ToString() + "Value";
            var value = (double?)new SpaceCell().GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
            return value is null;
        }
    }

}
