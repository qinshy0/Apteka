using System;
using System.Collections.Generic;

namespace Pharmacy
{
    public enum DiscountCategory
    {
        None = 0,
        Pensioner = 1,
        Disabled = 2,
        LargeFamily = 3
    }

    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DiscountCategory DiscountCategory { get; set; } = DiscountCategory.None;

        private List<Purchase> purchaseHistory = new List<Purchase>();
        private decimal totalSpent = 0;

        public class PurchaseItem
        {
            public Medicine Medicine { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }

        public class Purchase
        {
            public int ReceiptNumber { get; set; }
            public DateTime PurchaseDate { get; set; }
            public List<PurchaseItem> Items { get; set; } = new List<PurchaseItem>();
            public decimal TotalAmount { get; set; }
            public decimal DiscountApplied { get; set; }
        }
    }
}