using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRBattleRecorder
{
    class Trans
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
