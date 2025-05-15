using System;
using System.Collections.Generic;

namespace Rock_Paper_Scissors_Console_Game
{
    internal class Program
    {
        private enum FightOptionsEnum: byte
        {
            No,
            Yes
        }
        
        private enum Weapons: byte
        {
            Rock, 
            Paper, 
            Scissors
        }
        
        private static readonly FightOptionsEnum[] _fightOptions = { FightOptionsEnum.Yes, FightOptionsEnum.No };
        private static readonly Weapons[] _weaponOptions = { Weapons.Rock, Weapons.Paper, Weapons.Scissors };
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Rock-Paper-Scissors Arena";
            
            string nickname = AskNickname();
            byte age = AskAge();

            byte totalRounds = 0;
            byte totalWins = 0;
            
            while (true)
            {
                Console.Clear();
                ShowStats(nickname, age, totalRounds, totalWins);
                
                Console.WriteLine("Are you ready to fight? Use arrow keys to choose.\n");
                int readyIndex = ShowMenu(string.Empty, new[] { FightOptionsEnum.Yes.ToString(), FightOptionsEnum.No.ToString() });

                if (_fightOptions[readyIndex] == FightOptionsEnum.No)
                {
                    SayGoodbye(nickname);
                    break;
                }
                
                bool won = StartBattle(nickname);
                totalRounds++;
                if (won) totalWins++;
            }
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
        
        private static int ShowMenu(string prompt, string[] options)
        {
            int selected = 0;
            ConsoleKey key;
            
            int cursorTop = Console.CursorTop;

            if (!string.IsNullOrWhiteSpace(prompt))
            {
                Console.WriteLine($"{prompt}");
                cursorTop = Console.CursorTop;
            }

            do
            {
                Console.SetCursorPosition(0, cursorTop);

                for (int i = 0; i < options.Length; i++)
                {
                    Console.ForegroundColor = (i == selected) ? ConsoleColor.Green : ConsoleColor.Gray;
                    Console.WriteLine($"{(i == selected ? "> " : "  ")}{options[i]}".PadRight(Console.WindowWidth - 1));
                }
                Console.ResetColor();
                
                Console.SetCursorPosition(0, cursorTop);

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.UpArrow)
                {
                    selected = (selected == 0) ? options.Length - 1 : selected - 1;
                } else if (key == ConsoleKey.DownArrow)
                {
                    selected = (selected + 1) % options.Length;
                }

            } while (key != ConsoleKey.Enter);

            Console.SetCursorPosition(0, cursorTop + options.Length);
            return selected;
        }
        
        private static void SayGoodbye(string nickname)
        {
            Console.Clear();
            string[] messages = {
                $"Take care, {nickname}! See you next time!",
                $"Goodbye, {nickname}! Come back stronger!",
                $"Thanks for playing, {nickname}! Until next battle!"
            };

            Random rand = new Random();
            string message = messages[rand.Next(messages.Length)];

            string border = new string('=', message.Length + 4);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n{border}");
            Console.WriteLine($"= {message} =");
            Console.WriteLine($"{border}\n");
            Console.ResetColor();
        }
        
        private static bool StartBattle(string nickname)
        {
            int userWins = 0, aiWins = 0;
            Random rand = new Random();
            List<string> battleLog = new List<string>();

            for (int round = 1; round <= 3; round++)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("+----------------------+|      ROUND {0} START      |+----------------------+", round);
                Console.ResetColor();

                int selectedIndex = ShowMenu("Choose your weapon:", new[] { Weapons.Rock.ToString(), Weapons.Paper.ToString(), Weapons.Scissors.ToString() });
                Weapons userChoice = _weaponOptions[selectedIndex];
                Weapons aiChoice = _weaponOptions[rand.Next(_weaponOptions.Length)];

                PrintWeapon("You", userChoice);
                PrintWeapon("AI", aiChoice);

                string result = GetRoundWinner(userChoice, aiChoice);
                string resultText = result == "user" ? "You win" : result == "ai" ? "AI wins" : "Draw";
                battleLog.Add($"Round {round}: You → {userChoice.ToString().ToUpper()}, AI → {aiChoice.ToString().ToUpper()} → {resultText}");

                if (result == "user") userWins++;
                else if (result == "ai") aiWins++;
                
                Console.WriteLine($"\nPress any key to proceed to {(round == 3 ? "show result" : "next round")}...");
                Console.ReadKey(true);
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("╔═══════════════════════════════════════════════╗");
            Console.WriteLine("║                 BATTLE RESULT                 ║");
            Console.WriteLine("╚═══════════════════════════════════════════════╝");
            Console.ResetColor();

            foreach (var log in battleLog)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"→ {log}");
            }
            Console.ResetColor();
            Console.WriteLine();
            
            if (userWins >= 2)
                PrintVictoryMessage(nickname);
            else if (aiWins >= 2)
                PrintDefeatMessage(nickname);
            else
                PrintDrawMessage(nickname);
            
            Console.WriteLine("Press any key to view your updated stats...");
            Console.ReadKey(true);
            Console.Clear();
            return userWins >= 2;
        }

        private static string GetRoundWinner(Weapons user, Weapons ai)
        {
            if (user == ai) return "draw";
            if ((user == Weapons.Rock && ai == Weapons.Scissors) ||
                (user == Weapons.Scissors && ai == Weapons.Paper) ||
                (user == Weapons.Paper && ai == Weapons.Rock)) return "user";
            return "ai";
        }

        private static void PrintWeapon(string owner, Weapons weapon)
        {
            ConsoleColor boxColor = owner == "You" ? ConsoleColor.Green : ConsoleColor.Red;
            string weaponTitle = $"{owner} chose: {weapon.ToString().ToUpper()}";
            string[] art = GetAsciiArt(weapon);

            int width = Math.Max(weaponTitle.Length, 30);
            string border = new string('=', width);

            Console.ForegroundColor = boxColor;
            Console.WriteLine($"\n+{border}+");
            Console.WriteLine($"| {weaponTitle.PadRight(width)} |");
            Console.WriteLine($"+{border}+");
            Console.ResetColor();

            foreach (string line in art)
            {
                switch (weapon)
                {
                    case Weapons.Rock:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;
                    case Weapons.Paper:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case Weapons.Scissors:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                }
                Console.WriteLine("  " + line);
            }
            Console.ResetColor();

        }

        private static string[] GetAsciiArt(Weapons weapon)
        {
            switch(weapon)
            {
                case Weapons.Rock: {
                    return new[]
                    {
                        "    _______",
                        "---'   ____)",
                        "      (_____)",
                        "      (_____)",
                        "      (____)",
                        "---.__(___)"
                    };

                }
                case Weapons.Paper: {
                    return new[]
                    {
                        "     _______",
                        "---'   ____)____",
                        "          ______)",
                        "          _______)",
                        "         _______)",
                        "---.__________)"
                    };

                }
                case Weapons.Scissors: {
                    return new[]
                    {
                        "    _______",
                        "---'   ____)____",
                        "          ______)",
                        "       __________)",
                        "      (____)",
                        "---.__(___)"
                    };

                }
                default:
                {
                    return new[] { "[Unknown weapon]" };
                }
            };
        }

        private static void PrintVictoryMessage(string nickname)
        {
            string[] messages =
            {
                $"Well done, {nickname}! You're a true champion!",
                $"Victory is yours, {nickname}! Keep it up!",
                $"Fantastic win, {nickname}! You're unstoppable!"
            };
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(messages[new Random().Next(messages.Length)]);
            Console.ResetColor();
        }

        private static void PrintDefeatMessage(string nickname)
        {
            string[] messages =
            {
                $"Don't worry, {nickname}, you'll get them next time!",
                $"Keep going, {nickname}! Every loss is a lesson.",
                $"Chin up, {nickname}! Great effort!"
            };
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(messages[new Random().Next(messages.Length)]);
            Console.ResetColor();
        }
    
        private static void PrintDrawMessage(string nickname)
        {
            string[] messages = {
                $"Well fought, {nickname}! It’s a draw.",
                $"No winner this time, {nickname}. You held your ground!",
                $"Neither side won, {nickname}. Let’s call it a tie!"
            };

            Random rand = new Random();
            Console.WriteLine("\n" + messages[rand.Next(messages.Length)]);
        }
    
    }
}