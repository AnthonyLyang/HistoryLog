using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonManager1
{
    class Program
    {

        static void Main(string[] args)
        {
            PokeIllustrator Illustrator1 = new PokeIllustrator();
            bool continueadding = true;
            bool continuedoing = true;
            string FlagInput = null;
            int IdCounter = 0;
            while (continueadding)
            {
                IdCounter++;
                Pokemon PokeTemp = new Pokemon();
                PokeTemp.NewPoke(IdCounter);
                Illustrator1.AddNewPoke(PokeTemp);
                Console.WriteLine("是否继续添加？y or n");
                FlagInput = Console.ReadLine();
                if (!FlagInput.StartsWith("y"))
                    break;
            }
            Console.ReadLine();
            while (continuedoing)
            {
                Illustrator1.SearchPoke(Illustrator1);
                Console.Write("是否继续操作？y or n");
                FlagInput = Console.ReadLine();
                if (!FlagInput.StartsWith("y"))
                    break;
            }
            Console.ReadLine();
        }
        public static int IntInput()
        {
            string InputTemp = Console.ReadLine();
            int inttemp = 0;
            while (!int.TryParse(InputTemp, out inttemp))
            {
                Console.WriteLine("请输入整数数字！");
                InputTemp = Console.ReadLine();
            }
            return inttemp;
        }
    }
    class Pokemon
    {
        public string PokeName { get; set; }
        public int PokeLev { get; set; }
        public int PokeAtk { get; set; }
        public int PokeId { get; set; }
        public void NewPoke(int IdCounter)
        {
            Console.Write("请输入Pokemon的名字：");
            PokeName = Console.ReadLine();
            Console.Write("请输入Pokemon的等级：");
            PokeLev = Program.IntInput();
            Console.Write("请输入Pokemon的攻击力：");
            PokeAtk = Program.IntInput();
            PokeId = IdCounter;
        }
    }
    class PokeIllustrator
    {
        public Dictionary<int, Pokemon> PokeDict_ById = new Dictionary<int, Pokemon>();
        public Dictionary<string, List<Pokemon>> PokeDict_ByName = new Dictionary<string, List<Pokemon>>();

        public void AddNewPoke(Pokemon PokeInputed)
        {
            PokeDict_ById.Add(PokeInputed.PokeId, PokeInputed);
            if (!PokeDict_ByName.ContainsKey(PokeInputed.PokeName))
            {
                PokeDict_ByName.Add(PokeInputed.PokeName, new List<Pokemon>());
            }
            List<Pokemon> temp = PokeDict_ByName[PokeInputed.PokeName];
            temp.Add(PokeInputed);
        }
        public void SearchPoke(PokeIllustrator illustrator)
        {
            Console.Write("想用id查找还是名字查找？");
            string temp = Console.ReadLine();
            string temp3 = null;
            int temp2 = 0;
            if (temp.StartsWith("i"))
            {
                Console.Write("请输入ID：");
                temp2 = Program.IntInput();
                if (PokeDict_ById.ContainsKey(temp2))
                {
                    PrintAll(temp2);
                    Console.WriteLine("想删除这个pokemon吗？y or n");
                    temp = Console.ReadLine();
                    if (temp.StartsWith("y"))
                        illustrator.DeletePokemon(temp2);
                }
                else
                    Console.WriteLine("此ID未被使用。");
            }
            else
            {
                Console.WriteLine("请输入要查找的名字：");
                temp3 = Console.ReadLine();
                if (PokeDict_ByName.ContainsKey(temp3))
                {
                    PrintAll(temp3);
                    Console.WriteLine("想删除某一个pokemon吗？y or n");
                    temp = Console.ReadLine();
                    if (temp.StartsWith("y"))
                    {
                        Console.Write("请输入想删除的ID：");
                        temp2 = Program.IntInput();
                        bool EnableToDelete = false;
                        for (int a = PokeDict_ByName[temp3].Count - 1; a >= 0; a--)
                        {
                            if (PokeDict_ByName[temp3][a].PokeId == temp2)
                            {
                                EnableToDelete = true;
                                break;
                            }
                        }
                        if (!EnableToDelete)
                            Console.WriteLine("输入了错误的ID！");
                        else
                            illustrator.DeletePokemon(temp2);
                    }
                }
                else
                    Console.WriteLine("无此名称的pokemon。");
            }
        }
        public void PrintAll(int Id)
        {
            Console.WriteLine("{0} {1} {2} {3}",Id,PokeDict_ById[Id].PokeName,PokeDict_ById[Id].PokeLev,PokeDict_ById[Id].PokeAtk);
        }
        public void PrintAll(string Name)
        {
            Console.Write(Name);
            foreach (Pokemon pokemon in PokeDict_ByName[Name])
                Console.Write("Id：{0} 等级：{1}攻击力：{2}", pokemon.PokeId,pokemon.PokeLev, pokemon.PokeAtk);
        }
        public void DeletePokemon(int Id)
        {
            string name = PokeDict_ById[Id].PokeName;
            PokeDict_ById.Remove(Id);
            for (int m = PokeDict_ByName[name].Count - 1; m >= 0; m--)
            {
                if (PokeDict_ByName[name][m].PokeId == Id)
                {
                    PokeDict_ByName[name].RemoveAt(m);
                    if (PokeDict_ByName[name] is null)
                        PokeDict_ByName.Remove(name);
                }
            }

        }
    }
}
