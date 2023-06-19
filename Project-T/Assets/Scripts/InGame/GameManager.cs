using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    Dictionary<string, object>[] choices;

    List<Dictionary<string, object>> outputChoices;
    Dictionary<string, object> resource;
    
    GameObject textContent;
    GameObject imageContent;

    // Start is called before the first frame update
    void Start()
    {

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
        Dictionary<string, object> storyLoad = StoryManager.Instance.GetStory_load(storyID);

        //아이템 

        if (!string.Equals((string)storyLoad["Target1_type"], ""))
        {
            InventoryManager.Instance.Add((string)storyLoad["Target1_type"], (string)storyLoad["Target1_change_value"]);
        }

        if (!string.Equals((string)storyLoad["Target2_type"], ""))
        {
            InventoryManager.Instance.Add((string)storyLoad["Target1_type"], (string)storyLoad["Target2_change_value"]);
        }

        //리소스

        resource = StoryManager.Instance.GetResource(storyID);


        // 스토리 로드

        string storyT = (string)StoryManager.Instance.GetStory(storyID)["story"];
        string choiceID = (string)storyLoad["choice_group_ID"];

        DebugManager.Instance.PrintDebug(storyT);

        storyT = storyT.Replace("\\c", ",").Replace("\\n", "\n");

        DebugManager.Instance.PrintDebug(storyT);

        
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

        UIManager.Instance.ChoiceChange(outputChoices);

        if (!string.Equals((string)resource["Illustration"], ""))
        {
            GameObject imageInstance = Instantiate(imageContent);
            imageInstance.GetComponent<Image>().sprite = ImageLoader.Instance.LoadLocalImageToSprite((string)resource["Illustration"]);

            GameObject textInstance = Instantiate(textContent);

            StartCoroutine(UIManager.Instance.StartTyping(textInstance, storyT));

            StartCoroutine(UIManager.Instance.ScrollSmoothly(UIManager.Instance.ImageStoryChange(imageInstance, textInstance)));
        }
        else
        {
            GameObject textInstance = Instantiate(textContent);
            StartCoroutine(UIManager.Instance.StartTyping(textInstance, storyT));

            StartCoroutine(UIManager.Instance.ScrollSmoothly(UIManager.Instance.StoryChange(textInstance)));
        }

    }

    //각 버튼 함수
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



}
