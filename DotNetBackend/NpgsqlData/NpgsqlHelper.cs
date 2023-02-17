using Npgsql;
using System.Data;

namespace NpgsqlData
{
    public class NpgsqlHelper
    {
        public static int ExecuteNonQuery(NpgsqlConnection conn, CommandType cmdType, string cmdText, Dictionary<string, object>? cmdParms, NpgsqlTransaction? trans = null)
        {
            NpgsqlCommand cmd = conn.CreateCommand();
            PrepareCommand(cmd, conn, trans, cmdType, cmdText, cmdParms);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        public static async Task<int> ExecuteNonQueryAsync(NpgsqlConnection conn, string cmdText, Dictionary<string, object> cmdParms, CancellationToken cancellationToken = default)
        {
            NpgsqlCommand cmd = conn.CreateCommand();
            using (conn)
            {
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);
                int val = await cmd.ExecuteNonQueryAsync(cancellationToken);
                cmd.Parameters.Clear();
                return val;
            }
        }

        public static IDataReader ExecuteReader(NpgsqlConnection conn, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms)
        {
            CommandBehavior commandBehavior = CommandBehavior.Default;
            if (conn.State != ConnectionState.Open)
            {
                commandBehavior = CommandBehavior.CloseConnection;
            }

            NpgsqlCommand cmd = conn.CreateCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            NpgsqlDataReader rdr = cmd.ExecuteReader(commandBehavior);
            return rdr;
        }

        public static async Task<IDataReader> ExecuteReaderAsync(NpgsqlConnection conn, CommandType cmdType, string cmdText, Dictionary<string, object> cmdParms, CancellationToken cancellationToken = default)
        {
            CommandBehavior commandBehavior = CommandBehavior.Default;
            if (conn.State != ConnectionState.Open)
            {
                commandBehavior = CommandBehavior.CloseConnection;
            }

            NpgsqlCommand cmd = conn.CreateCommand();
            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            NpgsqlDataReader rdr = await cmd.ExecuteReaderAsync(commandBehavior, cancellationToken);
            return rdr;
        }

        private static void PrepareCommand(NpgsqlCommand cmd, NpgsqlConnection conn, NpgsqlTransaction? trans, CommandType cmdType, string cmdText, Dictionary<string, object>? cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = 30 * 60;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                foreach (KeyValuePair<string, object> param in cmdParms)
                {
                    NpgsqlParameter parameter = cmd.CreateParameter();
                    parameter.ParameterName = param.Key;
                    parameter.Value = param.Value;
                    cmd.Parameters.Add(parameter);
                }
            }
        }
    }
}