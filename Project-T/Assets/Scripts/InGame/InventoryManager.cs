using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonBehaviour<InventoryManager>
{

    private Dictionary<string, Item> itemList;

    private Dictionary<string, Dictionary<string, object>> itemTable;

    protected override void Awake()
    {
    }

    public void InventoryManagerInit(string storyPath)
    {
        itemList = new Dictionary<string, Item>();
        itemTable = CSVReader.Read(storyPath + "/Item_Definition");
    }

    public void SetItemList(Dictionary<string, Item> itemList)
    {
        this.itemList = itemList;
    }

    public void LoadItemList(string itemid, int value)
    {
        itemList.Add(itemid, new Item(itemid, itemTable[itemid]["Item_name"].ToString(),
                    itemTable[itemid]["Item_description"].ToString(), int.Parse(itemTable[itemid]["Item_type"].ToString()),
                    itemTable[itemid]["Is_Show"].ToString() == "True", itemTable[itemid]["Image_Path"].ToString(),
                    value));
    }

    public Dictionary<string, Item> GetItemList()
    {
        return itemList;
    }

    //아이템 요건 충족 확인 함수
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
            Dictionary<string, object> itemT = itemTable[itemid];
            int itemType = int.Parse(itemT["Item_type"].ToString());
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

    public void ItemChange(Dictionary<string, object> itemChangeInfo)
    {
        if (!string.Equals(itemChangeInfo["Target1_type"].ToString(), "Null"))
        {
            Add(itemChangeInfo["Target1_type"].ToString(), itemChangeInfo["Target1_change_value"].ToString());
        }
        if (!string.Equals(itemChangeInfo["Target2_type"].ToString(), "Null"))
        {
            Add(itemChangeInfo["Target2_type"].ToString(), itemChangeInfo["Target2_change_value"].ToString());
        }
        if (itemList.ContainsKey("Item_money"))
        {
            PlayUI.Instance.SetMoney(itemList["Item_money"].Data.amount.ToString("N0"));
        }
        else
        {
            PlayUI.Instance.SetMoney("0");
        }
    }
    //아이템 변동 함수
    public void Add(string itemid, string value)
    {
        DebugManager.Instance.PrintDebug("add 실행");
        Dictionary<string, object> itemT = itemTable[itemid];

        if (itemList.ContainsKey(itemid))
        {
            int itemType = itemList[itemid].Data.type;
            if (itemType == 1 || itemType == 2)
            {
                int result = itemList[itemid].Data.amount + int.Parse(value);
                if(result <= 0)
                {
                    itemList.Remove(itemid);
                }
                else
                {
                    itemList[itemid].Data.SetAmount(result);
                }
            }
            else if (itemType == 0)
            {
                int result = itemList[itemid].Data.amount + int.Parse(value);
                if (result <= 0)
                {
                    itemList.Remove(itemid);
                }
                else
                {
                    itemList[itemid].Data.SetAmount(1);
                }
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
                //DebugManager.Instance.PrintDebug(itemList[itemid].Data.amount);
            }
        }
        else
        {
            int itemType = int.Parse(itemT["Item_type"].ToString());
            if (itemType == 1 || itemType == 2)
            {
                if (int.Parse(value) > 0)
                {
                    itemList.Add(itemid, new Item(itemid, itemT["Item_name"].ToString(),
                    itemT["Item_description"].ToString(), int.Parse(itemT["Item_type"].ToString()),
                    itemT["Is_Show"].ToString() == "True", itemT["Image_Path"].ToString(),
                    int.Parse(value)));
                }

            }
            else if (itemType == 0)
            {
                if (int.Parse(value) > 0) {
                    itemList.Add(itemid, new Item(itemid, itemT["Item_name"].ToString(),
                    itemT["Item_description"].ToString(), int.Parse(itemT["Item_type"].ToString()),
                    itemT["Is_Show"].ToString() == "True", itemT["Image_Path"].ToString(),
                    int.Parse(value) == 1 ? 1 : 1));
                }
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
                itemList.Add(itemid, new Item(itemid, itemT["Item_name"].ToString(),
                    itemT["Item_description"].ToString(), int.Parse(itemT["Item_type"].ToString()),
                    itemT["Is_Show"].ToString() == "True", itemT["Image_Path"].ToString(),
                    result));
                //DebugManager.Instance.PrintDebug(itemList[itemid].Data.amount);
            }
        }
    }

    
}

