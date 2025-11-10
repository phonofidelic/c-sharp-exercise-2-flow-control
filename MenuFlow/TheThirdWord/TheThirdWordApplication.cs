using MenuFlow.Library;

namespace TheThirdWord
{
    public class TheThirdWordApplication(string name) : MenuApplication
    {
        public override string Name { get; set; } = name;
        public List<string>? Words { get; set; } = null;

        public override void Render()
        {
            do
            {
                Console.Clear();
                DisplayIntro();
                Console.WriteLine("\n");
                DisplayError(MenuApplicationException?.Message ?? "");
                Console.WriteLine($"Enter 3 words or more:");

                string rawInput = Console.ReadLine() ?? "";

                if (!ValidateSentence(rawInput))
                {
                    int rawWordCount = rawInput.Length == 0 ? 0 : rawInput.Trim().Split().Length;
                    string none = "did not provide any words";
                    string one = "only provided one word";
                    string plural = $"only provided {rawWordCount} words";
                    string providedWordsMessage;
                    switch (rawWordCount)
                    {
                        case 0:
                            providedWordsMessage = none;
                            break;
                        case 1: 
                            providedWordsMessage = one;
                            break;
                        case 2:
                            providedWordsMessage = plural;
                            break;
                        default:
                            providedWordsMessage = "did not provide enough";
                            break;
                    };
                    MenuApplicationException = 
                        new Exception($"Sorry, you {providedWordsMessage}. Try entering {3 - rawWordCount} more.");
                    Words = null;
                }
                else
                {
                    MenuApplicationException = null;
                    Words = [.. rawInput.Split(' ')];
                    Console.Clear();
                    DisplayIntro();
                    Console.WriteLine("\n");
                    Console.WriteLine("\n");
                    Console.WriteLine("\n");
                    Console.WriteLine($"{string.Join(' ', Words)}");
                    Console.WriteLine($"\n\tThe third word is \"{Words.ElementAt(2)}\"");
                    Console.WriteLine("\n");
                    Console.WriteLine("\n\tPress any key to return to the main menu.");
                    Console.ReadKey(true);
                }
            } while (Words == null | Words?.Count < 3);
        }

        protected bool ValidateSentence(string sentence)
        {
            string[] words = sentence.Split(' ');
            if (words.Length < 3)
            {
                return false;
            }

            return true;
        }
    }
}
