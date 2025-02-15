using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Interfaces
{
    public interface IProjectile
    {
        double q { get; }
        double d { get; }
        double r { get; }
    }
}
