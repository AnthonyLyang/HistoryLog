using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRoundBattle1
{
    class Character
    {
        public string character_name;
        public int HP_Init;
        public int HP;
        public int ATK;
        public int ATK_Init;
        public int EnrageThreshold;
        public int ActionPoint_Init;
        public int ActionPoint;
        public bool WindfuryMark;
        public bool IrritatedMark;
        public bool ShieldedMark;
        public bool IsDead;
        public List<Spells> spellbook = new List<Spells>();
        public List<Status> status = new List<Status>();

        public bool AddStatus(Status statustogo)
        {
            status.Add(statustogo);
            return true;
        }//接收状态
        public bool AddSpell(Spells spell)
        {
            spellbook.Add(spell);
            return true;
        }//添加技能
        public void DamageTaken(int valuedamage)
        {
            HP -= valuedamage;
            if (valuedamage > 0)
            {
                Console.WriteLine("{0}受到{1}点伤害", character_name, valuedamage);
            }
            else if (valuedamage < 0)
            {
                Console.WriteLine("{0}受到{1}点治疗", character_name, valuedamage*(-1));
            }
            if (HP <= 0)
                HP = 0;
            else if (HP >= HP_Init)
                HP = HP_Init;
        }//掉血
        public void DeadCheck()
        {
            if (HP == 0)
                IsDead = true;
        }//死亡检测

        public bool SpellTaken(Spells spell)
        {
            if (HP <= 0)
            {
                return false;
            }

            switch (spell.s_type)
            {
                case SpellType.Damage:
                    if (ShieldedMark)
                        return false;
                    DamageTaken(spell.s_damage);
                    
                    break;
                case SpellType.DOT:
                    if (ShieldedMark)
                        return false;
                    Status stats = new Status(StatusType.Damageovertime, spell.s_name, spell.s_duration, spell.s_bumpovertime, spell.s_actcoudown);
                    AddStatus(stats);
                    break;
                    //以上敌对 以下对友方
                case SpellType.Heal:
                    DamageTaken(spell.s_damage);
                    break;
                case SpellType.HOT:
                    Status stats2 = new Status(StatusType.Damageovertime, spell.s_name, spell.s_duration, spell.s_bumpovertime, spell.s_actcoudown);
                    AddStatus(stats2);
                    break;
                case SpellType.Irritated:
                    Status stats3 = new Status(StatusType.Irritated, spell.s_name, spell.s_duration, spell.s_bumpovertime, spell.s_atk);
                    AddStatus(stats3);
                    break;
                case SpellType.Shield:
                    Status stats4 = new Status(StatusType.Shielded, spell.s_name, spell.s_duration, spell.s_damage, spell.s_duration);
                    AddStatus(stats4);
                    break;
                case SpellType.Windfury:
                    Status stats5 = new Status(StatusType.Windfury, spell.s_name, spell.s_duration, spell.s_damage, spell.s_cooldown);
                    AddStatus(stats5);
                    break;
            }
            return true;
        }//技能起作用（技能令目标承受自己）

        public List<Status> StatusTakenEffect()
        {
            List<Status> renewed_status = new List<Status>();
            foreach (Status sta in status)
            {
                if (sta.s_duration == 0)
                {
                    continue;
                }
                {
                    renewed_status.Add(sta);
                    switch (sta.s_type)
                    {
                        case StatusType.Damageovertime:
                            if (HasStatus(StatusType.Shielded))
                            {
                                if (sta.s_damage > 0)
                                {
                                    sta.s_duration = 0;
                                    break;
                                }
                            }
                            DamageTaken(sta.s_damage);
                            sta.s_duration -= 1;
                            break;

                        case StatusType.Shielded:
                            sta.s_duration -= 1;
                            break;
                        case StatusType.Irritated:
                            sta.s_duration -= 1;
                            break;
                        case StatusType.Windfury:
                            sta.s_duration -= 1;
                            break;
                    }
                }
            }
            return renewed_status;
        }//状态生效  激怒风怒和无敌只算时间 dot hot 计时间和每一跳的数值
        //风怒 无敌和激怒需要单独检测状态
        public bool HasStatus(StatusType _statustype)
        {
            foreach (Status stat in status)
            {
                if (stat.s_type == _statustype)
                    return true;
            } 
            return false;
        }//其实可以检测所有的状态
        public void SpecialStatus()
        {
            IrritatedMark = false;
            ShieldedMark = false;
            WindfuryMark = false;
            foreach (Status stats in status)
            {
                switch (stats.s_type)
                {
                    case StatusType.Irritated:
                            IrritatedMark = true;
                        break;
                    case StatusType.Shielded:
                            ShieldedMark = true;
                        break;
                    case StatusType.Windfury:
                            WindfuryMark = true;
                        break;
                }
            }
        }
        public void IrritatedChara()
        {
            if (IrritatedMark)
                ATK = 2 * ATK_Init;
            else ATK = ATK_Init;
        }
        public void WindfuryChara()
        {
            if (WindfuryMark)
                ActionPoint = 2 * ActionPoint_Init;
            else ActionPoint = ActionPoint_Init;
        }//几种特殊状态的特殊结算
        public bool AbleToCast(Spells spell)
        {
            if (spell.s_cdtemp > 0)
            {
                return false;
            }
            else return true;
        }
        public Spells GetSpells(int index)
        {
            Spells cast = spellbook[index];
            return cast;
        }
        public Spells RandomSkill()
        {
            Random random = new Random();
            int r = random.Next(spellbook.Count);
            return GetSpells(r);
        }
        public void CoolDown()
        {
            foreach (Spells s in spellbook)
            {
                if (s.s_cdtemp > 0)
                    s.s_cdtemp -= 1;
            }
        }
    }
}
