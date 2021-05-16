using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Core_NewMVC.CustomFIlters
{
	public class LogActonFilterAttribute : ActionFilterAttribute, IActionFilter
	{

		private void LogRequest(string status, RouteData route)
		{
			string controllerName = route.Values["controller"].ToString();
			string actionName = route.Values["action"].ToString();

			Debug.WriteLine($"Current State of Reques = {status} " +
				$" In Controller Name   {controllerName}  " +
				$" In Action Name {actionName}");
		}


		public override void OnActionExecuting(ActionExecutingContext context)
		{
			LogRequest("On Action Executing ", context.RouteData);
		}

		public override void OnActionExecuted(ActionExecutedContext context)
		{
			LogRequest("On Action Executed ", context.RouteData);
		}

		public override void OnResultExecuting(ResultExecutingContext context)
		{
			LogRequest("On Result Executing ", context.RouteData);
		}

		public override void OnResultExecuted(ResultExecutedContext context)
		{
			LogRequest("On Result Executed ", context.RouteData);
		}
	}
}
