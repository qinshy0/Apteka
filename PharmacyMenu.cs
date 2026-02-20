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
            }
        }
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