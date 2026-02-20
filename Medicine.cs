// TODO:
// 1. Добавить поле для срока годности лекарства
// 2. Реализовать проверку корректности данных (цена, количество, срок годности)
// 3. Реализовать метод проверки необходимости рецепта


using System;

namespace Pharmacy
{
    public class Medicine
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool RequiresPrescription { get; set; }

        public Medicine() { }

        public Medicine(int id, string name, string manufacturer, decimal price,
                       int quantity, string category, DateTime expiryDate, bool requiresPrescription)
        {
            Id = id;
            Name = name;
            Manufacturer = manufacturer;
            Category = category;
            ExpiryDate = expiryDate;
            RequiresPrescription = requiresPrescription;
            Price = price;
            Quantity = quantity;
        }

        public bool IsExpired()
        {
            return ExpiryDate.Date < DateTime.Now.Date;
        }

        public bool IsExpiringSoon(int daysThreshold = 30)
        {
            if (IsExpired()) return false;
            int daysLeft = (ExpiryDate.Date - DateTime.Now.Date).Days;
            return daysLeft <= daysThreshold;
        }
    }
}