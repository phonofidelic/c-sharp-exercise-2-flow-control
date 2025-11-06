namespace MenuFlow
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<MenuOption> mainMenuOptions =
            [
                new MenuOption("Youth or Pensioner", MenuAction.Program1),
                new MenuOption("Repeat ten times", MenuAction.Program2),
                new MenuOption("The third word", MenuAction.Program3),
            ];

            Menu mainMenu = new("MenuFlow", mainMenuOptions);
            mainMenu.Display();

            Console.ReadLine();
        }
    }

    class Menu(string name, List<MenuOption> options)
    {
        public string Name { get; set; } = name;
        private List<MenuOption> MenuOptions { get; set; } = options;
        private MenuAction? SelectedMenuAction = null;
        private Exception? MenuException = null;
        private void SetMenuException()
        {
            MenuException = null;
        }
        private void SetMenuException(Exception exception)
        {
            MenuException = exception;
        }
        private MenuAction GetSelectedMenuAction()
        {
            SetMenuException();
            var rawInput = Console.ReadKey(true).KeyChar.ToString().ToUpper();

            if (rawInput == "Q")
            {
                return MenuAction.Quit;
            }

            if (!ValidateMenuInput(rawInput))
            {
                throw new ArgumentException($"\"{rawInput}\" is not a valid selection. \nPlease try again, or press \"Q\" to exit {Name}");
            }
            return Enum.Parse<MenuAction>(rawInput);
        }

        private bool ValidateMenuInput(string rawInput)
        {
            if (rawInput == null)
                return false;

            if (rawInput.Length != 1)
                return false;

            if (int.TryParse(rawInput, out int input))
            {
                if (!Enum.IsDefined(typeof(MenuAction), input))
                {
                    return false;
                }
            }

            return true;
        }
        public void Display()
        {
            do
            {
                Console.Clear();
                Console.WriteLine($"Welcome to {Name}!");
                Console.WriteLine("Enter an option from the list below to get started:\n");
                foreach (var option in MenuOptions)
                {
                    Console.WriteLine($"\t{(int)option.Action}) Run \"{option.Name}\"");
                }
                Console.WriteLine("\n\t\"Q\" to exit the program.");
                
                if (MenuException != null)
                    DisplayError(MenuException.Message);


                try
                {
                    SelectedMenuAction = GetSelectedMenuAction();

                    switch (SelectedMenuAction)
                    {
                        case MenuAction.Program1:
                            Console.Clear();
                            Console.WriteLine(MenuOptions[(int)MenuAction.Program1 -1].Name);
                            break;
                        case MenuAction.Program2:
                            Console.Clear();
                            Console.WriteLine(MenuOptions[(int)MenuAction.Program2 - 1].Name);
                            break;
                        case MenuAction.Program3:
                            Console.Clear();
                            Console.WriteLine(MenuOptions[(int)MenuAction.Program3 - 1].Name);
                            break;
                        default:
                            break;
                    }
                    Console.WriteLine($"\nPress any key to return to {Name}\n");
                    Console.ReadKey(true);
                    SelectedMenuAction = null;
                }
                catch (Exception ex)
                {
                    SetMenuException(ex);
                }
            } while ((SelectedMenuAction) == null);

            
        }

        private void DisplayError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n{message}\n");
            Console.ResetColor();
        }
    }

    class MenuOption(string name, MenuAction action)
    {
        public string Name { get; set; } = name;
        public MenuAction Action { get; set; } = action;
    }

    enum MenuAction
    {
        Program1 = 1,
        Program2 = 2,
        Program3 = 3,
        Quit = 0
    }
}
