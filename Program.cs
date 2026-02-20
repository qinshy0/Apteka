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