using System;

namespace _1000
{
    class Game
    {
        static public int Nplayers;
        static public int Round;
        //static public int Turn;

        static int[,] ScoreTable;

        static public void CreateTable()
        {
            ScoreTable = new int[1, Nplayers];
        }

        static public void intoTable(in int id, Player[] Player)
        {
            ScoreTable[Round, id] = Player[id].score;

            //если игрок набрал такое же кол-во очков как у соперника, очки соперника обнуляются
            //если игрок обоганл соперника, соперник теряет 50 очков
            if (true)
            {
                for (int i = 0; i < Nplayers; i++)
                {
                    if (i != id)
                    {
                        if (Player[id].score == Player[i].score && Player[i].score > 0)
                        {
                            Player[i].score = 0;
                            if (id < i)
                                ScoreTable[Round - 1, i] = 0;
                            else
                                ScoreTable[Round, i] = 0;
                            Console.WriteLine("Игрок {0} догнал игрока {1} и обнулил его очки", Player[id].name, Player[i].name);
                        }
                        int resLastRoundForId = Round > 0 ? ScoreTable[Round - 1, id] : 0;
                        if (resLastRoundForId < Player[i].score && ScoreTable[Round, id] > Player[i].score)
                        {
                            int penalty = Player[i].score < 50 ? Player[i].score : 50;
                            Player[i].score -= penalty;
                            if (id < i)
                                ScoreTable[Round - 1, i] -= penalty;
                            else
                                ScoreTable[Round, i] -= penalty;
                            Console.WriteLine("Игрок {0} обогнал игрока {1}, он теряет 50 очков", Player[id].name, Player[i].name);
                        }
                    }
                }
            }
        }
        static public void isBarrel(Player[] Player)
        {
            for (int i = 0; i < Nplayers; i++)
            {
                if (Player[i].score >= 300 && Player[i].score <= 400)
                    Player[i].barrel = 1;
                else if (Player[i].score >= 600 && Player[i].score <= 700)
                    Player[i].barrel = 2;
                else if (Player[i].score >= 900 && Player[i].score <= 1000)
                    Player[i].barrel = 3;
                else
                    Player[i].barrel = 0;
            }
        }
        static public void AddRound()
        {
            int[,] temparr = new int[ScoreTable.GetLength(0) + 1, Nplayers];

            for (int i = 0; i < ScoreTable.GetLength(0); i++)
            {
                for (int j = 0; j < Nplayers; j++)
                {
                    temparr[i, j] = ScoreTable[i, j];
                }
            }
            ScoreTable = temparr;
            Round++;
        }
        static public void PrintScore(in int id, Player[] Player)
        {
            Console.WriteLine();
            foreach (var pl in Player)
                Console.Write(pl.name + "\t");
            Console.WriteLine();

            for (int i = 0; i < ScoreTable.GetLength(0); i++)
            {
                for (int j = 0; j < ScoreTable.GetLength(1); j++)
                {
                    if (i == Round && j == id + 1)
                        break;
                    if (Round > 0 && i > 0)
                        if (ScoreTable[i - 1, j] == ScoreTable[i, j])
                        {
                            Console.Write(" - \t");
                            continue;
                        }
                    Console.Write(ScoreTable[i, j] + "\t");
                }
                Console.WriteLine();
            }

            if (Player[id].score > 1000)
                Console.WriteLine("{0} победил!", Player[id].name);
            Console.ReadLine();
        }

        static public bool HaveWinner(in int id, Player[] Player)
        {
            if (Player[id].score > 1000)
            {
                Console.WriteLine("{0} победил!", Player[id].name);
                Console.ReadLine();
                return true;
            }
            else return false;
            //что если победителя определять в конце раунда, чтобы дать шанс не сходившим игрокам 
            //foreach (var pl in player)
            //{
            //    pl.score > 1000...
            //}

        }
    }
}
