using System;
using System.Collections.Generic;

namespace Rock_Paper_Scissors_Console_Game
{
    internal class Program
    {
        // Enum for Yes/No fight prompt
        private enum FightOptionsEnum: byte
        {
            No,
            Yes
        }
        
        // Player enum
        private enum Player
        {
            User,
            AI,
        }
        // Enum for available weapons
        private enum Weapons: byte
        {
            Rock, 
            Paper, 
            Scissors
        }
        // Enum for possible outcomes of a round/battle
        private enum BattleOutcome
        {
            Victory,
            Defeat,
            Draw
        }
        
        // Arrays to store enum values for menu navigation
        private static readonly FightOptionsEnum[] _fightOptions = { FightOptionsEnum.Yes, FightOptionsEnum.No };
        private static readonly Weapons[] _weaponOptions = { Weapons.Rock, Weapons.Paper, Weapons.Scissors };
        
        /*
         * Main entry point of the application
         */
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Rock-Paper-Scissors Arena";

            // Prompt for user nickname and age
            string nickname = AskNickname();
            byte age = AskAge();

            // Initialize statistics for the session (not persisted)
            byte totalRounds = 0;
            byte totalWins = 0;
            
            // Main game loop: repeat until user chooses to exit
            while (true)
            {
                Console.Clear();

                WithColor(ConsoleColor.DarkCyan, () =>
                {
                    if (totalRounds == 0)
                    {
                        PrintBoxedText("Welcome to the Rock-Paper-Scissors Arena!", 0);
                    }
                    // Show user statistics for current session
                    ShowStats(nickname, age, totalRounds, totalWins);
                    Console.WriteLine($"Are you ready{(totalRounds > 0 ? " for the next " : " to ")}fight? (use arrow keys to choose):\n");
                });
                int readyIndex = ShowMenu(string.Empty, new[] { FightOptionsEnum.Yes.ToString(), FightOptionsEnum.No.ToString() });

                if (_fightOptions[readyIndex] == FightOptionsEnum.No)
                {
                    // Exit the game
                    SayGoodbye(nickname);
                    break;
                }
                
                // Start a battle and update statistics
                bool won = StartBattle(nickname);
                totalRounds++;
                if (won) totalWins++;
            }
        }
        
        /*
         * Ask player for nickname (non-empty input)
         */
        private static string AskNickname()
        {
            while (true)
            {
                Console.Clear();
                WithColor(ConsoleColor.DarkCyan, () =>
                {
                    PrintBoxedText("Nickname Required");
                    Console.Write("Enter your nickname: ");
                });

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

        /*
         * Ask player for age (must be a valid number >= 12)
         */
        private static byte AskAge()
        {
            while (true)
            {
                Console.Clear();
                WithColor(ConsoleColor.DarkCyan, () =>
                {
                    PrintBoxedText("Age Verification Required");
                    Console.Write("Enter your age: ");
                });

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
        
        /*
         * Start a battle of three rounds against AI; returns true if player wins the battle
         */
        private static bool StartBattle(string nickname)
        {
            int userWins = 0, aiWins = 0;
            Random rand = new Random();
            List<string> battleLog = new List<string>();

            for (int round = 1; round <= 3; round++)
            {
                Console.Clear();
                WithColor(ConsoleColor.DarkCyan, () =>
                    Console.WriteLine($"+----------------------+|      ROUND {round} START      |+----------------------+")
                );

                int selectedIndex = ShowMenu("Choose your weapon (use arrow keys to choose):", new[] { Weapons.Rock.ToString(), Weapons.Paper.ToString(), Weapons.Scissors.ToString() });
                Weapons userChoice = _weaponOptions[selectedIndex];
                Weapons aiChoice = _weaponOptions[rand.Next(_weaponOptions.Length)];

                // Display chosen weapons with ASCII art
                PrintWeapon(Player.User, userChoice);
                PrintWeapon(Player.AI, aiChoice);

                // Determine outcome of the round
                BattleOutcome result = GetRoundWinner(userChoice, aiChoice);
                string resultText = result == BattleOutcome.Victory ? "You win" : result == BattleOutcome.Defeat ? "AI wins" : "Draw";
                battleLog.Add($"Round {round}: You → {userChoice.ToString().ToUpper()}, AI → {aiChoice.ToString().ToUpper()} → {resultText}");

                if (result == BattleOutcome.Victory) userWins++;
                else if (result == BattleOutcome.Defeat) aiWins++;
                
                Console.WriteLine($"\nPress any key to proceed to {(round == 3 ? "show result" : "next round")}...");
                Console.ReadKey(true);
            }
            Console.Clear();
            BattleOutcome outcome = userWins >= 2 ? BattleOutcome.Victory : aiWins >= 2 ? BattleOutcome.Defeat : BattleOutcome.Draw;

            WithColor(ConsoleColor.DarkCyan, () =>
            {
                PrintBoxedText($"BATTLE RESULT: {outcome.ToString().ToUpper()}");
            });

            WithColor(ConsoleColor.DarkCyan, () =>
            {
                foreach (var log in battleLog)
                {
                    Console.WriteLine($"→ {log}");
                }
            });
            Console.WriteLine();

            // Print final battle outcome message
            PrintBattleMessage(nickname, outcome);
            
            Console.WriteLine("Press any key to view your updated stats...");
            Console.ReadKey(true);
            Console.Clear();
            return userWins >= 2;
        }

        /*
         * Determine the outcome of a single round based on user's and AI's weapons
         */
        private static BattleOutcome GetRoundWinner(Weapons user, Weapons ai)
        {
            if (user == ai) return BattleOutcome.Draw;
            if ((user == Weapons.Rock && ai == Weapons.Scissors) ||
                (user == Weapons.Scissors && ai == Weapons.Paper) ||
                (user == Weapons.Paper && ai == Weapons.Rock)) return BattleOutcome.Victory;
            return BattleOutcome.Defeat;
        }

        /*
         * Display ASCII art representation of the chosen weapon for a given player
         */
        private static void PrintWeapon(Player player, Weapons weapon)
        {
            ConsoleColor boxColor = player == Player.User ? ConsoleColor.DarkBlue : ConsoleColor.DarkRed;
            string playerText = player == Player.User ? "You" : player == Player.AI ? "AI" : player.ToString();
            string weaponTitle = $"{playerText} chose: {weapon.ToString().ToUpper()}";
            string[] art = GetAsciiArt(weapon);

            int width = Math.Max(weaponTitle.Length, 30);
            string border = new string('=', width);

            WithColor(boxColor, () =>
            {
                Console.WriteLine($"\n+{border}+");
                Console.WriteLine($"| {weaponTitle.PadRight(width)} |");
                Console.WriteLine($"+{border}+");
                
                foreach (string line in art)
                {
                    Console.WriteLine("  " + line);
                }
            });
        }

        /*
         * Return ASCII art lines for the specified weapon
         */
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
        
        /*
         * Show current stats screen
         */
        private static void ShowStats(string nickname, int age, int rounds, int wins)
        {
            int boxWidth = 40;
            string border = new string('=', boxWidth);
            string title = "= STATISTICS =";

            // Center title inside the box width
            int padding = (boxWidth - title.Length) / 2;
            string centeredTitle = new string(' ', padding) + title;

            Console.WriteLine($"\n{border}");
            Console.WriteLine(centeredTitle);
            Console.WriteLine($"{border}\n");

            Console.WriteLine($"| Nickname: {nickname}");
            Console.WriteLine($"| Age:      {age}");
            Console.WriteLine($"| Battles:  {rounds}");
            Console.WriteLine($"| Wins:     {wins}");
            Console.WriteLine(new string('=', boxWidth) + "\n");
        }

        /*
         * Print a randomized victory, defeat, or draw message based on battle outcome
         */
        private static void PrintBattleMessage(string nickname, BattleOutcome outcome)
        {
            string[] messages;
            ConsoleColor color;

            switch (outcome)
            {
                case BattleOutcome.Victory:
                {
                    messages = new[]
                    {
                        $"Well done, {nickname}! You're a true champion!",
                        $"Victory is yours, {nickname}! Keep it up!",
                        $"Fantastic win, {nickname}! You're unstoppable!",
                        $"Amazing skills, {nickname}! The Rock-Paper-Scissors crown belongs to you!",
                        $"Brilliant strategy, {nickname}! You've outsmarted the AI opponent!"
                    };
                    color = ConsoleColor.DarkGreen;
                    break;
                }
                case BattleOutcome.Defeat:
                {
                    messages = new[]
                    {
                        $"Don't worry, {nickname}, you'll get them next time!",
                        $"Keep going, {nickname}! Every loss is a lesson.",
                        $"Chin up, {nickname}! Great effort!",
                        $"The best warriors learn from defeat, {nickname}. You'll come back stronger!",
                        $"That was just bad luck, {nickname}! Your Rock-Paper-Scissors skills are still impressive!"
                    };
                    color = ConsoleColor.DarkRed;
                    break;
                }
                case BattleOutcome.Draw:
                default:
                {
                    messages = new[]
                    {
                        $"Well fought, {nickname}! It's a draw.",
                        $"No winner this time, {nickname}. You held your ground!",
                        $"Neither side won, {nickname}. Let's call it a tie!",
                        $"A perfect balance of skill, {nickname}! This match ends in equilibrium.",
                        $"Great minds think alike, {nickname}! This draw shows you're evenly matched with the AI!"
                    };
                    color = ConsoleColor.DarkYellow;
                    break;
                }
            }

            WithColor(color, () => Console.WriteLine("\n" + messages[new Random().Next(messages.Length)]));
            Console.ResetColor();
        }
        
        /*
         * Display a goodbye message when user exits the game
         */
        private static void SayGoodbye(string nickname)
        {
            Console.Clear();
            string[] messages = 
            {
                $"Take care, {nickname}! See you next time!",
                $"Goodbye, {nickname}! Come back stronger!",
                $"Thanks for playing, {nickname}! Until next battle!",
                $"The arena will await your return, {nickname}! Rest well, warrior!",
                $"Even champions need a break, {nickname}! The Rock-Paper-Scissors challenge will be here when you return!"
            };

            Random rand = new Random();
            string message = messages[rand.Next(messages.Length)];

            string border = new string('=', message.Length + 4);

            WithColor(ConsoleColor.DarkCyan, () =>
            {
                Console.Clear();
                Console.WriteLine($"\n{border}");
                Console.WriteLine($"= {message} =");
                Console.WriteLine($"{border}\n");
            });
        }
        
        /*
         * Arrow-key menu system
         */
        private static int ShowMenu(string prompt, string[] options)
        {
            int selected = 0;
            ConsoleKey key;
            
            int cursorTop = Console.CursorTop;
            Console.CursorVisible = false;

            if (!string.IsNullOrWhiteSpace(prompt))
            {
                WithColor(ConsoleColor.DarkCyan, () => Console.WriteLine(prompt));
                cursorTop = Console.CursorTop;
            }

            do
            {
                Console.SetCursorPosition(0, cursorTop);

                for (int i = 0; i < options.Length; i++)
                {
                    Console.ForegroundColor = (i == selected) ? ConsoleColor.Cyan : ConsoleColor.Gray;
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
            Console.CursorVisible = true;
            return selected;
        }
        
        /*
         * Display a styled red error box with a message
         */
        private static void ShowError(string message)
        {
            WithColor(ConsoleColor.Red, () =>
            {
                Console.WriteLine("\n+----------------------- ERROR -----------------------+");
                Console.WriteLine($"| {message.PadRight(49)} |");
                Console.WriteLine("+-----------------------------------------------------+");
            });
        }
        
        /*
         * Temporarily sets the console foreground color, runs the provided action, then resets the color.
         */
        private static void WithColor(ConsoleColor color, Action action)
        {
            Console.ForegroundColor = color;
            try
            {
                action();
            }
            finally
            {
                Console.ResetColor();
            }
        }
        
        /*
         * Displays a given text string inside a styled box with borders,
         * aligned from the left with minimum width.
         */
        private static void PrintBoxedText(string text, int indent = 1, int minWidth = 40)
        {
            int contentWidth = Math.Max(minWidth, text.Length + 4);
            int padding = (contentWidth - text.Length) / 2;

            string top = "╔" + new string('═', contentWidth) + "╗";
            string middle = "║" + new string(' ', padding) + text + new string(' ', contentWidth - text.Length - padding) + "║";
            string bottom = "╚" + new string('═', contentWidth) + "╝";

            Console.SetCursorPosition(indent, Console.CursorTop);
            Console.WriteLine(top);

            Console.SetCursorPosition(indent, Console.CursorTop);
            Console.WriteLine(middle);

            Console.SetCursorPosition(indent, Console.CursorTop);
            Console.WriteLine(bottom);
        }
    
    }
}