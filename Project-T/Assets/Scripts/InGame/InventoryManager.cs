using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingleTon<InventoryManager>
{

    private List<Item> _items;

    public InventoryManager()
    {
        _items = new List<Item>();
    }

    public bool isCondition(string itemid, string standard, string value)
    {
        for (int i = 0; i < _items.Count; i++)
        {
            if (string.Equals(_items[i].Data._id, itemid))
            {
                switch(standard)
                {
                    case "More":
                        if (_items[i].Data._amount > int.Parse(value)) return true;
                        else return false;
                    case "Less":
                        if (_items[i].Data._amount < int.Parse(value)) return true;
                        else return false;
                    case "Equal":
                        if (_items[i].Data._amount == int.Parse(value)) return true;
                        else return false;

                }

            }
        }
        return false;
    }
    public void add(string itemid, string value)
    {
        DebugManager.Instance.PrintDebug("add 실행");
        Dictionary<string, object> itemT = StoryManager.Instance.getItem(itemid);

        bool isValid = false;

        for (int i = 0; i < _items.Count; i++)
        {
            if (string.Equals(_items[i].Data._id, itemid))
            {
                _items[i].Data._amount += int.Parse(value);
                isValid = true;
            }
        }

        if (!isValid)
        {
            ItemData itemdata = new ItemData(itemid, (string)itemT["Item_name"], (string)itemT["Item_description"], int.Parse(value));
            _items.Add(new Item(itemdata));
        }
        
    }

    
}

