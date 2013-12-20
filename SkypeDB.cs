using System;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace SkypeHistoryEnc
{

    public class SkypeDB : GenericSqliteDB
    {
        public static string SkypeDBfile = null;

        public override string FileName { get { return SkypeDBfile; } set { SkypeDBfile = value; } }

    }

}