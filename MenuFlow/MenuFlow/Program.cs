using MenuFlow.Library;
using MenuFlow.YouthOrPensioner;

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

        public static YouthOrPensioner.YouthOrPensioner singleTicketPriceCheckerApp = 
            new("Check single ticket price");
        public static PlaceholderApp groupTicketPriceCheckerApp = new("Check group ticket price");
        public static List<MenuApplication> youthOrPensionerMenuApps = [
            singleTicketPriceCheckerApp,
            groupTicketPriceCheckerApp
        ];

        // Initialize the apps to be shown in the main menu:
        public static YouthOrPensioner.TicketPriceCheckerMenu youthOrPensionerMenu = 
            new("Ticket Price Checker", youthOrPensionerMenuApps);
        public static PlaceholderApp placeholderApp2 = new("Repeat Ten Times");
        public static PlaceholderApp placeholderApp3 = new("The Third Word");

        public static List<MenuApplication> mainMenuApps = [
            youthOrPensionerMenu,
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
