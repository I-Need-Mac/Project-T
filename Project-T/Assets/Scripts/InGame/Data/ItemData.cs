using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public string itemid { get; private set; }                    //아이디
    public string itemname { get; private set; }               //이름
    public string description { get; private set; }        //설명
    public int amount { get; private set; }                //수량

    public void SetItemid(string itemid) { this.itemid = itemid; }
    public void SetItemname(string itemname) { this.itemname = itemname; }
    public void SetDescription(string description) { this.description = description; }
    public void SetAmount(int amount) { this.amount = amount; }

}
