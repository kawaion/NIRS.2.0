using MyDouble;

namespace NIRS.Helpers
{
    /// <summary>
    /// Appoint задает параметры которые нужно сместить, Offset принимает парметры на которые нужно смещать
    /// </summary>
    static class OffseterNK
    {
        public static (LimitedDouble n, LimitedDouble k) Offset(this OffsetNodeNK<LimitedDouble> node, LimitedDouble newN, LimitedDouble newK)
        {
            return (node.N + node.N - newN, node.K + node.K - newK);
        }
        public static (double n, double k) Offset(this OffsetNodeNK<double> node, double newN, double newK) 
        {
            return (node.N + node.N - newN, node.K + node.K - newK);
        }
        public static OffsetNodeNK<T> Appoint<T>(T n, T k)
        {
            return new OffsetNodeNK<T>(n, k);
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


    static class OffseterN
    {
        public static LimitedDouble Offset(this OffsetNodeN<LimitedDouble> node, LimitedDouble newN)
        {
            return (node.N + node.N - newN);
        }
        public static double Offset(this OffsetNodeN<double> node, double newN)
        {
            return (node.N + node.N - newN);
        }
        public static OffsetNodeN<T> Appoint<T>(T n)
        {
            return new OffsetNodeN<T>(n);
        }
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
