using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowItemDetail : MonoBehaviour {
    public Text ItemName;
    public Text ItemDescription;
    public Button SellButton;
    public Button CompondButton;
    public Button ResolveButton;
    public Button EquipButton;
    public GameObject ItemDetail;
    ItemInfo iteminfo;
    public DetailPanelTweet tweet;
    bool theSameItem = true;
    // Use this for initialization
    private void Start ()
    {

	}
	// Update is called once per frame
	void Update ()
    {
		
	}
    void GetItemInfo()
    {
        iteminfo = GetComponent<LoadItemInfo>().iteminfo;
    }
    void ShowItemName()
    {
        ItemName.text = iteminfo.ItemName;
    }
    void ShowDescription()
    {
        ItemDescription.text = iteminfo.ItemDescription;
    }
    void ShowBehaviorButton()
    {
        switch (iteminfo.itemType)
        {
            case ItemType.Equip:
                SellButton.gameObject.SetActive(true);
                ResolveButton.gameObject.SetActive(true);
                EquipButton.gameObject.SetActive(true);
                break;
            case ItemType.Frag:
                CompondButton.gameObject.SetActive(true);
                SellButton.gameObject.SetActive(true);
                break;
            case ItemType.Other:
                ResolveButton.gameObject.SetActive(true);
                SellButton.gameObject.SetActive(true);
                break;
        }
    }
    void ResetButton()
    {
        SellButton.gameObject.SetActive(false);
        ResolveButton.gameObject.SetActive(false);
        EquipButton.gameObject.SetActive(false);
        CompondButton.gameObject.SetActive(false);
    }
    public void ShowThisItemDetail()
    {
        ResetButton();
        GetItemInfo();
        ShowItemName();
        ShowDescription();
        ShowBehaviorButton();
    }
    public void OnClickShowDetailPanel()
    {
        theSameItem = !theSameItem;
        if (!theSameItem)
        {
            if (!tweet.DetailPanelIsOn)
            {
                tweet.PanelOn();
            }
            ShowThisItemDetail();
        }
        else
            tweet.PanelOff();
    }
}
