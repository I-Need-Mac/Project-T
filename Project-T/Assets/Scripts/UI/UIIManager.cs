using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : SingleTon<UIManager>
{

    GameObject storyArea;
    GameObject optionArea;

    GameObject storyContent;
    TMP_Text[] choiceText;
    Button[] choiceBtn;

    Button itemBtn;

    GameObject textContent;
    GameObject imageContent;

    RectTransform storyRectTran;
    RectTransform optionRectTran;

    ScrollRect scrollRect;

    float contentH;
    float preferredH;

    float typingSpeed = 0.05f;

    int choiceCount;

    private float scrollDuration = 0.3f;

    bool isScrolling = true;

    public UIManager()
    {
        storyArea = GameObject.Find("StoryArea");
        optionArea = GameObject.Find("OptionArea");

        storyContent = GameObject.Find("Content");

        choiceText = new TMP_Text[4];
        choiceBtn = new Button[4];

        choiceText[0] = GameObject.Find("ChoiceText_1").GetComponent<TMP_Text>();
        choiceText[1] = GameObject.Find("ChoiceText_2").GetComponent<TMP_Text>();
        choiceText[2] = GameObject.Find("ChoiceText_3").GetComponent<TMP_Text>();
        choiceText[3] = GameObject.Find("ChoiceText_4").GetComponent<TMP_Text>();

        choiceBtn[0] = GameObject.Find("Choice_1").GetComponent<Button>();
        choiceBtn[1] = GameObject.Find("Choice_2").GetComponent<Button>();
        choiceBtn[2] = GameObject.Find("Choice_3").GetComponent<Button>();
        choiceBtn[3] = GameObject.Find("Choice_4").GetComponent<Button>();

        itemBtn = GameObject.Find("ItemButton").GetComponent<Button>();

        scrollRect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
        storyRectTran = storyArea.GetComponent<RectTransform>();
        optionRectTran = optionArea.GetComponent<RectTransform>();

        textContent = Resources.Load<GameObject>("Prefabs/TextContent");
        imageContent = Resources.Load<GameObject>("Prefabs/ImageContent");

        contentH = 1200.0f;
    }

    public Vector2 ImageStoryChange(GameObject imageInstance, GameObject textInstance)
    {
        float curContentH = 0;
        imageInstance.transform.parent = storyContent.transform;
        imageInstance.transform.localScale = new Vector3(1, 1, 1);
        imageInstance.transform.SetSiblingIndex(imageInstance.transform.GetSiblingIndex() - 1);


        curContentH += imageInstance.GetComponent<RectTransform>().rect.height;

        textInstance.transform.parent = storyContent.transform;
        textInstance.transform.localScale = new Vector3(1, 1, 1);
        textInstance.transform.SetSiblingIndex(textInstance.transform.GetSiblingIndex() - 1);

        //RectTransform textRectTransform = textInstance.GetComponent<RectTransform>();

        //float preferredH = textInstance.GetComponent<TextMeshProUGUI>().preferredHeight;
        //textRectTransform.sizeDelta = new Vector2(textRectTransform.sizeDelta.x, preferredH);

        //curContentH += preferredH;

        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        
        Vector2 scrollVector = new Vector2(1, 1 - ((contentH - 1200f) / (contentH + curContentH - scrollRect.viewport.rect.size.y)));
        
        contentH += curContentH;

        return scrollVector;
    }
    public Vector2 StoryChange(GameObject textInstance)
    {
        textInstance.transform.parent = storyContent.transform;
        textInstance.transform.localScale = new Vector3(1, 1, 1);
        textInstance.transform.SetSiblingIndex(textInstance.transform.GetSiblingIndex() - 1);

        //RectTransform textRectTransform = textInstance.GetComponent<RectTransform>();
        //float preferredH = textInstance.GetComponent<TextMeshProUGUI>().preferredHeight;
        //textRectTransform.sizeDelta = new Vector2(textRectTransform.sizeDelta.x, preferredH);

        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
        Vector2 scrollVector = new Vector2(1, 1 - ((contentH - 1200f) / (contentH - scrollRect.viewport.rect.size.y)));

        return scrollVector;
    }

    public void ChoiceChange(List<Dictionary<string, object>> choices)
    {
        optionRectTran.anchoredPosition = new Vector3(0, 0, 0);
        optionRectTran.sizeDelta = new Vector2(optionRectTran.sizeDelta.x, 0);

        choiceCount = 0;

        // 나눠주는거 구현들어오는거 판단해서 
        foreach (Dictionary<string, object> choice in choices)
        {
            string choiceType = (string)choice["Choice_type"];
            if (string.Equals(choiceType, "Dimmed"))
            {
                choiceBtn[choiceCount].interactable = InventoryManager.Instance.IsCondition((string)choice["hidden_Choice_condition_type"], (string)choice["hidden_Choice_condition_Standard"], (string)choice["hidden_Choice_condition_value"]);
            }
            else
            {
                choiceBtn[choiceCount].interactable = true;

            }
            choiceText[choiceCount].text = (string)choice["choice_text_ID"];
            choiceCount++;
        }

        

        //optionRectTran.anchoredPosition = new Vector3(0, 55 * choiceCount, 0);
        //optionRectTran.sizeDelta = new Vector2(optionRectTran.sizeDelta.x, 110 * choiceCount);

        //storyRectTran.offsetMin = new Vector2(storyRectTran.offsetMin.x, 110 * count);

    }

    public void SelectItemBtn()
    {
        DebugManager.Instance.PrintDebug("실제 위치 값");
        DebugManager.Instance.PrintDebug(scrollRect.normalizedPosition);
    }

    public IEnumerator StartTyping(GameObject textInstance, string targetText)
    {
        TextMeshProUGUI textComponent = textInstance.GetComponent<TextMeshProUGUI>();

        for (int i = 0; i <= targetText.Length; i++)
        {
            textComponent.text = targetText.Substring(0, i);
            yield return new WaitForSeconds(typingSpeed);
        }

        while (isScrolling)
        {
            yield return new WaitForSeconds(typingSpeed);
        }
        EndTyping(textInstance);
    }

    private void EndTyping(GameObject textInstance)
    {
        RectTransform textRectTransform = textInstance.GetComponent<RectTransform>();

        preferredH = textInstance.GetComponent<TextMeshProUGUI>().preferredHeight;
        textRectTransform.sizeDelta = new Vector2(textRectTransform.sizeDelta.x, preferredH);
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

        contentH += preferredH;

        // 선택창 올리기

        optionRectTran.anchoredPosition = new Vector3(0, 55 * choiceCount, 0);
        optionRectTran.sizeDelta = new Vector2(optionRectTran.sizeDelta.x, 110 * choiceCount);

    }

    public IEnumerator ScrollSmoothly(Vector2 targetNormalizedPosition)
    {
        isScrolling = true;

        Vector2 startPosition = scrollRect.normalizedPosition;
        Vector2 targetPosition = targetNormalizedPosition;

        float elapsedTime = 0f;

        while (elapsedTime < scrollDuration)
        {
            float t = elapsedTime / scrollDuration; 
            scrollRect.normalizedPosition = Vector2.Lerp(startPosition, targetPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        

        scrollRect.normalizedPosition = targetPosition;

        isScrolling = false;
    }
}

