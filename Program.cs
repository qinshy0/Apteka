using System;

namespace Pharmacy
{
    class Program
    {
        static void Main(string[] args)
        {
            // Настройка кодировки для русского языка
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            try
            {
                // Красивое приветствие
                Console.WriteLine("╔════════════════════════════════════════╗");
                Console.WriteLine("║        АПТЕКА 'ЗДОРОВЬЕ' v1.0         ║");
                Console.WriteLine("║      Система управления аптекой       ║");
                Console.WriteLine("╚════════════════════════════════════════╝\n");

                // Создаем и запускаем меню
                PharmacyMenu menu = new PharmacyMenu();
                menu.ShowMainMenu();

                // Прощание
                Console.WriteLine("\n╔════════════════════════════════════════╗");
                Console.WriteLine("║     Будьте здоровы! До новых встреч!   ║");
                Console.WriteLine("╚════════════════════════════════════════╝");
            }
            catch (Exception ex)
            {
                // Если случилась ошибка - показываем её
                Console.WriteLine($"\n❌ Произошла ошибка: {ex.Message}");
                Console.WriteLine($"Детали: {ex.StackTrace}");
            }
            finally
            {
                // Это гарантирует что окно не закроется сразу
                Console.WriteLine("\nНажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }
    }
}