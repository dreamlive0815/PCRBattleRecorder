using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCRBattleRecorder.Csv
{
    public class Csv
    {

        public static Csv FromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var headerLine = lines[0];
            var headers = ParseLine(headerLine);
            var data = new List<List<string>>();
            for (int i = 1; i < lines.Length; i++)
            {
                var dataLine = lines[i];
                var dataRow = ParseLine(dataLine);
                data.Add(dataRow);
            }
            var csv = new Csv();
            csv.SetHeaders(headers);
            csv.SetData(data);
            csv.filePath = filePath;
            return csv;
        }

        public static List<string> ParseLine(string line)
        {
            var arr = line.Split(new char[] { ',' });
            return new List<string>(arr);
        }

        private List<CsvHeader> headers;
        private Dictionary<string, int> header2Index;
        private List<List<string>> dataContainer;
        private Dictionary<string, List<string>> dataMap;
        private string filePath;

        private Csv()
        {

        }

        public List<CsvHeader> Headers
        {
            get { return headers; }
        }

        private void SetHeaders(List<string> headers)
        {
            SetHeaders(headers, null);
        }

        private void SetHeaders(List<string> headers, List<string> headerTypes)
        {
            this.headers = new List<CsvHeader>();
            header2Index = new Dictionary<string, int>();
            for (int i = 0; i < headers.Count; i++)
            {
                var header = headers[i];
                this.headers.Add(new CsvHeader(header));
                header2Index[header] = i;
            }
        }

        public int GetHeaderIndex(string header)
        {
            return header2Index[header];
        }

        public int RowKeyIndex
        {
            get { return 0; }
        }

        private void SetData(List<List<string>> data)
        {
            dataContainer = data;
            dataMap = new Dictionary<string, List<string>>();
            foreach (var row in data)
            {
                var key = row[RowKeyIndex];
                dataMap[key] = row;
            }
        }

        public CsvRow this[string key]
        {
            get
            {
                if (!dataMap.ContainsKey(key))
                    return null;
                var rowData = dataMap[key];
                return new CsvRow(this, rowData);
            }
        }

        public bool HasRow(string rowKey)
        {
            return dataMap.ContainsKey(rowKey);
        }

        public List<string> GetKeys()
        {
            var keys = new List<string>();
            for (int i = 0; i < dataContainer.Count; i++)
            {
                var data = dataContainer[i];
                var key = data[RowKeyIndex];
                keys.Add(key);
            }
            return keys;
        }
    }

    public class CsvHeader
    {
        public CsvHeader(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
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
