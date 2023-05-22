using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //-------UI 요소들----------
    public GameObject storyArea;
    public GameObject optionArea;

    public GameObject storyContent;
    public TMP_Text[] choiceText;
    public Button[] choicebtn;

    GameObject textContent;
    GameObject imageContent;

    RectTransform storyRectTran;
    RectTransform optionRectTran;

    ScrollRect scrollRect;
    //---------------------------

    Dictionary<string, object>[] choices;

    List<Dictionary<string, object>> outputChoices;


    // Start is called before the first frame update
    void Start()
    {
        scrollRect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
        storyRectTran = storyArea.GetComponent<RectTransform>();
        optionRectTran = optionArea.GetComponent<RectTransform>();

        textContent = Resources.Load<GameObject>("Prefabs/TextContent");
        imageContent = Resources.Load<GameObject>("Prefabs/ImageContent");

        DebugManager.Instance.PrintDebug("choice");

        StoryUpdate("Story_0001");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StoryUpdate(string storyID)
    {
        Dictionary<string, object> StoryLoad = StoryManager.Instance.GetStory_load(storyID);

        //아이템 

        if (!string.Equals((string)StoryLoad["Target1_type"], ""))
        {
            InventoryManager.Instance.Add((string)StoryLoad["Target1_type"], (string)StoryLoad["Target1_change_value"]);
        }

        if (!string.Equals((string)StoryLoad["Target2_type"], ""))
        {
            InventoryManager.Instance.Add((string)StoryLoad["Target1_type"], (string)StoryLoad["Target2_change_value"]);
        }

        // 스토리 로드

        string storyT = (string)StoryManager.Instance.GetStory(storyID)["story"];
        string choiceID = (string)StoryLoad["choice_group_ID"];

        DebugManager.Instance.PrintDebug(storyT);

        storyT = storyT.Replace("\\c", ",").Replace("\\n", "\n");

        DebugManager.Instance.PrintDebug(storyT);

        StoryChange(storyT);

        //선택지 관리
        choices = StoryManager.Instance.GetChoice(choiceID);

        outputChoices = new List<Dictionary<string, object>>();

        for (int i = 0; i < choices.Length; i++)
        {
            string choiceType = (string)choices[i]["Choice_type"];
            if(string.Equals(choiceType, "Hidden"))
            {
                if (InventoryManager.Instance.IsCondition((string)choices[i]["hidden_Choice_condition_type"], (string)choices[i]["hidden_Choice_condition_Standard"], (string)choices[i]["hidden_Choice_condition_value"]))
                {
                    outputChoices.Add(choices[i]);
                }
            }
            else
            {
                outputChoices.Add(choices[i]);
            }

        }

        ChoiceChange(outputChoices);


    }
    public void SelectChoice1()
    {
        SelectChoice(0);
    }
    public void SelectChoice2()
    {
        SelectChoice(1);
    }
    public void SelectChoice3()
    {
        SelectChoice(2);
    }
    public void SelectChoice4()
    {
        SelectChoice(3);
    }

    public void SelectChoice(int btnNum)
    {
        bool isCondition1 = InventoryManager.Instance.IsCondition((string)outputChoices[btnNum]["condition1_type"], (string)outputChoices[btnNum]["condition1_standard"], (string)outputChoices[btnNum]["condition1_value"]);
        bool isCondition2 = InventoryManager.Instance.IsCondition((string)outputChoices[btnNum]["condition2_type"], (string)outputChoices[btnNum]["condition2_standard"], (string)outputChoices[btnNum]["condition2_value"]);

        if (isCondition1)
        {
            if (isCondition2)
            {
                StoryUpdate(ResultDecode((string)outputChoices[btnNum]["And_result"]));
            }
            else
            {
                StoryUpdate(ResultDecode((string)outputChoices[btnNum]["Condition1_result"]));
            }
        }
        else
        {
            if (isCondition2)
            {
                StoryUpdate(ResultDecode((string)outputChoices[btnNum]["Condition2_result"]));
            }
            else
            {
                StoryUpdate(ResultDecode((string)outputChoices[btnNum]["fail_result"]));
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



    //UI 매니저에 들어갈 부분 ex) 단순 텍스트 요소 변경
    public void StoryChange(string story)
    {
        GameObject textInstance = Instantiate(textContent);
        textInstance.transform.parent = storyContent.transform;
        textInstance.transform.localScale = new Vector3(1, 1, 1);
        textInstance.GetComponent<TextMeshProUGUI>().text = story;
    }

    public void ChoiceChange(List<Dictionary<string, object>> choices)
    {
        int count = 0;

        // 나눠주는거 구현들어오는거 판단해서 
        foreach(Dictionary<string, object> choice in choices)
        {
            string choiceType = (string)choice["Choice_type"];
            if (string.Equals(choiceType, "Dimmed"))
            {
                choicebtn[count].interactable = InventoryManager.Instance.IsCondition((string)choice["hidden_Choice_condition_type"], (string)choice["hidden_Choice_condition_Standard"], (string)choice["hidden_Choice_condition_value"]);
            }
            else
            {
                choicebtn[count].interactable = true;
                
            }
            choiceText[count].text = (string)choice["choice_text_ID"];
            count++;
        }

        optionRectTran.anchoredPosition = new Vector3(0, 55 * count, 0);
        optionRectTran.sizeDelta = new Vector2(optionRectTran.sizeDelta.x, 110 * count);

        storyRectTran.offsetMin = new Vector2(storyRectTran.offsetMin.x, 110 * count);

    }
    //------------------------------------------------

    


}
