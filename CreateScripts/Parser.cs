using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace CreateScripts
{
    public interface IParser<T>
    {
        FileTemplateInfo File { get; }
        T ParseText(string text);
    }

    class ParserSchemaNames: IParser<Dictionary<string, List<string>>>
    {

        public FileTemplateInfo File => FileTemplateInfo.CreateInfo($"Working/SchemaNames.json", "[{\"name\":\"ExamplePackage\",\"schemas\":[\"ExamplePackageShema1\",\"ExamplePackageShema2\"]}]");

        /// <summary>
        /// Возращает Dictionary<string, List<string>>
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public Dictionary<string, List<string>> ParseText(string text)
        {
            var shemaNames = new Dictionary<string, List<string>>();

            var json = JsonDocument.Parse(text);

            foreach (var item in json.RootElement.EnumerateArray())
            {
                var packegeName = item.GetProperty("name").GetString();
                var schemasName = item.GetProperty("schemas").EnumerateArray().Select(e => e.GetString()).ToList();
                shemaNames.Add(packegeName, schemasName);

            }

            return shemaNames;
        }
    }

    class ParserConnectionString : IParser<string>
    {

        public FileTemplateInfo File => FileTemplateInfo.CreateInfo($"Working/ConnectionString.txt", "Здесь должна быть строка подключения к sqlServer");

        /// <summary>
        /// Возращает string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string ParseText(string text)
        {
            return text;
        }
    }

}
