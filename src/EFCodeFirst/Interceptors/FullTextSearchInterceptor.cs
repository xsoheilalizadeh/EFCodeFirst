using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Text.RegularExpressions;
using EFCodeFirst.Extensions;

namespace EFCodeFirst.Interceptors
{
    /// <summary>
    /// You Can Use This Interceptor In Full Text Search Feature in SQL Server.
    /// </summary>
    public class FullTextSearchInterceptor : IDbCommandInterceptor
    {
        private const string FullTextPrefix = "-FTSPREFIX-";

        /// <summary>
        /// This Method <code>return</code> String Type that contains FullText Search Command
        /// </summary>
        /// <param name="search">
        /// Give Your Search <value>Word</value>
        /// </param>
        /// <returns></returns>
        public static string FullTextSearch(string search)
        {
            return string.Format($"({FullTextPrefix}{search})");
        }

        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }

        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            RewriteFullTextQuery(command);
        }

        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }

        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            RewriteFullTextQuery(command);
        }

        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }

        private static void RewriteFullTextQuery(DbCommand dbCommand)
        {
            var text = dbCommand.CommandText;
            for (var i = 0; i < dbCommand.Parameters.Count; i++)
            {
                var parameter = dbCommand.Parameters[i];
                if (parameter.DbType.In(DbType.String, DbType.AnsiString, DbType.StringFixedLength,
                    DbType.AnsiStringFixedLength))
                {
                    if (parameter.Value == DBNull.Value)
                        continue;
                    var value = (string) parameter.Value;
                    if (value.IndexOf(FullTextPrefix, StringComparison.Ordinal) >= 0)
                    {
                        parameter.Size = 4096;
                        parameter.DbType = DbType.AnsiStringFixedLength;
                        value = value.Replace(FullTextPrefix, ""); // remove prefix we added n linq query
                        value = value.Substring(1,
                            value.Length - 2); // remove %% escaping by linq translator from string.Contains to sql LIKE
                        parameter.Value = value;
                        dbCommand.CommandText = Regex.Replace(text,
                            string.Format(
                                $@"\[(\w*)\].\[(\w*)\]\s*LIKE\s*@{parameter.ParameterName}\s?(?:ESCAPE N?'~')"),
                            string.Format($@"contains([$1].[$2], @{parameter.ParameterName})"));
                        if (text == dbCommand.CommandText)
                            throw new Exception("FTS was not replaced on: " + text);
                        text = dbCommand.CommandText;
                    }
                }
            }
        }
    }
}