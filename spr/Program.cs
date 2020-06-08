using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace spr
{
    class Program
    {
        private static string EvaluateMoves(short userMove, short computerMove, int moves)
        {
            if (userMove == computerMove) return "Tie!";
            short delta = (short)(userMove - computerMove);
            if((delta < 0 ? delta * -1 : delta) > moves / 2)
            {
                return delta < 0 ? "Computer loses" : "Computer wins";
            }
            else
            {
                return delta < 0 ? "Computer wins" : "Computer loses";
            }
    }

        static void Main(string[] args)
        {
            while (true)
            {
                if (args.Length % 2 == 0 || args.Length < 3)
                {
                    Console.WriteLine("Wrong arity");
                    return;
                }
                Random r = new Random();
                short computerMove = (short)r.Next(0, args.Length);
                byte[] key = new byte[32];
                RandomNumberGenerator generator = RandomNumberGenerator.Create();
                generator.GetBytes(key);
                HMACSHA256 hmac = new HMACSHA256(key);
                byte[] computedHmac = hmac.ComputeHash(Encoding.ASCII.GetBytes(args[computerMove]));
                Console.WriteLine(String.Format("HMAC: {0}", BitConverter.ToString(computedHmac).Replace("-", "")));
                int moveCode = 1;
                Console.WriteLine("U go. To exit enter 0\n Available moves:");
                foreach (var move in args)
                {
                    Console.WriteLine(String.Format("\n\t {0} = {1}", moveCode, move));
                    moveCode++;
                }
                short userMove = GetUserInput(args.Length);
                if (userMove == 0) return;
                userMove -= 1;
                Console.WriteLine("Your move: " + args[userMove]);
                Console.WriteLine("Computer move: " + args[computerMove]);
                Console.WriteLine(EvaluateMoves(userMove, computerMove, args.Length));
                Console.WriteLine(String.Format("HMAC KEY: {0}", BitConverter.ToString(key).Replace("-", "")));
                Console.ReadKey();
                Console.Clear();
            }
        }

        private static short GetUserInput(int length)
        {
            try
            {
                short input = Int16.Parse(Console.ReadLine());
                while (input > length || input < 0)
                {
                    Console.Write("Input incorrect! \n try again > ");
                    input = Int16.Parse(Console.ReadLine());
                }
                return input;
            }
            catch
            {
                Console.WriteLine("I didn't expect this to happen...");
                return GetUserInput(length);
            }
        }
    }
}
