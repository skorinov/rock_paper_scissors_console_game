using System;

namespace Rock_Paper_Scissors_Console_Game
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Rock-Paper-Scissors Arena";
            
            string nickname = AskNickname();
            byte age = AskAge();

            byte totalRounds = 0;
            byte totalWins = 0;
            
            ShowStats(nickname, age, totalRounds, totalWins);
        }
        
        private static void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n+----------------------- ERROR -----------------------+");
            Console.WriteLine($"| {message.PadRight(49)} |");
            Console.WriteLine("+-----------------------------------------------------+");
            Console.ResetColor();
        }
        
        private static string AskNickname()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("+-----------------------------+");
                Console.WriteLine("|      Nickname Required      |");
                Console.WriteLine("+-----------------------------+");
                Console.Write("Enter your nickname: ");

                string nickname = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nickname))
                {
                    return nickname;
                }
                
                ShowError("Nickname cannot be empty.");
                Console.WriteLine("Press any key to try again...");
                Console.ReadKey(true);
            }
        }

        private static byte AskAge()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("+-------------------------------+");
                Console.WriteLine("|    Age Verification Required  |");
                Console.WriteLine("+-------------------------------+");
                Console.Write("Enter your age: ");

                bool isValidNumber = byte.TryParse(Console.ReadLine(), out byte age);

                if (isValidNumber && age >= 12)
                {
                    return age;
                }

                ShowError(!isValidNumber ? "Should be valid number" : "You must be at least 12 years old.");
                Console.WriteLine("Press any key to try again...");
                Console.ReadKey(true);
            }
        }
        
        private static void ShowStats(string nickname, int age, int rounds, int wins)
        {
            Console.Clear();
            Console.WriteLine("+==============================+");
            Console.WriteLine("|         STATISTICS           |");
            Console.WriteLine("+==============================+");
            Console.WriteLine($"| Nickname: {nickname}");
            Console.WriteLine($"| Age:      {age}");
            Console.WriteLine($"| Battles:  {rounds}");
            Console.WriteLine($"| Wins:     {wins}");
            Console.WriteLine("+==============================+\n");
        }
    }
}