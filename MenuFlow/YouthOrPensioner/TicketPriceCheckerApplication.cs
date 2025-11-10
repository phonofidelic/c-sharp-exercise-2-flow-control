using MenuFlow.Library;

namespace TicketPriceChecker
{
    
    public class TicketPriceCheckerMenu(string name, List<IMenuListable> apps, MenuContext context) : Menu(name, apps, context)

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
    }

    public class GroupTicketPriceCheckerApplication : TicketPriceCheckerApplication
    {
        private int _iteration = 0;
        public GroupTicketPriceCheckerApplication(string name, MenuContext context)
            : base(name, context)
        {
            GroupCount = null;
        }

        public override void Render()
        {
            do
            {
                Console.Clear();
                DisplayIntro();
                DisplayError(MenuApplicationException?.Message ?? "");
                Console.WriteLine("\tHow many people are in your group?");
                string rawInput = Console.ReadLine() ?? "";
                bool isValidGroupCount = ValidateIntInput(rawInput);
                if (isValidGroupCount)
                {
                    GroupCount = int.Parse(rawInput);
                    //TODO: Enter ticket price increment loop
                    GetAge();
                } else
                {
                    MenuApplicationException = new Exception($"\"{rawInput}\" is not a valid group count.");
                }
            } while (GroupCount == null);

            DisplayResult();
            Console.WriteLine("\n");
            // TODO: figure out how to not hard-code this message
            Console.WriteLine($"Press any key to return to the Ticket Price Checker menu.");
            Console.ReadKey(true);
            // Reset program state
            GroupCount = null;
            TotalPrice = 0;
            _iteration = 0;
        }

        private void GetAge()
        {
            do
            {
                Console.Clear();
                DisplayIntro();
                DisplayError(MenuApplicationException?.Message ?? "");
                Console.WriteLine($"\tHow old is person #{_iteration + 1}?");
                string rawInput = Console.ReadLine() ?? "";
                MenuApplicationException = null;
                bool isValidAge = ValidateIntInput(rawInput);
                if (isValidAge)
                {
                    int age = int.Parse(rawInput);
                    decimal currentPrice = Prices[GetAgeCategory(age)];
                    TotalPrice += currentPrice;
                    _iteration++;
                } else
                {
                    MenuApplicationException = new Exception($"'{rawInput}' is not a valid age.");
                }
                if (_iteration < GroupCount)
                    GetAge();
            } while (_iteration < GroupCount);

            
        }

        private void DisplayResult()
        {
            Console.WriteLine($"\tThe total cost of tickets for {GroupCount} people is {TotalPrice:C}");
        }
    }

    public class TicketPriceCheckerApplication : MenuApplication, IMenuContext
    {
        public override string Name { get; set; }
        public MenuContext Context { get; set; }
        public string ContextName { get; set; }
        private const int YOUTH_MAX_AGE = 20;
        private const int PENSIONER_MIN_AGE = 64;
        protected readonly Dictionary<AgeCategory, decimal> Prices = new()
        {
            { AgeCategory.Youth, 80 },
            { AgeCategory.Adult, 120 },
            { AgeCategory.Pensioner, 90 }
        };
        public TicketPriceCheckerApplication(string name, MenuContext context)
        {
            Name = name;
            Context = context;
            ContextName = Name.ToUpper().Replace(" ", "_");
            RegisterContext(ContextName);
        }
        protected void RegisterContext(string contextName)
        {
            if (!Context.ContainsKey(contextName))
            {
                Context.Register(contextName);
            }
        }

        // TODO: Implement in MenuApplication base class
        protected Exception? MenuApplicationException { get; set; } = null;

        // TODO: Hold stat in context
        public int? GroupCount { get; set; } = 1;
        public decimal TotalPrice { get; set; }
        private int? CurrentAge { get; set; } = null;

        public override void Render()
        {
            do
            {
                Console.Clear();
                DisplayIntro();
                DisplayError(MenuApplicationException?.Message ?? "");
                Console.WriteLine("\tPlease enter your age: ");
                try
                {
                    MenuApplicationException = null;
                    string rawInput = Console.ReadLine() ?? "";
                    CurrentAge = null;
                    bool isValidAge = ValidateIntInput(rawInput);
                    if (isValidAge)
                    {
                        CurrentAge = int.Parse(rawInput);
                        DisplayResult(GetAgeCategory(CurrentAge.Value));
                    }
                    else
                    {
                        MenuApplicationException = new Exception($"\"{rawInput}\" is not a valid age.");
                    }
                }
                catch (Exception ex)
                {
                    MenuApplicationException = new Exception($"{ex.Message}");
                }
            } while (CurrentAge == null); 
        }

        protected void DisplayIntro()
        {
            Console.WriteLine($"Running application: {Name}");
            Console.WriteLine("\n");
            Console.WriteLine("This application checks if you are eligible for a discounted ticket based on your age.");
            Console.WriteLine("\n");
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
            // TODO: figure out how to not hard-code this message
            Console.WriteLine($"Press any key to return to the Ticket Price Checker menu.");
            Console.ReadKey(true);
        }

        protected bool ValidateIntInput(string rawInput)
        {
            if (int.TryParse(rawInput, out _))
            {
                return true;
            }
            return false;
        }

        // TODO: Implement in MenuApplication base class
        protected static void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\t{message}");
            Console.WriteLine("\n");
            Console.ResetColor();
        }

        protected AgeCategory GetAgeCategory(int age)
        {
            switch (age)
            {
                case < YOUTH_MAX_AGE:
                    return AgeCategory.Youth;
                case >= PENSIONER_MIN_AGE:
                    return AgeCategory.Pensioner;
                default:
                    return AgeCategory.Adult;
            }
        }

        protected enum AgeCategory
        {
            Youth,
            Adult,
            Pensioner
        }
    }

    
}
