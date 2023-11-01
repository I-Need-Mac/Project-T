using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public string itemid { get; private set; }                    //아이디
    public string itemname { get; private set; }               //이름
    public string description { get; private set; }        //설명
    public int type { get; private set; }                //타입
    public bool isShow { get; private set; }                //보이는지 여부
    public string imagePath { get; private set; }                //이미지 경로
    public int amount { get; private set; }                //수량


    public void SetItemid(string itemid) { this.itemid = itemid; }
    public void SetItemname(string itemname) { this.itemname = itemname; }
    public void SetDescription(string description) { this.description = description; }
    public void SetType(int type) { this.type = type; }
    public void SetIsShow(bool isShow) { this.isShow = isShow; }
    public void SetImagePath(string imagePath) { this.imagePath = imagePath; }
    public void SetAmount(int amount) { this.amount = amount; }

}
