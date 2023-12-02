using Newtonsoft.Json;
using TaxCalculator.Core;

namespace TaxCalculator.Data
{
	public class OrderRepository : IOrderRepository
	{
		public Order Read(string path)
		{
			string json = File.ReadAllText(path);
			return JsonConvert.DeserializeObject<Order>(json);
		}

		public void Write(Order order, string path)
		{
			var json = JsonConvert.SerializeObject(order);
			File.WriteAllText(path, json);
		}
	}
}
