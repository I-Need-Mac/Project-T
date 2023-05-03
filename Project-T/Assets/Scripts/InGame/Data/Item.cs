using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemData Data { get; private set; }

    public Item(string id, string name, string description, int amount)
    {
        Data = new ItemData();
        Data.SetItemid(id);
        Data.SetItemname(name);
        Data.SetDescription(description);
        Data.SetAmount(amount);

    }

}
