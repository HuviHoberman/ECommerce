using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClassLibrary1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace ECommerce.Controllers
{
	public class UserController : Controller
	{
		private IHostingEnvironment _environment;
		private string _connectionString;

		public UserController(IHostingEnvironment environment,
			IConfiguration configuration)
		{
			_environment = environment;
			_connectionString = configuration.GetConnectionString("ConStr");
		}

		[Authorize]
		public IActionResult Categories()
		{
			ItemsManager mgr = new ItemsManager(_connectionString);
			List<Category> categories = mgr.GetCategories();
			return View(categories);
		}

		[HttpPost]
		public IActionResult Categories(Category category)
		{
			UserManager mgr = new UserManager(_connectionString);
			mgr.AddCategory(category);
			return Redirect("/user/categories");
		}

		[Authorize]
		public IActionResult ItemsByCategory(int id)
		{
			ItemsManager mgr = new ItemsManager(_connectionString);
			ItemsViewModel vm = new ItemsViewModel
			{
				Items = mgr.GetItems(id),
				Category = mgr.GetCategoryForId(id)
			};
			return View(vm);
		}

		[HttpPost]
		public IActionResult ItemsByCategory(string name, decimal price, string description, IFormFile image, int categoryId)
		{
			string fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
			string fullPath = Path.Combine(_environment.WebRootPath, "itemsImages", fileName);
			using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew))
			{
				image.CopyTo(stream);
			}

			Item item = new Item
			{
				Name = name,
				Price = price,	
				Description = description,
				Image = fileName,
				CategoryId = categoryId
			};
			UserManager mgr = new UserManager(_connectionString);
			mgr.AddItem(item);
			return Redirect($"/user/itemsByCategory?id={item.CategoryId}");
		}

		public IActionResult SignUp()
		{
			return View();
		}

		[HttpPost]
		public IActionResult SignUp(User user)
		{
			UserManager mgr = new UserManager(_connectionString);
			user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
			mgr.AddUser(user);
			return Redirect("/user/logIn");
		}


		public IActionResult LogIn()
		{
			return View();
		}

		[HttpPost]
		public IActionResult LogIn(string email, string password)
		{
			UserManager mgr = new UserManager(_connectionString);
			string passwordFromDB = mgr.GetPasswordForEmail(email);
			if (passwordFromDB == null)
			{
				return Redirect("/user/logIn");
			}

			var claims = new List<Claim>
				{
					new Claim("user", email)
				};
			HttpContext.SignInAsync(new ClaimsPrincipal(
				new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();

			return Redirect("/user/categories");
		}

		public IActionResult LogOut()
		{
			HttpContext.SignOutAsync().Wait();
			return Redirect("/home/index");
		}
	}
}