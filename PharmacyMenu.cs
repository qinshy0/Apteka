// TODO:
// 1. Реализовать поиск лекарств по названию и категории
// 2. Реализовать процесс покупки лекарств
// 3. Реализовать работу с рецептами и льготами



using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacy
{
    public class PharmacyMenu
    {
        private PharmacyManager manager;

        public PharmacyMenu()
        {
            manager = new PharmacyManager();
            InitializeData();
        }

        private void InitializeData()
        {
            manager.AddMedicine(new Medicine(1, "Нурофен", "Рекитт Бенкизер", 250.50m, 50, "болеутоляющее",
                DateTime.Now.AddMonths(18), false));
            manager.AddMedicine(new Medicine(2, "Амоксициллин", "Синтез", 180.80m, 30, "антибиотик",
                DateTime.Now.AddMonths(6), true));
            manager.AddMedicine(new Medicine(3, "Компливит", "Фармстандарт", 350.25m, 40, "витамины",
                DateTime.Now.AddMonths(24), false));
            manager.AddMedicine(new Medicine(4, "Эналаприл", "Гедеон Рихтер", 120.40m, 25, "сердечно-сосудистые",
                DateTime.Now.AddMonths(9), true));
            manager.AddMedicine(new Medicine(5, "Аспирин", "Байер", 90.90m, 60, "болеутоляющее",
                DateTime.Now.AddMonths(15), false));
            manager.AddMedicine(new Medicine(6, "Парацетамол", "Фармстандарт", 45.50m, 100, "жаропонижающее",
                DateTime.Now.AddMonths(12), false));
            manager.AddMedicine(new Medicine(7, "Цитрамон", "Фармстандарт", 65.30m, 45, "болеутоляющее",
                DateTime.Now.AddMonths(8), false));

            manager.AddPrescription(new Prescription(1, "Иванов И.И.", "Петров П.П.", "Амоксициллин", 20, "Л-12345"));
            manager.AddPrescription(new Prescription(2, "Сидоров С.С.", "Васильев В.В.", "Эналаприл", 30, "Л-67890"));
            manager.AddPrescription(new Prescription(3, "Петрова А.А.", "Смирнов С.С.", "Амоксициллин", 14, "Л-54321", 45));
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

        private void ProcessPurchase()
        {
            Console.Clear();
            Console.WriteLine("=== ОФОРМЛЕНИЕ ПОКУПКИ ===");

            Console.Write("Введите номер телефона клиента: ");
            string phone = Console.ReadLine();

            Customer customer = manager.FindCustomerByPhone(phone);
            if (customer == null)
            {
                Console.WriteLine("Клиент не найден. Сначала зарегистрируйте.");
                return;
            }

            var purchase = customer.CreatePurchase(manager.GetNextReceiptNumber());

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"ЧЕК №{purchase.ReceiptNumber} | Клиент: {customer.FullName}");

                if (purchase.Items.Count > 0)
                {
                    decimal subtotal = 0;
                    foreach (var item in purchase.Items)
                    {
                        Console.WriteLine($"{item.Medicine.Name} x{item.Quantity} = {item.Price * item.Quantity:C2}");
                        subtotal += item.Price * item.Quantity;
                    }
                    Console.WriteLine($"\nСумма: {subtotal:C2}");
                    Console.WriteLine($"Скидка: {customer.CalculateDiscount(purchase):C2}");
                }

                Console.WriteLine("\n1. Добавить товар  2. Завершить  3. Отмена");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("ID лекарства: ");
                    if (!int.TryParse(Console.ReadLine(), out int id)) continue;

                    var medicine = manager.GetAllMedicines().FirstOrDefault(m => m.Id == id && !m.IsExpired());
                    if (medicine == null)
                    {
                        Console.WriteLine("Лекарство не найдено!");
                        Console.ReadKey();
                        continue;
                    }

                    Console.Write("Количество: ");
                    if (!int.TryParse(Console.ReadLine(), out int qty)) continue;

                    if (medicine.RequiresPrescription)
                    {
                        var prescription = manager.FindPrescription(customer.FullName, medicine.Name);
                        if (prescription == null || !prescription.Use(qty))
                        {
                            Console.WriteLine("Ошибка: требуется действительный рецепт!");
                            Console.ReadKey();
                            continue;
                        }
                    }

                    customer.AddToPurchase(purchase, medicine, qty);
                }
                else if (choice == "2" && purchase.Items.Count > 0)
                {
                    customer.CompletePurchase(purchase);
                    manager.RecordSale(purchase.TotalAmount);
                    break;
                }
                else if (choice == "3") break;
            }
        }

        private void ManagePrescriptions()
        {
            Console.Clear();
            Console.WriteLine("=== УПРАВЛЕНИЕ РЕЦЕПТАМИ ===");
            Console.WriteLine("1. Показать все рецепты");
            Console.WriteLine("2. Проверить рецепт");
            Console.Write("Выберите: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                foreach (var p in manager.GetAllPrescriptions())
                    p.ShowPrescriptionInfo();
            }
            else if (choice == "2")
            {
                Console.Write("Введите номер рецепта: ");
                if (int.TryParse(Console.ReadLine(), out int id))
                {
                    var prescription = manager.GetAllPrescriptions().FirstOrDefault(p => p.Id == id);
                    if (prescription != null) prescription.ShowPrescriptionInfo();
                    else Console.WriteLine("Рецепт не найден");
                }
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

            foreach (var m in expiring)
            {
                int days = (m.ExpiryDate.Date - DateTime.Now.Date).Days;
                Console.WriteLine($"{m.Name} - годен до {m.ExpiryDate:dd.MM.yyyy} (осталось {days} дн.)");
            }
        }

        private void ShowPharmacyStats()
        {
            Console.Clear();
            Console.WriteLine("=== СТАТИСТИКА АПТЕКИ ===");

            var medicines = manager.GetAllMedicines();
            var valid = medicines.Where(m => !m.IsExpired()).ToList();

            Console.WriteLine($"Выручка: {manager.GetDailyRevenue():C2}");
            Console.WriteLine($"Лекарств в наличии: {valid.Count}");
            Console.WriteLine($"Всего единиц: {valid.Sum(m => m.Quantity)}");
            Console.WriteLine($"Клиентов: {manager.GetAllCustomers().Count}");
        }

        private void RegisterCustomer()
        {
            Console.Clear();
            Console.WriteLine("=== РЕГИСТРАЦИЯ КЛИЕНТА ===");

            Console.Write("ФИО: ");
            string name = Console.ReadLine();

            Console.Write("Телефон: ");
            string phone = Console.ReadLine();

            Console.Write("Адрес: ");
            string address = Console.ReadLine();

            Console.Write("Льготная категория (нет/пенсионер/инвалид/многодетный): ");
            string category = Console.ReadLine().ToLower();

            DiscountCategory disc = DiscountCategory.None;
            if (category.Contains("пенс")) disc = DiscountCategory.Pensioner;
            else if (category.Contains("инв")) disc = DiscountCategory.Disabled;
            else if (category.Contains("мног")) disc = DiscountCategory.LargeFamily;

            manager.AddCustomer(new Customer
            {
                FullName = name,
                Phone = phone,
                Address = address,
                BirthDate = DateTime.Now,
                DiscountCategory = disc
            });
        }
    }
}