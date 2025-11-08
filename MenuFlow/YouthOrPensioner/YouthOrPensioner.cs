using MenuFlow.Library;

namespace MenuFlow.YouthOrPensioner
{
    public class YouthOrPensioner(string name) : MenuApplication
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
        public int? Age { get; set; } = null;


        public override string Name { get; set; } = name;
        public override void Run()
        {
            do
            {
                Console.Clear();
                Console.WriteLine($"Running application: {Name}");
                DisplayAgePrompt();
                if (MenuApplicationException != null)
                {
                    DisplayError(MenuApplicationException.Message);
                }
            } while (Age == null);

            
        }

        private void DisplayAgePrompt()
        {
            Console.WriteLine("\nThis application checks if you are a youth or a pensioner based on your age.");
            Console.Write("\tPlease enter your age: ");
            try
            {
                //MenuApplicationException = null;
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
            Console.WriteLine($"Your ticket price is {Prices[ageCategory]:C2}");
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
            Console.WriteLine($"\n{message}\n");
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
