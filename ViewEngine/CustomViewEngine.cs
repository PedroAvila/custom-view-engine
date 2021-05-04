using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomViewEngine.ViewEngine
{
    public class CustomViewEngine : VirtualPathProviderViewEngine
    {
        public CustomViewEngine()
        {
            ViewLocationFormats = new string[]
            {
                "~/Views/{1}{0}.html",
                "~/Views/{0}.html"
            };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            var view = new ViewEngine.CustomView();
            view.ViewFilePath = controllerContext.HttpContext.Server.MapPath(partialPath);
            return view;
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            var view = new ViewEngine.CustomView();
            view.ViewFilePath = controllerContext.HttpContext.Server.MapPath(viewPath);
            return view;
        }
    }
}