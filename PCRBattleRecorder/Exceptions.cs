using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRBattleRecorder
{

    class NoTrackTraceException : Exception
    {
        public NoTrackTraceException(string msg) : base(msg)
        {
        }
    }

    class BreakException : NoTrackTraceException
    {
        public BreakException(string msg) : base(msg)
        {
        }
    }

    class GetFilePathException : Exception
    {
        public GetFilePathException(string msg) : base(msg)
        {
        }
    }
    

    class ShellException : Exception
    {
        public ShellException(string msg) : base(msg)
        {
        }
    }


}
