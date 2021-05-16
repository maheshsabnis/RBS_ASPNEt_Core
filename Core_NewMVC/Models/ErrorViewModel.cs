using System;

namespace Core_NewMVC.Models
{
	public class ErrorViewModel
	{
		public string RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

		public string ControllerName { get; set; }

		public string ActionName { get; set; }
		public Exception Exception { get; set; }
	}
}
