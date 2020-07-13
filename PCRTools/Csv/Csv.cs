using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRBattleRecorder.Csv
{
    public class Csv
    {

        public static Csv FromFile(string filePath)
        {
            var csv = new Csv();
            return csv;
        }

        private List<string> headers;
        private Dictionary<string, int> header2Index;
        private List<List<string>> container;

        private Csv()
        {

        }

        private void SetHeaders(List<string> headers)
        {
            this.headers = headers;
            header2Index = new Dictionary<string, int>();
            for (int i = 0; i < headers.Count; i++)
            {
                header2Index[headers[i]] = i;
            }
        }

        public int GetHeaderIndex(string header)
        {
            return header2Index[header];
        }
    }

    public class CsvRow
    {

        private Csv csv;
        private List<string> rowData;

        public CsvRow(Csv csv, List<string> rowData)
        {
            this.csv = csv;
            this.rowData = rowData;
        }

        public string this[string header]
        {
            get
            {
                var index = csv.GetHeaderIndex(header);
                return rowData[index];
            }
            set
            {
                var index = csv.GetHeaderIndex(header);
                rowData[index] = value; 
            }
        }
    }
}
