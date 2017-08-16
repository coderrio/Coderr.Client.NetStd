using System;
using System.Collections.Generic;
using System.Text;
using OneTrueError.Client.Processor;

namespace OneTrueError.Client.NetStd.Tests.Processor.Items
{
    class Filter : IReportFilter
    {
        public bool Answer { get; set; }

        public void Invoke(ReportFilterContext context)
        {
            context.CanSubmitReport = Answer;
        }
    }
}
