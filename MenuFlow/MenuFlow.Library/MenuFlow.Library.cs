namespace MenuFlow.Library
{
    public class Menu<Actions>(string name, List<MenuOption> options) where Actions : Enum
    {
        public string Name { get; set; } = name;
        private List<MenuOption> MenuOptions { get; set; } = options;
        private int? SelectedMenuAction = null;
        private Exception? MenuException = null;
        private void SetMenuException()
        {
            MenuException = null;
        }
        private void SetMenuException(Exception exception)
        {
            MenuException = exception;
        }
        private int GetSelectedMenuAction()
        {
            SetMenuException();
            var rawInput = Console.ReadKey(true).KeyChar.ToString().ToUpper();

            if (rawInput == "Q")
            {
                return MenuOptions[0].Action;
            }

            if (!ValidateMenuInput(rawInput))
            {
                throw new ArgumentException($"\"{rawInput}\" is not a valid selection.");
            }
            return int.Parse(rawInput);
        }

        private bool ValidateMenuInput(string rawInput)
        {
            if (rawInput == null)
                return false;

            if (rawInput.Length != 1)
                return false;

            if (!int.TryParse(rawInput, out int input))
            {
                return false;
            }

            if (!Enum.IsDefined(typeof(Actions), input))
            {
                return false;
            }

            return true;
        }
        public void Display()
        {
            do
            {
                Console.Clear();
                Console.WriteLine($"Welcome to {Name}!");
                Console.WriteLine("\nEnter an option from the list below to get started:\n");
                for (int i = 1; i < MenuOptions.Count; i++)
                {
                    Console.WriteLine($"\t{MenuOptions[i].Action}) Run \"{MenuOptions[i].Name}\"");
                }
                    Console.WriteLine("\n\t\"Q\" to exit the program.");

                if (MenuException != null)
                    DisplayError(MenuException.Message);

                try
                {
                    SelectedMenuAction = GetSelectedMenuAction();

                    switch (SelectedMenuAction)
                    {
                        case 0:
                            Console.WriteLine($"\nThank you for using {Name}. Goodbye!");
                            Environment.Exit(0);
                            break;
                        case null:
                            break;
                        default:
                            Console.Clear();
                            // TODO: App implementation goes here
                            Console.WriteLine(MenuOptions[SelectedMenuAction.Value].Name);
                            break;
                    }
                    Console.WriteLine($"\n\tPress any key to return to {Name}\n");
                    Console.ReadKey(true);
                    SelectedMenuAction = null;
                }
                catch (Exception ex)
                {
                    SetMenuException(new Exception($"\t{ex.Message}\n\tPlease try again, or press \"Q\" to exit {Name}"));
                }
            } while (SelectedMenuAction == null);
        }

        private void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{message}\n");
            Console.ResetColor();
        }

        public static Dictionary<int, string> CreateMenuActions<T>() where T : Enum
        {
            var result = new Dictionary<int, string>();
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                result.Add((int)value, Enum.GetName(typeof(T), value)!);
            }
            return result;
        }
    }

    public class MenuOption(string name, int action)
    {
        public string Name { get; set; } = name;
        public int Action { get; set; } = action;
    }
}
