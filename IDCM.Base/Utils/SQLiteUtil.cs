using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace IDCM.Base.Utils
{
    public class SQLiteUtil
    {
        public static readonly string[] SqliteKeyWords = new string[]{ "ABORT", "ACTION", "ADD", "AFTER", "ALL", "ALTER", "ANALYZE", "AND", "AS", "ASC", "ATTACH", "AUTOINCREMENT", "BEFORE", "BEGIN", "BETWEEN", "BY", "CASCADE", "CASE", "CAST", "CHECK", "COLLATE", "COLUMN", "COMMIT", "CONFLICT", "CONSTRAINT", "CREATE", "CROSS", "CURRENT_DATE", "CURRENT_TIME", "CURRENT_TIMESTAMP", "DATABASE", "DEFAULT", "DEFERRABLE", "DEFERRED", "DELETE", "DESC", "DETACH", "DISTINCT", "DROP", "EACH", "ELSE", "END", "ESCAPE", "EXCEPT", "EXCLUSIVE", "EXISTS", "EXPLAIN", "FAIL", "FOR", "FOREIGN", "FROM", "FULL", "GLOB", "GROUP", "HAVING", "IF", "IGNORE", "IMMEDIATE", "IN", "INDEX", "INDEXED", "INITIALLY", "INNER", "INSERT", "INSTEAD", "INTERSECT", "INTO", "IS", "ISNULL", "JOIN", "KEY", "LEFT", "LIKE", "LIMIT", "MATCH", "NATURAL", "NO", "NOT", "NOTNULL", "NULL", "OF", "OFFSET", "ON", "OR", "ORDER", "OUTER", "PLAN", "PRAGMA", "PRIMARY", "QUERY", "RAISE", "REFERENCES", "REGEXP", "REINDEX", "RELEASE", "RENAME", "REPLACE", "RESTRICT", "RIGHT", "ROLLBACK", "ROW", "SAVEPOINT", "SELECT", "SET", "TABLE", "TEMP", "TEMPORARY", "THEN", "TO", "TRANSACTION", "TRIGGER", "UNION", "UNIQUE", "UPDATE", "USING", "VACUUM", "VALUES", "VIEW", "VIRTUAL", "WHEN", "WHERE" };
        public static readonly string[] SqliteOperators = new string[] { "||", "*", "/", "%", "", "+", "-", "<<", ">>", "&", "|", "<", "<=", ">", ">=", "=", "==", "!=", "<>", "IN", "AND", "OR" };
        public static readonly string[] SqliteOPChars = new string[] { "/", "'", "%", "&", "(", ")" };

        /// <summary>  
        ///   
        /// 将对象属性转换为key-value对  
        /// </summary>  
        /// <param name="o"></param>  
        /// <returns></returns>  
        public static Dictionary<String, Object> ObjectToMap(Object o)
        {
            Dictionary<String, Object> map = new Dictionary<string, object>();
            if (o != null)
            {
                Type t = o.GetType();
                PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo p in pi)
                {
                    MethodInfo mi = p.GetGetMethod();

                    if (mi != null && mi.IsPublic)
                    {
                        map.Add(p.Name, mi.Invoke(o, new Object[] { }));
                    }
                }
            }
            return map;
        }
        public static bool isDBNameOk(string name)
        {
            if (name == null || name.StartsWith("sqlite_",StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }
            foreach (string str in SqliteKeyWords)
            {
                if (str.Equals(name, StringComparison.CurrentCultureIgnoreCase))
                    return false;
            }
            foreach (string str in SqliteOperators)
            {
                if (name.Contains(str))
                    return false;
            }
            foreach (string str in SqliteOPChars)
            {
                if (name.Contains(str))
                    return false;
            }
            return true;
        }
        public static string sqliteEscape(string keyWord)
        {
            keyWord = keyWord.Replace("/", "//");
            keyWord = keyWord.Replace("'", "''");
            keyWord = keyWord.Replace("[", "/[");
            keyWord = keyWord.Replace("]", "/]");
            keyWord = keyWord.Replace("%", "/%");
            keyWord = keyWord.Replace("&", "/&");
            keyWord = keyWord.Replace("_", "/_");
            keyWord = keyWord.Replace("(", "/(");
            keyWord = keyWord.Replace(")", "/)");
            return keyWord;
        }

        public static string parameterizedSQLEscape(string sqlExp, params object[] vals)
        {
            char[] schs = sqlExp.ToArray();
            StringBuilder sb = new StringBuilder();
            try
            {
                int idx = 0; int cur = 0;
                while (idx < schs.Length)
                {
                    char ch = schs[idx];
                    idx++;
                    if (ch != '@')
                    {
                        sb.Append(ch);
                    }
                    else
                    {
                        while (idx < schs.Length && ch != ' ' && ch != ',' && ch != '(' && ch != ';')
                        {
                            idx++;
                            ch = schs[idx];
                        }
                        object val = vals[cur];
                        if(val==null)
                            sb.Append("NULL");
                        else if (typeof(string).Equals(val) || typeof(Guid).Equals(val)||typeof(DateTime).Equals(val))
                            sb.Append("'").Append(sqliteEscape(val.ToString())).Append("'");
                        else if(typeof(bool).Equals(val))
                            sb.Append((bool)val?1:0);
                        else 
                            sb.Append(sqliteEscape(val.ToString()));
                        cur++;
                    }
                }
            }
            catch (Exception ex)
            {
                sb.Length = 0;
                log.Debug("Error in parameterizedSQLEscape(...)", ex);
                throw new IDCMException("Error in parameterizedSQLEscape(...)");
            }
            return sb.ToString();
        }
        public static string parameterizedSQLEscape(string sqlExp, object obj)
        {
            Dictionary<string, object> valMap = ObjectToMap(obj);
            char[] schs = sqlExp.ToArray();
            StringBuilder fsb = new StringBuilder();
            StringBuilder sb = new StringBuilder();
            try
            {
                int idx = 0; int cur = 0;
                while (idx < schs.Length)
                {
                    char ch = schs[idx];
                    idx++;
                    if (ch != '@')
                    {
                        sb.Append(ch);
                    }
                    else
                    {
                        fsb.Length = 0;
                        while (idx < schs.Length && ch != ' ' && ch != ',' && ch != '(' && ch != ';' && ch != ')')
                        {
                            fsb.Append(ch);
                            idx++;
                            ch = schs[idx];
                        }
                        object val = valMap[fsb.ToString()];
                        if (val == null)
                            sb.Append("NULL");
                        else if (typeof(string).Equals(val) || typeof(Guid).Equals(val) || typeof(DateTime).Equals(val))
                            sb.Append("'").Append(sqliteEscape(val.ToString())).Append("'");
                        else if (typeof(bool).Equals(val))
                            sb.Append((bool)val ? 1 : 0);
                        else
                            sb.Append(sqliteEscape(val.ToString()));
                        cur++;
                    }
                }
            }
            catch (Exception ex)
            {
                sb.Length = 0;
                log.Debug("Error in parameterizedSQLEscape(...)", ex);
                throw new IDCMException("Error in parameterizedSQLEscape(...)");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 对批量执行SQL的返回结果值的全部完成标识判断
        /// </summary>
        /// <param name="exeResults"></param>
        /// <returns></returns>
        public static bool checkExecuteOk(params int[] exeResults)
        {
            if (exeResults == null || exeResults.Length == 0)
                return false;
            foreach (int res in exeResults)
            {
                if (res < 0)
                    return false;
            }
            return true;
        }
        public static string toSQLiteTypeDef(Type clrType)
        {
            string type = null;
            if (clrType == typeof(Boolean) || clrType == typeof(Byte) || clrType == typeof(UInt16) || clrType == typeof(SByte) || clrType == typeof(Int16) || clrType == typeof(Int32))
            {
                type = "integer";
            }
            else if (clrType == typeof(UInt32) || clrType == typeof(Int64))
            {
                type = "bigint";
            }
            else if (clrType == typeof(Single) || clrType == typeof(Double) || clrType == typeof(Decimal))
            {
                type = "double";
            }
            else if (clrType == typeof(String))
            {
                type = "nvarchar";
            }
            else if (clrType == typeof(DateTime))
            {
                type = "datetime";
            }
            else if (clrType.IsEnum)
            {
                type = "integer";
            }
            else if (clrType == typeof(byte[]))
            {
                type = "blob";
            }
            else if (clrType == typeof(Guid))
            {
                type = "nvarchar(36)";
            }
            else
            {
                throw new NotSupportedException("Unknown Type for ModelToDBSQL(...). @clrTYpe=" + clrType);
            }
            return type;
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
