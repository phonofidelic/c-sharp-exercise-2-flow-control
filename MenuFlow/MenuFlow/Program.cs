using MenuFlow.Library;

namespace MenuFlow
{
    internal class Program
    {
        /*
         * First command is always Quit (0). 
         * The rest of the commands are named using snake_case for conversion 
         * to display names when rendered in the menu.
         */
        public enum Commands
        {
            Quit,
            Youth_Or_Pensioner,
            Repeat_Ten_Times,
            The_Third_Word,
            Another_app,
            Hello_Again
        }
        static void Main(string[] args)
        {
            

            List<MenuOption> mainMenuOptions = [];
            

            Dictionary<int, string> actions = Menu<Commands>.CreateMenuActions<Commands>();

            foreach (var action in actions)
            {
                mainMenuOptions.Add(new MenuOption(action.Value.ToString().Replace('_', ' '), action.Key));
            }

            Menu<Commands> mainMenu = new("MenuFlow", mainMenuOptions);
            mainMenu.Display();

            Console.ReadLine();
        }
    }
}
