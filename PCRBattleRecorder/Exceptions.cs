using System;

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

    class SetFilePathException : Exception
    {
        public SetFilePathException(string msg) : base(msg)
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
