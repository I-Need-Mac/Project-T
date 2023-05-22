using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingleTon<InventoryManager>
{

    private Dictionary<string, Item> itemList;

    public InventoryManager()
    {
        itemList = new Dictionary<string, Item>();
    }

    public bool IsCondition(string itemid, string standard, string value)
    {
        if (itemList.ContainsKey(itemid))
        {
            switch(standard)
            {
                case "More":
                    if (itemList[itemid].Data.amount > int.Parse(value)) return true;
                    else return false;
                case "Less":
                    if (itemList[itemid].Data.amount < int.Parse(value)) return true;
                    else return false;
                case "Equal":
                    if (itemList[itemid].Data.amount == int.Parse(value)) return true;
                    else return false;

            }
        }
        return false;
    }
    public void Add(string itemid, string value)
    {
        DebugManager.Instance.PrintDebug("add 실행");
        Dictionary<string, object> itemT = StoryManager.Instance.GetItem(itemid);

        if (itemList.ContainsKey(itemid))
        {
            itemList[itemid].Data.SetAmount(itemList[itemid].Data.amount + int.Parse(value));
        }
        else
        {
            itemList.Add(itemid, new Item(itemid, (string)itemT["Item_name"], (string)itemT["Item_description"], int.Parse(value)));
        }
        
    }

    
}

