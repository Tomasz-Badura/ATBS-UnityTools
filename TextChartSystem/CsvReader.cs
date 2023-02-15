using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace ATBS.TextSystem
{
    public class CsvReader
    {
        private char delimiter;
        private TextAsset csvFile;
        private Dictionary<string, int> headers;

        public CsvReader(TextAsset csvFile, char delimiter = ',')
        {
            this.delimiter = delimiter;
            this.csvFile = csvFile;
        }

        public Dictionary<string, string> GetColumnData(string colKey)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            using (MemoryStream stream = new MemoryStream(csvFile.bytes))
            using (var reader = new StreamReader(stream))
            {
                // Read the header row
                string[] headers = reader.ReadLine().Split(delimiter);

                // Create a dictionary to store the headers and their corresponding indices
                this.headers = new Dictionary<string, int>();
                for (int i = 0; i < headers.Length; i++)
                {
                    this.headers[headers[i]] = i;
                }

                // Look up the index of the column title in the dictionary
                if (!this.headers.TryGetValue(colKey, out int colIndex))
                {
                    throw new KeyNotFoundException("Column title not found: " + colKey);
                }
                // Read the rest of the rows
                StringBuilder sb = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    int ch = reader.Read();
                    if (ch == delimiter || ch == '\n')
                    {
                        string[] rows = sb.ToString().Split(delimiter);
                        data[rows[0]] = rows[colIndex];
                        sb.Clear();
                    }
                    else
                    {
                        sb.Append((char)ch);
                    }
                }
            }
            return data;
        }

        public string FindValue(string rowKey, string colKey)
        {
            using (MemoryStream stream = new MemoryStream(csvFile.bytes))
            using (var reader = new StreamReader(stream))
            {
                // Read the header row
                string[] headers = reader.ReadLine().Split(delimiter);

                // Create a dictionary to store the headers and their corresponding indices
                this.headers = new Dictionary<string, int>();
                for (int i = 0; i < headers.Length; i++)
                {
                    this.headers[headers[i]] = i;
                }

                // Look up the index of the column title in the dictionary
                if (!this.headers.TryGetValue(colKey, out int colIndex))
                {
                    throw new KeyNotFoundException("Column title not found: " + colKey);
                }

                // Read the rest of the rows
                StringBuilder sb = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    int ch = reader.Read();
                    if (ch == delimiter || ch == '\n')
                    {
                        string[] rows = sb.ToString().Split(delimiter);
                        if (rows[0] == rowKey)
                        {
                            return rows[colIndex];
                        }
                        sb.Clear();
                    }
                    else
                    {
                        sb.Append((char)ch);
                    }
                }
            }

            throw new KeyNotFoundException("Row title not found: " + rowKey);
        }
    }
}