using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : SingleTon<UIManager>
{
    public GameObject optionArea;
    public float EmptyContent;

    public GameObject storyContent;
    public TMP_Text[] choiceText;
    public Button[] choiceBtn;

    public RectTransform optionRectTran;

    public ScrollRect scrollRect;

    public TMP_Text TextLengthContent;


    public int choiceCount;

    public float scrollDuration = 0.3f;

    public float contentHeight = 0;

    public UIManager()
    {
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

        scrollRect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
        optionRectTran = optionArea.GetComponent<RectTransform>();

        EmptyContent = GameObject.Find("EmptyContent").GetComponent<RectTransform>().sizeDelta.y;

        TextLengthContent = GameObject.Find("LengthText").GetComponent<TMP_Text>(); ;
    }
    
    public Vector2 ImageStoryChange(GameObject imageInstance, GameObject textInstance) //이미지, 텍스트 추가
    {
        //이미지 콘텐츠 설정
        imageInstance.transform.SetParent(storyContent.transform, false);
        imageInstance.transform.localScale = new Vector3(1, 1, 1);
        imageInstance.transform.SetSiblingIndex(imageInstance.transform.GetSiblingIndex() - 1);

        //텍스트 콘텐츠 설정
        textInstance.transform.SetParent(storyContent.transform, false);
        textInstance.transform.localScale = new Vector3(1, 1, 1);
        textInstance.transform.SetSiblingIndex(textInstance.transform.GetSiblingIndex() - 1);

        //계산 전 리빌드
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

        //스크롤 위치 반환값 계산
        float contentH = scrollRect.content.rect.height;
        float imageH = imageInstance.GetComponent<RectTransform>().rect.height;
        Vector2 scrollVector = new Vector2(1, 1 - ((contentH - imageH - EmptyContent) / (contentH - scrollRect.viewport.rect.size.y)));

        contentHeight = imageH + TextLengthContent.rectTransform.rect.height;

        return scrollVector;
    }
    public Vector2 StoryChange(GameObject textInstance) //텍스트 추가
    {
        //텍스트 콘텐츠 설정
        textInstance.transform.SetParent(storyContent.transform, false);
        textInstance.transform.localScale = new Vector3(1, 1, 1);
        textInstance.transform.SetSiblingIndex(textInstance.transform.GetSiblingIndex() - 1);

        //계산 전 리빌드
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

        //스크롤 위치 반환값 계산
        float contentH = scrollRect.content.rect.height;
        Vector2 scrollVector = new Vector2(1, 1 - ((contentH - EmptyContent) / (contentH - scrollRect.viewport.rect.size.y)));

        contentHeight = TextLengthContent.rectTransform.rect.height;

        return scrollVector;
    }

    public void ChoiceChange(List<Dictionary<string, object>> choices) //선택지 관리
    {
        optionRectTran.anchoredPosition = new Vector3(0, 0, 0);
        optionRectTran.sizeDelta = new Vector2(optionRectTran.sizeDelta.x, 0);

        choiceCount = 0;

        //초기화
        choiceBtn[0].interactable = false;
        choiceBtn[1].interactable = false;
        choiceBtn[2].interactable = false;
        choiceBtn[3].interactable = false;

        //딤드를 포함한 선택지 구조 설정
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
    }

    public void RebuildLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
    }

    public void SetTextLength(string text)
    {
        TextLengthContent.text = text;
    }

    public bool IsOver()
    {
        return (1164 - 110 * choiceCount) < contentHeight;
    }

    //public IEnumerator StartTyping(GameObject textInstance, string targetText) //타이핑 IEnumerator
    //{
    //    TextMeshProUGUI textComponent = textInstance.GetComponent<TextMeshProUGUI>();

    //    for (int i = 0; i <= targetText.Length; i++)
    //    {
    //        textComponent.text = targetText.Substring(0, i);
    //        yield return new WaitForSeconds(typingSpeed);
    //    }

    //    while (isScrolling)
    //    {
    //        yield return new WaitForSeconds(typingSpeed);
    //    }

    //    EndTyping(textInstance);
    //}

    //private void EndTyping(GameObject textInstance) //타이핑 종료 후 텍스트 높이 조절
    //{
    //    RectTransform textRectTransform = textInstance.GetComponent<RectTransform>();

    //    textRectTransform.sizeDelta = new Vector2(textRectTransform.sizeDelta.x, textInstance.GetComponent<TextMeshProUGUI>().preferredHeight);
    //    LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);

    //}

    //public void ScrollSmoothly(Vector2 targetNormalizedPosition) //스크롤 IEnumerator
    //{
    //    isScrolling = true;

    //    Vector2 startPosition = scrollRect.normalizedPosition;
    //    Vector2 targetPosition = targetNormalizedPosition;

    //    float elapsedTime = 0f;

    //    while (elapsedTime < scrollDuration)
    //    {
    //        float t = elapsedTime / scrollDuration; 
    //        scrollRect.normalizedPosition = Vector2.Lerp(startPosition, targetPosition, t);

    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }
        

    //    scrollRect.normalizedPosition = targetPosition;

    //    isScrolling = false;
    //}

    //public IEnumerator SmoothlyUP()
    //{
    //    float elapsedTime = 0f;

    //    Vector2 startPosition = optionRectTran.anchoredPosition;
    //    Vector3 targetPosition = new Vector3(0, 55 * choiceCount, 0);

    //    Vector2 startPosition2 = optionRectTran.sizeDelta;
    //    Vector2 targetPosition2 = new Vector2(optionRectTran.sizeDelta.x, 110 * choiceCount);

    //    while (elapsedTime < scrollDuration)
    //    {
    //        float t = elapsedTime / scrollDuration;
    //        optionRectTran.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
    //        optionRectTran.sizeDelta = Vector2.Lerp(startPosition2, targetPosition2, t);


    //        elapsedTime += Time.deltaTime;
    //        yield return null;
    //    }

    //    optionRectTran.anchoredPosition = new Vector3(0, 55 * choiceCount, 0);
    //    optionRectTran.sizeDelta = new Vector2(optionRectTran.sizeDelta.x, 110 * choiceCount);
    //}
}

