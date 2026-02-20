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
            Price = ValidatePrice(price);      // <-- добавил проверку
            Quantity = ValidateQuantity(quantity); // <-- добавил проверку
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
private decimal ValidatePrice(decimal price)
{
    if (price < 0)
    {
        Console.WriteLine($"Предупреждение: Цена не может быть отрицательной. Установлено 0.");
        return 0;
    }
    return price;
}

private int ValidateQuantity(int quantity)
{
    if (quantity < 0)
    {
        Console.WriteLine($"Предупреждение: Количество не может быть отрицательным. Установлено 0.");
        return 0;
    }
    return quantity;
}

public bool Sell(int amount)
{
    if (amount <= 0) return false;
    if (Quantity < amount) return false;
    if (IsExpired()) return false;

    Quantity -= amount;
    return true;
}

public void Restock(int amount)
{
    if (amount > 0) Quantity += amount;
}