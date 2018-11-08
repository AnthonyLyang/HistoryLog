using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRoundBattle1
{
    class Program
    {
        static Character Player;
        static Character NPC;
        static Character CreatePlayer()
        {
            Character p1 = new Character();
            p1.character_name = "PLAYERONE";
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
        static Character CreateNPC()
        {
            Character p1 = new Character();
            p1.character_name = "NPCONE";
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
        static void InitiateBattle()
        {
            NPC = CreateNPC();
        }
        static Spells InputSpells(List<Spells> spelllist)
        {
            int n = -1;
            while (n <= 0 || n > spelllist.Count)
            {
                ConsoleKeyInfo keyinfo = Console.ReadKey();
                int.TryParse(keyinfo.KeyChar.ToString(), out n);
            }
            return spelllist[n - 1];
        }//get输入字符返回技能 玩家选择技能
        static void ShowSpellBook(Character A)
        {
            for(int i = 0; i < A.spellbook.Count; i++)
            {
                if (A.spellbook[i].s_cdtemp > 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.WriteLine("{0} {1}",i+1,A.spellbook[i].s_name);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
        static void ShowInfo(Character A, Character B)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("{0} HP：{1}/{2}，ATK：{3}", A.character_name, A.HP, A.HP_Init, A.ATK);
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (Status k in A.status)
            {
                if (k.s_duration > 0)
                {
                    Console.Write(k);
                }
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0} HP：{1}/{2}，ATK：{3}", B.character_name, B.HP, B.HP_Init, B.ATK);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (Status k in B.status)
            {
                if (k.s_duration > 0)
                {
                    Console.Write(k);
                }
            }
            Console.WriteLine();
        }
        static void Action(Character A, Character B)
        {
            while (A.ActionPoint > 0)
            {
                Spells spell1 = null;
                if (A == Player)
                {
                    ShowSpellBook(A);
                    while (true)
                    {
                        spell1 = InputSpells(A.spellbook);
                        if (!A.AbleToCast(spell1))
                        {
                            Console.WriteLine("技能尚未冷却，请重新选择");
                            continue;
                        }
                        break;
                    }
                }
                else
                {
                    spell1 = A.RandomSkill();
                }
                Console.WriteLine("{0}施放了{1}", A.character_name, spell1.s_name);
                if (spell1.s_type == SpellType.Heal || spell1.s_type == SpellType.HOT || spell1.s_type == SpellType.Shield || spell1.s_type == SpellType.Windfury || spell1.s_type == SpellType.Irritated)
                {
                    if (A.SpellTaken(spell1))
                    {

                        if (spell1.s_type == SpellType.Heal)
                        {
                            Console.WriteLine("{0}恢复了{1}生命值", A.character_name, spell1.s_damage * (-1));
                        }
                        else
                        {
                            Console.WriteLine("{0}获得了{1}状态", A.character_name, spell1.s_name);
                        }
                    }
                }
                else
                {
                    if (B.SpellTaken(spell1))
                    {
                        if(!(spell1.s_type==SpellType.Damage))
                            Console.WriteLine("{0}获得了{1}状态", B.character_name, spell1.s_name);
                    }
                }
                A.ActionPoint -= 1;
                spell1.s_cdtemp = spell1.s_cooldown;
                B.DeadCheck();
                if (B.IsDead)
                    break;

            }
            A.ActionPoint = A.ActionPoint_Init;
        }
        static void Stage(Character A, Character B)
        {
            int Round = 0;
            while ((!A.IsDead) && (!B.IsDead))
            {
                Round += 1;
                Console.Clear();
                Console.WriteLine("--------第{0}回合开始--------------", Round);
                A.CoolDown();
                ShowInfo(A, B);
                A.status = A.StatusTakenEffect();
                A.SpecialStatus();
                A.IrritatedChara();
                A.WindfuryChara();
                A.DeadCheck();
                if (A.IsDead)
                {
                    break;
                }
                Action(A, B);
                if (B.IsDead)
                {
                    break;
                }
                B.CoolDown();
                B.status = B.StatusTakenEffect();
                B.SpecialStatus();
                B.DeadCheck();
                if (B.IsDead)
                    {

                        break;
                    }
                Action(B, A);
                if (A.IsDead)
                {
                    break;
                }
                Console.ReadKey();
            }
            if (A.IsDead)
            {
                Console.WriteLine("{0}已被击杀",A.character_name);
            }
            else if (B.IsDead)
            {
                Console.WriteLine("{0}已被击杀",B.character_name);
            }
        }
        static void Main(string[] args)
        {
            Player = CreatePlayer();
            InitiateBattle();
            Stage(Player, NPC);
        }
    }
}
