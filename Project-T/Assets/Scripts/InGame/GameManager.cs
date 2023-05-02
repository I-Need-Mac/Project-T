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

    Dictionary<string, object>[] choices;

    private string[] choiceStoryID = new string[4];

    RectTransform StoryRectTran;
    RectTransform OptionRectTran;


    // Start is called before the first frame update
    void Start()
    {
        StoryRectTran = StoryArea.GetComponent<RectTransform>();
        OptionRectTran = OptionArea.GetComponent<RectTransform>();

        DebugManager.Instance.PrintDebug("choice");

        StoryUpdate("Story_0001");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StoryUpdate(string storyID)
    {
        Dictionary<string, object> StoryLoad = StoryManager.Instance.getStory_load(storyID);

        //아이템 

        if (!string.Equals((string)StoryLoad["Target1_type"], ""))
        {
            InventoryManager.Instance.add((string)StoryLoad["Target1_type"], (string)StoryLoad["Target1_change_value"]);
        }

        if (!string.Equals((string)StoryLoad["Target2_type"], ""))
        {
            InventoryManager.Instance.add((string)StoryLoad["Target1_type"], (string)StoryLoad["Target2_change_value"]);
        }

        // 스토리 로드

        string storyT = (string)StoryManager.Instance.getStory(storyID)["story"];
        string choiceID = (string)StoryLoad["choice_group_ID"];

        DebugManager.Instance.PrintDebug(storyT);

        storyT = storyT.Replace("\\c", ",").Replace("\\n", "\n");

        DebugManager.Instance.PrintDebug(storyT);

        StoryChange(storyT);


        choices = StoryManager.Instance.getChoice(choiceID);

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
        bool isCondition1 = InventoryManager.Instance.isCondition((string)choices[0]["condition1_type"], (string)choices[0]["condition1_standard"], (string)choices[0]["condition1_value"]);
        bool isCondition2 = InventoryManager.Instance.isCondition((string)choices[0]["condition2_type"], (string)choices[0]["condition2_standard"], (string)choices[0]["condition2_value"]);

        if (isCondition1)
        {
            if (isCondition2)
            {
                StoryUpdate(ResultDecode((string)choices[0]["And_result"]));
            }
            else
            {
                StoryUpdate(ResultDecode((string)choices[0]["Condition1_result"]));
            }
        }
        else
        {
            if (isCondition2)
            {
                StoryUpdate(ResultDecode((string)choices[0]["Condition2_result"]));
            }
            else
            {
                StoryUpdate(ResultDecode((string)choices[0]["fail_result"]));
            }
        }
    }
    public void SelectChoice2()
    {
        
        bool isCondition1 = InventoryManager.Instance.isCondition((string)choices[1]["condition1_type"], (string)choices[1]["condition1_standard"], (string)choices[1]["condition1_value"]);
        bool isCondition2 = InventoryManager.Instance.isCondition((string)choices[1]["condition2_type"], (string)choices[1]["condition2_standard"], (string)choices[1]["condition2_value"]);

        if (isCondition1)
        {
            if (isCondition2)
            {
                StoryUpdate(ResultDecode((string)choices[1]["And_result"]));
            }
            else
            {
                StoryUpdate(ResultDecode((string)choices[1]["Condition1_result"]));
            }
        }
        else
        {
            if (isCondition2)
            {
                StoryUpdate(ResultDecode((string)choices[1]["Condition2_result"]));
            }
            else
            {
                StoryUpdate(ResultDecode((string)choices[1]["fail_result"]));
            }
        }
    }
    public void SelectChoice3()
    {
        bool isCondition1 = InventoryManager.Instance.isCondition((string)choices[2]["condition1_type"], (string)choices[2]["condition1_standard"], (string)choices[2]["condition1_value"]);
        bool isCondition2 = InventoryManager.Instance.isCondition((string)choices[2]["condition2_type"], (string)choices[2]["condition2_standard"], (string)choices[2]["condition2_value"]);

        if (isCondition1)
        {
            if (isCondition2)
            {
                StoryUpdate(ResultDecode((string)choices[2]["And_result"]));
            }
            else
            {
                StoryUpdate(ResultDecode((string)choices[2]["Condition1_result"]));
            }
        }
        else
        {
            if (isCondition2)
            {
                StoryUpdate(ResultDecode((string)choices[2]["Condition2_result"]));
            }
            else
            {
                StoryUpdate(ResultDecode((string)choices[2]["fail_result"]));
            }
        }
    }
    public void SelectChoice4()
    {
        bool isCondition1 = InventoryManager.Instance.isCondition((string)choices[3]["condition1_type"], (string)choices[3]["condition1_standard"], (string)choices[3]["condition1_value"]);
        bool isCondition2 = InventoryManager.Instance.isCondition((string)choices[3]["condition2_type"], (string)choices[3]["condition2_standard"], (string)choices[3]["condition2_value"]);

        if (isCondition1)
        {
            if (isCondition2)
            {
                StoryUpdate(ResultDecode((string)choices[3]["And_result"]));
            }
            else
            {
                StoryUpdate(ResultDecode((string)choices[3]["Condition1_result"]));
            }
        }
        else
        {
            if (isCondition2)
            {
                StoryUpdate(ResultDecode((string)choices[3]["Condition2_result"]));
            }
            else
            {
                StoryUpdate(ResultDecode((string)choices[3]["fail_result"]));
            }
        }
    }

    public string ResultDecode(string resultText)
    {
        string[] resultSplit = resultText.Split("_");

        int randomNum = Random.Range(0, 100);

        for (int i = 0; i < resultText.Length/2; i++)
        {
            randomNum -= int.Parse(resultSplit[i * 2 + 1].Substring(1));
            if(randomNum < 0)
            {
                return resultSplit[i * 2].Replace("R", "Story_");
            }
        }
        return "오류";
    }


}
