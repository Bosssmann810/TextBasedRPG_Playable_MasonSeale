using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace TextBasedRPG_MasonSeale
{
    internal class Program
    {
        static List<(int, int)> collected = new List<(int, int)>();
        static int gold = 0;
        static List<(int, int)> spots = new List<(int, int)>();
        static List<(int, int)> lava = new List<(int, int)>();
        static List<(int, int)> goldspots = new List<(int, int)>();
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
        static int Sideoffset = 1;
        static int downoffset = 2;
        static int offscreen = 4;

        static void Main(string[] args)
        {
            

            Console.CursorVisible = false;
            while (going == true)
            {
               
                
                Console.SetCursorPosition(0, 0);
                hud();
                Console.WriteLine();
                DisplayMap();  
                enemymovement();
                otherenemymovement();
                playermovment();  
                endcheck();
            }
            Console.ReadKey(true);


        }

        //Displays the map, as well as everything on the map, including players enemies and diffrent tiles
        static void DisplayMap(int x = 1)
        {
            

            string path = "map.txt";
            string[] map = File.ReadAllLines(path);
            //these two will hep with the boarders later on
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
                           
                            lava.Add((j + Sideoffset, i + downoffset));
                            Console.BackgroundColor = ConsoleColor.Black;
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        }
                        if(map[i][j] == '~')
                        {
                            spots.Add((j +Sideoffset,i +downoffset));
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.Blue;
                        }
                        if(map[i][j] == '*')
                        {
                            goldspots.Add((j + Sideoffset, i + downoffset));
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
                        //increase verticlebouds value every time for later use 
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
                foreach ((int, int) coin in collected)
                {
                    Console.SetCursorPosition(coin.Item1, coin.Item2);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("*");
                }

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
        //controls how the first enemy moves
        static void enemymovement()
        {
            if (enemyhp <= 0)
            {
                enemypos.Item1 = 0;
                enemypos.Item2 = verticlebounds / horziontalbound + offscreen;
                return;
            }


            Epreviouspos.Item1 = enemypos.Item1;
            Epreviouspos.Item2 = enemypos.Item2;
            int horv = enemyrng.Next(0, 2);
            //ranomly moves the enemy either horazontally or vertically (h or v == horv) always moves towards the player
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
            if (spots.Contains(enemypos))
            {
                enemypos = Epreviouspos;
            }
            if (lava.Contains(enemypos))
            {
                enemyhp -= 1;
            }
            if (enemyhp <= 0)
            {
                return;
            }
        }
        //controls how the other enemy moves (its basically the same as the first enemy but with diffret variables 
        static void otherenemymovement()
        {
            if(otherenmyhp <= 0)
            {
                otherenemypos.Item1 = 0;
                otherenemypos.Item2 = verticlebounds / horziontalbound + offscreen;
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
            if (spots.Contains(otherenemypos))
            {
                otherenemypos = Opreviouspos;
            }
            if (lava.Contains(otherenemypos))
            {
                otherenmyhp -= 1;
            }
            if (enemyhp <= 0)
            {
                return;
            }

        }
        static void hud()
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"Your HP: {playerhp} ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"Gold {gold} ");
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"Enemy HP: {enemyhp} ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write($"Other Enemy HP: {otherenmyhp} ");
        }
        //controls player movment, as well handles checks related to where they are trying to go (damage or boundries)
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
            //if it tries to go on the same tile as the enemy attack and reset to previous tile.
            if (playerpos.Item1 == enemypos.Item1)
            {
                if(playerpos.Item2 == enemypos.Item2)
                {
                    enemyhp -= 1;
                    playerpos.Item1 = Ppreviouspos.Item1;
                    playerpos.Item2 = Ppreviouspos.Item2;
                }

            }
            //same but other enemy
            if(playerpos.Item1 == otherenemypos.Item1)
            {
                if(playerpos.Item2 == otherenemypos.Item2)
                {
                    otherenmyhp -= 1;
                    playerpos.Item1 = Ppreviouspos.Item1;
                    playerpos.Item2 = Ppreviouspos.Item2;
                }
            }
            //if it cant move to a tile, set it back to the previous tile.
            if (playerpos.Item2 == 1)
            {
                playerpos.Item2 = Ppreviouspos.Item2;
            }
            if (playerpos.Item1 == 0)
            {
                playerpos.Item1 = Ppreviouspos.Item1;
            }
            
            if (playerpos.Item2 == verticlebounds / horziontalbound + downoffset)
            {
                playerpos.Item2 = Ppreviouspos.Item2;
            }
            if (playerpos.Item1 == horziontalbound + Sideoffset)
            {
                playerpos.Item1 = Ppreviouspos.Item1;
            }

            if (spots.Contains(playerpos))
            {
                playerpos = Ppreviouspos;
            }
            //if it steps on lava do damage
            if (lava.Contains(playerpos))
            {
                playerhp -= 1;
            }
            //if it steps on a gold tile
            if (goldspots.Contains(playerpos))
            {
                //if its already collected return
                if (collected.Contains(playerpos))
                {
                    return;
                }
                //otherwise add it to collected and increase gold
                goldspots.Remove(playerpos);
                collected.Add(playerpos);
                gold += 1;
            }
        }
        //Endcheck keeps track of if either the player or both enemies are dead and ends the game
        static void endcheck()
        {
            //if player is dead
            if(playerhp <= 0)
            {
                //stop the game
                going = false;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You died");
            }
            if (enemyhp <= 0 && otherenmyhp <= 0 )
            {
                going = false;
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("You win");
            }

        }
        
    }
    


}
