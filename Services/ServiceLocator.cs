using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Services
{
    public static class ServiceLocator
    {
        public static object Resolve(Type repositoryType)
        {
            return Activator.CreateInstance(repositoryType);
        }
    }
}