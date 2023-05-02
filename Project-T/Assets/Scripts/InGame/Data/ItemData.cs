using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public string _id { get; set; }                    //아이디
    public string _name { get; set; }               //이름
    public string _description { get; set; }        //설명
    public int _amount { get; set; }                //수량

    public ItemData(string id, string name, string description, int amount) 
    {
        _id = id; 
        _name = name;
        _description = description;
        _amount = amount;
    } 
}
