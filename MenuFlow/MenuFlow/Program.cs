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
            //Another_app,
            //Hello_Again
        }

        public class TestApp(string name) : MenuApplication
        {
            public override string Name { get; set; } = name;
            public override void Run()
            {
                Console.WriteLine($"Running application: {Name}");
                Console.WriteLine("\nThis is a Test App");
            }
        }
        public static TestApp testApp1 = new("Youth Or Pensioner");
        public static TestApp testApp2 = new("Repeat Ten Times");
        public static TestApp testApp3 = new("The Third Word");

        public static List<MenuApplication> testApps = [
            testApp1,
            testApp2,
            testApp3
        ];

        static void Main(string[] args)
        {
            Dictionary<int, string> actions = Menu<Commands>.CreateMenuActions<Commands>();

            Menu<Commands> mainMenu = new("MenuFlow", testApps);
            mainMenu.Display();

            Console.ReadLine();
        }
    }
}
