using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookStore.Repository;
using BookStore.Models;
using BookStore.Services;

namespace BookStore.Infrastruture
{
    public class CustomModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (typeof(Entity).IsAssignableFrom(bindingContext.ModelType))
            {
                if (controllerContext.RouteData.Values["action"].ToString().IndexOf("Create") == -1 && controllerContext.HttpContext.Request.HttpMethod != "Post")
                {
                    ValueProviderResult value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

                    if (value != null)
                    {
                        if (!string.IsNullOrEmpty(value.AttemptedValue))
                        {
                            int id;
                            if (int.TryParse(value.AttemptedValue, out id))
                            {
                                Type repositoryType = typeof(Repository<>).MakeGenericType(bindingContext.ModelType);
                                var _repository = (IRepository)ServiceLocator.Resolve(repositoryType);
                                Entity data = (Entity)_repository.GetById(id);
                                return data;
                            }
                        }
                    }
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}