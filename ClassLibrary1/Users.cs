using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ClassLibrary1
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}

	public class UserManager
	{
		private readonly string _connectionString;

		public UserManager(string connnectionString)
		{
			_connectionString = connnectionString;
		}

		public void AddUser(User user)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();
			command.CommandText = "INSERT INTO Users VALUES (@name, @email, @password)";
			command.Parameters.AddWithValue("@name", user.Name);
			command.Parameters.AddWithValue("@email", user.Email);
			command.Parameters.AddWithValue("@password", user.Password);
			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
			connection.Dispose();
		}

		public string GetPasswordForEmail (string email)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();
			command.CommandText = "SELECT TOP 1 * FROM Users WHERE Email = @email";
			command.Parameters.AddWithValue("@email", email);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			string password = "";
			while (reader.Read())
			{
				password = (string)reader["Password"];
			}
			connection.Close();
			connection.Dispose();
			return password;

		}

		public void AddCategory(Category category)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();
			command.CommandText = "INSERT INTO Categories VALUES (@name)";
			command.Parameters.AddWithValue("@name", category.Name);
			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
			connection.Dispose();
		}

		public void AddItem(Item item)
		{
			SqlConnection connection = new SqlConnection(_connectionString);
			SqlCommand command = connection.CreateCommand();
			command.CommandText = "INSERT INTO Items VALUES (@name, @price, @description, @image, @categoryId)";
			command.Parameters.AddWithValue("@name", item.Name);
			command.Parameters.AddWithValue("@price", item.Price);
			command.Parameters.AddWithValue("@description", item.Description);
			command.Parameters.AddWithValue("@image", item.Image);
			command.Parameters.AddWithValue("@categoryId", item.CategoryId);
			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
			connection.Dispose();
		}
	}
}
