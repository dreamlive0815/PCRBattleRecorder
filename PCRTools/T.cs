using System;

namespace PCRBattleRecorder
{
    public class Trans
    {
        public static string T(string s)
        {
            return s;
        }

        public static string T(string s, params object[] vals)
        {
            return string.Format(s, vals);
        }
    }
}
