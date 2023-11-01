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
            int itemType = itemList[itemid].Data.type;
            if (itemType != 3) // 3번이 아닐 경우
            {
                switch (standard)
                {
                    case "More":
                        return itemList[itemid].Data.amount > int.Parse(value);
                    case "Less":
                        return itemList[itemid].Data.amount < int.Parse(value);
                    case "Equal":
                        return itemList[itemid].Data.amount == int.Parse(value);

                }
            }
            else
            {// 3번 일 경우
                if (standard == "Equal")
                {
                    switch (value)
                    {
                        case "1000":
                            return itemList[itemid].Data.amount / 1000 == 1;
                        case "100":
                            return (itemList[itemid].Data.amount % 1000) / 100 == 1;
                        case "10":
                            return (itemList[itemid].Data.amount % 100) / 10 == 1;
                        case "1":
                            return (itemList[itemid].Data.amount % 10) / 1 == 1;
                        case "0000":
                            return itemList[itemid].Data.amount == 0;
                        case "0001":
                            return itemList[itemid].Data.amount == 1;
                        case "0011":
                            return itemList[itemid].Data.amount == 11;
                        case "0111":
                            return itemList[itemid].Data.amount == 111;
                        default:
                            Debug.LogError("틀린 서식");
                            return false;
                    }
                }
                else
                {
                    Debug.LogError("틀린 서식");
                    return false;
                }
            }
        }
        else if(itemid != "Null")
        {
            Dictionary<string, object> itemT = StoryManager.Instance.GetItem(itemid);
            int itemType = int.Parse((string)itemT["Item_type"]);
            Debug.Log(itemType);
            if (itemType != 3)
            {
                switch (standard)
                {
                    case "More":
                        if (0 > int.Parse(value)) return true;
                        else return false;
                    case "Less":
                        if (0 < int.Parse(value)) return true;
                        else return false;
                    case "Equal":
                        if (0 == int.Parse(value)) return true;
                        else return false;
                }
            }
            else
            {
                if (standard == "Equal")
                {
                    switch (value)
                    {
                        case "1000":
                            return true;
                        case "100":
                            return true;
                        case "10":
                            return true;
                        case "1":
                            return true;
                        default:
                            Debug.LogError("틀린 서식");
                            return false;
                    }
                }
                else
                {
                    Debug.LogError("틀린 서식");
                    return false;
                }
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
            int itemType = itemList[itemid].Data.type;
            if (itemType == 1 || itemType == 2)
            {
                int result = itemList[itemid].Data.amount + int.Parse(value);
                itemList[itemid].Data.SetAmount(result < 0 ? 0 : result);
            }
            else if (itemType == 0)
            {
                int result = itemList[itemid].Data.amount + int.Parse(value);
                itemList[itemid].Data.SetAmount(result >= 1 ? 1 : 0);
            }
            else if (itemType == 3)
            {
                int result = 1111;
                switch (value)
                {
                    case "-1000":
                        if (itemList[itemid].Data.amount / 1000 == 1)
                        {
                            result = itemList[itemid].Data.amount + int.Parse(value);
                        }
                        break;
                    case "-100":
                        if ((itemList[itemid].Data.amount % 1000) / 100 == 1)
                        {
                            result = itemList[itemid].Data.amount + int.Parse(value);
                        }
                        break;
                    case "-10":
                        if ((itemList[itemid].Data.amount % 100) / 10 == 1)
                        {
                            result = itemList[itemid].Data.amount + int.Parse(value);
                        }
                        break;
                    case "-1":
                        if ((itemList[itemid].Data.amount % 10) / 1 == 1)
                        {
                            result = itemList[itemid].Data.amount + int.Parse(value);
                        }
                        break;
                }
                itemList[itemid].Data.SetAmount(result);
            }
        }
        else
        {
            int itemType = int.Parse((string)itemT["Item_type"]);
            if (itemType == 1 || itemType == 2)
            {
                itemList.Add(itemid, new Item(itemid, (string)itemT["Item_name"],
                    (string)itemT["Item_description"], int.Parse((string)itemT["Item_type"]),
                    (string)itemT["Is_Show"] == "True", (string)itemT["Image_Path"],
                    int.Parse(value) < 0 ? 0 : int.Parse(value)));
            }
            else if (itemType == 0)
            {
                itemList.Add(itemid, new Item(itemid, (string)itemT["Item_name"],
                    (string)itemT["Item_description"], int.Parse((string)itemT["Item_type"]),
                    (string)itemT["Is_Show"] == "True", (string)itemT["Image_Path"],
                    int.Parse(value) > 1 ? 1 : 0));
            }
            else if (itemType == 3)
            {
                int result = 1111;
                switch (value)
                {
                    case "-1000":
                        result = 111;
                        break;
                    case "-100":
                        result = 1011;
                        break;
                    case "-10":
                        result = 1101;
                        break;
                    case "-1":
                        result = 1110;
                        break;
                }
                itemList.Add(itemid, new Item(itemid, (string)itemT["Item_name"],
                    (string)itemT["Item_description"], int.Parse((string)itemT["Item_type"]),
                    (string)itemT["Is_Show"] == "True", (string)itemT["Image_Path"],
                    result)); 
            }
        }
    }

    
}

