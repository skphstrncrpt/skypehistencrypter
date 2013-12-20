using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace SkypeHistoryEnc
{

    public class AppDB : GenericSqliteDB
    {
        private string _FileName = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\') + "\\db.sl";
        public override string FileName { get { return _FileName; } set { _FileName = value; } }

    }

}