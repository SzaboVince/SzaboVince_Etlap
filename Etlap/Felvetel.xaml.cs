using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Etlap
{
	/// <summary>
	/// Interaction logic for Felvetel.xaml
	/// </summary>
	public partial class Felvetel : Window
	{
		private FoodService foodService;
		private Food food;
		public Felvetel(FoodService foodService)
		{
			InitializeComponent();
			this.foodService = foodService;
		}

		public Felvetel(FoodService foodService, Food food)
		{
			InitializeComponent();
			this.foodService = foodService;
			this.food = food;
			this.btnadd.Visibility = Visibility.Collapsed;

			tbname.Text = food.Name;
			tbdesc.Text = food.Description;
			cbcat.Text = food.Category;
			tbprice.Text = food.Price.ToString();
		}

		private void Add_Button(object sender, RoutedEventArgs e)
		{
			try
			{
				Food food = CreateFoodFromInputData();
				if (foodService.Create(food))
				{
					MessageBox.Show("Sikeres hozzáadás");
					tbname.Text = "";
					tbdesc.Text = "";
					tbprice.Text = "";
				}
				else
				{
					MessageBox.Show("Hiba történt a hozzáadás során");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private Food CreateFoodFromInputData()
		{
			string name = tbname.Text.Trim();
			string description = tbdesc.Text.Trim();
			string category = cbcat.Text.Trim();
			string price = tbprice.Text.Trim();

			if (string.IsNullOrEmpty(name))
			{
				throw new Exception("Név megadása kötelező");
			}

			if (string.IsNullOrEmpty(price))
			{
				throw new Exception("Ár megadása kötelező");
			}
			if (!int.TryParse(price, out int age))
			{
				throw new Exception("Ár csak szám lehet");
			}


			Food food = new Food();
			food.Name = name;
			food.Description = description;
			food.Category = category;
			food.Price = Convert.ToInt32(price);
			return food;
		}
	}
}
