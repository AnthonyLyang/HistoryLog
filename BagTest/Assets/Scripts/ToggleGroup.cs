using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGroup : MonoBehaviour {
    List<GameObject> ListTemp = null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ShowItemInType(ItemType type)
    {
        ListTemp = BackPackManager.Instance.ItemObjectList;
        if (ListTemp == null || ListTemp.Count == 0)
        {
            return;
        }
        foreach (GameObject gobj in ListTemp)
        {
            var ObjItemInfoType = gobj.GetComponent<LoadItemInfo>().iteminfo.itemType;
            if (type != ItemType.Other && type != ObjItemInfoType)
            {
                gobj.SetActive(false);
            }
            else
                gobj.SetActive(true);
        }
    }
    public void OnClickShowItemAll()
    {
        ShowItemInType(ItemType.Other);
    }
    public void OnClickShowItemEquipment()
    {
        ShowItemInType(ItemType.Equip);
    }
    public void OnClickShowItemFragment()
    {
        ShowItemInType(ItemType.Frag);
    }

}
