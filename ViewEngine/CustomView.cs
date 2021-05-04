using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CustomViewEngine.ViewEngine
{
    public class CustomView : IView
    {
        public string ViewFilePath { get; set; }
        public object Model { get; set; }

        public void Render(ViewContext viewContext, TextWriter writer)
        {
            Model = viewContext.ViewData.Model;
            var ViewFileContent = File.ReadAllText(ViewFilePath);
            string htmlContent = GetHTML(ViewFileContent);
            writer.Write(htmlContent);
        }

        private string GetHTML(string viewFileContent)
        {
            string result = viewFileContent;

            string patternControl =
                "<" +
                "(?<Control>[a-z]+)" +
                ".*" +
                "?/>";
            result = Regex.Replace(viewFileContent, patternControl, ControlCodeGenerator, RegexOptions.IgnoreCase);

            return result;
        }

        private string ControlCodeGenerator(Match currentMatch)
        {
            string value = currentMatch.Value;
            string control = currentMatch.Result("${Control}");
            switch (control.ToLower())
            {
                case "datagrid":
                    if (Model != null)
                    {
                        value = RenderDataGrid(value);
                    }
                    break;
                case "time":
                    value = DateTime.Now.ToShortTimeString();
                    break;
                default:
                    break;
            }
            return value;
        }
    }
}