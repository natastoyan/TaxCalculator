using TaxCalculator.Core;

namespace TaxCalculator.Data
{
	public interface IOrderRepository
	{
		Order Read(string path);
		void Write(Order order, string path);
	}
}
