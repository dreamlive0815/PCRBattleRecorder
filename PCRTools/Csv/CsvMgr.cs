using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRTools.Csv
{
    public class CsvMgr
    {
        private static CsvMgr instance;

        public static CsvMgr GetInstance()
        {
            if (instance == null)
            {
                instance = new CsvMgr();
            }
            return instance;
        }

        private CsvMgr()
        {
        }
    }
}
