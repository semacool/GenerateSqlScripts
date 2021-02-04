using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlTypes;

namespace CreateScripts
{
    class SqlBuilder
    {
        private string _tableName { get; set; }
        public string Sql { get; set; }

        public SqlBuilder(string tableName) 
        {
            _tableName = tableName;
            Sql = string.Empty;
        }

        public string GetSql(Dictionary<string, object> values)
        {
            var sql = $"IF NOT EXISTS (SELECT Id FROM {_tableName} WHERE Id = '{values["Id"]}')\n";
            var sqlInsert = GetSqlInsert(values);
            sql += sqlInsert;
            Sql += sql + "\n";
            return sql;
        }

        private string GetSqlInsert(Dictionary<string, object> values) 
        {
            var sql = string.Empty;

            var columnNames = values.Keys;
            var columnValues = values.Values;

            var columnNamesString = string.Join(',', columnNames);

            var columnValuesStrings = new List<string>();
            foreach (var value in columnValues)
                columnValuesStrings.Add(GetSqlValue(value));

            var columnValuesString = string.Join(',', columnValuesStrings);


            sql = $"INSERT INTO {_tableName }({columnNamesString}) VALUES({columnValuesString})\n";

            return sql;
        }

        private string GetSqlValue<T>(T value) 
        {
            var sql = string.Empty;
            var type = value.GetType().Name;
            switch (type)
            {
                case "DateTime":
                    var date = value as DateTime?;
                    sql = $"'{date:yyyy-MM-dd hh:mm:ss.fffffff}'";
                    break;
                case "Boolean":
                    sql = $"CONVERT(bit,'{value}')";
                    break;
                case "DBNull":
                    sql = "NULL";
                    break;
                case "Guid":
                    sql = $"'{value}'";
                    break;
                default:
                    sql = $"{value}";
                    break;
            }

            return sql;
        }
    }
}
