using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TextBasedRPG_MasonSeale
{
    internal class Program
    {
        static bool enemystuck;
        static bool playerstuck = false;
        static bool going = true;
        static (int,int) playerpos = (10,5);
        static (int,int) Ppreviouspos = ( 2, 2 );
        static (int,int) enemypos = ( 12, 7 );
        static (int, int) Epreviouspos = (2, 2);
        static Random enemyrng = new Random();
        static int playerhp = 10;
        static int enemyhp = 10;


        static void Main(string[] args)
        {

            Console.CursorVisible = false;
            while (going == true)
            {
               
                endcheck();
                Console.SetCursorPosition(0, 0);
                Console.WriteLine();
                hud();
                DisplayMap();
                enemymovement();
                playermovment();


            }


        }


        static void DisplayMap(int x = 1)
        {
            

            string path = "map.txt";
            string[] map = File.ReadAllLines(path);



            if (x == 1)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("┌");
                string current = string.Concat(Enumerable.Repeat("─", map[0].Length));
                Console.Write(current);
                Console.Write("┐");
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    Console.WriteLine();
                    Console.Write("│");
                    foreach(char j in map[i])
                    {
                        if(j == '+')
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        }
                        if(j == '~')
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        if(j == '*')
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        if(j == '^')
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        }
                        if(j == '`')
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                        }
                        Console.Write(j);
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("│");

                }
                Console.WriteLine();
                Console.Write("└");
                Console.Write(current);
                Console.Write("┘");
            }
        }

        static void legend()
        {

            Console.WriteLine("┌────────────┐");
            Console.WriteLine("│Legend:     │");
            Console.Write("│");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("' = grass   ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("│");
            Console.WriteLine();
            Console.Write("│");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("~ = water   ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("│");
            Console.WriteLine();
            Console.Write("│");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("^ = mountain");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("│");
            Console.WriteLine();
            Console.Write("│");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("* = forest  ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("│");
            Console.WriteLine();
            Console.Write("└────────────┘");
            Console.WriteLine();
        }
        static void enemymovement()
        {


            Epreviouspos.Item1 = enemypos.Item1;
            Epreviouspos.Item2 = enemypos.Item2;
            int horv = enemyrng.Next(0, 2);
            if (horv == 0)
            {
                if (playerpos.Item1 > enemypos.Item1)
                {
                    enemypos.Item1 += 1;
                }
                if (playerpos.Item1 < enemypos.Item1)
                {
                    enemypos.Item1 -= 1;
                }
            }
            if (horv == 1)
            { 
                if(playerpos.Item2 > enemypos.Item2)
                {
                    enemypos.Item2 += 1;
                }
                if (playerpos.Item2 < enemypos.Item2)
                {
                    enemypos.Item2 -= 1;
                }

            }


            if(enemypos.Item1 == playerpos.Item1)
            {
                if (enemypos.Item2 == playerpos.Item2)
                {
                    playerhp -= 2;
                    enemypos.Item1 = Epreviouspos.Item1;
                    enemypos.Item2 = Epreviouspos.Item2;
                }
            }   
            if (enemystuck == true)
            {

                enemypos.Item1  = Epreviouspos.Item1;
                enemypos.Item2 = Epreviouspos.Item2;
                enemystuck = false;
                return;
            }
            if(enemyhp <= 0)
            {
                return;
            }
            Console.SetCursorPosition(enemypos.Item1, enemypos.Item2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("i");
        }
        static void hud()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Your HP: {playerhp} ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enemy HP: {enemyhp} ");
        }
        static void playermovment()
        {
            Console.SetCursorPosition(playerpos.Item1, playerpos.Item2);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("i");

            Ppreviouspos.Item1 = playerpos.Item1;
            Ppreviouspos.Item2 = playerpos.Item2;
            ConsoleKeyInfo key = Console.ReadKey(true);
            if(key.Key == ConsoleKey.W)
            {
                playerpos.Item2 -= 1;
            }
            if (key.Key == ConsoleKey.S)
            {
                playerpos.Item2 += 1;
            }
            if(key.Key == ConsoleKey.A)
            {
                playerpos.Item1 -= 1;
            }
            if (key.Key == ConsoleKey.D)
            {
                playerpos.Item1 += 1;
            }
            
            if (playerpos.Item1 == enemypos.Item1)
            {
                if(playerpos.Item2 == enemypos.Item2)
                {
                    enemyhp -= 1;
                    playerpos.Item1 = Ppreviouspos.Item1;
                    playerpos.Item2 = Ppreviouspos.Item2;
                }

            }
            if (playerpos.Item2 == 3)
            {
                playerpos.Item2 = Ppreviouspos.Item2;
            }
            if (playerpos.Item1 == 0)
            {
                playerpos.Item1 = Ppreviouspos.Item1;
            }
            if (playerpos.Item2 == 16)
            {
                playerpos.Item2 = Ppreviouspos.Item2;
            }
            if(playerpos.Item1 == 32)
            {
                playerpos.Item1 = Ppreviouspos.Item1;
            }
            
            if (playerstuck == true)
            {

                playerstuck = false;
                playerpos.Item1 = Ppreviouspos.Item1;
                playerpos.Item2 = Ppreviouspos.Item2;
            }
            Console.SetCursorPosition(playerpos.Item1, playerpos.Item2);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("i");
        }
        static void endcheck()
        {
            if(playerhp <= 0)
            {
                going = false;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You died");
            }
            if(enemyhp <= 0)
            {
                Console.SetCursorPosition(enemypos.Item1, enemypos.Item2);
                Console.Write("");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(0, 17);
                Console.WriteLine("You win");
            }
        }
        
    }
    


}
