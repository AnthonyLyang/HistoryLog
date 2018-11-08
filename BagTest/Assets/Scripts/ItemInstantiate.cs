using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInstantiate : MonoBehaviour {
    public Transform ItemParent;
    public Transform ItemPrototype;
    GameObject ItemToShow;
    // Use this for initialization
    void Start()
    {
        CreateItemObjsInUI();

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    public GameObject InstanItem()
    {
        GameObject iteminstan = Instantiate(ItemPrototype.gameObject, ItemParent);
        return iteminstan;
    }
    public void CreateItemObjsInUI()
    {
        var List = BackPackManager.Instance.ItemList;
        if (List == null || List.Count == 0)
            return;
        else
        {
            foreach (ItemInfo i in List)
            {
                ItemToShow = InstanItem();
                ItemToShow.GetComponent<LoadItemInfo>().LoadItemData(i);
                BackPackManager.Instance.ItemObjectList.Add(ItemToShow);
                ItemToShow.SetActive(true);
            }
        }
    }
}
