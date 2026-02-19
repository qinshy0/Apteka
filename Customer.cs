// TODO:
// 1. Добавить поле для льготной категории (пенсионер, инвалид и т.д.)
// 2. Реализовать учет покупок клиента
// 3. Реализовать расчет скидки в зависимости от категории


using System;
using System.Collections.Generic;
using System.Linq;

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
        }

        public decimal CalculateDiscount(Purchase purchase)
        {
            if (purchase == null) return 0;

            decimal discount = 0;
            decimal total = purchase.Items.Sum(item => item.Price * item.Quantity);

            switch (DiscountCategory)
            {
                case DiscountCategory.Pensioner:
                    discount = total * 0.10m;
                    Console.WriteLine($"Скидка пенсионера: 10%");
                    break;
                case DiscountCategory.Disabled:
                    discount = total * 0.15m;
                    Console.WriteLine($"Скидка инвалида: 15%");
                    break;
                case DiscountCategory.LargeFamily:
                    discount = total * 0.05m;
                    Console.WriteLine($"Скидка многодетным: 5%");
                    break;
                default:
                    Console.WriteLine($"Скидка не предоставляется");
                    break;
            }

            if (discount > 1000) discount = 1000;
            return Math.Round(discount, 2);
        }

        public void CompletePurchase(Purchase purchase)
        {
            if (purchase == null) return;

            decimal subtotal = purchase.Items.Sum(item => item.Price * item.Quantity);
            decimal discount = CalculateDiscount(purchase);

            purchase.DiscountApplied = discount;
            purchase.TotalAmount = subtotal - discount;
            purchaseHistory.Add(purchase);
            totalSpent += purchase.TotalAmount;

            Console.WriteLine($"\n=== ЧЕК №{purchase.ReceiptNumber} ===");
            Console.WriteLine($"Дата: {purchase.PurchaseDate:dd.MM.yyyy HH:mm}");
            Console.WriteLine($"Клиент: {FullName}");
            Console.WriteLine($"Категория: {GetDiscountCategoryName()}");
            Console.WriteLine($"Сумма без скидки: {subtotal:C2}");
            Console.WriteLine($"Скидка: {discount:C2}");
            Console.WriteLine($"ИТОГО: {purchase.TotalAmount:C2}");
        }

        private string GetDiscountCategoryName()
        {
            switch (DiscountCategory)
            {
                case DiscountCategory.Pensioner: return "Пенсионер";
                case DiscountCategory.Disabled: return "Инвалид";
                case DiscountCategory.LargeFamily: return "Многодетный";
                default: return "Нет льгот";
            }
        }

        public void ShowCustomerInfo()
        {
            Console.WriteLine($"=== КЛИЕНТ ===");
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"ФИО: {FullName}");
            Console.WriteLine($"Телефон: {Phone}");
            Console.WriteLine($"Льготная категория: {GetDiscountCategoryName()}");
            Console.WriteLine($"Всего покупок: {purchaseHistory.Count}");
            Console.WriteLine($"Общая сумма: {totalSpent:C2}");
        }

        public List<Purchase> GetPurchaseHistory() => purchaseHistory.ToList();
        public decimal GetTotalSpent() => totalSpent;
    }
}