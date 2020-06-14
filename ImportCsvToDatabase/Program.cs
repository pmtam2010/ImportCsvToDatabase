using ImportCsvToDatabase.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;

namespace ImportCsvToDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=DESKTOP-2DOEOSC\\MSSQLSERVER01;Database=inex;Trusted_Connection=True;MultipleActiveResultSets=True";
            string filepath = "Test Data/purchases.csv";
            DataTable dt = ConvertCSVtoDataTable(filepath);
            foreach (DataRow dataRow in dt.Rows)
            {
                var outParam = new SqlParameter("@ReturnCode", System.Data.SqlDbType.NVarChar, 20)
                {
                    Direction = System.Data.ParameterDirection.Output
                };

                SqlParameter[] parameters =
                        {
                            new SqlParameter("@ID", dataRow.ItemArray.GetValue(0).ToString()),
                            new SqlParameter("@contact_id", dataRow.ItemArray.GetValue(1).ToString()),
                            new SqlParameter("@product_id", dataRow.ItemArray.GetValue(2).ToString()),
                            new SqlParameter("@purchase_date", dataRow.ItemArray.GetValue(3).ToString()),
                            outParam
                        };
                SqlHelper.ExecuteProcedureReturnString(connectionString, "InsertPurchases", parameters);
                switch ((string)outParam.Value)
                {
                    case "C200":
                        break;
                    case "C201":
                        break;
                    case "C202":
                        break;
                }
            }
        }

        private static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            StreamReader sr = new StreamReader(strFilePath);
            var headers = sr.ReadLine().Split(',');
            DataTable dt = new DataTable();
            foreach(var header in headers)
            {
                dt.Columns.Add(header);
            }
            while(!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow row = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    row[i] = rows[i];
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
    }
}
