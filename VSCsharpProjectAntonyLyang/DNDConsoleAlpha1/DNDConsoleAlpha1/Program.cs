using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNDConsole1
{
    class Program
    {
        public static Character CreatePlayer()
        {
            Character p1 = new Character();
            p1.character_name = "PLAYERONE";
            p1.character_type = CharacterType.player;
            p1.HP_Init = 200;
            p1.HP = p1.HP_Init;
            p1.ActionPoint_Init = 1;
            p1.ActionPoint = p1.ActionPoint_Init;
            p1.ATK_Init = 20;
            p1.ATK = p1.ATK_Init;
            p1.EnrageThreshold = p1.HP_Init * 4 / 10;
            p1.WindfuryMark = false;
            p1.IrritatedMark = false;
            p1.ShieldedMark = false;
            p1.IsDead = false;
            p1.AddSpell(Spells.CreateNormallAttackSpell("攻击", p1.ATK));
            p1.AddSpell(Spells.CreateWindFurySpell("风怒", 3, 6));
            p1.AddSpell(Spells.CreatHOTSpell("治疗之泉", -10, 3, 5));
            p1.AddSpell(Spells.CreateShieldSpell("大地之盾", 2, 6));
            return p1;
        }
        public static Character CreateNPC()
        {
            Character p1 = new Character();
            p1.character_name = "NPCONE";
            p1.character_type = CharacterType.monster;
            p1.HP_Init = 500;
            p1.HP = p1.HP_Init;
            p1.ActionPoint_Init = 1;
            p1.ActionPoint = p1.ActionPoint_Init;
            p1.ATK_Init = 1;
            p1.ATK = p1.ATK_Init;
            p1.EnrageThreshold = 0;
            p1.WindfuryMark = false;
            p1.IrritatedMark = false;
            p1.ShieldedMark = false;
            p1.IsDead = false;
            p1.AddSpell(Spells.CreateNormallAttackSpell("攻击", p1.ATK));
            return p1;
        }


        static int map_width = 31;
        static int map_height = 31;
        static Coordinate[,] coordinates = new Coordinate[map_width, map_height];
        static Object player = Object.CreatePlayer(1, 1);
        static ConsoleCanvas PrintConsole = new ConsoleCanvas(map_width, map_height);
        public static void LetThereBeLight()
        {
            for (int i = 0; i < map_width; i++)
            {
                for (int j = 0; j < map_height; j++)
                {
                    coordinates[i, j] = new Coordinate();
                    coordinates[i, j].Point[0] = i;
                    coordinates[i, j].Point[1] = j;
                }
            }
        }
        //创建场景地基
        public static void InitBorderline()
        {
            for (int i = 0; i < map_width; i++)
            {
                if (coordinates[i, 0].FreeToAdd)
                {
                    coordinates[i, 0].AddObject(Object.CreateWall(i, 0));
                    coordinates[i, 0].FreeToAddCheck();
                }
                if (coordinates[i, map_height - 1].FreeToAdd)
                {
                    coordinates[i, map_height - 1].AddObject(Object.CreateWall(i, map_height - 1));
                    coordinates[i, map_height - 1].FreeToAddCheck();
                }
            }
            for (int j = 0; j < map_height; j++)
            {
                if (coordinates[0, j].FreeToAdd)
                {
                    coordinates[0, j].AddObject(Object.CreateWall(0, j));
                    coordinates[0, j].FreeToAddCheck();
                }
                if (coordinates[map_width - 1, j].FreeToAdd)
                {
                    coordinates[map_width - 1, j].AddObject(Object.CreateWall(map_width - 1, j));
                    coordinates[map_width - 1, j].FreeToAddCheck();
                }
            }

        }
        public static void AddPlayer(int x, int y)
        {
            player.InitCoordinate[0] = x;
            player.InitCoordinate[1] = y;
            player.CoordinateDestSim[0] = x;
            player.CoordinateDestSim[1] = y;
            coordinates[x, y].AddObject(player);
            coordinates[x, y].FreeToAddCheck();
        }
        public static void AddWall(int x, int y)
        {
            coordinates[x, y].AddObject(Object.CreateWall(x, y));
            coordinates[x, y].FreeToAddCheck();
        }
        public static void AddLoot(int x, int y)
        {
            coordinates[x, y].AddObject(Object.CreateLoot(x, y));
            coordinates[x, y].FreeToAddCheck();
        }
        public static void AddTrap(int x, int y)
        {
            coordinates[x, y].AddObject(Object.CreateTrap(x, y));
            coordinates[x, y].FreeToAddCheck();
        }
        public static void AddTriggeredTrap(int x, int y)
        {
            coordinates[x, y].AddObject(Object.CreateTriggeredTrap(x, y));
            coordinates[x, y].FreeToAddCheck();
        }
        public static void AddNPC(int x, int y)
        {
            coordinates[x, y].AddObject(Object.CreateNPC(x, y));
            coordinates[x, y].FreeToAddCheck();

        }
        public static bool NoLootLeft()
        {
            foreach (Coordinate c in coordinates)
            {
                if (c.ObjectInThisCoordinate.Count != 0)
                {
                    foreach (Object o in c.ObjectInThisCoordinate)
                    {
                        if (o.object_type == ObjectType.Loot)
                            return false;
                    }
                }
            }
            return true;
        }
        public static void RefreshLoot()
        {
            Random rand1 = new Random();
            Random rand2 = new Random();
            int x = rand1.Next(0, map_width);
            int y = rand2.Next(0, map_height);
            while (!coordinates[x, y].FreeToAdd)
            {
                x = rand1.Next(0, map_width);
                y = rand2.Next(0, map_height);
            }
            AddLoot(x, y);
        }
        public static void InitStage()
        {
            AddPlayer(2, 2);
            AddLoot(4, 4);
            AddTrap(5, 5);
            AddWall(1, 2);
            AddWall(3, 4);
            AddWall(5, 8);
            AddTrap(6, 6);
            AddTrap(4, 4);
            AddNPC(10, 10);
        }//初始化关卡


        public static void PrintMap()
        {
            for (int j = 0; j < map_width; j++)
            {
                for (int i = 0; i < map_height; i++)
                {
                    coordinates[i, j].PrintThisCoordinate();
                    PrintConsole.buffer[i, j] = coordinates[i, j].Printer;
                    PrintConsole.colorbuffer[i, j] = coordinates[i, j].PrintColor;
                }
            }
            PrintConsole.RefreshBuffer();
            PrintConsole.CopyBuffer(PrintConsole.bufferbackup, PrintConsole.buffer);
        }

        static void Main(string[] args)
        {
            Console.WindowHeight = 60;
            Console.WindowWidth = 100;

            Console.CursorVisible = false;
            LetThereBeLight();
            PrintMap();
            InitBorderline();
            InitStage();
            PrintMap();
            while (!player.CharacterTrait.IsDead)
            {
                player.GetNextMove(player.GetMoveCode());
                player.MakeNextMove(coordinates);
                if (player.CharacterTrait.IsDead)
                {
                    Character.ClearBattlePanel(coordinates.GetLength(1));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("YOU DIED");
                    break;
                }
                if (NoLootLeft())
                {
                    RefreshLoot();
                }
                PrintMap();
                Thread.Sleep(100);
            }
            Console.ReadLine();
        }
    }
}