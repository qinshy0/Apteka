// ВСЕ ВЫПОЛНЕННЫЕ TODO В ПРОЕКТЕ "АПТЕКА"

// Customer.cs (3 задачи)

// Коммит 1: Добавлено поле DiscountCategory для льготной категории
```csharp
public enum DiscountCategory
{
    None = 0,
    Pensioner = 1,
    Disabled = 2,
    LargeFamily = 3
}

public DiscountCategory DiscountCategory { get; set; } = DiscountCategory.None;

// Коммит 2: Реализован учет покупок клиента
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
    if (purchase == null || medicine == null) return false;
    if (medicine.Quantity < quantity) return false;
    if (medicine.IsExpired()) return false;

    var purchaseItem = new PurchaseItem
    {
        Medicine = medicine,
        Quantity = quantity,
        Price = medicine.Price
    };

    purchase.Items.Add(purchaseItem);
    medicine.Sell(quantity);
    return true;
}
// Коммит 3: Добавлен расчет скидки в зависимости от категории
public decimal CalculateDiscount(Purchase purchase)
{
    if (purchase == null) return 0;

    decimal discount = 0;
    decimal total = purchase.Items.Sum(item => item.Price * item.Quantity);

    switch (DiscountCategory)
    {
        case DiscountCategory.Pensioner:
            discount = total * 0.10m;
            break;
        case DiscountCategory.Disabled:
            discount = total * 0.15m;
            break;
        case DiscountCategory.LargeFamily:
            discount = total * 0.05m;
            break;
        default:
            discount = 0;
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

// Medicine.cs (3 задачи)
// Коммит 4: Добавлено поле ExpiryDate для срока годности

csharp
public DateTime ExpiryDate { get; set; }

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

// Коммит 5: Реализована проверка корректности данных
csharp
private decimal ValidatePrice(decimal price)
{
    if (price < 0) return 0;
    return price;
}

private int ValidateQuantity(int quantity)
{
    if (quantity < 0) return 0;
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
// Коммит 6: Добавлен метод проверки рецепта
csharp
public bool CheckPrescriptionRequirement()
{
    return RequiresPrescription;
}

public string GetStatus()
{
    if (IsExpired()) return "ПРОСРОЧЕНО";
    if (IsExpiringSoon()) return "СКОРО ИСТЕКАЕТ";
    if (Quantity < 10) return "МАЛО";
    return "НОРМА";
}

public override string ToString()
{
    string prescription = RequiresPrescription ? " [Рецепт]" : "";
    string expiryStatus = IsExpired() ? " ПРОСРОЧЕНО!" : (IsExpiringSoon() ? " Скоро истекает!" : "");
    return $"{Name} ({Manufacturer}) - {Price:C2} - {Quantity} шт.{prescription}{expiryStatus}";
}
// Prescription.cs(3 задачи)
// Коммит 7: Реализовано хранение информации о рецепте
csharp
public class Prescription
{
    public int Id { get; set; }
    public string PatientName { get; set; }
    public string DoctorName { get; set; }
    public string MedicineName { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int MaxQuantity { get; set; }
    public int UsedQuantity { get; set; }
    public string DoctorLicense { get; set; }

    public bool IsFullyUsed => UsedQuantity >= MaxQuantity;
    public int RemainingQuantity => MaxQuantity - UsedQuantity;

    public Prescription(int id, string patient, string doctor, string medicine,
                       int maxQuantity, string doctorLicense)
    {
        Id = id;
        PatientName = patient;
        DoctorName = doctor;
        MedicineName = medicine;
        IssueDate = DateTime.Now;
        ExpiryDate = IssueDate.AddDays(30);
        MaxQuantity = maxQuantity > 0 ? maxQuantity : 1;
        UsedQuantity = 0;
        DoctorLicense = doctorLicense ?? "Не указана";
    }

    public Prescription(int id, string patient, string doctor, string medicine,
                       int maxQuantity, string doctorLicense, int daysValid)
        : this(id, patient, doctor, medicine, maxQuantity, doctorLicense)
    {
        ExpiryDate = IssueDate.AddDays(daysValid);
    }
}
// Коммит 8: Добавлена проверка срока действия рецепта
csharp
public bool IsValid()
{
    return DateTime.Now.Date <= ExpiryDate.Date && UsedQuantity < MaxQuantity;
}

public void ShowPrescriptionInfo()
{
    Console.WriteLine("╔════════════════════════════════════════════╗");
    Console.WriteLine($"║                 РЕЦЕПТ #{Id,-13}           ║");
    Console.WriteLine("╠════════════════════════════════════════════╣");
    Console.WriteLine($"║ Пациент:        {PatientName,-25} ║");
    Console.WriteLine($"║ Врач:           {DoctorName,-25} ║");
    Console.WriteLine($"║ Лицензия врача: {DoctorLicense,-25} ║");
    Console.WriteLine($"║ Лекарство:      {MedicineName,-25} ║");
    Console.WriteLine($"║ Выписан:        {IssueDate:dd.MM.yyyy,-25} ║");
    Console.WriteLine($"║ Действует до:   {ExpiryDate:dd.MM.yyyy,-25} ║");
    Console.WriteLine($"║ Количество:     {UsedQuantity}/{MaxQuantity,-22} ║");
    Console.WriteLine($"║ Осталось:       {RemainingQuantity,-25} ║");

    if (IsValid())
        Console.WriteLine("║ Статус:         ДЕЙСТВИТЕЛЕН             ║");
    else if (DateTime.Now.Date > ExpiryDate.Date)
        Console.WriteLine("║ Статус:         ПРОСРОЧЕН                ║");
    else if (IsFullyUsed)
        Console.WriteLine("║ Статус:         ИСПОЛЬЗОВАН             ║");
    else
        Console.WriteLine("║ Статус:         НЕДЕЙСТВИТЕЛЕН          ║");

    Console.WriteLine("╚════════════════════════════════════════════╝");
}

public bool Extend(int additionalDays)
{
    if (additionalDays <= 0) return false;
    ExpiryDate = ExpiryDate.AddDays(additionalDays);
    return true;
}
Коммит 9: Реализовано использование рецепта при покупке
csharp
public bool Use(int quantity)
{
    if (quantity <= 0) return false;
    if (!IsValid()) return false;
    if (UsedQuantity + quantity > MaxQuantity) return false;

    UsedQuantity += quantity;
    return true;
}

public double GetUsagePercentage()
{
    return (double)UsedQuantity / MaxQuantity * 100;
}

public override string ToString()
{
    string status = IsValid() ? "✓ Действителен" :
                   (DateTime.Now.Date > ExpiryDate.Date ? "✗ Просрочен" : "✗ Использован");

    return $"Рецепт #{Id}: {PatientName} - {MedicineName} " +
           $"[{UsedQuantity}/{MaxQuantity}] {status}";
}
//  PharmacyManager.cs(3 задачи)
// Коммит 10: Реализовано хранение лекарств, клиентов и рецептов
csharp
private List<Medicine> medicines = new List<Medicine>();
private List<Customer> customers = new List<Customer>();
private List<Prescription> prescriptions = new List<Prescription>();

private int nextCustomerId = 1;
private int nextReceiptNumber = 1000;
private decimal dailyRevenue = 0;

public void AddMedicine(Medicine medicine)
{
    if (medicine != null && !medicines.Any(m => m.Id == medicine.Id))
        medicines.Add(medicine);
}

public void AddCustomer(Customer customer)
{
    if (customer != null)
    {
        customer.Id = nextCustomerId++;
        customers.Add(customer);
    }
}

public void AddPrescription(Prescription prescription)
{
    if (prescription != null)
        prescriptions.Add(prescription);
}

public List<Medicine> GetAllMedicines() => medicines.ToList();
public List<Customer> GetAllCustomers() => customers.ToList();
public List<Prescription> GetAllPrescriptions() => prescriptions.ToList();
public int GetNextReceiptNumber() => nextReceiptNumber++;

// Коммит 11: Реализован поиск лекарств по названию и категории
csharp
public List<Medicine> FindMedicineByName(string name)
{
    if (string.IsNullOrWhiteSpace(name)) return new List<Medicine>();

    return medicines.Where(m => !m.IsExpired() &&
        m.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
        .OrderBy(m => m.Name).ToList();
}

public List<Medicine> FindMedicineByCategory(string category)
{
    if (string.IsNullOrWhiteSpace(category)) return new List<Medicine>();

    return medicines.Where(m => !m.IsExpired() &&
        m.Category.IndexOf(category, StringComparison.OrdinalIgnoreCase) >= 0)
        .OrderBy(m => m.Name).ToList();
}

public Customer FindCustomerByPhone(string phone)
{
    if (string.IsNullOrWhiteSpace(phone)) return null;

    string cleanPhone = phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
    return customers.FirstOrDefault(c =>
        c.Phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "") == cleanPhone);
}

public List<Medicine> GetExpiringMedicines(int daysThreshold = 30)
{
    return medicines.Where(m => m.IsExpiringSoon(daysThreshold) && !m.IsExpired())
        .OrderBy(m => m.ExpiryDate).ToList();
}

public List<Medicine> GetLowStockMedicines(int threshold = 10)
{
    return medicines.Where(m => m.Quantity <= threshold && !m.IsExpired())
        .OrderBy(m => m.Quantity).ToList();
}
// Коммит 12: Реализован учет продаж и проверка рецептов
csharp
public Prescription FindPrescription(string patientName, string medicineName)
{
    if (string.IsNullOrWhiteSpace(patientName) || string.IsNullOrWhiteSpace(medicineName))
        return null;

    return prescriptions.FirstOrDefault(p =>
        p.PatientName.IndexOf(patientName, StringComparison.OrdinalIgnoreCase) >= 0 &&
        p.MedicineName.IndexOf(medicineName, StringComparison.OrdinalIgnoreCase) >= 0 &&
        p.IsValid());
}

public void RecordSale(decimal amount)
{
    if (amount > 0) dailyRevenue += amount;
}

public decimal GetDailyRevenue() => dailyRevenue;

public int GetPrescriptionMedicinesCount()
{
    return medicines.Count(m => m.RequiresPrescription && !m.IsExpired());
}

public void ResetDailyRevenue()
{
    dailyRevenue = 0;
}

// PharmacyMenu.cs(3 задачи)
// Коммит 13: Реализован поиск лекарств в меню
csharp
private void SearchMedicines()
{
    Console.Clear();
    Console.WriteLine("=== ПОИСК ЛЕКАРСТВ ===");
    Console.WriteLine("1. По названию");
    Console.WriteLine("2. По категории");
    Console.Write("Выберите: ");

    string choice = Console.ReadLine();
    Console.Write("Введите поисковый запрос: ");
    string query = Console.ReadLine();

    List<Medicine> results = choice == "1" ? manager.FindMedicineByName(query) :
                            choice == "2" ? manager.FindMedicineByCategory(query) : null;

    if (results == null || results.Count == 0)
    {
        Console.WriteLine("Ничего не найдено.");
        return;
    }

    Console.WriteLine($"\nНайдено: {results.Count}");
    foreach (var m in results)
        Console.WriteLine($"ID {m.Id}: {m.Name} - {m.Price:C2} ({m.Quantity} шт.)");
}

//Коммит 14: Реализован процесс покупки в меню
csharp
private void ProcessPurchase()
{
    Console.Clear();
    Console.WriteLine("=== ОФОРМЛЕНИЕ ПОКУПКИ ===");

    Console.Write("Введите номер телефона клиента: ");
    string phone = Console.ReadLine();

    Customer customer = manager.FindCustomerByPhone(phone);
    if (customer == null)
    {
        Console.WriteLine("Клиент не найден.");
        return;
    }

    var purchase = customer.CreatePurchase(manager.GetNextReceiptNumber());

    bool addingItems = true;
    while (addingItems)
    {
        Console.Clear();
        Console.WriteLine($"=== ЧЕК №{purchase.ReceiptNumber} | Клиент: {customer.FullName} ===\n");

        if (purchase.Items.Count > 0)
        {
            decimal subtotal = 0;
            foreach (var item in purchase.Items)
            {
                Console.WriteLine($"  • {item.Medicine.Name} - {item.Quantity} шт. x {item.Price:C2} = {item.Price * item.Quantity:C2}");
                subtotal += item.Price * item.Quantity;
            }
            Console.WriteLine($"\n  Сумма без скидки: {subtotal:C2}");
            decimal discount = customer.CalculateDiscount(purchase);
            Console.WriteLine($"  Скидка: {discount:C2}");
            Console.WriteLine($"  ИТОГО: {subtotal - discount:C2}");
        }

        Console.WriteLine("\n1. Добавить товар");
        Console.WriteLine("2. Завершить покупку");
        Console.WriteLine("3. Отменить покупку");
        Console.Write("Выберите: ");

        string choice = Console.ReadLine();

        if (choice == "1")
        {
            Console.Write("ID лекарства: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var medicine = manager.GetAllMedicines().FirstOrDefault(m => m.Id == id && !m.IsExpired());
                if (medicine == null)
                {
                    Console.WriteLine("Лекарство не найдено!");
                }
                else
                {
                    Console.Write("Количество: ");
                    if (int.TryParse(Console.ReadLine(), out int qty))
                    {
                        if (medicine.RequiresPrescription)
                        {
                            var prescription = manager.FindPrescription(customer.FullName, medicine.Name);
                            if (prescription == null || !prescription.Use(qty))
                            {
                                Console.WriteLine("Ошибка: Требуется рецепт!");
                                Console.ReadKey();
                                continue;
                            }
                        }
                        customer.AddToPurchase(purchase, medicine, qty);
                    }
                }
            }
        }
        else if (choice == "2" && purchase.Items.Count > 0)
        {
            customer.CompletePurchase(purchase);
            manager.RecordSale(purchase.TotalAmount);
            addingItems = false;
        }
        else if (choice == "3")
        {
            addingItems = false;
        }

        if (addingItems)
        {
            Console.WriteLine("\nНажмите Enter...");
            Console.ReadLine();
        }
    }
}

// Коммит 15: Реализована работа с рецептами в меню
csharp
private void ManagePrescriptions()
{
    Console.Clear();
    Console.WriteLine("=== УПРАВЛЕНИЕ РЕЦЕПТАМИ ===");
    Console.WriteLine("1. Выписать новый рецепт");
    Console.WriteLine("2. Проверить рецепт");
    Console.WriteLine("3. Показать все рецепты");
    Console.Write("Выберите: ");

    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            CreateNewPrescription();
            break;
        case "2":
            CheckPrescription();
            break;
        case "3":
            ShowAllPrescriptions();
            break;
        default:
            Console.WriteLine("Неверный выбор!");
            break;
    }
}

private void CreateNewPrescription()
{
    Console.Clear();
    Console.WriteLine("=== ВЫПИСКА НОВОГО РЕЦЕПТА ===");

    Console.Write("ФИО пациента: ");
    string patient = Console.ReadLine();

    Console.Write("ФИО врача: ");
    string doctor = Console.ReadLine();

    Console.Write("Название лекарства: ");
    string medicine = Console.ReadLine();

    Console.Write("Максимальное количество: ");
    if (!int.TryParse(Console.ReadLine(), out int maxQuantity)) return;

    Console.Write("Номер лицензии врача: ");
    string license = Console.ReadLine();

    Console.Write("Срок действия (дней, Enter = 30): ");
    string daysInput = Console.ReadLine();

    int nextId = manager.GetAllPrescriptions().Count + 1;
    Prescription prescription;

    if (string.IsNullOrWhiteSpace(daysInput))
        prescription = new Prescription(nextId, patient, doctor, medicine, maxQuantity, license);
    else if (int.TryParse(daysInput, out int days))
        prescription = new Prescription(nextId, patient, doctor, medicine, maxQuantity, license, days);
    else
        prescription = new Prescription(nextId, patient, doctor, medicine, maxQuantity, license);

    manager.AddPrescription(prescription);
    prescription.ShowPrescriptionInfo();
}

private void CheckPrescription()
{
    Console.Clear();
    Console.WriteLine("=== ПРОВЕРКА РЕЦЕПТА ===");

    Console.Write("Введите номер рецепта: ");
    if (int.TryParse(Console.ReadLine(), out int id))
    {
        var prescription = manager.GetAllPrescriptions().FirstOrDefault(p => p.Id == id);
        if (prescription != null)
            prescription.ShowPrescriptionInfo();
        else
            Console.WriteLine($"Рецепт #{id} не найден!");
    }
}

private void ShowAllPrescriptions()
{
    Console.Clear();
    Console.WriteLine("=== ВСЕ РЕЦЕПТЫ ===");

    var prescriptions = manager.GetAllPrescriptions();
    if (prescriptions.Count == 0)
    {
        Console.WriteLine("Рецептов нет.");
        return;
    }

    foreach (var p in prescriptions)
    {
        Console.WriteLine();
        p.ShowPrescriptionInfo();
    }
}

private void CheckExpiringMedicines()
{
    Console.Clear();
    Console.WriteLine("=== ЛЕКАРСТВА С ИСТЕКАЮЩИМ СРОКОМ ===");

    var expiring = manager.GetExpiringMedicines();
    if (expiring.Count == 0)
    {
        Console.WriteLine("Нет лекарств с истекающим сроком.");
        return;
    }

    Console.WriteLine($"\nНайдено {expiring.Count} лекарств:\n");
    foreach (var m in expiring)
    {
        int days = (m.ExpiryDate.Date - DateTime.Now.Date).Days;
        Console.WriteLine($"• {m.Name} - годен до {m.ExpiryDate:dd.MM.yyyy} (осталось {days} дн.)");
        Console.WriteLine($"  Производитель: {m.Manufacturer}, в наличии: {m.Quantity} шт.");
    }
}

private void ShowPharmacyStats()
{
    Console.Clear();
    Console.WriteLine("=== СТАТИСТИКА АПТЕКИ ===");

    var medicines = manager.GetAllMedicines();
    var valid = medicines.Where(m => !m.IsExpired()).ToList();
    var customers = manager.GetAllCustomers();

    Console.WriteLine($"\n💰 ВЫРУЧКА: {manager.GetDailyRevenue():C2}");
    Console.WriteLine($"\n💊 ЛЕКАРСТВА:");
    Console.WriteLine($"  • Всего: {medicines.Count}");
    Console.WriteLine($"  • В наличии: {valid.Count}");
    Console.WriteLine($"  • Просрочено: {medicines.Count(m => m.IsExpired())}");
    Console.WriteLine($"\n👥 КЛИЕНТЫ: {customers.Count}");

    var lowStock = manager.GetLowStockMedicines();
    if (lowStock.Count > 0)
    {
        Console.WriteLine($"\n⚠️ ЗАКАНЧИВАЮТСЯ:");
        foreach (var m in lowStock)
            Console.WriteLine($"  • {m.Name} - осталось {m.Quantity} шт.");
    }
}

private void RegisterCustomer()
{
    Console.Clear();
    Console.WriteLine("=== РЕГИСТРАЦИЯ КЛИЕНТА ===");

    Console.Write("ФИО: ");
    string name = Console.ReadLine();

    Console.Write("Дата рождения (дд.мм.гггг): ");
    if (!DateTime.TryParse(Console.ReadLine(), out DateTime birthDate))
    {
        Console.WriteLine("Неверный формат!");
        return;
    }

    Console.Write("Телефон: ");
    string phone = Console.ReadLine();

    Console.Write("Адрес: ");
    string address = Console.ReadLine();

    Console.Write("Льготная категория (нет/пенсионер/инвалид/многодетный): ");
    string category = Console.ReadLine().ToLower();

    DiscountCategory discountCategory = DiscountCategory.None;
    if (category.Contains("пенс")) discountCategory = DiscountCategory.Pensioner;
    else if (category.Contains("инв")) discountCategory = DiscountCategory.Disabled;
    else if (category.Contains("мног")) discountCategory = DiscountCategory.LargeFamily;

    Customer customer = new Customer
    {
        FullName = name,
        BirthDate = birthDate,
        Phone = phone,
        Address = address,
        DiscountCategory = discountCategory
    };

    manager.AddCustomer(customer);
    customer.ShowCustomerInfo();
}

public void ShowMainMenu()
{
    bool running = true;

    while (running)
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════╗");
        Console.WriteLine("║       АПТЕКА 'ЗДОРОВЬЕ'       ║");
        Console.WriteLine("╠════════════════════════════════╣");
        Console.WriteLine("║ 1. 🔍 Поиск лекарств           ║");
        Console.WriteLine("║ 2. 📋 Все лекарства           ║");
        Console.WriteLine("║ 3. 🛒 Оформить покупку        ║");
        Console.WriteLine("║ 4. 📝 Управление рецептами    ║");
        Console.WriteLine("║ 5. ⏰ Сроки годности          ║");
        Console.WriteLine("║ 6. 📊 Статистика аптеки       ║");
        Console.WriteLine("║ 7. 👤 Регистрация клиента     ║");
        Console.WriteLine("║ 8. 🚪 Выход                   ║");
        Console.WriteLine("╚════════════════════════════════╝");
        Console.Write("Выберите: ");

        switch (Console.ReadLine())
        {
            case "1": SearchMedicines(); break;
            case "2": ShowAllMedicines(); break;
            case "3": ProcessPurchase(); break;
            case "4": ManagePrescriptions(); break;
            case "5": CheckExpiringMedicines(); break;
            case "6": ShowPharmacyStats(); break;
            case "7": RegisterCustomer(); break;
            case "8": running = false; continue;
            default: Console.WriteLine("Неверный выбор!"); break;
        }

        Console.WriteLine("\nНажмите Enter...");
        Console.ReadLine();
    }
}

private void ShowAllMedicines()
{
    Console.Clear();
    Console.WriteLine("=== ВСЕ ЛЕКАРСТВА ===");

    var medicines = manager.GetAllMedicines().Where(m => !m.IsExpired()).ToList();
    if (medicines.Count == 0)
    {
        Console.WriteLine("Нет лекарств в наличии.");
        return;
    }

    foreach (var m in medicines)
        Console.WriteLine($"ID {m.Id}: {m.Name} - {m.Price:C2} ({m.Quantity} шт.) [{m.GetStatus()}]");
}
//  Program.cs(1 задача)
// Коммит 16: Обновлен Program.cs с обработкой ошибок
csharp
using System;

namespace Pharmacy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            try
            {
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║        АПТЕКА 'ЗДОРОВЬЕ' v1.0         ║");
                Console.WriteLine("║      Система управления аптекой       ║");
                Console.WriteLine("╚════════════════════════════════════════╝\n");

                PharmacyMenu menu = new PharmacyMenu();
                menu.ShowMainMenu();

                Console.WriteLine("\n╔════════════════════════════════════════╗");
                Console.WriteLine("║     Будьте здоровы! До новых встреч!   ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ Ошибка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\nНажмите любую клавишу...");
                Console.ReadKey();
            }
        }
    }
}