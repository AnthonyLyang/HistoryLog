using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadItemInfo : MonoBehaviour {
    public ItemInfo iteminfo;
    public Image RankImage;
    public Text Quan;
    public Text Type;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LoadItemData(ItemInfo ItemInList)
    {
        iteminfo = ItemInList;
        switch (iteminfo.ItemRank)
        {
            case 0:
                RankImage.color = Color.white;
                break;
            case 1:
                RankImage.color = Color.green;
                break;
            case 2:
                RankImage.color = Color.blue;
                break;
            case 3:
                RankImage.color = Color.magenta;
                break;
            case 4:
                RankImage.color = Color.yellow;
                break;
        }
        switch (iteminfo.itemType)
        {
            case ItemType.Equip:
                Type.text = "Eq";
                break;
            case ItemType.Frag:
                Type.text = "Fr";
                break;
            case ItemType.Other:
                break;
        }
        Quan.text = iteminfo.ItemQuantity.ToString();
    }
}
