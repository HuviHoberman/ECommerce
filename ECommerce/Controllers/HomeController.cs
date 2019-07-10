using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Models;
using Microsoft.Extensions.Configuration;
using ClassLibrary1;
using Microsoft.AspNetCore.Http;


namespace ECommerce.Controllers
{
	public class HomeController : Controller
	{
		private string _connectionString;

		public HomeController(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("ConStr");
		}
		public IActionResult Index(int categoryId)
		{
			ItemsManager mgr = new ItemsManager(_connectionString);
			List<Category> categories = mgr.GetCategories();
			return View(categories);
		}

		public IActionResult GetItems(int categoryId)
		{
			ItemsManager mgr = new ItemsManager(_connectionString);
			List<Item> items = mgr.GetItems(categoryId);
			return Json(items);
		}


		public IActionResult Item(int id)
		{
			ItemsManager mgr = new ItemsManager(_connectionString);
			ItemViewModel vm = new ItemViewModel
			{
				Item = mgr.GetItemForId(id),
				Categories = mgr.GetCategories()
			};
			return View(vm);
		}

		[HttpPost]
		public IActionResult AddToCart(AddToCart addToCart)
		{
			ItemsManager mgr = new ItemsManager(_connectionString);

			string cartId = HttpContext.Session.GetString("cartId");
			if (cartId == null)
			{
				cartId = mgr.AddCartGetId().ToString();
				HttpContext.Session.SetString("cartId", cartId);
			}
			mgr.AddToCart(addToCart, int.Parse(cartId));
			if (!addToCart.KeepShopping)
			{
				return Redirect($"/home/viewCart?cartId={cartId}");
			}
			return Redirect("/");
		}

		public IActionResult ViewCart(int cartId)
		{
			return View(cartId);
		}

		[HttpPost]
		public IActionResult GetCartItems(int cartId)
		{
			ItemsManager mgr = new ItemsManager(_connectionString);
			List<CartItem> items = mgr.GetCartItems(cartId);
			return Json(items);
		}

		[HttpPost]
		public IActionResult DeleteItem(int cartId, int itemId)
		{
			ItemsManager mgr = new ItemsManager(_connectionString);
			mgr.DeleteFromCart(cartId, itemId);
			return Json(GetCartItems(cartId));
		}

		[HttpPost]
		public IActionResult UpdateItem(int quantity, int cartId, int itemId)
		{
			ItemsManager mgr = new ItemsManager(_connectionString);
			mgr.UpdateCart(quantity, cartId, itemId);
			return Json(GetCartItems(cartId));
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
