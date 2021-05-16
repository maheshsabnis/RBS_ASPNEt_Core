using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core_NewMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Core_NewMVC.CustomSessionProvider;
using Core_NewMVC.CustomFIlters;
namespace Core_NewMVC.Controllers
{
	[Authorize] // this will use the Authorization Middleware

	 [LogActonFilter]
	public class DepartmentController : Controller
	{
		private readonly CompanyContext context;

		public DepartmentController(CompanyContext context)
		{
			this.context = context;
		}

		// GET: DepartmentController
		//[Authorize(Roles="Admin,Manager,Clerk")]
		//[Authorize(Policy ="ReadPolicy")]
		public ActionResult Index()
		{
			var depts = context.Department.ToList();
			return View(depts);
		}

		// GET: DepartmentController/Details/5
		public ActionResult Details(int id)
		{

			return View();
		}

		// GET: DepartmentController/Create
		//[Authorize(Roles = "Admin,Manager")]
	//	[Authorize(Policy = "WritePolicy")]
		public ActionResult Create()
		{
			var dept = new Department();
			return View(dept);
		}

		// POST: DepartmentController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Department dept)
		{
			try
			{
				if (ModelState.IsValid)
				{
				
					context.Department.Add(dept);
					context.SaveChanges();
					return RedirectToAction(nameof(Index));
				}
				else 
				{
					return View(); // say on same view with error messages
				}
			}
			catch
			{
				return View();
			}
		}

		// GET: DepartmentController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: DepartmentController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: DepartmentController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: DepartmentController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}


		public IActionResult ShowEmployees(int id)
		{
			// HttpContext.Session.SetInt32("DeptNo",id);

			var dept = context.Department.Find(id);

			HttpContext.Session.SetEntity<Department>("dept",dept);

			return RedirectToAction("Index", "Employee");
		}
	}
}
