using MyDouble;

namespace NIRS.Helpers
{
    /// <summary>
    /// Appoint задает параметры которые нужно сместить, Offset принимает парметры на которые нужно смещать
    /// </summary>
    static class OffseterNK
    {
        public static (LimitedDouble n, LimitedDouble k) Offset(this OffsetNode<LimitedDouble> node, LimitedDouble newN, LimitedDouble newK)
        {
            return (node.N + node.N - newN, node.K + node.K - newK);
        }
        public static (double n, double k) Offset(this OffsetNode<double> node, double newN, double newK) 
        {
            return (node.N + node.N - newN, node.K + node.K - newK);
        }
        public static OffsetNode<T> Appoint<T>(T n, T k)
        {
            return new OffsetNode<T>(n, k);
        }
    }
    class OffsetNode<T>
    {
        public T N { get; }
        public T K { get; }
        public OffsetNode(T n, T k)
        {
            N = n;
            K = k;
        }
    }
}
