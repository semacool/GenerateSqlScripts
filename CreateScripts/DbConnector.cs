using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace CreateScripts
{
    class DbConnectorRight
    {

        public static string SysEntitySchemaRecordDefRight = "SysEntitySchemaRecordDefRight";
        public static string SysEntitySchemaOperationRight = "SysEntitySchemaOperationRight";
        public static string SysAdminUnit = "SysAdminUnit";

        public string _connectionString { get; set; }

        public DbConnectorRight(string connectionString) 
        {
            _connectionString = connectionString;
        }

        public List<string> GetColumns(string nameTable)
        {
            List<string> nameColumns = new List<string>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"select COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '{nameTable}'", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var nameColumn = reader.GetString(0);
                        nameColumns.Add(nameColumn);
                    }
                }
            }
            return nameColumns;
        }

        public string GetUIdByNameSchema(string nameSchema,string namePackage)
        {
            string schemaUId = string.Empty;
            var sql = $"select ss.Uid from SysSchema ss join SysPackage sp ON sp.Id = ss.SysPackageId where ss.Name = '{nameSchema}' and sp.Name = '{namePackage}'";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        schemaUId = reader.GetGuid(0).ToString();
                    }
                }
            }
            return schemaUId;
        }

        public List<Dictionary<string,object>> GetTableRowsWithFilter(string nameTable, string UId)
        {
            var tableRows = new List<Dictionary<string, object>>();
            List<string> columnsName = GetColumns(nameTable);

            string columnsString = string.Join(',', columnsName);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"select {columnsString} from {nameTable} where SubjectSchemaUId = '{UId}'", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader["Id"];
                        var values = new Dictionary<string, object>();
                        
                        foreach(var colName in columnsName) 
                        {
                            var value = reader[colName];
                            values.Add(colName, value);
                        }

                        tableRows.Add(values);

                    }
                }
            }
            return tableRows;
        }
    }
}
