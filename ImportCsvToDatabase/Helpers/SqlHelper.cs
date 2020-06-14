using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ImportCsvToDatabase.Helpers
{
    public static class SqlHelper
    {
        public static string ExecuteProcedureReturnString(string connString, string procName, params SqlParameter[] paramters)
        {
            var result = "";
            using (var sqlConnection = new SqlConnection(connString))
            {
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = procName;
                    if (paramters != null)
                    {
                        command.Parameters.AddRange(paramters);
                    }
                    try
                    {
                        sqlConnection.Open();
                        var ret = command.ExecuteScalar();
                        if (ret != null)
                        {
                            result = Convert.ToString(ret);
                        }
                    }
                    catch (Exception ex)
                    {
                        return "";
                    }
                }
            }
            return result;
        }

        ///Methods to get values of   
        ///individual columns from sql data reader  
        #region Get Values from Sql Data Reader  
        public static string GetNullableString(SqlDataReader reader, string colName)
        {
            return reader.IsDBNull(reader.GetOrdinal(colName)) ? null : Convert.ToString(reader[colName]);
        }

        public static int GetNullableInt32(SqlDataReader reader, string colName)
        {
            return reader.IsDBNull(reader.GetOrdinal(colName)) ? 0 : Convert.ToInt32(reader[colName]);
        }

        public static bool GetBoolean(SqlDataReader reader, string colName)
        {
            return reader.IsDBNull(reader.GetOrdinal(colName)) ? default(bool) : Convert.ToBoolean(reader[colName]);
        }

        //this method is to check wheater column exists or not in data reader  
        public static bool IsColumnExists(this System.Data.IDataRecord dr, string colName)
        {
            try
            {
                return (dr.GetOrdinal(colName) >= 0);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
