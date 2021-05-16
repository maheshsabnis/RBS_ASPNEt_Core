using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace Core_NewMVC.CustomSessionProvider
{
	public static class CustomSessoinExtensions
	{
		/// <summary>
		/// An Extension Method for storing CLR object in Session State in JSON Form
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="session">The Session Contract</param>
		/// <param name="key">The Key of the Session</param>
		/// <param name="value">The Value to be Stored in Session</param>
		public static void SetEntity<T>(this ISession session, string key, T value)
		{
			session.SetString(key, JsonSerializer.Serialize(value));
		}

		public static T GetEntity<T>(this ISession session, string key)
		{
			var data = session.GetString(key);
			if (data == null)
			{
				return default(T);   // return an empty object
			}
			else
			{
				return JsonSerializer.Deserialize<T>(data);
			}
		}
	}
}
