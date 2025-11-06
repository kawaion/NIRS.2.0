using MyDouble;
using NIRS.Interfaces;
using NIRS.Parameter_names;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIRS.Visualization.Progress
{
    public class Progresser
    {
        IProgress<ProgressInfo> _progress;
        ProgressInfo info = new ProgressInfo();
        DateTime startTime;
        public Progresser(IProgress<ProgressInfo> progress)
        {
            _progress = progress;
            startTime = DateTime.Now;
        }

        public void Update(int progressbarValue, double layerValue)
        {
            info.progressbarValue = progressbarValue;
            info.layerValue = layerValue;
            info.time = DateTime.Now - startTime;
            _progress.Report(info);
        }
    }
}
