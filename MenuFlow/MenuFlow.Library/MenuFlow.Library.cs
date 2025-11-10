using System.Reflection;
using System.Reflection.Emit;

namespace MenuFlow.Library
{
    public interface IMenuListable
    {
        public string Name { get; set; }
        public abstract void Render();
    }
    public class Menu : IMenuListable
    {
        public MenuContext Context { get; set; }
        private readonly string ContextName;
        public string Name { get; set; }
        protected List<MenuOption> MenuOptions { get; set; } = [];

        public Menu(string name, List<IMenuListable> apps, MenuContext context)
        {
            Name = name;
            MenuOptions.Add(new MenuOption("Quit", 0));
            foreach ((IMenuListable app, int index) in apps.Select((app, index) => (app, index)))
            {
                MenuOptions.Add(new MenuOption(app, index + 1));
            }
            Context = context;
            ContextName = Name.ToUpper().Replace(" ", "_");
            RegisterContext(ContextName);
        }
        private void RegisterContext(string contextName)
        {
            if (!Context.ContainsKey(contextName))
            {
                Context.Register(contextName);
            }
        }

        private Exception? MenuException = null;
        private void SetMenuException()
        {
            MenuException = null;
        }
        private void SetMenuException(Exception exception)
        {
            MenuException = exception;
        }
        private int ReadMenuActionFromKeyPress()
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
                DisplayIntro();
                for (int i = 1; i < MenuOptions.Count; i++)
                {
                    DisplayMenuOption(MenuOptions[i]);
                }

                DisplayExitCommand();

                if (MenuException != null)
                    DisplayError(MenuException.Message);

                try
                {
                    Context.SetSelectedAction(ContextName, ReadMenuActionFromKeyPress());
                    
                    switch (Context.GetSelectedAction(ContextName))
                    {
                        case 0:
                            RenderExit();
                            break;
                        case null:
                            break;
                        default:
                            var selectedMenuOption = MenuOptions[Context.GetSelectedAction(ContextName)!.Value];
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
                    
                    if (Context.GetSelectedAction(ContextName) == null)
                    {
                        Console.WriteLine($"\n\tPress any key to return to {Name}\n");
                        Console.ReadKey(true);
                    }
                }
                catch (Exception ex)
                {
                    SetMenuException(new Exception($"\t{ex.Message}\n\tPlease try again, or press \"Q\" to exit {Name}"));
                }
            } while (Context.GetSelectedAction(ContextName) != 0);
        }

        protected virtual void DisplayIntro() { }
        protected virtual void DisplayMenuOption(MenuOption menuOption) { }
        protected virtual void DisplayExitCommand() { }
        protected virtual void RenderExit() { }

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

    public class MenuContext : Dictionary<string, int?>
    {
        public int? SelectedAction { get; set; } = null;
        public void Register(string contextName)
        {
            this[contextName] = null;
        }
        public void SetSelectedAction(string contextName, int? selectedAction)
        {
            this[contextName] = selectedAction;
            SelectedAction = selectedAction;
        }
        public int? GetSelectedAction(string contextName)
        {
            return this[contextName];
        }
        public override string ToString()
        {
            string result = "";
            foreach (var item in this) {
                result += $"{item.Key} : {item.Value}\n";
            }
            return result;
        }
    }
}
