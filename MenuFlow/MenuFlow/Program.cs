using MenuFlow.Library;

namespace MenuFlow
{
    internal class Program
    {
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
            Menu mainMenu = new("MenuFlow", testApps);
            mainMenu.Display();

            Console.ReadLine();
        }
    }
}
