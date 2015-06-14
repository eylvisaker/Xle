using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERY.Xle
{
    public class XleCoreImpl : IXleCore
    {
        XleCore legacyCore;

        public XleCoreImpl()
        {
            legacyCore = new XleCore();
        }

        public void ProcessArguments(string[] args)
        {
            legacyCore.ProcessArguments(args);
        }

        public void Run(XleGameFactory xleFactory)
        {
            legacyCore.Run(xleFactory);
        }
    }
}
