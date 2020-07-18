using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCRBattleRecorder.Config;

namespace PCRBattleRecorder.Csv
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

        //public bool UseCache { get; set; } = !ConfigMgr.GetInstance().DebugMode;
        public bool UseCache { get; set; } = true;

        private Dictionary<string, Csv> csvCacheDict = new Dictionary<string, Csv>();

        public void ClearCsvCache()
        {
            csvCacheDict.Clear();
        }
             
        public Csv GetCsv(string filePath)
        {
            if (UseCache)
            {
                if (csvCacheDict.ContainsKey(filePath)) return csvCacheDict[filePath];
            }
            var csv = Csv.FromFile(filePath);
            if (UseCache)
            {
                csvCacheDict[filePath] = csv;
            }
            return csv;
        }
    }
}
