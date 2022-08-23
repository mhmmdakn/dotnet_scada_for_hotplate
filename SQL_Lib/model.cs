using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Text;


namespace SQL_Lib
{
    public class model
    {

        #region SQLSERVER
        public static string GetConnectionString(string path)
        {
            Dictionary<string, string> props = new Dictionary<string, string>();

            // XLSX - Excel 2007, 2010, 2012, 2013
            props["Provider"] = "Microsoft.ACE.OLEDB.12.0";
            props["Extended Properties"] = "Excel 12.0 XML";
            props["Data Source"] = path;

            // XLS - Excel 2003 and Older
            //props["Provider"] = "Microsoft.Jet.OLEDB.4.0";
            //props["Extended Properties"] = "Excel 8.0";
            //props["Data Source"] = "C:\\MyExcel.xls";

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            return sb.ToString();
        }
        public static DataSet ReadExcelFile(string path)
        {
            DataSet ds = new DataSet();

            string connectionString = GetConnectionString(path);

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = conn;

                // Get all Sheets in Excel File
                DataTable dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                // Loop through all Sheets to get data
                foreach (DataRow dr in dtSheet.Rows)
                {
                    string sheetName = dr["TABLE_NAME"].ToString();

                    if (!sheetName.EndsWith("$"))
                        continue;

                    // Get all rows from the Sheet
                    cmd.CommandText = "SELECT * FROM [" + sheetName + "]";

                    DataTable dt = new DataTable();
                    dt.TableName = sheetName;

                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    da.Fill(dt);

                    ds.Tables.Add(dt);
                }

                cmd = null;
                conn.Close();
            }

            return ds;
        }

        //public static void insertCommand(DataGridViewColumnCollection cols, DataGridViewRow row, string Table, string conn)
        //{
        //    StringBuilder insertCmd = new StringBuilder();
        //    string colSt = "";
        //    string val = "";


        //    for (int i = 0; i < cols.Count; i++)
        //    {
        //        if (cols[i].Visible && cols[i].Name != "id")
        //            colSt += cols[i].Name + ",";
        //    }

        //    colSt = colSt.Substring(0, colSt.Length - 1);

        //    val = "";
        //    for (int i = 0; i < cols.Count; i++)
        //    {
        //        if (cols[i].Visible && cols[i].Name != "id")
        //            val += "'" + row.Cells[i].Value.ToString().Replace(',', '.') + "',";
        //    }


        //    val = val.Substring(0, val.Length - 1);
        //    insertCmd.AppendFormat("INSERT INTO [dbo].[{0}] ({1}) VALUES ({2})", Table, colSt, val);
        //    CreateCommand(insertCmd.ToString(), conn);


        //}


        //public static void updateCommand(DataGridViewColumnCollection cols, DataGridViewRow row, string Table, string conn, int id)
        //{
        //    StringBuilder updatedCmd = new StringBuilder();
        //    string set = "";

        //    for (int i = 1; i < cols.Count; i++)
        //    {
        //        set += cols[i].Name + "='" + row.Cells[cols[i].Name].Value + "',";
        //    }

        //    set = set.Substring(0, set.Length - 1);
        //    updatedCmd.AppendFormat("update [{0}] set {1} where id={2} ", Table, set, id);
        //    CreateCommand(updatedCmd.ToString(), conn);


        //}

        public static DataTable SelectCommand(string queryString, string connectionString)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dt);
                command.Connection.Close();
            }
            return dt;
        }


        //public static DataTable SelectSQLiteCommand(string queryString, string connectionString)
        //{
        //    DataTable dt = new DataTable();
        //    using (SQLiteConnection connection = new SQLiteConnection(
        //               connectionString))
        //    {
        //        SQLiteCommand command = new SQLiteCommand(queryString, connection);
        //        command.Connection.Open();
        //        command.ExecuteNonQuery();
        //        SQLiteDataAdapter da = new SQLiteDataAdapter(command);
        //        da.Fill(dt);

        //    }
        //    return dt;
        //}
        public static void CreateCommand(string queryString, string connectionString)
        {

            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();

                command.Connection.Close();
            }

        }

        public static DataTable ExecuteCommandDT(SqlCommand command, string connectionString)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                command.Connection = connection;
                command.Connection.Open();
                // command.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dt);
                command.Connection.Close();

            }
            return dt;
        }
        public static void ExecuteCommand(SqlCommand command, string connectionString)
        {

            using (SqlConnection connection = new SqlConnection(
                       connectionString))
            {
                command.Connection = connection;
                command.Connection.Open();
                command.ExecuteNonQuery();

                command.Connection.Close();

            }

        }




        #endregion


        #region SQLSERVER Compact 4.0
        public static DataTable SelectCeCommand(string queryString, string connectionString)
        {
            DataTable dt = new DataTable();
            using (SqlCeConnection connection = new SqlCeConnection(
                       connectionString))
            {
                SqlCeCommand command = new SqlCeCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
                SqlCeDataAdapter da = new SqlCeDataAdapter(command);
                da.Fill(dt);

            }
            return dt;
        }
        public static DataTable ExecuteCeCommandDT(SqlCeCommand command, string connectionString)
        {
            DataTable dt = new DataTable();
            using (SqlCeConnection connection = new SqlCeConnection(
                       connectionString))
            {
                command.Connection = connection;
                command.Connection.Open();
                // command.ExecuteNonQuery();
                SqlCeDataAdapter da = new SqlCeDataAdapter(command);
                da.Fill(dt);
                command.Connection.Close();

            }
            return dt;
        }
        public static void ExecuteCeCommand(SqlCeCommand command, string connectionString)
        {

            using (SqlCeConnection connection = new SqlCeConnection(
                       connectionString))
            {
                command.Connection = connection;
                command.Connection.Open();
                command.ExecuteNonQuery();
                command.Connection.Close();

            }

        }
        public static void CreateCeCommand(string queryString, string connectionString)
        {

            using (SqlCeConnection connection = new SqlCeConnection(
                       connectionString))
            {
                SqlCeCommand command = new SqlCeCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();

                command.Connection.Close();
            }

        }

        //public static void insertCeCommand(DataGridViewColumnCollection cols, DataGridViewRow row, string Table, string conn)
        //{
        //    StringBuilder insertCmd = new StringBuilder();
        //    string colSt = "";
        //    string val = "";


        //    for (int i = 0; i < cols.Count; i++)
        //    {
        //        if (cols[i].Visible && cols[i].Name != "id")
        //            colSt += cols[i].Name + ",";
        //    }

        //    colSt = colSt.Substring(0, colSt.Length - 1);

        //    val = "";
        //    for (int i = 0; i < cols.Count; i++)
        //    {
        //        if (cols[i].Visible && cols[i].Name != "id")
        //            val += "'" + row.Cells[i].Value.ToString().Replace(',', '.') + "',";
        //    }


        //    val = val.Substring(0, val.Length - 1);
        //    insertCmd.AppendFormat("INSERT INTO [dbo].[{0}] ({1}) VALUES ({2})", Table, colSt, val);
        //    CreateCeCommand(insertCmd.ToString(), conn);


        //}

        //public static void updateCeCommand(DataGridViewColumnCollection cols, DataGridViewRow row, string Table, string conn, int id)
        //{
        //    StringBuilder updatedCmd = new StringBuilder();
        //    string set = "";

        //    for (int i = 1; i < cols.Count; i++)
        //    {
        //        set += cols[i].Name + "='" + row.Cells[cols[i].Name].Value + "',";
        //    }

        //    set = set.Substring(0, set.Length - 1);
        //    updatedCmd.AppendFormat("update [{0}] set {1} where id={2} ", Table, set, id);
        //    CreateCeCommand(updatedCmd.ToString(), conn);


        //}
        #endregion



    }
}
