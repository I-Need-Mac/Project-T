using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text StoryText;
    public TMP_Text[] ChoiceText;

    public string curentStoryID;

    // Start is called before the first frame update
    void Start()
    {
        DebugManager.Instance.PrintDebug("choice");

        StoryUpdate("RYTA1001");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StoryUpdate(string storyID)
    {
        string storyT = (string)StoryManager.Instance.getStory(storyID)["story"];
        string choiceID = (string)StoryManager.Instance.getStory_load(storyID)["choice_group_ID"];

        DebugManager.Instance.PrintDebug(storyT);

        storyT = storyT.Replace("\\c", ",").Replace("\\n", "\n");

        DebugManager.Instance.PrintDebug(storyT);

        StoryChange(storyT);


        Dictionary<string, object>[] choices = StoryManager.Instance.getChoice(choiceID);

        string[] choice = new string[choices.Length];
        for(int i = 0; i < choices.Length; i++)
        {
            choice[i] = (string)choices[i]["choice_text_ID"];
        }

        ChoiceChange(choice);
    }

    //UI 매니저에 들어갈 부분
    public void StoryChange(string story)
    {
        StoryText.text = story;
    }
    public void ChoiceChange(string[] choice)
    {
        // 나눠주는거 구현들어오는거 판단해서 
        for (int i = 0; i < choice.Length; i++)
        {
            ChoiceText[i].text = choice[i];
        }
    }

}
