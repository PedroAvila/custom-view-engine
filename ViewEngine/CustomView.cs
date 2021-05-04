using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        private string RenderDataGrid(string value)
        {
            string pattern =
                "{Binding\\s+" +
                "(?<Items>[a-z_][a-z0-9_]*)" +
                "\\s*}";
            var r = new Regex(pattern, RegexOptions.IgnoreCase);
            var m = r.Match(value);
            if (m.Success)
            {
                string bindingExpression = value;
                string itemsPropertyName = m.Result("${Items}");
                var sb = new StringBuilder();
                try
                {
                    var items = Model.GetType().GetProperty(itemsPropertyName).GetValue(Model) as IEnumerable;
                    if (items != null)
                    {
                        sb.Append("<table class=\"DataGrid\">");
                        string[] propertyNames = null;
                        bool alt = false;
                        foreach (var item in items)
                        {
                            if (propertyNames == null)
                            {
                                propertyNames = item.GetType().GetProperties().Select(p => p.Name).ToArray();
                                sb.Append("<tr>");
                                foreach (var e in propertyNames)
                                {
                                    sb.AppendFormat("<th>{0}</th>", e);
                                }
                                sb.Append("</tr>");
                            }
                            sb.AppendFormat("<tr {0}>", alt ? "class=\"Alt\"" : "");
                            foreach (var e in propertyNames)
                            {
                                sb.AppendFormat("<td>{0}</td>", item.GetType().GetProperty(e).GetValue(item));
                            }
                            sb.Append("</tr>");
                            alt = !alt;
                        }
                        sb.Append("</table>");
                        value = sb.ToString();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return value;
        }
    }
}