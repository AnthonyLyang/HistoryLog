using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemBehavior
{
    ItemSell=0,//出售
    ItemCompond,//合成
    ItemResolve,//分解
    ItemEquip,//装备
    ItemBehaviorMax
}
public enum ItemType
{
    Equip=0,//装备
    Frag,//碎片
    Other,//其他
    ItemTypeMax
}
public class ItemInfo
{
    public int ItemID;
    public string ItemName;
    public string ItemDescription;
    public string ItemIcon;
    public string RankIcon;
    public int ItemQuantity;
    public int ItemRank;
    public ItemBehavior itemBehavior;
    public ItemType itemType;

    public ItemInfo(int id, string name, string description, string icon, string rankicon, int quan, int rank, int behavior, int type)//构造函数
    {
        ItemName = name;
        ItemDescription = description;
        ItemIcon = icon;
        RankIcon = rankicon;
        ItemQuantity = quan;
        ItemID = id;
        ItemRank = rank;
        itemType = (ItemType)type;
        itemBehavior = (ItemBehavior)behavior;
    }
}
