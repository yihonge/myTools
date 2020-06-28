namespace myTools
{
    #region 数据库访问类
    /// <summary>
    /// 数据库访问类
    /// </summary>
    public class SqliteHelper
    {
        /// <summary>
        /// 执行表的增、删、改操作
        /// </summary>
        /// <param name="connection">数据库连接字符串</param>
        /// <param name="sql">查询语句</param>
        /// <returns>受影响的行数</returns>
        public static int UpdateData(string connection, string sql)
        {
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(connection);
            System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }

        /// <summary>
        /// 执行结果集中的第一个数据的查询
        /// </summary>
        /// <param name="connection">数据库连接字符串</param>
        /// <param name="sql">查询语句</param>
        /// <returns>受影响的结果集中的第一个数据</returns>
        public static object GetExecuteScalar(string connection, string sql)
        {
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(connection);
            System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Dispose();
                conn.Close();
            }
        }

        /// <summary>
        /// 执行结果集中的所有数据的查询
        /// </summary>
        /// <param name="connection">数据库连接字符串</param>
        /// <param name="sql">查询语句</param>
        /// <returns>受影响的结果集中的所有数据</returns>
        public static System.Data.SQLite.SQLiteDataReader GetExecuteReader(string connection, string sql)
        {
            System.Data.SQLite.SQLiteConnection conn = new System.Data.SQLite.SQLiteConnection(connection);
            System.Data.SQLite.SQLiteCommand cmd = new System.Data.SQLite.SQLiteCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            }
            catch (System.Exception ex)
            {
                conn.Close();
                throw ex;
            }
        }
    }
    #endregion
}