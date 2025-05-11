using MyDouble;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using NIRS.Parameter_Type;
using NIRS.RAM_folder;
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
        RAM<PN, double> ram;
        public SpaceCell()
        {
            ram = new RAM<PN, double>(6);
        }
        //private double? dynamic_mValue = null;
        //private double? MValue = null;
        //private double? vValue = null;
        //private double? wValue = null;

        //private double? rValue = null;
        //private double? eValue = null;
        //private double? psiValue = null;
        //private double? zValue = null;
        //private double? aValue = null;
        //private double? mValue = null;
        //private double? pValue = null;
        //private double? roValue = null;

        //public double dynamic_m { get { return dynamic_mValue.Value; } set { dynamic_mValue = value; } } 
        //public double M { get { return MValue.Value; } set { MValue = value; } }
        //public double v { get { return vValue.Value; } set { vValue = value; } }
        //public double w { get { return wValue.Value; } set { wValue = value; } }

        //public double r { get { return rValue.Value; } set { rValue = value; } }
        //public double e { get { return eValue.Value; } set { eValue = value; } }
        //public double psi { get { return psiValue.Value; } set { psiValue = value; } }
        //public double z { get { return zValue.Value; } set { zValue = value; } }
        //public double a { get { return aValue.Value; } set { aValue = value; } }
        //public double m { get { return mValue.Value; } set { mValue = value; } }
        //public double p { get { return pValue.Value; } set { pValue = value; } }
        //public double ro { get { return roValue.Value; } set { roValue = value; } }

        public double dynamic_m { get; set; }
        public double M { get; set; }
        public double v { get; set; }
        public double w { get; set; }

        public double r { get; set; }
        public double e { get; set; }
        public double psi { get; set; }
        public double z { get; set; }
        public double a { get; set; }
        public double m { get; set; }
        public double p { get; set; }
        public double ro { get; set; }

        public double One_minus_m 
        { 
            get {return 1 - m;} 
        }

        public double this[PN pn]
        {
            get
            {
                if(ram.isContains(pn))
                    return ram.Get(pn);

                switch (pn)
                {
                    case PN.v: ram.Add(pn,v); return v;
                    case PN.w: ram.Add(pn, w); return w;
                    case PN.a: ram.Add(pn, a); return a;
                    case PN.dynamic_m: ram.Add(pn, dynamic_m); return dynamic_m;
                    case PN.e: ram.Add(pn, e); return e;
                    case PN.M: ram.Add(pn, M); return M;
                    case PN.m: ram.Add(pn, m); return m;
                    case PN.One_minus_m: ram.Add(pn, One_minus_m); return One_minus_m;
                    case PN.p: ram.Add(pn, p); return p;
                    case PN.psi: ram.Add(pn, psi); return psi;
                    case PN.r: ram.Add(pn, r); return r;
                    case PN.ro: ram.Add(pn, ro); return ro;
                    case PN.z: ram.Add(pn, z); return z;
                }
                throw new Exception();
                //return (double)GetType().GetProperty(pn.ToString()).GetValue(this);
            }
            set
            {
                switch (pn)
                {
                    case PN.v: v = value;break;
                    case PN.w: w = value; break;
                    case PN.a: a = value; break;
                    case PN.dynamic_m: dynamic_m = value; break;
                    case PN.e: e = value; break;
                    case PN.M: M = value; break;
                    case PN.m: m = value; break;
                    case PN.One_minus_m: m = 1-value; break;
                    case PN.p: p = value; break;
                    case PN.psi: psi = value; break;
                    case PN.r: r = value; break;
                    case PN.ro: ro = value; break;
                    case PN.z: z = value; break;
                }
                //GetType().GetProperty(pn.ToString()).SetValue(this, value);
            }
        }
        //public bool isNull(PN pn)
        //{
        //    var name = pn.ToString() + "Value";
        //    var value = (double?)new SpaceCell().GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this);
        //    return value is null;
        //}
    }

}
