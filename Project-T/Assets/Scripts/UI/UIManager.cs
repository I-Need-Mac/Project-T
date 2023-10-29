using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text StoryText;
    public TMP_Text Choice_1;
    public TMP_Text Choice_2;
    public TMP_Text Choice_3;
    public TMP_Text Choice_4;

    public void StoryChange(string story)
    {
        StoryText.text = story;
    }
    public void choiceChange(string story)
    {
        //오류 있어서 좀 물어봐야함.
        StoryText.text = story;
    }
}
