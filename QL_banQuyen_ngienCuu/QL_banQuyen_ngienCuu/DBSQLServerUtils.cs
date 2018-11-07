using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;

namespace QL_banQuyen_ngienCuu
{
    class DBSQLServerUtils
    {
        public static SqlConnection
            GetDBConnection()
        {
            string connString = @"Data Source=DESKTOP-ADJ2THT;Initial Catalog=quanli_detai;Integrated Security=True";

            SqlConnection conn = new SqlConnection(connString);
            //conn.ConnectionString.
            return conn;
        }
        public static SqlCommand
            GetBDCommand(SqlConnection conn, String sql)
        {
            ///conn = Db
            SqlCommand cmd = new SqlCommand(sql, conn);
            
            return cmd;
        }
       /* public static DbDataReader
            getDB(SqlCommand cmd)
        {
            DbDataReader reader = cmd.ExecuteReader();
            return reader;
        }*/
    }
}
