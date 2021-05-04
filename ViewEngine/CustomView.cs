using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }
}