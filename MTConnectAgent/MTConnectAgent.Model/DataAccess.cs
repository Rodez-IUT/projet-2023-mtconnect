using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace MTConnectAgent.Model
{
    public static class DataAccess
    {
        public static void init()
        {
            SqliteConnection con = new SqliteConnection(@"Data Source=C: \Users\rayann.karon\Desktop\sqlite.db");
            con.Open();
        }
    }
}
