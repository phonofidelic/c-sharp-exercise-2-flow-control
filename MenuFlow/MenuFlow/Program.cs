using MenuFlow.Library;
using TicketPriceChecker;

namespace MenuFlow
{
    internal class Program
    {
        public static TicketPriceChecker.TicketPriceCheckerApplication singleTicketPriceCheckerApp =
            new("Check single ticket price");
        public static PlaceholderApp groupTicketPriceCheckerApp = new("Check group ticket price");

        public static List<IMenuListable> ticketPriceCheckerMenuApps = [
            singleTicketPriceCheckerApp,
            groupTicketPriceCheckerApp
        ];

        public static MenuContext mainMenuContext = new();
        public static MenuContext ticketPriceCheckerMenuContext = new();
        // Initialize the apps to be shown in the main menu:
        public static TicketPriceChecker.TicketPriceCheckerMenu ticketPriceCheckerMenu =
            new("Ticket Price Checker", ticketPriceCheckerMenuApps, ticketPriceCheckerMenuContext, mainMenuContext);
        public static PlaceholderApp placeholderApp2 = new("Repeat Ten Times");
        public static PlaceholderApp placeholderApp3 = new("The Third Word");

        public static List<IMenuListable> mainMenuApps = [
            ticketPriceCheckerMenu,
            placeholderApp2,
            placeholderApp3
        ];

        static void Main(string[] args)
        {
            // Initialize the main menu:
            MainMenu mainMenu = new("MenuFlow", mainMenuApps, mainMenuContext);
            mainMenu.Render();

            Console.ReadLine();
        }
    }
    public class MainMenu(string name, List<IMenuListable> apps, MenuContext context) 
        : Menu(name, apps, context)
    {
        protected override void DisplayIntro()
        {
            Console.WriteLine($"Welcome to {Name}!");
            Console.WriteLine("\nEnter an option from the list below to get started:\n");
        }

        protected override void DisplayMenuOption(MenuOption menuOption)
        {
            Console.WriteLine($"\t{menuOption.Action}) Run \"{menuOption.Name}\"");
        }

        protected override void DisplayExitCommand()
        {
            Console.WriteLine("\n\t\"Q\" to exit the program.");
        }

        protected override void RenderExit()
        {
            Console.WriteLine($"\nThank you for using {Name}. Goodbye!");
            Environment.Exit(0);
        }
    }

    public class PlaceholderApp(string name) : MenuApplication
    {
        public override string Name { get; set; } = name;
        public override void Render()
        {
            Console.WriteLine($"Running application: {Name}");
            Console.WriteLine("\nThis is a placeholder app");
        }
    }
}
