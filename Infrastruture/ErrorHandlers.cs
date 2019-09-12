using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;

namespace BookStore.Infrastruture
{
    public interface IErrorHandler
    {
        void Error(Exception exception, string message, string controller, string action);
    }

    public class LogHandlers : IErrorHandler
    {
        public void Error(Exception exception, string message, string controller, string action)
        {
            string logDir = "~/Content/Log";
            
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(logDir)))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(logDir));
            }
            
            using(StreamWriter sw = new StreamWriter(HttpContext.Current.Server.MapPath(logDir + "/" + DateTime.Now.ToString("yyyyMMdd") + ".log"), true))
            {
                sw.WriteLine("[" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "] 例外狀況 : (" + exception.Source.ToString() + ") - {" + controller + " | " + action + "} " + message.Replace(Environment.NewLine, ""));
                sw.Close();
            }
        }
    }
}