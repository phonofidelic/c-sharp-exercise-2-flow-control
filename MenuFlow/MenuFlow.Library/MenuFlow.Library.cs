using System.Reflection;
using System.Reflection.Emit;

namespace MenuFlow.Library
{
    public interface IMenuListable
    {
        public string Name { get; set; }
        public abstract void Render();
    }
    public abstract class Menu : IMenuListable
    {
        public string Name { get; set; }
        protected List<MenuOption> MenuOptions { get; set; } = [];

        public Menu(string name, List<IMenuListable> apps)
        {
            Name = name;
            MenuOptions.Add(new MenuOption("Quit", 0));
            foreach ((IMenuListable app, int index) in apps.Select((app, index) => (app, index)))
            {
                MenuOptions.Add(new MenuOption(app, index + 1));
            }
        }

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
        private int ReadMenuActionFromKey()
        {
            SetMenuException();
            var rawInput = Console.ReadKey(true).KeyChar.ToString().ToUpper();

            if (rawInput == "Q")
            {
                return 0;
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

            if (input < 0 || input >= MenuOptions.Count)
            {
                return false;
            }

            // TODO: Re-enable Enum validation if we can create dynamic Actions at runtime?
            //if (!Enum.IsDefined(typeof(Actions), input))
            //{
            //    return false;
            //}

            return true;
        }

        // TODO: Only deal with structure of the rendering logic here?
        // The content should be handled in the implementation of a Menu?
        public void Render()
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
                if (this is Menu)
                    Console.WriteLine("\n\t\"Q\" to exit the program.");
                else
                    Console.WriteLine($"\n\t\"Q\" to return to {Name}");

                if (MenuException != null)
                    DisplayError(MenuException.Message);

                try
                {
                    SelectedMenuAction = ReadMenuActionFromKey();
                    var selectedMenuOption = MenuOptions[SelectedMenuAction.Value];

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
                            if (selectedMenuOption.MenuApp != null)
                            {
                                // TODO: Make sure each MenuApp creates only one instance of itself.
                                selectedMenuOption.MenuApp.Render();
                                break;
                            }
                            else
                                Console.WriteLine($"The '{selectedMenuOption.Name}' app has not been implemented yet.");
                            break;
                    }
                    // TODO: selectedMenuOption.MenuApp?.RenderReturnPrompt() ?? ...
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

        //public static Dictionary<int, string> CreateMenuActions<T>() where T : Enum
        //{
        //    var result = new Dictionary<int, string>();
        //    foreach (var value in Enum.GetValues(typeof(T)))
        //    {
        //        result.Add((int)value, Enum.GetName(typeof(T), value)!);
        //    }
        //    return result;
        //}

        /* TODO: Use TypeBuilder to create a MenuAction Enum at runtime?
         * https://learn.microsoft.com/en-us/dotnet/api/system.reflection.emit.typebuilder?view=net-9.0#examples
         */
        //public static void MenuActionBuilder()
        //{
        //    AppDomain currentDomain = AppDomain.CurrentDomain;

        //    AssemblyName assemblyName = new AssemblyName("DynamicMenuActionsAssembly");
        //    AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
        //        assemblyName,
        //        AssemblyBuilderAccess.Run);

        //    ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(
        //        assemblyName.Name ?? "DynamicMenuActionsAssembly");

        //    TypeBuilder typeBuilder = moduleBuilder.DefineType(
        //        "DynamicMenuActions",
        //        TypeAttributes.Public);
        //}
    }

    public abstract class MenuApplication : IMenuListable
    {
        public abstract string Name { get; set; }
        public abstract void Render();
    }

    public class MenuOption
    {
        public string Name;
        public int Action;
        public IMenuListable? MenuApp;

        public MenuOption(string name, int action)
        {
            Name = name;
            Action = action;
            MenuApp = null;
        }
        public MenuOption(IMenuListable menuApp, int action)
        {
            Name = menuApp.Name;
            Action = action;
            MenuApp = menuApp;
        }
    }
}
