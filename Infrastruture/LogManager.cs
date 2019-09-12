using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Infrastruture
{
    public class LogManager
    {
        private static IErrorHandler _iErrorHandler;
        public static IErrorHandler GetHandler(string errorType)
        { 
            switch(errorType)
            {
                case "Log":
                    _iErrorHandler = new LogHandlers();
                    break;
                default:
                    break;
            }
            return _iErrorHandler;
        }
    }
}