using System;

namespace PCRBattleRecorder
{

    public class NoTrackTraceException : Exception
    {
        public NoTrackTraceException(string msg) : base(msg)
        {
        }
    }

    public class BreakException : NoTrackTraceException
    {
        public BreakException(string msg) : base(msg)
        {
        }
    }

    public class SetFilePathException : Exception
    {
        public SetFilePathException(string msg) : base(msg)
        {
        }
    }

    public class ShellException : Exception
    {
        public ShellException(string msg) : base(msg)
        {
        }
    }


}
