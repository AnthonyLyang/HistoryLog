using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNDConsole1
{
    enum CharacterType
    {
        player,
        npc,
        monster,
    }
    enum ObjectType
    {
        Barricade,
        Trap,
        Gate,
        Loot,
        Player,
        NPC,
        Missile,
        TriggeredTrap,
    }
    enum SpellType
    {
        Heal,
        HOT,
        Windfury,
        Irritated,
        Shield,
        DOT,
        Damage,
    }
    enum StatusType
    {
        Shielded,
        Damageovertime,
        Windfury,
        Irritated,
    }
}
namespace DNDConsole1
{

    class Coordinate
    {
        public string Printer = "  ";
        public ConsoleColor PrintColor;
        public bool FreeToAdd = true;
        public int[] Point = { 0, 0 };
        public List<Object> ObjectInThisCoordinate = new List<Object>();
        public void PrintThisCoordinate()
        {
            string temp = "  ";
            ConsoleColor temp2 = ConsoleColor.Gray;
            int pritemp = 0;
            if (ObjectInThisCoordinate.Count > 0)
            {
                for (int i = 0; i < ObjectInThisCoordinate.Count; i++)
                {
                    if (ObjectInThisCoordinate[i].Priority > pritemp)
                    {
                        pritemp = ObjectInThisCoordinate[i].Priority;
                        temp = ObjectInThisCoordinate[i].Apperance;
                        temp2 = ObjectInThisCoordinate[i].ApperanceColor;
                    }
                }
            }
            Printer = temp;
            PrintColor = temp2;
        }
        public void FreeToAddCheck()
        {
            foreach (Object i in ObjectInThisCoordinate)
            {
                if (!i.Interactive)
                {
                    FreeToAdd = false;
                    break;
                }
            }
        }
        public bool AddObject(Object @object)
        {
            ObjectInThisCoordinate.Add(@object);
            return true;
        }
        public bool RemoveObject(Object @object)
        {
            ObjectInThisCoordinate.Remove(@object);
            return true;
        }
        public override string ToString()
        {
            return Printer.ToString();
        }
    }
    class Object
    {
        public ObjectType object_type;
        public ConsoleColor ApperanceColor;
        public int Priority;
        public string Apperance;
        public bool Interactive;
        public bool Motive;
        public int[] InitCoordinate = { 0, 0 };
        public int[] CoordinateDestSim = { 0, 0 };
        public Character CharacterTrait;
        public bool GetMove()
        {
            return true;
        }
        public static Object CreateWall(int x, int y)
        {
            Object wall = new Object();
            wall.object_type = ObjectType.Barricade;
            wall.Priority = 3;
            wall.Apperance = "█";
            wall.ApperanceColor = ConsoleColor.DarkMagenta;
            wall.Interactive = false;
            wall.Motive = false;
            wall.InitCoordinate[0] = x;
            wall.InitCoordinate[1] = y;
            wall.CoordinateDestSim[0] = x;
            wall.CoordinateDestSim[1] = y;
            return wall;
        }
        public static Object CreatePlayer(int x, int y)
        {
            Object player = new Object();
            player.object_type = ObjectType.Player;
            player.Priority = 10;
            player.Apperance = "卍";
            player.ApperanceColor = ConsoleColor.DarkGreen;
            player.Motive = true;
            player.Interactive = true;
            player.InitCoordinate[0] = x;
            player.InitCoordinate[1] = y;
            player.CoordinateDestSim[0] = x;
            player.CoordinateDestSim[1] = y;
            player.CharacterTrait = Program.CreatePlayer();
            return player;
        }
        public static Object CreateNPC(int x, int y)
        {
            Object player = new Object();
            player.object_type = ObjectType.NPC;
            player.Priority = 9;
            player.Apperance = "卐";
            player.ApperanceColor = ConsoleColor.Red;
            player.Motive = true;
            player.Interactive = true;
            player.InitCoordinate[0] = x;
            player.InitCoordinate[1] = y;
            player.CoordinateDestSim[0] = x;
            player.CoordinateDestSim[1] = y;
            player.CharacterTrait = Program.CreateNPC();
            return player;
        }
        public static Object CreateLoot(int x, int y)
        {
            Object loot = new Object();
            loot.object_type = ObjectType.Loot;
            loot.Priority = 8;
            loot.Apperance = "＄";
            loot.ApperanceColor = ConsoleColor.Yellow;
            loot.Motive = false;
            loot.Interactive = true;
            loot.InitCoordinate[0] = x;
            loot.InitCoordinate[1] = y;
            loot.CoordinateDestSim[0] = x;
            loot.CoordinateDestSim[1] = y;
            return loot;
        }
        public static Object CreateTrap(int x, int y)
        {
            Object trap = new Object();
            trap.object_type = ObjectType.Trap;
            trap.Priority = 5;
            trap.Apperance = "田";
            trap.ApperanceColor = ConsoleColor.DarkBlue;
            trap.Motive = false;
            trap.Interactive = true;
            trap.InitCoordinate[0] = x;
            trap.InitCoordinate[1] = y;
            trap.CoordinateDestSim[0] = x;
            trap.CoordinateDestSim[1] = y;
            return trap;
        }
        public static Object CreateTriggeredTrap(int x, int y)
        {
            Object trap = new Object();
            trap.object_type = ObjectType.TriggeredTrap;
            trap.Priority = 6;
            trap.Apperance = "田";
            trap.ApperanceColor = ConsoleColor.DarkGray;
            trap.Motive = false;
            trap.Interactive = true;
            trap.InitCoordinate[0] = x;
            trap.InitCoordinate[1] = y;
            trap.CoordinateDestSim[0] = x;
            trap.CoordinateDestSim[1] = y;
            return trap;
        }

        public void GetNextMove(int i)
        {
            CoordinateDestSim[0] = InitCoordinate[0];
            CoordinateDestSim[1] = InitCoordinate[1];
            switch (i)
            {
                case 1:
                    CoordinateDestSim[0] += 1;//d
                    break;
                case 2:
                    CoordinateDestSim[0] -= 1;//a
                    break;
                case 3:
                    CoordinateDestSim[1] += 1;//w
                    break;
                case 4:
                    CoordinateDestSim[1] -= 1;//s
                    break;
                default:
                    CoordinateDestSim[0] = InitCoordinate[0];
                    CoordinateDestSim[1] = InitCoordinate[1];
                    break;

            }

        }
        public int GetMoveCode()
        {
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            while (Console.KeyAvailable)
            {
                key = Console.ReadKey(true);
            }
            switch (key.KeyChar)
            {
                case 'w':
                    return 4;
                case 's':
                    return 3;
                case 'a':
                    return 2;
                case 'd':
                    return 1;
                default:
                    return 0;
            }

        }
        public void MakeNextMove(Coordinate[,] board)
        {
            if (board[CoordinateDestSim[0], CoordinateDestSim[1]].FreeToAdd)
            {
                if (board[CoordinateDestSim[0], CoordinateDestSim[1]].ObjectInThisCoordinate.Count == 0)
                {
                    board[CoordinateDestSim[0], CoordinateDestSim[1]].AddObject(this);
                    board[InitCoordinate[0], InitCoordinate[1]].RemoveObject(this);
                    InitCoordinate[0] = CoordinateDestSim[0];
                    InitCoordinate[1] = CoordinateDestSim[1];
                }
                else
                {
                    for (int k = board[CoordinateDestSim[0], CoordinateDestSim[1]].ObjectInThisCoordinate.Count - 1; k >= 0; k--)
                    {
                        switch (board[CoordinateDestSim[0], CoordinateDestSim[1]].ObjectInThisCoordinate[k].object_type)
                        {
                            case ObjectType.Loot:
                                board[CoordinateDestSim[0], CoordinateDestSim[1]].ObjectInThisCoordinate.RemoveAt(k);
                                board[CoordinateDestSim[0], CoordinateDestSim[1]].FreeToAddCheck();
                                break;
                            case ObjectType.Missile:
                                board[CoordinateDestSim[0], CoordinateDestSim[1]].ObjectInThisCoordinate.RemoveAt(k);
                                board[CoordinateDestSim[0], CoordinateDestSim[1]].FreeToAddCheck();
                                break;
                            case ObjectType.Trap:
                                board[CoordinateDestSim[0], CoordinateDestSim[1]].ObjectInThisCoordinate.RemoveAt(k);
                                Character.ClearBattlePanel(board.GetLength(1));
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine("{0}中了陷阱，这是个圈套！",CharacterTrait.character_name);
                                CharacterTrait.DamageTaken(10);
                                Console.WriteLine("{0}的当前生命值：{1}/{2}", CharacterTrait.character_name, CharacterTrait.HP, CharacterTrait.HP_Init);
                                CharacterTrait.DeadCheck();
                                Program.AddTriggeredTrap(CoordinateDestSim[0], CoordinateDestSim[1]);
                                board[CoordinateDestSim[0], CoordinateDestSim[1]].FreeToAddCheck();
                                break;
                            case ObjectType.NPC:
                                Character.ClearBattlePanel(board.GetLength(1));
                                Console.ForegroundColor = ConsoleColor.Gray;
                                Console.WriteLine("{0}遭遇了{1}！",CharacterTrait.character_name, board[CoordinateDestSim[0], CoordinateDestSim[1]].ObjectInThisCoordinate[k].CharacterTrait.character_name);
                                Console.WriteLine("按任意键开始战斗...");
                                Console.ReadKey(true);
                                Character.Stage(CharacterTrait, board[CoordinateDestSim[0], CoordinateDestSim[1]].ObjectInThisCoordinate[k].CharacterTrait, board.GetLength(1));
                                if (board[CoordinateDestSim[0], CoordinateDestSim[1]].ObjectInThisCoordinate[k].CharacterTrait.IsDead)
                                {
                                    Character.ClearBattlePanel(board.GetLength(1));
                                    Console.WriteLine("{0}获得了战斗的胜利！", CharacterTrait.character_name);
                                    board[CoordinateDestSim[0], CoordinateDestSim[1]].ObjectInThisCoordinate.RemoveAt(k);
                                }
                                break;
                        }
                    }
                    if (!CharacterTrait.IsDead)
                    {
                        board[CoordinateDestSim[0], CoordinateDestSim[1]].AddObject(this);
                        board[InitCoordinate[0], InitCoordinate[1]].RemoveObject(this);
                        InitCoordinate[0] = CoordinateDestSim[0];
                        InitCoordinate[1] = CoordinateDestSim[1];
                    }
                    else
                    {
                        CoordinateDestSim[0] = InitCoordinate[0];
                        CoordinateDestSim[1] = InitCoordinate[1];
                    }
                }
            }
            else
            {
                CoordinateDestSim[0] = InitCoordinate[0];
                CoordinateDestSim[1] = InitCoordinate[1];
            }
        }

    }
}