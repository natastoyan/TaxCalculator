using Newtonsoft.Json;

namespace TaxCalculator.Core
{ 
    public class Tax
    {
        public string Id { get; set; }
        public string InventoryTaxId { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public int FederalType { get; set; }
        public bool Default { get; set; }
        public int Scope { get; set; }
        public int Receiver { get; set; }
        public int Type { get; set; }
        public List<object> Attribute { get; set; }
    }

    public class CalculatedAmounts
    {
        public double TotalAmount { get; set; }
        public double DiscountAmount { get; set; }
        public double TaxAmount { get; set; }
        public double SubTotalAmount { get; set; }
        public double CartDiscountAmount { get; set; }
        public object DeliveryFeeAmount { get; set; }
    }

    public class LineItem
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public int Quantity { get; set; }
        public CalculatedAmounts CalculatedAmounts { get; set; }
    }

    public class Order
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public int LocationId { get; set; }
        public int PosId { get; set; }
        public int Status { get; set; }
        public List<Tax> Taxes { get; set; }
        public List<LineItem> LineItems { get; set; }

        public bool IsValid { get; private set;  }
        private void CheckValid()
		{
            // return 1 > this.Taxes.value >= 0 20 >= lineItems.quantity > 0
            IsValid =  Taxes.FirstOrDefault()?.Value >= 0
                && Taxes.FirstOrDefault()?.Value < 1
                && LineItems.Count() <= 20
                && LineItems.Count() > 0;
        }

        public void CalculateTax(string country)
		{
            CheckValid();
            if (IsValid)
            {
                switch (country.ToLower())
                {
                    case "rus":
                        CalculateTaxRus();
                        break;
                    case "usa":
                        CalculateTaxUsa();
                        break;
                }
            }
		}

        private void CalculateTaxRus()
		{
            //(order.lineItems.quantity * order.lineItems.calculatedAmounts.subTotalAmount)
            //- (order.lineItems.quantity * order.lineItems.calculatedAmounts.subTotalAmount) 
            // / (1 + order.taxes.value)
            foreach(var item in LineItems)
			{
                var totalAmount = item.Quantity * item.CalculatedAmounts.SubTotalAmount;
                var tax = totalAmount - (totalAmount / (1 + Taxes.First().Value));
                item.CalculatedAmounts.TaxAmount = Math.Round(tax, 2); ;
            }
          
        }

        private void CalculateTaxUsa()
		{
            //(order.lineItems.quantity * order.lineItems.calculatedAmounts.subTotalAmount) * order.taxes.value
            foreach(var item in LineItems)
			{
                var tax = item.Quantity * item.CalculatedAmounts.SubTotalAmount * Taxes.First().Value;
                item.CalculatedAmounts.TaxAmount = Math.Round(tax, 2);
			}

        }

		public override string ToString()
		{
            var validString = IsValid ? "Order is valid" : "Order is not valid";

            var json = JsonConvert.SerializeObject(this);
            return $"{validString}\n{json}";
		}
	}
}