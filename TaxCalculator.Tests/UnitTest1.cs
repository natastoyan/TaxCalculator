using TaxCalculator.Core;
using NUnit.Framework;
using System.Collections.Generic;

namespace TaxCalculator.Tests
{

    namespace TaxCalculator.Core.Tests
    {
        [TestFixture]
        public class OrderTests
        {
            [Test]
            public void CalculateTax_WithValidOrderAndRusCountry_CalculatesTaxForRus()
            {
                // Arrange
                var order = new Order
                {
                    Taxes = new List<Tax> { new Tax { Value = 0.1 } },
                    LineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        Quantity = 5,
                        CalculatedAmounts = new CalculatedAmounts { SubTotalAmount = 10.0 }
                    }
                }
                };

                // Act
                order.CalculateTax("rus");

                // Assert
                Assert.AreEqual(4.54, order.LineItems[0].CalculatedAmounts.TaxAmount, 0.01);
            }

            [Test]
            public void CalculateTax_WithInvalidOrder_DoesNotCalculateTax()
            {
                // Arrange
                var order = new Order
                {
                    Taxes = new List<Tax> { new Tax { Value = 1 } },
                    LineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        Quantity = 5,
                        CalculatedAmounts = new CalculatedAmounts { SubTotalAmount = 10.0 }
                    }
                }
                };

                order.CalculateTax("rus");
                // Assert
                Assert.False(order.IsValid);
            }
        }
    }
}