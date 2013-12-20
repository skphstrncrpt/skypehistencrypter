using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Data.SQLite.Generic;
using System.Configuration;
namespace SkypeHistoryEnc
{

    public class GenericSqliteDB
    {
        private string _FileName = "SQLiteConnectionStringRead";
        public virtual string FileName { get { return _FileName; } set { _FileName = value; } }

        public virtual SQLiteConnection GetDbConnRead()
        {
            return GetDbConn();
        }

        public virtual SQLiteConnection GetDbConnWrite()
        {
            return GetDbConn();
        }

        public virtual SQLiteConnection GetDbConn()
        {
            SQLiteConnection conn = new SQLiteConnection();
            conn.ConnectionString = "Data Source=" + FileName;
            conn.Open();
            return conn;
        }

        public virtual void ReturnDbConn(SQLiteConnection conn)
        {
            conn.Close();
        }

        public virtual object GetFieldFromTable(string table, string field, string idfield, object id)
        {
            SQLiteConnection conn = GetDbConnRead();
            SQLiteCommand sql = conn.CreateCommand();
            sql.CommandText = "select " + field + " from " + table
                + " where " + idfield + " = @id order by " + field + " limit 1";
            sql.Parameters.AddWithValue("@id", id);
            object result = DBNull.Value;
            try
            {
                result = sql.ExecuteScalar();
            }
            finally
            {
                conn.Close();
            }
            return result;
        }

        public virtual object GetFieldFromTable(string query, string paramKey, object paramValue)
        {
            return GetFieldFromTable(query, new Hashtable() { { paramKey, paramValue } });
        }

        public virtual object GetFieldFromTable(string query, Hashtable sqlparams)
        {
            SQLiteCommand sql = GetDbConnRead().CreateCommand();
            sql.CommandText = query;

            foreach (DictionaryEntry de in sqlparams)
                sql.Parameters.AddWithValue("@" + de.Key, de.Value);

            object result = DBNull.Value;
            try
            {
                result = sql.ExecuteScalar();
            }
            finally
            {
                sql.Connection.Close();
            }
            return result;
        }

        public virtual object GetFieldFromTable(string table, string field, Hashtable sqlparams)
        {
            string query = "select " + field + " from " + table + " where 1=1 limit 1";

            foreach (DictionaryEntry de in sqlparams)
                query += " and (" + de.Key + " = @" + de.Key + ")";

            return GetFieldFromTable(query, sqlparams);
        }

        public virtual int GetIntFromTable(string table, string field, string idfield, object id)
        {
            int result = int.MinValue;
            try
            {
                object resfield = GetFieldFromTable(table, field, idfield, id);
                if (resfield != DBNull.Value && resfield != null)
                    result = Convert.ToInt32(resfield.ToString());
            }
            catch { }
            return result;
        }

        public virtual decimal GetDecimalFromTable(string table, string field, string idfield, object id)
        {
            decimal result = decimal.MinValue;
            try
            {
                object resfield = GetFieldFromTable(table, field, idfield, id);
                if (resfield != DBNull.Value && resfield != null)
                    result = Convert.ToDecimal(resfield.ToString());
            }
            catch { }
            return result;
        }

        public virtual string GetStringFromTable(string table, string field, string idfield, object id)
        {
            string result = null;
            try
            {
                object resfield = GetFieldFromTable(table, field, idfield, id);
                if (resfield != DBNull.Value && resfield != null)
                    result = resfield.ToString();
            }
            catch { }
            return result;
        }

        public virtual bool GetBoolFromTable(string table, string field, string idfield, object id, bool defaultval)
        {
            bool result = defaultval;
            try
            {
                object resfield = GetFieldFromTable(table, field, idfield, id);
                if (resfield != DBNull.Value && resfield != null)
                    result = (bool)resfield;
            }
            catch { }
            return result;
        }

        public virtual DataTable Reader2Table(SQLiteDataReader rdr)
        {
            DataTable result = new DataTable();
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                string colname = rdr.GetName(i);
                if (result.Columns.Contains(colname))
                    colname = colname + "_" + i;
                result.Columns.Add(colname, rdr.GetFieldType(i));
            }
            while (rdr.Read())
            {
                object[] vals = new object[rdr.FieldCount];
                rdr.GetValues(vals);
                result.Rows.Add(vals);
            }
            rdr.Close();
            return result;
        }

        public virtual Dictionary<string, object> Row2Dictionary(DataRow row)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (row != null)
            {
                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    string field = row.Table.Columns[i].ColumnName;
                    result[field] = row[field];
                }
            }
            return result;
        }

        public virtual Hashtable Row2Hashtable(DataRow row)
        {
            Hashtable result = new Hashtable();
            if (row != null)
            {
                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    string field = row.Table.Columns[i].ColumnName;
                    result[field] = row[field];
                }
            }
            return result;
        }

        public virtual Hashtable Row2Hashtable(SQLiteDataReader rdr)
        {
            Hashtable result = new Hashtable();
            if (rdr.Read())
            {
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    string field = rdr.GetName(i);
                    result[field] = rdr[field];
                }
            }
            rdr.Close();
            return result;
        }

        public virtual ArrayList TableToList(DataTable Data)
        {
            ArrayList result = new ArrayList();
            foreach (DataRow row in Data.Rows)
                result.Add(row[0]);
            return result;
        }

        public virtual DataTable GetDataSource(string query)
        {
            return GetDataSource(query, null as Hashtable);
        }

        public virtual DataTable GetDataSource(string query, SQLiteParameterCollection parameters)
        {
            SQLiteConnection conn = GetDbConnRead();
            SQLiteCommand sql = conn.CreateCommand();
            sql.CommandText = query;
            if (parameters != null)
                foreach (SQLiteParameter param in parameters)
                    sql.Parameters.AddWithValue(param.ParameterName, param.Value);
            DataTable dt = Reader2Table(sql.ExecuteReader(CommandBehavior.CloseConnection));
            return dt;
        }

        public virtual DataTable GetDataSource(string query, Hashtable parameters)
        {
            SQLiteConnection conn = GetDbConnRead();
            SQLiteCommand sql = conn.CreateCommand();
            sql.CommandText = query;
            if (parameters != null)
                foreach (string param in parameters.Keys)
                    sql.Parameters.AddWithValue(param, parameters[param]);
            DataTable dt = Reader2Table(sql.ExecuteReader(CommandBehavior.CloseConnection));
            return dt;
        }

        public virtual DataTable GetDataSource(string query, string paramName, object paramValue)
        {
            SQLiteCommand sql = new SQLiteCommand(query);
            sql.Parameters.AddWithValue(paramName, paramValue);
            DataTable result = GetDataSource(sql.CommandText, sql.Parameters);
            return result;
        }

        public virtual DataTable GetDataSource(string query, string paramName, object paramValue, string idField, string nameField, string nameVal)
        {
            SQLiteCommand sql = new SQLiteCommand(query);
            sql.Parameters.AddWithValue(paramName, paramValue);
            DataTable result = GetDataSource(sql.CommandText, sql.Parameters);

            DataRow newrow = result.NewRow();
            newrow[idField] = Int32.MinValue;
            newrow[nameField] = nameVal;
            result.Rows.InsertAt(newrow, 0);

            return result;
        }

        public virtual DataRow GetDataRow(string query, string paramName, object paramValue)
        {
            return GetDataRow(query, new Hashtable() { { paramName, paramValue } });
        }

        public virtual DataRow GetDataRow(string query, Hashtable sqlparams)
        {
            SQLiteCommand sql = new SQLiteCommand(query);
            if (sqlparams != null)
                foreach (string key in sqlparams.Keys)
                    sql.Parameters.AddWithValue(key, sqlparams[key]);
            DataTable result = GetDataSource(sql.CommandText, sql.Parameters);
            if (result.Rows.Count > 0)
                return result.Rows[0];
            else
                return null;
        }


        public virtual int UpdateRow(string table, string idField, object id, string updField, object updVal)
        {
            SQLiteCommand sql = GetDbConnWrite().CreateCommand();

            sql.CommandText = "update " + table + " set " + updField + " = @upd where " + idField + " = @id";
            sql.Parameters.AddWithValue("@id", id);
            sql.Parameters.AddWithValue("@upd", updVal);

            try
            {
                return sql.ExecuteNonQuery();
            }
            finally
            {
                sql.Connection.Close();
            }
        }

        public virtual int UpdateRow(string table, string idField, object id, Hashtable sqlparams)
        {
            SQLiteCommand sql = GetDbConnWrite().CreateCommand();

            sql.CommandText = "update " + table + " set ";
            foreach (DictionaryEntry de in sqlparams)
                sql.CommandText += "[" + de.Key + "] = @" + de.Key + ",";
            sql.CommandText = sql.CommandText.TrimEnd(new char[] { ',' });
            sql.CommandText += " where " + idField + " = @id";

            sql.Parameters.AddWithValue("@id", id);

            foreach (DictionaryEntry de in sqlparams)
                sql.Parameters.AddWithValue("@" + de.Key, de.Value);

            try
            {
                return sql.ExecuteNonQuery();
            }
            finally
            {
                sql.Connection.Close();
            }
        }

        public virtual void InsertRows(string query, string paramName, object paramValue)
        {
            InsertRows(query, new Hashtable() { { paramName.TrimStart('@'), paramValue } });
        }

        public virtual void InsertRows(string query, Hashtable sqlparams)
        {
            SQLiteCommand sql = GetDbConnWrite().CreateCommand();

            sql.CommandText = query;
            foreach (DictionaryEntry de in sqlparams)
                sql.Parameters.AddWithValue("@" + de.Key, de.Value);

            try
            {
                sql.ExecuteNonQuery();
            }
            finally
            {
                sql.Connection.Close();
            }
        }

        public virtual int InsertRow(string table, Hashtable sqlparams)
        {
            SQLiteCommand sql = GetDbConnWrite().CreateCommand();

            sql.CommandText = "insert into " + table + " (";
            foreach (DictionaryEntry de in sqlparams)
                sql.CommandText += "[" + de.Key + "],";
            sql.CommandText = sql.CommandText.TrimEnd(new char[] { ',' });
            sql.CommandText += ") values (";
            foreach (DictionaryEntry de in sqlparams)
                sql.CommandText += "@" + de.Key + ",";
            sql.CommandText = sql.CommandText.TrimEnd(new char[] { ',' });
            sql.CommandText += ")";

            foreach (DictionaryEntry de in sqlparams)
                sql.Parameters.AddWithValue("@" + de.Key, de.Value);

            try
            {
                return sql.ExecuteNonQuery();
            }
            finally
            {
                sql.Connection.Close();
            }
        }

        public virtual Guid InsertOrUpdateRow(string table, string guidfield, Guid guidid, Hashtable sqlparams)
        {
            bool isInsert = guidid == Guid.Empty;

            SQLiteCommand sql = GetDbConnWrite().CreateCommand();
            if (isInsert)
            {
                guidid = Guid.NewGuid();
                sqlparams[guidfield] = guidid;
                sql.CommandText = "insert into " + table + " ([" + guidfield + "],";
                foreach (DictionaryEntry de in sqlparams)
                    sql.CommandText += "[" + de.Key + "],";
                sql.CommandText = sql.CommandText.TrimEnd(new char[] { ',' });
                sql.CommandText += ") values ('" + guidid.ToString() + "',";
                foreach (DictionaryEntry de in sqlparams)
                    sql.CommandText += "@" + de.Key + ",";
                sql.CommandText = sql.CommandText.TrimEnd(new char[] { ',' });
                sql.CommandText += ")";
            }
            else
            {
                sql.CommandText = "update " + table + " set ";
                foreach (DictionaryEntry de in sqlparams)
                    sql.CommandText += "[" + de.Key + "] = @" + de.Key + ",";
                sql.CommandText = sql.CommandText.TrimEnd(new char[] { ',' });
                sql.CommandText += " where " + guidfield + " = @guidid";
                sql.Parameters.AddWithValue("@guidid", guidid);
            }

            foreach (DictionaryEntry de in sqlparams)
                sql.Parameters.AddWithValue("@" + de.Key, de.Value);

            try
            {
                sql.ExecuteNonQuery();
            }
            finally
            {
                sql.Connection.Close();
            }

            return guidid;
        }

        public virtual int InsertOrUpdateRow(string table, string idfield, int id, Hashtable sqlparams)
        {
            return (int)InsertOrUpdateRow(table, idfield, (Int64)id, sqlparams);
        }

        public virtual Int64 InsertOrUpdateRow(string table, string idfield, Int64 id, Hashtable sqlparams)
        {
            bool isInsert = id <= 0;
            SQLiteCommand sql = GetDbConnWrite().CreateCommand();
            if (isInsert)
            {
                sql.CommandText = "insert into " + table + " (";
                foreach (DictionaryEntry de in sqlparams)
                    sql.CommandText += "[" + de.Key + "],";
                sql.CommandText = sql.CommandText.TrimEnd(new char[] { ',' });
                sql.CommandText += ") values (";
                foreach (DictionaryEntry de in sqlparams)
                    sql.CommandText += "@" + de.Key + ",";
                sql.CommandText = sql.CommandText.TrimEnd(new char[] { ',' });
                sql.CommandText += ")";
            }
            else
            {
                sql.CommandText = "update " + table + " set ";
                foreach (DictionaryEntry de in sqlparams)
                    sql.CommandText += "[" + de.Key + "] = @" + de.Key + ",";
                sql.CommandText = sql.CommandText.TrimEnd(new char[] { ',' });
                sql.CommandText += " where " + idfield + " = @id";
                sql.Parameters.AddWithValue("@id", id);
            }

            foreach (DictionaryEntry de in sqlparams)
                sql.Parameters.AddWithValue("@" + de.Key, de.Value);

            Int64 result = id;
            try
            {
                sql.ExecuteNonQuery();

                if (isInsert && !string.IsNullOrEmpty(idfield))
                {
                    result = sql.Connection.LastInsertRowId;
                }
            }
            finally
            {
                sql.Connection.Close();
            }

            //if (isInsert)
            //    RecLog.LogInsert(table, (int?)result, sqlparams);
            //else
            //    RecLog.LogUpdate(table, (int)result, sqlparams);

            return result;

        }

        public virtual void InsertOrUpdateRow2(string table, string idfield, int id, Hashtable sqlparams)
        {
            InsertOrUpdateRow2(table, idfield, (Int64)id, sqlparams);
        }

        public virtual void InsertOrUpdateRow2(string table, string idfield, Int64 id, Hashtable sqlparams)
        {
            bool isInsert = id <= 0;
            SQLiteCommand sql = GetDbConnWrite().CreateCommand();
            if (isInsert)
            {
                sql.CommandText = "insert into " + table + " (";
                foreach (DictionaryEntry de in sqlparams)
                    sql.CommandText += "[" + de.Key + "],";
                sql.CommandText = sql.CommandText.TrimEnd(new char[] { ',' });
                sql.CommandText += ") values (";
                foreach (DictionaryEntry de in sqlparams)
                    sql.CommandText += "@" + de.Key + ",";
                sql.CommandText = sql.CommandText.TrimEnd(new char[] { ',' });
                sql.CommandText += ")";
            }
            else
            {
                sql.CommandText = "update " + table + " set ";
                foreach (DictionaryEntry de in sqlparams)
                    sql.CommandText += "[" + de.Key + "] = @" + de.Key + ",";
                sql.CommandText = sql.CommandText.TrimEnd(new char[] { ',' });
                sql.CommandText += " where " + idfield + " = @id";
                sql.Parameters.AddWithValue("@id", id);
            }

            foreach (DictionaryEntry de in sqlparams)
                sql.Parameters.AddWithValue("@" + de.Key, de.Value);

            try
            {
                sql.ExecuteNonQuery();
            }
            finally
            {
                sql.Connection.Close();
            }
        }

        public virtual bool DeleteFromTable(string table, string idfield, long id)
        {
            SQLiteCommand sql = GetDbConnWrite().CreateCommand();
            sql.CommandText = "delete from " + table + " where " + idfield + "=@" + idfield;
            sql.Parameters.AddWithValue("@" + idfield, id);

            bool ret = false;
            try
            {
                sql.ExecuteNonQuery();
                ret = true;
            }
            finally
            {
                sql.Connection.Close();
            }

            //RecLog.LogDelete(table, (int)id);

            return ret;
        }

        public virtual bool DeleteFromTable(string table, string guidfield, Guid guid_id)
        {
            SQLiteCommand sql = GetDbConnWrite().CreateCommand();
            sql.CommandText = "delete from " + table + " where " + guidfield + "=@" + guidfield;
            sql.Parameters.AddWithValue("@" + guidfield, guid_id);

            bool ret = false;
            try
            {
                sql.ExecuteNonQuery();
                ret = true;
            }
            finally
            {
                sql.Connection.Close();
            }

            return ret;
        }
        public virtual bool DeleteFromTable(string table, Hashtable sqlparams)
        {
            if (sqlparams.Count < 1) return false;
            SQLiteCommand sql = GetDbConnWrite().CreateCommand();
            sql.CommandText = "delete from " + table + " where ";

            foreach (DictionaryEntry de in sqlparams)
                sql.CommandText += de.Key + " = @" + de.Key + " and ";
            sql.CommandText += "markforreplace";
            sql.CommandText = sql.CommandText.Replace(" and markforreplace", "");

            foreach (DictionaryEntry de in sqlparams)
                sql.Parameters.AddWithValue("@" + de.Key, de.Value);

            bool ret = false;
            try
            {
                sql.ExecuteNonQuery();
                ret = true;
            }
            finally
            {
                sql.Connection.Close();
            }

            return ret;
        }

        public virtual int ExecuteQuery(string query, SQLiteParameterCollection parameters)
        {
            SQLiteCommand sql = GetDbConnWrite().CreateCommand();
            sql.CommandText = query;
            if (parameters != null)
                foreach (SQLiteParameter param in parameters)
                    sql.Parameters.AddWithValue(param.ParameterName, param.Value);
            int result = 0;
            try
            {
                result = sql.ExecuteNonQuery();
            }
            finally
            {
                sql.Connection.Close();
            }
            return result;
        }

        public virtual void DeleteRowsFromTable(string table, string paramName, object paramValue)
        {
            SQLiteCommand sql = this.GetDbConnWrite().CreateCommand();
            sql.CommandText = "delete from " + table + " where " + paramName + "=@param";
            sql.Parameters.AddWithValue("@param", paramValue);
            sql.ExecuteNonQuery();
            sql.Connection.Close();
        }


        public virtual int ExecuteQuery(string query, string paramName, object paramValue)
        {
            SQLiteCommand sql = new SQLiteCommand(query);
            sql.Parameters.AddWithValue(paramName, paramValue);
            return ExecuteQuery(sql.CommandText, sql.Parameters);
        }

        public virtual int ExecuteQuery(string query, Hashtable parameters)
        {
            SQLiteCommand sql = GetDbConnWrite().CreateCommand();
            sql.CommandText = query;
            if (parameters != null)
                foreach (string key in parameters.Keys)
                    sql.Parameters.AddWithValue(key, parameters[key]);
            int result = 0;
            try
            {
                result = sql.ExecuteNonQuery();
            }
            finally
            {
                sql.Connection.Close();
            }
            return result;
        }



        public virtual int ExecuteQuery(string query)
        {
            return ExecuteQuery(query, (Hashtable)null);
        }


        public virtual int CheckRowExistsAndUpdate(string table, string idfield, Int64 id, Hashtable sqlparams)
        {
            SQLiteCommand sql = GetDbConnWrite().CreateCommand();
            bool exists = false;

            sql.CommandText = "select count(*) from " + table + " where " + idfield + " = @id";
            sql.Parameters.AddWithValue("@id", id);
            exists = Convert.ToInt32(sql.ExecuteScalar()) > 0;

            sql = sql.Connection.CreateCommand();
            if (!exists)
            {
                sql.CommandText = "insert into " + table + " (";
                foreach (DictionaryEntry de in sqlparams)
                    sql.CommandText += de.Key + ",";
                sql.CommandText += idfield + ") values (";
                foreach (DictionaryEntry de in sqlparams)
                    sql.CommandText += "@" + de.Key + ",";
                sql.CommandText += "@id)";
            }
            else
            {
                sql.CommandText = "update " + table + " set ";
                foreach (DictionaryEntry de in sqlparams)
                    sql.CommandText += de.Key + " = @" + de.Key + ",";
                sql.CommandText = sql.CommandText.TrimEnd(new char[] { ',' });
                sql.CommandText += " where " + idfield + " = @id";
            }

            sql.Parameters.AddWithValue("@id", id);
            foreach (DictionaryEntry de in sqlparams)
                sql.Parameters.AddWithValue("@" + de.Key, de.Value);
            int result = 0;
            try
            {
                result = sql.ExecuteNonQuery();
            }
            finally
            {
                sql.Connection.Close();
            }

            //if (exists)
            //    RecLog.LogUpdate(table, (int)id, sqlparams);
            //else
            //    RecLog.LogInsert(table, (int)id, sqlparams);
            return result;
        }


        public void SwitchOrder(int ID1, int ID2, string Table, string IdField, string OrderField)
        {
            SQLiteConnection conn = GetDbConnWrite();
            try
            {
                SQLiteCommand sql = conn.CreateCommand();
                sql.CommandText = @"select " + OrderField + " from " + Table + " where " + IdField + " = @ID1";
                sql.Parameters.AddWithValue("@ID1", ID1);
                int Order1 = (int)sql.ExecuteScalar();

                sql = conn.CreateCommand();
                sql.CommandText = @"select " + OrderField + " from " + Table + " where " + IdField + " = @ID2";
                sql.Parameters.AddWithValue("@ID2", ID2);
                int Order2 = (int)sql.ExecuteScalar();

                if (Order1 == Order2)
                    Order2 = Order2 + 1;

                sql = conn.CreateCommand();
                sql.CommandText = @"update " + Table + " set " + OrderField + " = @Order2 where " + IdField + " = @ID1";
                sql.Parameters.AddWithValue("@ID1", ID1);
                sql.Parameters.AddWithValue("@Order2", Order2);
                sql.ExecuteNonQuery();

                sql = conn.CreateCommand();
                sql.CommandText = @"update " + Table + " set " + OrderField + " = @Order1 where " + IdField + " = @ID2";
                sql.Parameters.AddWithValue("@ID2", ID2);
                sql.Parameters.AddWithValue("@Order1", Order1);
                sql.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }
    }

}