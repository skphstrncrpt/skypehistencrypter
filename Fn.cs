using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace SkypeHistoryEnc
{
    public class DoubleBufferDataGrid : DataGridView
    {
        public DoubleBufferDataGrid()
            : base()
        {
            this.DoubleBuffered = true;
        }
    }

    public class unenc
    {
        public int begin;
        public int end;
        public string convo_id;
        public string displayname;

        public string Name { get { return displayname; } }
        public DateTime Last { get { return Fn.UnixTimeStampToDateTime(end); } }
        public DateTime First { get { return Fn.UnixTimeStampToDateTime(begin); } }
    }

    public class chat
    {
        public Int64 cid;
        public string name;
        public string skid;
        public string stamp;

        public string Name { get { return Crypt.DecString(name); } }
        public DateTime Last { get { return Crypt.DecDate(stamp); } }
    }

    public class message
    {
        public Int64 mid;
        public string skid;
        public string msg;
        public string stamp;
        public string author;

        public string Author { get { return Crypt.DecString(author); } }
        public string Message { get { return System.Web.HttpUtility.HtmlDecode(Crypt.DecString(msg)); } }
        public DateTime Date { get { return Crypt.DecDate(stamp); } }
    }

    /// <summary>
    /// Summary description for Fn
    /// </summary>
    public class Fn
    {
        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTimeStamp);
        }
        public static int DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (int)Math.Floor((dateTime - new DateTime(1970, 1, 1)).TotalSeconds);
        }

        public static string Serialize(object obj)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            string result = "";
            try
            {
                formatter.Serialize(ms, obj);
                result = Convert.ToBase64String(ms.GetBuffer(), 0, Convert.ToInt32(ms.Length));
            }
            finally
            {
                ms.Close();
            }
            return result;
        }

        public static object Deserialize(string str)
        {
            byte[] buf = Convert.FromBase64CharArray(str.ToCharArray(), 0, str.Length);

            MemoryStream ms = new MemoryStream(buf);
            BinaryFormatter formatter = new BinaryFormatter();
            object result = "";
            try
            {
                result = formatter.Deserialize(ms);
            }
            finally
            {
                ms.Close();
            }
            return result;
        }

        public static object Clone(object theObject)
        {
            return Deserialize(Serialize(theObject));
        }

    }
}