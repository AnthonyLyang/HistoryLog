using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;

public class BackPackManager
{
    private static BackPackManager mInstance;
    static readonly object insLock = new object();
    public static BackPackManager Instance
    {
        get
        {
            if (mInstance == null)
            {
                lock (insLock)
                {
                    if (mInstance == null)
                    {
                        mInstance = new BackPackManager();
                    }
                }
            }
            return mInstance;
        }
    }
    private JsonData itemJsonData;
    public List<ItemInfo> ItemList = new List<ItemInfo>();
    public List<GameObject> ItemObjectList = new List<GameObject>();
    private BackPackManager()
    {
        //CreateTestItemList();
        LoadItemFile();
        LoadJson();
    }
    private void CreateTestItemList()
    {
        for (int i= 0; i < 100; i++)
        {
            int id = 10000 + i;
            ItemInfo item = new ItemInfo
                (
                id,
                "item" + id.ToString(),
                "this is an item for test",
                "a",
                "SF Generic",
                Random.Range(1, 100),
                Random.Range(0, 5),
                Random.Range(0, (int)ItemBehavior.ItemBehaviorMax),
                Random.Range(0, (int)ItemType.ItemTypeMax)
                );
            ItemList.Add(item);
        }
    }
    private void LoadItemFile()
    {
        var textAsset = Resources.Load<TextAsset>("DataTable/ItemList");
        if (textAsset.text == null)
        {
            Debug.Log("file error");
            return;
        }
        Decode(textAsset.text);
    }
    private bool Decode(string content)
    {
        string[] row = content.Replace("\r", "").Split(new char[] { '\n' });
        string[] columnHeads = row[0].Split(new char[] { ',' });
        Debug.Log(row.Length);
        for (int i = 1; i < row.Length; i++)
        {
            string[] line = row[i].Split(new char[] { ',' });
            if (line.Length < 9)
            {
                Debug.Log("NoItem");
                continue;
            }
            int j = 0;
            int id = int.Parse(line[j++]);
            string name = line[j++];
            string description = line[j++];
            string itemicon = line[j++];
            string rankicon = line[j++];
            int quan = int.Parse(line[j++]);
            int rank = int.Parse(line[j++]);
            int itembehavior = int.Parse(line[j++]);
            int itemtype = int.Parse(line[j++]);
            var Item = new ItemInfo(id, name, description, itemicon, rankicon, quan, rank, itembehavior, itemtype);
            ItemList.Add(Item);
            Debug.Log(Item);
        }
        return true;
    }
    private void LoadJson()
    {
        var text = Resources.Load<TextAsset>("Datatable/ItemJsonFile").text.ToString();
        Debug.Log(text);
        itemJsonData = JsonMapper.ToObject(text);
        Debug.Log("111");
        for (int i = 0; i < itemJsonData.Count; i++)
        {
            Debug.Log("JSON");
            int id = (int)itemJsonData[i]["ID"];
            string name = itemJsonData[i]["ItemName"].ToString();
            string description = itemJsonData[i]["Description"].ToString();
            string itemicon = itemJsonData[i]["ItemIcon"].ToString();
            string rankicon = itemJsonData[i]["RankIcon"].ToString();
            int itemquantity = (int)itemJsonData[i]["ItemQuantity"];
            int rank = (int)itemJsonData[i]["Rank"];
            int itembehavior = (int)itemJsonData[i]["itemBehavior"];
            int itemtype = (int)itemJsonData[i]["itemType"];
            var Item = new ItemInfo(id, name, description, itemicon, rankicon, itemquantity, rank, itembehavior, itemtype);
            ItemList.Add(Item);
        }
    }
}
