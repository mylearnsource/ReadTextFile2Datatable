using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadTextFile
{
    class Program
    {
        static void Main(string[] args)
        {           
            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\CP262BP\Desktop\TestData.txt");
            string[] dataColValues = null;
            string[] headers = null;
            bool hasHeader = true;
            // Display the file contents by using a foreach loop.

            int i = 0;
            int rowIndex = 0;
            DataTable _myDataTable = new DataTable();

            foreach (string line in lines.Where(x => !string.IsNullOrEmpty(x)).ToArray())
            {
                if (i == 0)
                {
                    headers = line.Split(Convert.ToChar(ConfigurationManager.AppSettings["DataSeparator"].Trim()));

                    foreach (var header in headers)
                    {
                        _myDataTable.Columns.Add(header);
                    }
                }
                else
                {
                    dataColValues = line.Split(Convert.ToChar(ConfigurationManager.AppSettings["DataSeparator"].Trim()));
                    DataRow row = _myDataTable.NewRow();
                    int colIndex = 0;
                    foreach (var col in dataColValues)
                    {
                        row[headers[colIndex].ToString()] = col;
                        colIndex++;
                    }

                    _myDataTable.Rows.Add(row);

                }
                i++;
                rowIndex++;
            }

            DataView view = new DataView(_myDataTable);
            DataTable dtQueryTable = view.ToTable(false, ConfigurationManager.AppSettings["SelectColumns"].Trim().Split('|'));
                     
            StringBuilder InsertQuery = new StringBuilder();
            foreach (DataRow dr in dtQueryTable.Rows)
            {
                InsertQuery.Append(string.Format("Insert into tableName ({0}) values ({1});" + Environment.NewLine, ConfigurationManager.AppSettings["InsertColumns"].Trim().Replace('|', ','), string.Join(",", dr.ItemArray)));
            }

            string finalQuery = InsertQuery.ToString();
        }
    }
}
