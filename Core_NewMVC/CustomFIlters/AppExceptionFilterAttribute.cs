using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_NewMVC.CustomFIlters
{
	public class AppExceptionFilterAttribute : ExceptionFilterAttribute
	{
		private readonly IModelMetadataProvider modelMetadataProvider;
		public AppExceptionFilterAttribute(IModelMetadataProvider modelMetadataProvider)
		{
			this.modelMetadataProvider = modelMetadataProvider;
		}

		/// <summary>
		/// Method  to handle exception
		/// </summary>
		/// <param name="context"></param>
		public override void OnException(ExceptionContext context)
		{
			// handle the exception to make sure that MVC knos the current request
			// processed 
			context.ExceptionHandled = true;
			// read an exception message message
			string msg = context.Exception.Message;

			ViewResult viewResult = new ViewResult()
			{ 
			   ViewName ="Error"
			};

			ViewDataDictionary dictionary = new ViewDataDictionary(modelMetadataProvider, context.ModelState);
			dictionary["ControllerName"] = context.RouteData.Values["controller"].ToString();
			dictionary["ActionName"] = context.RouteData.Values["action"].ToString();
			dictionary["Message"] = msg;

			viewResult.ViewData = dictionary;

			// set the result
			context.Result = viewResult;
		}
	}
}
