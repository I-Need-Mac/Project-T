using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemData Data { get; private set; }

    public Item(string id, string name, string description, int type, bool isShow,string imagePath,  int amount)
    {
        Data = new ItemData();
        Data.SetItemid(id);
        Data.SetItemname(name);
        Data.SetDescription(description);
        Data.SetType(type);
        Data.SetIsShow(isShow);
        Data.SetImagePath(imagePath);
        Data.SetAmount(amount);

    }

}
