using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject StoryArea;
    public GameObject OptionArea;

    public TMP_Text StoryText;
    public TMP_Text[] ChoiceText;

    private string curentStoryID;

    private string[] choiceStoryID = new string[4];
    private string[] choiceResultID = new string[4];

    RectTransform StoryRectTran;
    RectTransform OptionRectTran;


    // Start is called before the first frame update
    void Start()
    {
        StoryRectTran = StoryArea.GetComponent<RectTransform>();
        OptionRectTran = OptionArea.GetComponent<RectTransform>();

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
            choiceStoryID[i] = (string)choices[i]["And_result"];
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
        int count = choice.Length;

        OptionRectTran.anchoredPosition = new Vector3(0, 55 * count, 0);
        OptionRectTran.sizeDelta = new Vector2(OptionRectTran.sizeDelta.x, 110 * count);

        StoryRectTran.offsetMin = new Vector2(StoryRectTran.offsetMin.x, 110 * count);

        // 나눠주는거 구현들어오는거 판단해서 
        for (int i = 0; i < count; i++)
        {
            ChoiceText[i].text = choice[i];
        }
    }

    public void SelectChoice1()
    {
        Dictionary<string, object> choice_Result = StoryManager.Instance.getChoice_result(choiceStoryID[0]);

        StoryUpdate((string)choice_Result["Story_id_1"]);

    }
    public void SelectChoice2()
    {
        Dictionary<string, object> choice_Result = StoryManager.Instance.getChoice_result(choiceStoryID[1]);

        StoryUpdate((string)choice_Result["Story_id_1"]);
    }
    public void SelectChoice3()
    {
        Dictionary<string, object> choice_Result = StoryManager.Instance.getChoice_result(choiceStoryID[2]);

        StoryUpdate((string)choice_Result["Story_id_1"]);
    }
    public void SelectChoice4()
    {
        Dictionary<string, object> choice_Result = StoryManager.Instance.getChoice_result(choiceStoryID[3]);

        StoryUpdate((string)choice_Result["Story_id_1"]);
    }


}
