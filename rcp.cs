using System;
using System.Security.Cryptography;
using static System.Console;

namespace igra
{
    class RNG
    {
        public static byte[] GenKey()
        {
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            byte[] tokenData = new byte[32];
            rng.GetBytes(tokenData);
            //string token = Convert.ToBase64String(tokenData);
            return (tokenData);
        }
    }
    class HashGen
    {
        public static byte[] ComputeHmacsha3(byte[] data, byte[] key)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(data);
            }
        }
    }
    class Winner
    {
        public static int Resulte(int Comppos, int curpos, int count)
        {
            int halfcount = count / 2;
            if (Comppos == curpos)
            {
                return 2;
            }
            else if (curpos + halfcount <= count - 1)
            {
                if (curpos > Comppos || Comppos > curpos + halfcount)
                    return 1;
                else return 0;
            }
            else
            {
                int c = (curpos + halfcount) % count;
                if ((Comppos > c && Comppos < curpos))
                    return 1;
                else return 0;
            }
        }
    }
    class Info
    {
        public static void inf(int count, string[] args)
        {
            for (int i = 0; i < count; i++)
            {
                WriteLine();
                Write($"{i + 1} - ");
                for (int j = 0; j < count; j++)
                {
                    if (Winner.Resulte(j, i, count) == 1)
                    {
                        Write($"{args[j]} ");
                    }

                }

            }
            WriteLine();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3 || args.Length % 2 == 0)
            {
                WriteLine("The number of moves must be odd and greater than 3!!!");
            }
            else
            {
                byte[] key = RNG.GenKey();
                int ChoiseComp = GenChoise(args);
                byte[] hmac = HashGen.ComputeHmacsha3(System.Text.Encoding.UTF8.GetBytes(args[ChoiseComp]), key);
                WriteLine($"HMAC: {Convert.ToBase64String(hmac)}");

                int plchoice = -99;
                while (plchoice == -99)
                {
                    PRNTMoves(args);
                    string move = ReadLine();
                    if (move == "?")
                    {
                        Info.inf(args.Length, args);
                    }
                    else if (char.IsDigit(move[0]) && Convert.ToInt32(move) >= 0 && Convert.ToInt32(move) <= args.Length)
                    {
                        plchoice = Convert.ToInt32(move);
                    }
                }
                if (plchoice == 0)
                {
                    Environment.Exit(0);
                }
                plchoice -= 1;
                WriteLine($"your move: {args[plchoice]}");
                WriteLine($"Computer move: {args[ChoiseComp]}");
                int res = Winner.Resulte(ChoiseComp, plchoice, args.Length);
                if (res == 0)
                    WriteLine("You Lose!");
                else if (res == 1)
                    WriteLine("You Win!");
                else
                    WriteLine("Draw!");
                WriteLine($"HMAC key: {Convert.ToBase64String(key)}");

            }
        }
        static int GenChoise(string[] args)
        {
            Random rnd = new Random();
            return rnd.Next(0, args.Length);

        }
        static void PRNTMoves(string[] args)
        {
            WriteLine("Available moves:");
            for (int i = 0; i < args.Length; i++)
            {
                WriteLine($"{i + 1} - {args[i]}");

            }
            WriteLine("0 - exit");
            WriteLine("? - help");
            WriteLine("Enter your move: ");
        }
    }
}

