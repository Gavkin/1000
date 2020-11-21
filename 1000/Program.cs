using System;

namespace _1000
{
    class Program
    {
        static void Main()
        {
            //Begining
            Console.Title = "1000 - Культовая игра всех времен и народов";

            Console.WriteLine("_консольная_имитация_игры_в_кости_ТЫЩЩА_она_же_1000_она_же_КОСАРЬ".ToUpper());

            Console.WriteLine("\nЧтобы продолжить, нажмите вполне определённую клавишу [Space] ...");

            string[] phrases =
                {
                "Вы не можете попасть по пробелу?",
                "[Space] это такая продолговатая клавиша на клавиатуре под блоком с буквами",
                "Опеределённую клавишу [Space] - значит нажать нужно именно [Space]"
                };

            ConsoleKey key;
            string comment = "";
            while (true)
            {
                Console.WriteLine(comment);
                key = Console.ReadKey(true).Key;
                Console.Clear();
                if (key == ConsoleKey.Spacebar) break;
                else
                {
                    var rnd = new Random();
                    comment = phrases[rnd.Next(0, phrases.Length)];
                }
            }

            //Main menu
            while (true)
            {
                Console.Clear();
                Console.WriteLine("ГЛАВНОЕ МЕНЮ");
                Console.WriteLine("[F1] - правила\n[Enter] - начать игру\n[Esc] - выход");
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.F1:
                        PrintRules();
                        break;
                    case ConsoleKey.Enter:
                        NewGame();
                        break;
                    case ConsoleKey.Escape:
                        return;
                    default:
                        break;
                }
            }
        }

        private static void NewGame()
        {

            ConsoleKey key;
            GetNplayers();

            Game.CreateTable();

            var Player = new Player[Game.Nplayers];

            for (int i = 0; i < Game.Nplayers; i++)
            {
                Player[i] = new Player() { id = i, barrel = 0, entered = false };
                Console.WriteLine($"Введите имя {i + 1}-го игрока");
                string name = Console.ReadLine();
                Player[i].name = (name.Trim() == "") ? ("Никто" + (i + 1)) : name;
            }

            Game.Round = 0;
            int id = 0;
            int Spot;
            int QuantityOfDices;
            int minSpot;

            while (true)
            {
                QuantityOfDices = 5;
                minSpot = 0;

                Console.Clear();
                Console.WriteLine("(бросить кости/продолжить [Enter], перебросить несыгравшие кости [Num +])\n");
                Console.WriteLine("{0}-й раунд\n", Game.Round + 1);
                for (int i = 0; i < Game.Nplayers; i++)
                    Console.Write("{0}\t", Player[i].name);
                Console.WriteLine();
                for (int i = 0; i < Game.Nplayers; i++)
                    Console.Write("{0}\t", Player[i].score);

                Console.Write("\n\nХодит {0} \n", Player[id].name);

                // МИНСПОТ
                if (!Player[id].entered)
                {
                    Console.WriteLine("\nДля открытия счета первоначально наберите 50 очков!");
                    minSpot = 50;
                }

                switch (Player[id].barrel)
                {
                    case 1:
                        Console.WriteLine("\nВы в первой бочке! (300 - 400)");
                        minSpot = 405 - Player[id].score;
                        break;
                    case 2:
                        Console.WriteLine("\nВы во второй бочке! (600 - 700)");
                        minSpot = 705 - Player[id].score;
                        break;
                    case 3:
                        Console.WriteLine("\nВы в третьей бочке! (900 - 1000)");
                        minSpot = 1005 - Player[id].score;
                        break;
                    default:
                        break;
                }

                if (Player[id].barrel != 0)
                    Console.WriteLine("Для выхода из бочки необходимо набрать {0} очков", minSpot);

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Enter)
                    Spot = Turn.Hurl(ref QuantityOfDices); // Бросок костей (число костей CC..)
                else
                    continue; // Хрень.

                Console.WriteLine("У вас {0} очков!\n", Spot);

                Console.WriteLine(WordDice("Можно перебросить", QuantityOfDices));

                key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Add && Spot > 0)
                {
                    while (true)
                    {
                        Spot += Turn.Hurl(ref QuantityOfDices);

                        if (QuantityOfDices > 0)
                        {
                            Console.WriteLine("Вы набрали {0} очков!\t" + WordDice("Можно перебросить", QuantityOfDices), Spot);
                            key = Console.ReadKey(true).Key;
                            if (key == ConsoleKey.Add)
                                continue;
                            else
                                break;
                        }
                        else
                        {
                            Spot = 0;
                            Console.WriteLine("У вас 0 очков. Вы всё потеряли!");
                            Console.ReadKey(true);
                            break;
                        }
                    }
                }
                Console.Clear();
                if (Spot < minSpot)
                {
                    Spot = 0;
                    Console.WriteLine("{0}, вы набрали недостаточное количество очков!", Player[id].name);
                }

                if (!Player[id].entered && Spot >= 50)
                    Player[id].entered = true;

                Player[id].IsFail(Spot);

                Player[id].score += Spot;

                if (Player[id].score == 555)
                {
                    Console.WriteLine("{0}, вы набрали 555 очков! Ваш счёт обнуляется!", Player[id].name);
                    Player[id].score = 0;
                }

                Game.IntoTable(id, Player);

                Game.IsBarrel(Player);

                Game.PrintScore(id, Player);

                if (Game.HaveWinner(id, Player))
                    break;

                if (id < Game.Nplayers - 1) id++;
                else
                {
                    Game.AddRound();
                    id = 0;
                }
            }
            Console.WriteLine("В конце концов, конец в конце");
        }

        private static void GetNplayers()
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Введите число игроков");
                    Game.Nplayers = int.Parse(Console.ReadLine());
                    break;
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Необходимо ввести цифру!");
                    Console.Beep(1000, 1000);
                }
            }
        }

        protected static string WordDice(string phrase, int QuantityOfDices)
        {
            switch (QuantityOfDices)
            {
                case 0: return "";
                case 1: return phrase + " 1 кость";
                case 2:
                case 3:
                case 4: return phrase + " " + QuantityOfDices + " кости";
                case 5: return phrase + " 5 костей";
                default: return "ОШИБКА В КОДЕ";
            }
        }
        private static void PrintRules()
        {
            Console.WriteLine("\nПРАВИЛА \n\n (на самом деле можно не читать, т.к. приложение не даст играть не по правилам)\n" +
                "\nОсновная цель игры - превысить 1000 очков\n" +
                "Игроки ходят по очереди, бросая до 5 игральных костей\n" +
                "Очередность определяется вне функционала приложения, как-нибудь разберетесь\n\n" +
                "В начала игрок типа бросает все 5 костей.\n" +
                "Если среди выпавших костей есть сыгравшие (однёрки, пятёрки или комбинации),\n" +
                "которые дают очки - игрок может перебросить кости, которые очков не принесли.\n" +
                "Если сыграли все 5 костей, игрок может перебросить их все. Если ни одна кость \n" +
                "при первом броске или переброске не сыграла, то игрок не получает очков, а ход \n" +
                "передается следующему игроку\n\n" +
                "В ситуации, когда игрок перебрасывает 2 кости и выпадает дубль (например 2-2),\n" +
                "можно перебросить эти кости повторно или всё-таки записать счёт\n\n" +
                "Первоначально игрок должен набрать не менее 50 очков, до этого очки не засчитываются.\n" +
                "Если игрок не набирает очков три хода подряд, ему выписывается так называемый болт \n" +
                "и игрок теряет 50 очков, если таковые имеются (счет не может быть отрицательным).\n\n" +
                "По мере набора очков от 0 до 1000 встречаются так называемые бочки:\n" +
                "Первая бочка от 300 до 400\n" +
                "Вторая бочка от 600 до 700\n" +
                "Третья бочка от 900 до 1000\n\n" +
                "Попав в бочку игрок должен выбраться из неё за один раз, то есть набрать такое количество\n" +
                "очков, чтобы в результате хода счет превысил верхнюю границу бочки.\n\n" +
                "Если игрок набирает такое же число очков, как у соперника, то все очки соперника сгорают.\n" +
                "Если игрок обгоняет соперника, соперник теряет 50 очков.\n" +
                "Если счет игрока составит 555 очков, игрок теряет все очки, которые он набрал за игру.\n\n" +
                "Подсчет очков и комбинации костей:\n" +
                "По одной очки приносят только 1 и 5: \n" +
                "1 - 10 очков; \n" +
                "5 - 5 очков\n" +
                "Сет (3 кости одного значения) - значение кости (для единицы - 10) умножается на 10\n" +
                "Каре (4 кости одного значения - значение кости (для единицы - 10) умножается на 20\n" +
                "Флеш (5 костей одного значения - значение кости (для единицы - 10) умножается на 100\n" +
                "Младший стрит - по одной выпали кости со значением от 1 до 5 - 100 очков\n" +
                "Старший стрит - по одной выпали кости со значением от 2 до 6 - 200 очков\n");
            Console.ReadKey(true);
        }
    }
}

