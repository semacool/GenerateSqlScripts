using System;
using System.Collections.Generic;
using System.Text;

namespace CreateScripts
{
    public class FileTemplateInfo
    {
        public static FileTemplateInfo CreateInfo(string path, string template)
        {
            var file = new FileTemplateInfo();
            file.Path = path;
            file.Template = template;
            return file;
        }

        public string Path { get; set; }
        public string Template { get; set; }
    }
}
