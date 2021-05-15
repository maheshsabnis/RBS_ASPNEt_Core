using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_NewMVC.Controllers
{
	public class RoleController : Controller
	{
		private readonly RoleManager<IdentityRole> _role;

		public RoleController(RoleManager<IdentityRole> role)
		{
			_role = role;
		}
		public async Task<IActionResult> Index()
		{
			var roles = await _role.Roles.ToListAsync();
			return View(roles);
		}

		public IActionResult Create()
		{
			return View(new IdentityRole());
		}

		[HttpPost]
		public async Task<IActionResult> Create(IdentityRole role)
		{
			await _role.CreateAsync(role);
			return RedirectToAction("Index");
		}
	}
}
