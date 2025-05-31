using MyDouble;
using NIRS.Data_Parameters.Input_Data_Parameters;
using NIRS.Interfaces;

namespace NIRS.Helpers
{
    public class XGetter
    {
        private double h;
        public XGetter(IConstParameters constParameters)
        {
            h = constParameters.h;
        }
        /// <summary>
        /// индексатор используется как метод, класс ничего не хранит
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        //public double this[LimitedDouble k] => k.Value * h;
        public double this[double k] => k * h;
    }
}
