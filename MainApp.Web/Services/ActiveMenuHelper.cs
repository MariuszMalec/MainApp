using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Web.Mvc;

namespace MainApp.Web.Services
{
    public static class ActiveMenuHelper
    {
        public static System.Web.Mvc.MvcHtmlString IsActive(this Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper htmlHelper, string action, string controller, string activeClass = "class=\"active\"", string inActiveClass = "")
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeAction = routeData.Values["action"].ToString();
            var routeController = routeData.Values["controller"].ToString();

            var returnActive = (controller == routeController && action == routeAction);

            return new System.Web.Mvc.MvcHtmlString(returnActive ? activeClass : inActiveClass);
        }

        public static System.Web.Mvc.MvcHtmlString IsActiveController(this Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper htmlHelper, string controller, string activeClass = "class=\"active\"", string inActiveClass = "")
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeController = routeData.Values["controller"].ToString();

            var returnActive = (controller == routeController);

            return new System.Web.Mvc.MvcHtmlString(returnActive ? activeClass : inActiveClass);
        }

        public static System.Web.Mvc.MvcHtmlString MenuLink(this Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string areaName)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var routeAction = routeData.Values["action"].ToString();
            var routeController = routeData.Values["controller"].ToString();
            var currentArea = routeData.Values["area"].ToString();

            var builder = new System.Web.Mvc.TagBuilder("li")
            {
                InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName).ToString()
                //InnerHtml = "<a href=\"" + new UrlHelper(htmlHelper.ViewContext.HttpContext).Action(actionName, controllerName, new { area = areaName }).ToString() + "\">" + linkText + "</a>"
            };

            if (String.Equals(controllerName, routeController, StringComparison.CurrentCultureIgnoreCase) && String.Equals(actionName, routeAction, StringComparison.CurrentCultureIgnoreCase))
                builder.AddCssClass("active");

            return new MvcHtmlString(builder.ToString());

        }

        public static String NavActive(this Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelper htmlHelper,
                      string actionName,
                      string controllerName)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var controller = routeData.Values["controller"].ToString();
            var action = routeData.Values["action"].ToString();

            if (controllerName == controller && action == actionName)
                return "active";
            else
                return String.Empty;
        }
    }
}
