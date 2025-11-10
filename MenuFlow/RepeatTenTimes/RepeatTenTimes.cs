using MenuFlow.Library;

namespace RepeatTenTimes
{
    public class RepeatTenTimesApplication(string name) : MenuApplication
    {
        public override string Name { get; set; } = name;

        public override void Render()
        {
            DisplayIntro();
            Console.WriteLine($"\nEnter a word to see it repeated ten times:");
            //TODO: Add validation
            string word = Console.ReadLine() ?? "";
            for (int i = 0; i < 10; i++) { 
                string trailingComma = i < 9 ? "," : "";
                Console.Write($"{i + 1}. {word}{trailingComma} ");
            }
            Console.WriteLine("\n");
            Console.WriteLine("\n\tPress any key to return to the main menu.");
            Console.ReadKey(true);
        }
    }
}
