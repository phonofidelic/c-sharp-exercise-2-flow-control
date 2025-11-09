using MenuFlow.Library;
using TicketPriceChecker;

namespace MenuFlow
{
    internal class Program
    {
        public class PlaceholderApp(string name) : MenuApplication
        {
            public override string Name { get; set; } = name;
            public override void Run()
            {
                Console.WriteLine($"Running application: {Name}");
                Console.WriteLine("\nThis is a placeholder app");
            }
        }

        public static TicketPriceChecker.TicketPriceCheckerApplication singleTicketPriceCheckerApp = 
            new("Check single ticket price");
        public static PlaceholderApp groupTicketPriceCheckerApp = new("Check group ticket price");
        public static List<MenuApplication> ticketPriceCheckerMenuApps = [
            singleTicketPriceCheckerApp,
            groupTicketPriceCheckerApp
        ];

        // Initialize the apps to be shown in the main menu:
        public static TicketPriceChecker.TicketPriceCheckerMenu ticketPriceCheckerMenu = 
            new("Ticket Price Checker", ticketPriceCheckerMenuApps);
        public static PlaceholderApp placeholderApp2 = new("Repeat Ten Times");
        public static PlaceholderApp placeholderApp3 = new("The Third Word");

        public static List<MenuApplication> mainMenuApps = [
            ticketPriceCheckerMenu,
            placeholderApp2,
            placeholderApp3
        ];

        static void Main(string[] args)
        {
            // Initialize the main menu:
            Menu mainMenu = new("MenuFlow", mainMenuApps);
            mainMenu.Display();

            Console.ReadLine();
        }
    }
}
