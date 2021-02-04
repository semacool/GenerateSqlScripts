using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace CreateScripts
{

	public class FileConnector
	{
		public T ReadFile<T>(IParser<T> parser)
		{
			var source = parser.File;
			if (!File.Exists(source.Path))
			{
				var indexFile = source.Path.LastIndexOf('/');
				var foldersPath = source.Path.Substring(0, indexFile);
				Directory.CreateDirectory(foldersPath);
				File.WriteAllText(source.Path, source.Template);
			}
			var text = File.ReadAllText(source.Path);
			var result = parser.ParseText(text);
			return result;
		}


		public virtual void SaveFiles(Dictionary<string, string> pathesContent)
		{
			foreach (var item in pathesContent)
			{
				if(item.Value != "" || !string.IsNullOrWhiteSpace(item.Value))
                {
					var path = "Working/"+item.Key;
					var indexFile = path.LastIndexOf('/');
					Directory.CreateDirectory(path.Substring(0,indexFile));
					File.WriteAllText(path, item.Value);
                }
			}
		}
	}
}
