using MenuFlow.Library;

namespace TicketPriceChecker
{
    
    public class TicketPriceCheckerMenu(string name, List<IMenuListable> apps, MenuContext context, MenuContext parentContext) 
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
            Console.WriteLine("\n\t\"Q\" to return to the main menu.");
        }

        protected override void RenderExit()
        {
            if (context.SelectedAction == 0)
            {
                Console.WriteLine($"\nThank you for using {Name}! Returning to the main menu...");
                context.SetSelectedAction(0);
                parentContext.SetSelectedAction(null);
            }
        }
    }

    public class TicketPriceCheckerApplication(string name) : MenuApplication
    {
        private const int YOUTH_MAX_AGE = 20;
        private const int PENSIONER_MIN_AGE = 64;
        private readonly Dictionary<AgeCategory, decimal> Prices = new()
        {
            { AgeCategory.Youth, 80 },
            { AgeCategory.Adult, 120 },
            { AgeCategory.Pensioner, 90 }
        };
        // TODO: Implement in MenuApplication base class
        public Exception? MenuApplicationException { get; set; } = null;
        private int? Age { get; set; } = null;


        public override string Name { get; set; } = name;
        public override void Render()
        {
            do
            {
                Console.Clear();
                DisplayIntro();
                DisplayError(MenuApplicationException?.Message ?? "");
                
                DisplayAgePrompt();
            } while (Age == null); 
        }

        private void DisplayIntro()
        {
            Console.WriteLine($"Running application: {Name}");
            Console.WriteLine("\n");
            Console.WriteLine("This application checks if you are a youth or a pensioner based on your age.");
            Console.WriteLine("\n");
        }

        private void DisplayAgePrompt()
        {
            Console.Write("\tPlease enter your age: ");
            try
            {
                MenuApplicationException = null;
                string input = Console.ReadLine() ?? "";
                Age = null;
                bool isValidAge = ValidateAgeInput(input);
                if (isValidAge)
                {
                    Age = int.Parse(input);
                    DisplayResult(GetAgeCategory(Age.Value));
                } else
                {
                    MenuApplicationException = new($"\"{input}\" is not a valid age.");
                }
            }
            catch (Exception ex)
            {
                MenuApplicationException = new Exception($"{ex.Message}");
            }
        }

        private void DisplayResult(AgeCategory ageCategory)
        {
            var n = ageCategory == AgeCategory.Adult ? "n" : null;
            Console.Clear();
            DisplayIntro();
            DisplayError(MenuApplicationException?.Message ?? "");
            Console.WriteLine($"\tYou qualify as a{n} {ageCategory}.");
            Console.WriteLine($"\tYour ticket price is {Prices[ageCategory]:C2}");
            Console.WriteLine("\n");
        }

        private bool ValidateAgeInput(string rawInput)
        {
            if (int.TryParse(rawInput, out _))
            {
                return true;
            }
            return false;
        }

        // TODO: Implement in MenuApplication base class
        private static void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\t{message}");
            Console.WriteLine("\n");
            Console.ResetColor();
        }

        private AgeCategory GetAgeCategory(int age)
        {
            switch(age)
            {
                case < YOUTH_MAX_AGE:
                    return AgeCategory.Youth;
                case >= PENSIONER_MIN_AGE:
                    return AgeCategory.Pensioner;
                default:
                    return AgeCategory.Adult;
            }
        }

        private enum AgeCategory
        {
            Youth,
            Adult,
            Pensioner
        }
    }

    
}
