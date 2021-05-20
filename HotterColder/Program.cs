using System;

namespace HotterColder
{

    /// <summary>
    /// RandomGenerator: wrapper class to generate a random number
    /// </summary>
    public static class RandomGenerator
    {
        private static readonly Random _random = new Random();

        // Generates a random number within a range.      
        public static int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
    }

    /// <summary>
    /// Game: Represents the Game
    /// If not over engineered i.e. out of scope and not required, 
    /// this could use a factory pattern implementing IGame
    /// i.e. Check() and NextGuess() could be given a different implementations on creation
    /// </summary>
    public interface IGame { }

    public class Game : IGame
    {
        private readonly int num;
        // more changes
        public string res { get; private set; }
        public bool state { get; private set; }
        private int guesssCount = 1;
        private int lastdiff = -1;

        public Game(int thenum) 
        {
            num = thenum;
        }

        /// <summary>
        /// NextGuess: Prompts and generates the next number, only accepts int
        /// </summary>
        /// <returns></returns>
        public int NextGuess()
        {
            int number = 0;
            string input;
            do
            {
                Console.Write("Please guess a number and press [Enter] ");
                input = Console.ReadLine();
            } while (int.TryParse(input, out number) == false);

            return number;
        }

        /// <summary>
        /// Check: Implements the game rules
        /// Better if implementation was configurable...
        /// </summary>
        /// <param name="guess"></param>
        /// <returns></returns>
        public bool Check(int guess)
        {
            
            int diff = Math.Abs(num - guess);

            if (diff == 0)
            {
                res = "You got it!!!!";
                return true;
            }

            if (guesssCount == 1)
            {
                res = (diff < 11) ? "Hot!" : "Cold!";
            }
            else
            {
                res = (diff < lastdiff) ? "Hotter!!!" : "Colder :-(";
            }

            lastdiff = diff;
            guesssCount++;
            return false;
        }



    }
    /// <summary>
    /// Program: Hot or Cold client container for the Game
    /// </summary>
    class Program
    {

        
        static void Main(string[] args)
        {
            int thenum = RandomGenerator.RandomNumber(1, 101);

            Console.WriteLine($"A random number between has been chosen. {Environment.NewLine}");

            bool guessed = false;
            // Instantiate the game
            // DI inject Rules as an IStrategy pattern?
            Game theGame = new(thenum);

            // Keep playing indefinitely until number is guessed
            // Might be better to have a rule when empty return marks user ends game rather than ^C
            while (!guessed)
            { 
                int guess = theGame.NextGuess();
                guessed = theGame.Check(guess);
                Console.WriteLine(theGame.res);
            }
            Console.Write("Press to exit.");
            Console.ReadKey();

        }
    }
}
