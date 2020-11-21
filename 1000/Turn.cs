using System;
using System.Linq;

namespace _1000
{
    class Turn : Program
    {
        /// <summary>
        /// Возвращает число очков выпавших от броска кубиков (от 1 до 5) и изменяет число доступных костей для переброса 
        /// </summary>
        /// <param name="QuantityOfDices"></param>
        /// <returns></returns>
        static public int Hurl(ref int QuantityOfDices)
        {
            if (QuantityOfDices == 0) { Console.WriteLine("Этого не может быть!"); return 0; }

            /////////////////////// HURL PROCESSING /////////////////////////

            Random random = new Random();

            int[] ValuesOfDices = new int[QuantityOfDices];

            for (int Dice = 0; Dice < QuantityOfDices; Dice++)
            {
                ValuesOfDices[Dice] = random.Next(1, 7);
            }
            Console.WriteLine();

            for (int Dice = 0; Dice < QuantityOfDices; Dice++)  
            {
                Console.Write(ValuesOfDices[Dice] + "  ");
            }
            Console.WriteLine("\n");

            ////////////////////// SCORE CALCULATION /////////////////////////

            int[] QuantityOfEachEdge = new int[6];

            for (int Edge = 0; Edge < 6; Edge++)
            {
                for (int Dice = 0; Dice < QuantityOfDices; Dice++)
                {
                    if (ValuesOfDices[Dice] == Edge + 1)
                    {
                        QuantityOfEachEdge[Edge] += 1;
                    }
                }
            }

            //TEST straight
            //QuantityOfEachEdge[5] = 0;
            //for (int i = 0; i < 5; i++)
            //    QuantityOfEachEdge[i] = 1;

            int[] minor_straight = { 1, 1, 1, 1, 1, 0 }; // 1 2 3 4 5  combination 

            if (QuantityOfEachEdge.SequenceEqual(minor_straight))
            {
                QuantityOfDices = 5;
                Console.WriteLine("Выпал Младший стрит!\n");
                return 100;
            }

            int[] major_straight = { 0, 1, 1, 1, 1, 1 }; // 2 3 4 5 6 combination 

            if (QuantityOfEachEdge.SequenceEqual(major_straight))
            {
                QuantityOfDices = 5;
                Console.WriteLine("Выпал Старший стрит!\n");
                return 200;
            }

            int HurlScore = 0;
            int[] EdgeCost = { 10, 2, 3, 4, 5, 6 };
            int[] ScoreCoefficient = { 0, 1, 2, 10, 20, 100 };
            int NotPlayedDices = 0;
            int NotPlayedDouble = 0;
            for (int Edge = 0; Edge < 6; Edge++)
            {
                int InterResult = 0;

                InterResult += ScoreCoefficient[QuantityOfEachEdge[Edge]] * EdgeCost[Edge];

                if (InterResult % 5 == 0)
                {
                    HurlScore += InterResult;
                }
                else
                {
                    NotPlayedDices += QuantityOfEachEdge[Edge];
                }
                if (QuantityOfDices == 2 && QuantityOfEachEdge[Edge] == 2)
                {
                    NotPlayedDouble += 1;
                }
            }
            //////////////// DETERMINING THE QUANTITY OF DICES /////////////////

            int NewQuantityOfDices = 0;

            if (HurlScore == 0 && NotPlayedDouble != 1)
            {
                NewQuantityOfDices = 0;
                Console.WriteLine("Ни одна кость не сыграла\n");
            }
            else if (HurlScore == 0 && NotPlayedDouble == 1)
            {
                NewQuantityOfDices = 2;
                Console.WriteLine("Выпал дубль, можно перебросить\n");
            }
            else if (HurlScore > 0 && NotPlayedDices > 0)
            {
                NewQuantityOfDices = NotPlayedDices;
                Console.WriteLine(WordDice("Сыграло", (QuantityOfDices - NotPlayedDices)) + $"! + {HurlScore} очков\n");
            }
            else if (HurlScore > 0 && NotPlayedDices == 0)
            {
                NewQuantityOfDices = 5;
                Console.WriteLine("Сыграли все кости! + {0} очков\n", HurlScore);
            }
            else
            {
                Console.WriteLine("СЛУЧИЛАСЬ КАКАЯ-ТО НЕПРЕДВИДЕННАЯ \n");
                Console.Beep(1000, 1000);
            }
            QuantityOfDices = NewQuantityOfDices;
            return HurlScore;
        }
    }
}
