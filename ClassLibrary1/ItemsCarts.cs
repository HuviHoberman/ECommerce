using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ClassLibrary1
{
	public class Category
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class Item
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public string Image { get; set; }
		public int CategoryId { get; set; }

	}

	public class ItemsViewModel
	{
		public List<Item> Items { get; set; }
		public Category Category { get; set; }
	}

	public class ItemViewModel
	{
		public Item Item { get; set; }
		public List<Category> Categories { get; set; }
	}

	public class CartItem : Item
	{
		public int Quantity { get; set; }
	}

	public class AddToCart
	{
		public int ItemId { get; set; }
		public int Quantity { get; set; }
		public bool KeepShopping { get; set; }
	}

	public class Cart
	{
		public int Id { get; set; }
		public DateTime DateCreated { get; set; }
	}

	public class ItemsManager
	{
		private readonly string _connectionString;

		public ItemsManager(string connnectionString)
		{
			_connectionString = connnectionString;
		}


		public List<Category> GetCategories()
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM Categories";
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			List<Category> categories = new List<Category>();
			while (reader.Read())
			{
				categories.Add(new Category
				{
					Id = (int)reader["Id"],
					Name = (string)reader["Name"]
				});
			}
			connection.Close();
			connection.Dispose();
			return categories;
		}

		public Category GetCategoryForId(int id)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();
			command.CommandText = "SELECT TOP 1 * FROM Categories WHERE Id = @id";
			command.Parameters.AddWithValue("@id", id);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			Category category = new Category();
			while (reader.Read())
			{
				category.Id = id;
				category.Name = (string)reader["Name"];
			}
			connection.Close();
			connection.Dispose();
			return category;
		}

		public List<Item> GetItems(int categoryId)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();
			command.CommandText = "SELECT * FROM Items WHERE CategoryId = @categoryId";
			command.Parameters.AddWithValue("@categoryId", categoryId);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			List<Item> items = new List<Item>();
			while (reader.Read())
			{
				items.Add(new Item
				{
					Id = (int)reader["Id"],
					Name = (string)reader["Name"],
					Price = (decimal)reader["Price"],
					Description = (string)reader["Description"],
					Image = (string)reader["Image"],
					CategoryId = (int)reader["CategoryId"]
				});
			}
			connection.Close();
			connection.Dispose();
			return items;
		}

		public Item GetItemForId(int id)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();
			command.CommandText = "SELECT TOP 1* FROM Items WHERE Id = @Id";
			command.Parameters.AddWithValue("@Id", id);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			Item item = new Item();
			while (reader.Read())
			{
				item.Id = (int)reader["Id"];
				item.Name = (string)reader["Name"];
				item.Price = (decimal)reader["Price"];
				item.Description = (string)reader["Description"];
				item.Image = (string)reader["Image"];
				item.CategoryId = (int)reader["CategoryId"];
			}
			connection.Close();
			connection.Dispose();
			return item;
		}

		public int AddCartGetId()
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();
			command.CommandText = "INSERT INTO Carts VALUES (@date);SELECT SCOPE_IDENTITY()";
			command.Parameters.AddWithValue("@date", DateTime.Now);
			connection.Open();
			int cartId = (int)(decimal)command.ExecuteScalar();
			connection.Close();
			connection.Dispose();
			return cartId;
		}

		public void AddToCart(AddToCart addToCart, int cartId)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();
			command.CommandText = "INSERT INTO CartsItems VALUES (@cartId, @itemId, @quantity)";
			command.Parameters.AddWithValue("@cartId", cartId);
			command.Parameters.AddWithValue("@itemId", addToCart.ItemId);
			command.Parameters.AddWithValue("@quantity", addToCart.Quantity);
			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
			connection.Dispose();
		}

		public List<CartItem> GetCartItems(int cartId)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();
			command.CommandText = "SELECT* FROM CartsItems C JOIN Items I ON C.ItemId = I.Id WHERE C.CartId = @cartId";
			command.Parameters.AddWithValue("@cartId", cartId);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			List<CartItem> items = new List<CartItem>();
			while (reader.Read())
			{
				items.Add(new CartItem
				{
					Id = (int)reader["Id"],
					Name = (string)reader["Name"],
					Price = (decimal)reader["Price"],
					Description = (string)reader["Description"],
					Image = (string)reader["Image"],
					CategoryId = (int)reader["CategoryId"],
					Quantity = (int)reader["Quantity"]
				});
			}
			connection.Close();
			connection.Dispose();
			return items;
		}

		public void DeleteFromCart(int cartId, int itemId)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();
			command.CommandText = "DELETE FROM CartsItems WHERE CartId = @cartId AND ItemId = @itemId";
			command.Parameters.AddWithValue("@cartId", cartId);
			command.Parameters.AddWithValue("@itemId", itemId);
			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
			connection.Dispose();
		}

		public void UpdateCart(int quantity, int cartId, int itemId)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();
			command.CommandText = "UPDATE CartsItems SET Quantity = @quantity WHERE CartId=@cartId AND ItemId=@itemId";
			command.Parameters.AddWithValue("@quantity", quantity);
			command.Parameters.AddWithValue("@cartId", cartId);
			command.Parameters.AddWithValue("@itemId", itemId);
			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
			connection.Dispose();
		}
	}
}
