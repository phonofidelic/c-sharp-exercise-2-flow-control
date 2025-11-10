using MenuFlow.Library;
using RepeatTenTimes;
using TicketPriceChecker;

namespace MenuFlow
{
    internal class Program
    {
        public static MenuContext globalContext = new();
        public static TicketPriceChecker.TicketPriceCheckerApplication singleTicketPriceCheckerApp =
            new("Check single ticket price", globalContext);
        public static GroupTicketPriceCheckerApplication groupTicketPriceCheckerApp = 
            new("Check group ticket price", globalContext);

        public static List<IMenuListable> ticketPriceCheckerMenuApps = [
            singleTicketPriceCheckerApp,
            groupTicketPriceCheckerApp
        ];
        
        // Initialize the apps to be shown in the main menu:
        public static TicketPriceChecker.TicketPriceCheckerMenu ticketPriceCheckerMenu =
            new("Ticket Price Checker", ticketPriceCheckerMenuApps, globalContext);
        public static RepeatTenTimesApplication repeatTenTimesApp = new("Repeat Ten Times");
        public static PlaceholderApp placeholderApp3 = new("The Third Word");

        public static List<IMenuListable> mainMenuApps = [
            ticketPriceCheckerMenu,
            repeatTenTimesApp,
            placeholderApp3
        ];

        static void Main(string[] args)
        {
            // Initialize the main menu:
            MainMenu mainMenu = new("MenuFlow", mainMenuApps, globalContext);
            mainMenu.Render();

            Console.ReadLine();
        }
    }
    public class MainMenu(string name, List<IMenuListable> apps, MenuContext context) 
        : Menu(name, apps, context)
    {
        protected override void DisplayIntro()
        {
            Context.Debug();
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
            Console.WriteLine("\n\tPress any key to return to the main menu.");
            Console.ReadKey(true);
        }
    }
}
