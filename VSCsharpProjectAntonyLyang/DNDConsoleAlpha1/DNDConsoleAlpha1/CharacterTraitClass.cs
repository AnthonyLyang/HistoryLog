using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNDConsole1
{
    class Status
    {
        public StatusType s_type;
        public string s_name;
        public int s_duration;
        public int s_damage;
        public int s_actioncountdown;
        public Status(StatusType t, string name, int duration, int damage, int other) //创建状态
        {
            s_type = t;
            s_name = name;
            switch (t)
            {
                case StatusType.Damageovertime:
                    s_duration = duration;
                    s_damage = damage;
                    break;
                case StatusType.Shielded:
                    s_duration = duration;
                    break;
                case StatusType.Windfury:
                    s_actioncountdown = other;
                    s_duration = duration;
                    break;
                case StatusType.Irritated:
                    s_damage = other;
                    s_duration = duration;
                    break;
            }
        }

        public override string ToString()
        {
            return s_name + "(" + s_duration + ")";
        }
    } //状态类

    class Spells
    {
        public SpellType s_type;
        public string s_name;
        public int s_damage;
        public int s_duration;
        public int s_bumpovertime;//DOT/HOT类型
        public int s_actcoudown;//风怒
        public int s_cooldown;//CD
        public int s_cdtemp;//当前cd
        public int s_times;//使用次数
        public int s_atk;//激怒
        public static Spells CreateNormallAttackSpell(string name, int damage)
        {
            Spells spell = new Spells();
            spell.s_type = SpellType.Damage;
            spell.s_name = name;
            spell.s_damage = damage;
            spell.s_times = -1;
            spell.s_cooldown = 0;
            spell.s_cdtemp = 0;
            return spell;
        }//普通攻击
        public static Spells CreateDOTSpell(string name, int bump, int duration, int cooldown)
        {
            Spells spell = new Spells();
            spell.s_type = SpellType.DOT;
            spell.s_name = name;
            spell.s_bumpovertime = bump;
            spell.s_duration = duration;
            spell.s_cooldown = cooldown;
            spell.s_cdtemp = 0;
            spell.s_times = -1;

            return spell;
        }//DOT
        public static Spells CreatHOTSpell(string name, int bump, int duration, int cooldown)
        {
            Spells spell = new Spells();
            spell.s_type = SpellType.HOT;
            spell.s_name = name;
            spell.s_bumpovertime = bump;
            spell.s_duration = duration;
            spell.s_cooldown = cooldown;
            spell.s_cdtemp = 1;
            spell.s_times = -1;
            return spell;
        }//HOT
        public static Spells CreateShieldSpell(string name, int duration, int cooldown)
        {
            Spells spell = new Spells();
            spell.s_name = name;
            spell.s_type = SpellType.Shield;
            spell.s_duration = duration;
            spell.s_cooldown = cooldown;
            spell.s_cdtemp = cooldown;
            spell.s_times = -1;
            return spell;
        }//圣盾
        public static Spells CreateWindFurySpell(string name, int duration, int cooldown)
        {
            Spells spell = new Spells();
            spell.s_name = name;
            spell.s_type = SpellType.Windfury;
            spell.s_duration = duration;
            spell.s_cooldown = cooldown;
            spell.s_cdtemp = 0;
            spell.s_times = -1;
            spell.s_actcoudown = 2;
            return spell;
        }//风怒
        public static Spells CreateEnrageSpell(string name, int duration, int atk)
        {
            Spells spell = new Spells();
            spell.s_name = name;
            spell.s_type = SpellType.Irritated;
            spell.s_duration = duration;
            spell.s_cooldown = 0;
            spell.s_cdtemp = 0;
            spell.s_times = -1;
            spell.s_atk = atk;
            return spell;
        }//激怒
        public static Spells CreateHealSpell(string name, int damage, int cooldown)
        {
            Spells spell = new Spells();
            spell.s_type = SpellType.Heal;
            spell.s_name = name;
            spell.s_damage = damage;
            spell.s_times = -1;
            spell.s_cooldown = cooldown;
            spell.s_cdtemp = cooldown;
            return spell;
        }//治疗
    }//技能类


}