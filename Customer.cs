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
public Purchase CreatePurchase(int receiptNumber)
{
    return new Purchase
    {
        ReceiptNumber = receiptNumber,
        PurchaseDate = DateTime.Now,
        Items = new List<PurchaseItem>(),
        TotalAmount = 0,
        DiscountApplied = 0
    };
}

public bool AddToPurchase(Purchase purchase, Medicine medicine, int quantity)
{
    if (purchase == null || medicine == null)
    {
        Console.WriteLine("Ошибка: Покупка или лекарство не указаны!");
        return false;
    }

    if (medicine.Quantity < quantity)
    {
        Console.WriteLine($"Ошибка: Недостаточно товара {medicine.Name}. В наличии: {medicine.Quantity}, запрошено: {quantity}");
        return false;
    }

    if (medicine.IsExpired())
    {
        Console.WriteLine($"Ошибка: Лекарство {medicine.Name} просрочено!");
        return false;
    }

    var purchaseItem = new PurchaseItem
    {
        Medicine = medicine,
        Quantity = quantity,
        Price = medicine.Price
    };

    purchase.Items.Add(purchaseItem);

    bool sold = medicine.Sell(quantity);
    if (!sold)
    {
        purchase.Items.Remove(purchaseItem);
        return false;
    }

    Console.WriteLine($"✓ Товар {medicine.Name} в количестве {quantity} шт. добавлен в чек №{purchase.ReceiptNumber}");
    return true;