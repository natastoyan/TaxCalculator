using TaxCalculator.Data;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Usage: YourProgram.exe <inputPath> <outputPath> <country>");
            return;
        }

        string inputPath = args[0];
        string outputPath = args[1];
        string country = args[2];

        IOrderRepository repo = new OrderRepository();
        var order = repo.Read(inputPath);
        order.CalculateTax(country);
        order.ToString();
        repo.Write(order, outputPath);
        Console.WriteLine(order.LineItems.First().CalculatedAmounts.TaxAmount);
    }
}