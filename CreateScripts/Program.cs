using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace CreateScripts
{
    class Program
    {
        static private FileConnector _file;
        static private DbConnectorRight _db;

        static void Main(string[] args)
        {
            try
            {

                _file = new FileConnector();
                var connectionString = _file.ReadFile(new ParserConnectionString());
                _db = new DbConnectorRight(connectionString);

                var packages = _file.ReadFile(new ParserSchemaNames());
                var schemas = new Dictionary<string, string>();
                foreach (var packageName in packages)
                {
                    foreach (var schemaName in packageName.Value)
                    {
                        var schemaUId = _db.GetUIdByNameSchema(schemaName, packageName.Key);
                        if(schemaUId != string.Empty)
                        {
                            schemas.Add(schemaName, schemaUId);
                        }
                        else
                        {
                            Console.WriteLine($"Схема {schemaName} не найдена");
                        }
                    }
                }
                var pathesContents = new Dictionary<string, string>();
                foreach (var schema in schemas)
                {
                    SavePathContent(DbConnectorRight.SysEntitySchemaOperationRight, schema, pathesContents);
                    SavePathContent(DbConnectorRight.SysEntitySchemaRecordDefRight, schema, pathesContents);
                }
                _file.SaveFiles(pathesContents);
                Console.WriteLine("Выполнено без ошибок");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        static void SavePathContent(string tableName, KeyValuePair<string, string> schema, Dictionary<string, string> pathedContents)
        {
            var rows = _db.GetTableRowsWithFilter(tableName, schema.Value);
            var sql = new SqlBuilder(tableName);
            foreach (var row in rows)
            {
                sql.GetSql(row);
            }
            pathedContents.Add($"{tableName}/{schema.Key}.sql", sql.Sql);
        }

    }
}
