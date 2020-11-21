using System;

namespace _1000
{
    // 1 игрок ...
    // его свойства:
    // тек счет (бочка/не бочка), вход в игру (набрал первый раз 50), количество неудач подряд перед ходом от 0 до 3 (штраф -50 если набрал 3) ; ...
    class Player
    {
        public int id;
        public string name;
        public bool entered;
        public int score;
        public int fails;
        public int barrel;

        public void isFail(int Spot)
        {
            if (Spot == 0) fails += 1;
            else fails = 0;
            if (fails >= 3)
            {
                Console.WriteLine("{0} получает болт (-50 очков, если счет больше нуля)", name);
                if (score >= 50)
                    score -= 50;
                else
                    score = 0;
                fails = 0;
            }
        }
    }
}
