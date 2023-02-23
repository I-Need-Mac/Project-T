using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text StoryText;
    public TMP_Text[] ChoiceText;

    Dictionary<string, Dictionary<string, object>> story_load;
    Dictionary<string, Dictionary<string, object>> story;
    Dictionary<string, Dictionary<string, object>> choice;

    // Start is called before the first frame update
    void Start()
    {
        story_load = CSVReader.Read("Story_load");
        DebugManager.Instance.PrintDebug("story_load");

        story = CSVReader.Read("Story");
        DebugManager.Instance.PrintDebug("story");
        
        choice = CSVReader.Read("Choice");
        DebugManager.Instance.PrintDebug("choice");

        StoryUpdate("RYTA1001");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //스토리 매니저에 들어갈 부분
    void StoryUpdate(string storyID)
    {
        string storyT = (string)story[storyID]["story"];
        string choice_ID = (string)story_load[storyID]["choice_group_ID"];

        DebugManager.Instance.PrintDebug(storyT);

        storyT = storyT.Replace("\\c", ",").Replace("\\n", "\n");

        DebugManager.Instance.PrintDebug(storyT);

        StoryChange(storyT);

        string choice_1 = choice.ContainsKey(choice_ID + "1") ? (string)choice[choice_ID + "1"]["choice_text_ID"] : "";
        string choice_2 = choice.ContainsKey(choice_ID + "2") ? (string)choice[choice_ID + "2"]["choice_text_ID"] : "";
        string choice_3 = choice.ContainsKey(choice_ID + "3") ? (string)choice[choice_ID + "3"]["choice_text_ID"] : "";
        string choice_4 = choice.ContainsKey(choice_ID + "4") ? (string)choice[choice_ID + "4"]["choice_text_ID"] : "";


        ChoiceChange(choice_1, choice_2, choice_3, choice_4);
    }
    //UI 매니저에 들어갈 부분
    public void StoryChange(string story)
    {
        StoryText.text = story;
    }
    public void ChoiceChange(string choice_1, string choice_2, string choice_3, string choice_4)
    {
        // 나눠주는거 구현들어오는거 판단해서 
        ChoiceText[0].text = choice_1;
        ChoiceText[1].text = choice_2;
        ChoiceText[2].text = choice_3;
        ChoiceText[3].text = choice_4;
    }

}
