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
        static List<(int, int)> spots = new List<(int, int)>();
        static bool enemystuck;
        static bool playerstuck = false;
        static bool going = true;
        static (int,int) playerpos = (10,5);
        static (int,int) Ppreviouspos = ( 2, 2 );
        static int otherenmyhp = 30;
        static (int, int) otherenemypos = (20,10);
        static (int, int) Opreviouspos = (20, 10);
        static (int,int) enemypos = ( 12, 7 );
        static (int, int) Epreviouspos = (2, 2);
        static Random enemyrng = new Random();
        static int playerhp = 30;
        static int enemyhp = 10;
        static bool tired = false;
        static int verticlebounds = 0;
        static int horziontalbound;

        static void Main(string[] args)
        {
            

            Console.CursorVisible = false;
            while (going == true)
            {
               
                
                Console.SetCursorPosition(0, 0);
                Console.WriteLine();
                DisplayMap();
                hud();  
                enemymovement();
                otherenemymovement();
                playermovment();  
                endcheck();
            }


        }


        static void DisplayMap(int x = 1)
        {
            

            string path = "map.txt";
            string[] map = File.ReadAllLines(path);
            horziontalbound = map[0].Length;
            verticlebounds = 0;

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
                    for(int j = 0; j < map[i].Length; j++)
                    {
                        if(map[i][j] == '+')
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        }
                        if(map[i][j] == '~')
                        {
                            spots.Add((j +1,i +2));
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        if(map[i][j] == '*')
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                        }
                        if(map[i][j] == '^')
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        }
                        if(map[i][j] == '`')
                        {
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                        }
                        verticlebounds += 1;
                        Console.Write(map[i][j]);
                    }

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("│");

                }
                Console.WriteLine();
                Console.Write("└");
                Console.Write(current);
                Console.Write("┘");

                Console.SetCursorPosition(enemypos.Item1, enemypos.Item2);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("i");
                Console.SetCursorPosition(otherenemypos.Item1, otherenemypos.Item2);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("i");
                Console.SetCursorPosition(playerpos.Item1, playerpos.Item2);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("i");
                if (enemyhp <= 0)
                {
                    Console.SetCursorPosition(enemypos.Item1, enemypos.Item2);
                    Console.Write(" ");
                }
                if (otherenmyhp <= 0)
                {
                    Console.SetCursorPosition(otherenemypos.Item1, otherenemypos.Item2);
                    Console.Write(" ");
                }
            }
        }
        static void enemymovement()
        {
            if (enemyhp <= 0)
            {
                enemypos.Item1 = 0;
                enemypos.Item2 = 0;
                return;
            }


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
            if(enemypos.Item1 == otherenemypos.Item1)
            {
                if(enemypos.Item2 == otherenemypos.Item2)
                {
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
        }
        static void otherenemymovement()
        {
            if(otherenmyhp <= 0)
            {
                otherenemypos.Item1 = 0;
                otherenemypos.Item2 = 0;
                return;
            }
            if(tired == true)
            {
                tired = false;
                return;
            }
            else if (tired == false)
            {
                tired = true;
            }
            Opreviouspos.Item1 = otherenemypos.Item1;
            Opreviouspos.Item2 = otherenemypos.Item2;
            int horv = enemyrng.Next(0, 2);
            if (horv == 0)
            {
                if (playerpos.Item1 > otherenemypos.Item1)
                {
                    otherenemypos.Item1 += 1;
                }
                if (playerpos.Item1 < otherenemypos.Item1)
                {
                    otherenemypos.Item1 -= 1;
                }
            }
            if (horv == 1)
            {
                if (playerpos.Item2 > otherenemypos.Item2)
                {
                    otherenemypos.Item2 += 1;
                }
                if (playerpos.Item2 < otherenemypos.Item2)
                {
                    otherenemypos.Item2 -= 1;
                }

            }


            if (otherenemypos.Item1 == playerpos.Item1)
            {
                if (otherenemypos.Item2 == playerpos.Item2)
                {
                    playerhp -= 1;
                    otherenemypos.Item1 = Opreviouspos.Item1;
                    otherenemypos.Item2 = Opreviouspos.Item2;
                }
            }
            if (otherenemypos.Item1 == enemypos.Item1)
            {
                if(otherenemypos.Item2 == enemypos.Item2)
                {
                    otherenemypos.Item1 = Opreviouspos.Item1;
                    otherenemypos.Item2 = Opreviouspos.Item2;
                }
            }
            if (enemyhp <= 0)
            {
                return;
            }

        }
        static void hud()
        {
            Console.SetCursorPosition(0, verticlebounds / horziontalbound + 3);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Your HP: {playerhp} ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Enemy HP: {enemyhp} Other Enemy HP: {otherenmyhp} ");
        }
        static void playermovment()
        {

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
            if (playerstuck == true)
            {

                playerstuck = false;
                playerpos.Item1 = Ppreviouspos.Item1;
                playerpos.Item2 = Ppreviouspos.Item2;
                return;
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
            if(playerpos.Item1 == otherenemypos.Item1)
            {
                if(playerpos.Item2 == otherenemypos.Item2)
                {
                    otherenmyhp -= 1;
                    playerpos.Item1 = Ppreviouspos.Item1;
                    playerpos.Item2 = Ppreviouspos.Item2;
                }
            }
            if (playerpos.Item2 == 1)
            {
                playerpos.Item2 = Ppreviouspos.Item2;
            }
            if (playerpos.Item1 == 0)
            {
                playerpos.Item1 = Ppreviouspos.Item1;
            }
            if (playerpos.Item2 == verticlebounds / horziontalbound + 2)
            {
                playerpos.Item2 = Ppreviouspos.Item2;
            }
            if (playerpos.Item1 == horziontalbound + 1)
            {
                playerpos.Item1 = Ppreviouspos.Item1;
            }

            if (spots.Contains(playerpos))
            {
                playerpos = Ppreviouspos;
            } 
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
            if (enemyhp <= 0 && otherenmyhp <= 0 )
            {
                going = false;
                Console.Clear();
                Console.WriteLine("You win");
            }

        }
        
    }
    


}
