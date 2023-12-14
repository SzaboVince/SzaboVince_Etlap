using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Etlap
{
	public class FoodService
	{
		MySqlConnection connection;
		public FoodService()
		{
			MySqlConnectionStringBuilder builder= new MySqlConnectionStringBuilder();
			builder.Server = "localhost";
			builder.Port = 3306;
			builder.UserID = "root";
			builder.Password = "";
			builder.Database = "etlapdb";

			connection = new MySqlConnection(builder.ConnectionString);
		}

		public bool Create(Food food)
		{
			OpenConnection();
			string sql = "INSERT INTO etlap(nev,leiras,ar,kategoria) VALUES (@name,@desc,@price,@category)";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			command.Parameters.AddWithValue("@name", food.Name);
			command.Parameters.AddWithValue("@desc", food.Description);
			command.Parameters.AddWithValue("@price", food.Price);
			command.Parameters.AddWithValue("@category", food.Category);

			int affectedRows = command.ExecuteNonQuery();

			CloseConnection();
			return affectedRows == 1;

		}

		public List<Food> GetAll()
		{
			List<Food> foods = new List<Food>();
			OpenConnection();
			string sql = "SELECT * FROM etlap";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			using (MySqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					Food food = new Food();
					food.Id = reader.GetInt32("id");
					food.Name = reader.GetString("nev");
					food.Description = reader.GetString("leiras");
					food.Price = reader.GetInt32("ar");
					food.Category = reader.GetString("kategoria");
					foods.Add(food);
				}
			}
			CloseConnection();
			return foods;
		}

		public bool Update(int id, Food newValues)
		{
			OpenConnection();
			string sql = @"UPDATE etlap
                            SET nev = @name, 
                                leiras = @desc, 
                                ar = @price, 
                                kategoria = @cat 
                            WHERE id = @id";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			command.Parameters.AddWithValue("@name", newValues.Name);
			command.Parameters.AddWithValue("@desc", newValues.Description);
			command.Parameters.AddWithValue("@price", newValues.Price);
			command.Parameters.AddWithValue("@cat", newValues.Category);
			command.Parameters.AddWithValue("@id", id);
			int affectedRows = command.ExecuteNonQuery();
			CloseConnection();
			return affectedRows == 1;
		}

		public bool Delete(int id)
		{
			OpenConnection();
			string sql = "DELETE FROM etlap WHERE id = @id";
			MySqlCommand command = connection.CreateCommand();
			command.CommandText = sql;
			command.Parameters.AddWithValue("@id", id);
			int affectedRows = command.ExecuteNonQuery();
			CloseConnection();
			return affectedRows == 1;
		}

		private void CloseConnection()
		{
			if (connection.State == System.Data.ConnectionState.Open)
			{
				connection.Close();
			}
		}

		private void OpenConnection()
		{
			if (connection.State != System.Data.ConnectionState.Open)
			{
				connection.Open();
			}
		}
	}
}
