using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

using Image = UnityEngine.UI.Image;
using Button = UnityEngine.UIElements.Button;

public class GameManager : MonoBehaviour
{
    
    Dictionary<string, object>[] choices;

    List<Dictionary<string, object>> outputChoices;
    Dictionary<string, object> resource;
    
    GameObject textContent;
    GameObject imageContent;

    string storyT;
    GameObject textInstance;

    float typingSpeed = 0.05f;

    public Button[] choiceBtn = new Button[4];

    // Start is called before the first frame update
    void Start()
    {
        var root = GameObject.Find("UIDocument").GetComponent<UIDocument>().rootVisualElement;

        textContent = Resources.Load<GameObject>("Prefabs/TextContent");
        imageContent = Resources.Load<GameObject>("Prefabs/ImageContent");

        DebugManager.Instance.PrintDebug("choice");

        choiceBtn[0] = root.Q<Button>("choice-0");
        choiceBtn[1] = root.Q<Button>("choice-1");
        choiceBtn[2] = root.Q<Button>("choice-2");
        choiceBtn[3] = root.Q<Button>("choice-3");

        choiceBtn[0].RegisterCallback<ClickEvent>(SelectChoice1);
        choiceBtn[1].RegisterCallback<ClickEvent>(SelectChoice2);
        choiceBtn[2].RegisterCallback<ClickEvent>(SelectChoice3);
        choiceBtn[3].RegisterCallback<ClickEvent>(SelectChoice4);

        StoryUpdate("Story_0000");

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StoryUpdate(string storyID)
    {
        Dictionary<string, object> storyLoad = StoryManager.Instance.GetStory_load(storyID);

        //아이템 

        if (!string.Equals((string)storyLoad["Target1_type"], "Null"))
        {
            InventoryManager.Instance.Add((string)storyLoad["Target1_type"], (string)storyLoad["Target1_change_value"]);
        }

        if (!string.Equals((string)storyLoad["Target2_type"], "Null"))
        {
            InventoryManager.Instance.Add((string)storyLoad["Target1_type"], (string)storyLoad["Target2_change_value"]);
        }

        //리소스

        resource = StoryManager.Instance.GetResource(storyID);


        // 스토리 로드

        storyT = (string)StoryManager.Instance.GetStory(storyID)["story"];
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
        int choiceCount = 0;
        foreach (Dictionary<string, object> outputChoice in outputChoices)
        {
            string choiceType = (string)outputChoice["Choice_type"];
            if (string.Equals(choiceType, "Dimmed"))
            {
                if (InventoryManager.Instance.IsCondition((string)outputChoice["hidden_Choice_condition_type"], (string)outputChoice["hidden_Choice_condition_Standard"], (string)outputChoice["hidden_Choice_condition_value"]))
                {
                    SelectChoiceInt(choiceCount);
                }
                else
                {
                    UnSelectChoiceInt(choiceCount);
                }
            }
            else
            {
                SelectChoiceInt(choiceCount);
            }
            choiceCount++;
        }

        UIManager.Instance.ChoiceChange(outputChoices);
        UIManager.Instance.SetTextLength(storyT);

        if (!string.Equals((string)resource["Illustration"], ""))
        {
            GameObject imageInstance = Instantiate(imageContent);
            imageInstance.GetComponent<Image>().sprite = ImageLoader.Instance.LoadLocalImageToSprite((string)resource["Illustration"]);

            textInstance = Instantiate(textContent);

            StartCoroutine(ScrollSmoothly(UIManager.Instance.ImageStoryChange(imageInstance, textInstance)));
        }
        else
        {
            textInstance = Instantiate(textContent);

            StartCoroutine(ScrollSmoothly(UIManager.Instance.StoryChange(textInstance)));
        }
    }

    //각 버튼 함수
    public void SelectChoice1(ClickEvent evt)
    {
        SelectChoice(0);
    }
    public void SelectChoice2(ClickEvent evt)
    {
        SelectChoice(1);
    }
    public void SelectChoice3(ClickEvent evt)
    {
        SelectChoice(2);
    }
    public void SelectChoice4(ClickEvent evt)
    {
        SelectChoice(3);
    }

    public void SelectChoiceInt(int i)
    {
        switch (i)
        {
            case 0:
                choiceBtn[0].RegisterCallback<ClickEvent>(SelectChoice1);
                break;
            case 1:
                choiceBtn[1].RegisterCallback<ClickEvent>(SelectChoice2);
                break;
            case 2:
                choiceBtn[2].RegisterCallback<ClickEvent>(SelectChoice3);
                break;
            case 3:
                choiceBtn[3].RegisterCallback<ClickEvent>(SelectChoice4);
                break;
        }
    }

    public void UnSelectChoiceInt(int i)
    {
        switch (i)
        {
            case 0:
                choiceBtn[0].UnregisterCallback<ClickEvent>(SelectChoice1);
                break;
            case 1:
                choiceBtn[1].UnregisterCallback<ClickEvent>(SelectChoice2);
                break;
            case 2:
                choiceBtn[2].UnregisterCallback<ClickEvent>(SelectChoice3);
                break;
            case 3:
                choiceBtn[3].UnregisterCallback<ClickEvent>(SelectChoice4);
                break;
        }
    }

    public void SelectChoice(int btnNum)
    {
        UIManager.Instance.StoryStateSetting();

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

    public IEnumerator StartTyping(GameObject textInstance, string targetText) //타이핑 IEnumerator
    {
        TextMeshProUGUI textComponent = textInstance.GetComponent<TextMeshProUGUI>();

        for (int i = 0; i <= targetText.Length; i++)
        {
            textComponent.text = targetText.Substring(0, i);
            yield return new WaitForSeconds(typingSpeed);
        }

        RectTransform textRectTransform = textInstance.GetComponent<RectTransform>();

        textRectTransform.sizeDelta = new Vector2(textRectTransform.sizeDelta.x, textInstance.GetComponent<TextMeshProUGUI>().preferredHeight);

        UIManager.Instance.RebuildLayout();

        UIManager.Instance.ChoiceStateSetting();
        //StartCoroutine(SmoothlyUP());
    }

    public IEnumerator ScrollSmoothly(Vector2 targetNormalizedPosition) //스크롤 IEnumerator
    {
        Vector2 startPosition = UIManager.Instance.scrollRect.normalizedPosition;
        Vector2 targetPosition = targetNormalizedPosition;

        float elapsedTime = 0f;

        while (elapsedTime < UIManager.Instance.scrollDuration)
        {
            float t = elapsedTime / UIManager.Instance.scrollDuration;
            UIManager.Instance.scrollRect.normalizedPosition = Vector2.Lerp(startPosition, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        UIManager.Instance.scrollRect.normalizedPosition = targetPosition;

        StartCoroutine(StartTyping(textInstance, storyT));
    }
    /*
    public IEnumerator SmoothlyUP()
    {
        float elapsedTime = 0f;

        Vector2 startPosition = UIManager.Instance.optionRectTran.anchoredPosition;
        Vector3 targetPosition = new Vector3(0, 55 * UIManager.Instance.choiceCount, 0);

        Vector2 startPosition2 = UIManager.Instance.optionRectTran.sizeDelta;
        Vector2 targetPosition2 = new Vector2(UIManager.Instance.optionRectTran.sizeDelta.x, 110 * (UIManager.Instance.choiceCount));

        Vector2 startPosition3 = UIManager.Instance.scrollRect.normalizedPosition;
        
        Vector2 targetPosition3 = UIManager.Instance.IsOver() ? new Vector2(1, Mathf.Clamp01(UIManager.Instance.scrollRect.normalizedPosition.y - ((110 * UIManager.Instance.choiceCount) / UIManager.Instance.scrollRect.content.rect.height))) : UIManager.Instance.scrollRect.normalizedPosition;

        while (elapsedTime < UIManager.Instance.scrollDuration)
        {
            float t = elapsedTime / UIManager.Instance.scrollDuration;
            UIManager.Instance.optionRectTran.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            UIManager.Instance.optionRectTran.sizeDelta = Vector2.Lerp(startPosition2, targetPosition2, t);
            UIManager.Instance.scrollRect.normalizedPosition = Vector2.Lerp(startPosition3, targetPosition3, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        UIManager.Instance.optionRectTran.anchoredPosition = targetPosition;
        UIManager.Instance.optionRectTran.sizeDelta = targetPosition2;
        UIManager.Instance.scrollRect.normalizedPosition = targetPosition3;
    }
    */
}
