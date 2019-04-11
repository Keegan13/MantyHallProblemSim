using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MantyHallProblemSim
{
    class Program
    {
        static void Main(string[] args)
        {
            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            Test.Run(100000);
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
        }
    }


    public static class Test
    {
        public static void Run(int count)
        {
            int sameWonCount = 0, changeWonCount = 0;
            var games = Game.GetRandomGames(count);
            Random r = new Random();
            foreach (var game in games)
            {
                var door = r.Next(1, 4);
                sameWonCount += game.Select(door).Select(door).Prize == "Car" ? 1 : 0;
                game.Reset();
                game.Select(door);
                var another = game.ClosedDoors.Single(x => x != door);
                if (another == door)
                    Console.WriteLine("Wrong door picked");
                changeWonCount += game.Select(door).Select(another).Prize == "Car" ? 1 : 0;
            }
            Console.WriteLine("After {0} games: \r\n Changing strategy: {1}({2:f2})% \r\n Same strategy: {3}({4:f2}%)", count, changeWonCount, 100f*changeWonCount / count,sameWonCount,100f*sameWonCount/count);

        }
    }

    public class Game
    {

        private int carPosition;
        private List<int> GoatDoors;
        public IEnumerable<int> ClosedDoors => GoatDoors.Concat(new[] { carPosition }).OrderBy(x => x);
        public string Prize = "NO";
        public Game(int doorWithCar)
        {
            this.GoatDoors = new List<int>();
            for (int i = 1; i < 4; i++)
            {
                if (doorWithCar != i)
                    GoatDoors.Add(i);
            }
            this.carPosition = doorWithCar;

        }
        public Game Select(int door)
        {
            if (!ClosedDoors.Contains(door))
                Console.WriteLine("Error");
            //Print();
            switch (GoatDoors.Count)
            {
                case 2:
                    var index = GoatDoors.IndexOf(door);
                    if (index >= 0)
                        GoatDoors.RemoveAt(index == 0 ? 1 : 0);
                    else
                        GoatDoors.RemoveAt((new Random()).Next(2));
                    break;

                case 1:
                    this.Prize = door == carPosition ? "Car" : "Goat";
                    break;
                default: break;
            }
            return this;
        }
        public void Print()
        {
            string str = String.Format("Car: {0} : Goats: {1}", carPosition, GoatDoors.Aggregate("", (a, n) => a += " " + n));



            Console.WriteLine(str);
        }
        public void Reset()
        {
            GoatDoors.Clear();
            for (int i = 1; i < 4; i++)
            {
                if (carPosition != i)
                    GoatDoors.Add(i);
            }
        }
        public static IEnumerable<Game> GetRandomGames(int count)
        {
            Game[] games = new Game[count];
            Random r = new Random();
            for (int i = 0; i < count; i++)
            {
                games[i] = new Game(r.Next(1, 3));
            }
            return games;
        }
    }
}
