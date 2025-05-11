using MyDouble;
using System.Xml.Linq;

namespace NIRS.Helpers
{
    /// <summary>
    /// Appoint задает параметры которые нужно сместить, Offset принимает парметры на которые нужно смещать
    /// </summary>
    static class OffseterNK
    {
        //public static (LimitedDouble n, LimitedDouble k) Offset(this OffsetNodeNK node, LimitedDouble newN, LimitedDouble newK)
        //{
        //    return (node.N + node.N - newN, node.K + node.K - newK);
        //}
        //public static (double n, double k) Offset(this OffsetNodeNK<double> node, double newN, double newK) 
        //{
        //    return (node.N + node.N - newN, node.K + node.K - newK);
        //}
        //public static OffsetNodeNK<T> Appoint<T>(T n, T k)
        //{
        //    return new OffsetNodeNK<T>(n, k);
        //}
        //public static OffsetNodeNK Appoint(LimitedDouble n, LimitedDouble k)
        //{
        //    return new OffsetNodeNK(n, k);
        //}
        public static (LimitedDouble n, LimitedDouble k) AppointAndOffset(LimitedDouble n, double offsetN, LimitedDouble k, double offsetK)
        {
            LimitedDouble newN;
            LimitedDouble newK;
            if (offsetN == 0)
                newN = n;
            else
                newN = n - offsetN;

            if (offsetK == 0)
                newK = k;
            else
                newK = k - offsetK;
            return (newN, newK);
        }
        public static (double n, double k) AppointAndOffset(double n, double offsetN, double k, double offsetK)
        {
            double newN;
            double newK;
            if (offsetN == 0)
                newN = n;
            else
                newN = n - offsetN;

            if (offsetK == 0)
                newK = k;
            else
                newK = k - offsetK;
            return (newN, newK);
        }
    }
    class OffsetNodeNK<T>
    {
        public T N { get; }
        public T K { get; }
        public OffsetNodeNK(T n, T k)
        {
            N = n;
            K = k;
        }
    }
    class OffsetNodeNK
    {
        public LimitedDouble N { get; }
        public LimitedDouble K { get; }
        public OffsetNodeNK(LimitedDouble n, LimitedDouble k)
        {
            N = n;
            K = k;
        }
    }


    static class OffseterN
    {
        public static LimitedDouble AppointAndOffset(LimitedDouble n, double offsetN)
        {
            LimitedDouble newN;
            if (offsetN == 0)
                newN = n;
            else
                newN = n - offsetN;
            return newN;
        }
        public static double AppointAndOffset(double n, double offsetN)
        {
            double newN;
            if (offsetN == 0)
                newN = n;
            else
                newN = n - offsetN;
            return newN;
        }
        //public static LimitedDouble Offset(this OffsetNodeN<LimitedDouble> node, LimitedDouble newN)
        //{
        //    return (node.N + node.N - newN);
        //}
        //public static double Offset(this OffsetNodeN<double> node, double newN)
        //{
        //    return (node.N + node.N - newN);
        //}
        //public static OffsetNodeN<T> Appoint<T>(T n)
        //{
        //    return new OffsetNodeN<T>(n);
        //}
    }
    class OffsetNodeN<T>
    {
        public T N { get; }
        public OffsetNodeN(T n)
        {
            N = n;
        }
    }
}
