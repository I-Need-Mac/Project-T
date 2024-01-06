using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

using Button = UnityEngine.UIElements.Button;

public class UIManager : SingleTon<UIManager>
{
    
    //public GameObject optionArea;
    //public TMP_Text[] choiceText;
    //public Button[] choiceBtn;
    //public int choiceCount;
    //public RectTransform optionRectTran;

    public float EmptyContent;
    public GameObject storyContent;

    public ScrollRect scrollRect;

    public TMP_Text TextLengthContent;

    public float scrollDuration = 0.3f;

    public float contentHeight = 0;

    private VisualElement _bottomList;
    private VisualElement _handleContainer;
    private VisualElement _buttonList;

    private VisualElement _sidemenuBtn;
    private VisualElement _sidemenuBg;
    private VisualElement _sidemenu;
    private VisualElement _sidemenuCloseBtn;

    private VisualElement _inventoryBtn;
    private VisualElement _inventoryContainer;
    private VisualElement _inventoryCloseBtn;

    private VisualElement[] _line = new VisualElement[4];
    private Button[] _choiceBtn = new Button[4];

    public Transform InventoryCanvas;
    public Transform InventoryContent;

    //추가


    public UIManager()
    {
        //optionArea = GameObject.Find("OptionArea");
        //choiceText = new TMP_Text[4];
        //choiceBtn = new Button[4];
        //optionRectTran = optionArea.GetComponent<RectTransform>();
        //choiceText[0] = GameObject.Find("ChoiceText_1").GetComponent<TMP_Text>();
        //choiceText[1] = GameObject.Find("ChoiceText_2").GetComponent<TMP_Text>();
        //choiceText[2] = GameObject.Find("ChoiceText_3").GetComponent<TMP_Text>();
        //choiceText[3] = GameObject.Find("ChoiceText_4").GetComponent<TMP_Text>();

        //choiceBtn[0] = GameObject.Find("Choice_1").GetComponent<Button>();
        //choiceBtn[1] = GameObject.Find("Choice_2").GetComponent<Button>();
        //choiceBtn[2] = GameObject.Find("Choice_3").GetComponent<Button>();
        //choiceBtn[3] = GameObject.Find("Choice_4").GetComponent<Button>();

        storyContent = GameObject.Find("Content");
        scrollRect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
        EmptyContent = GameObject.Find("EmptyContent").GetComponent<RectTransform>().sizeDelta.y;
        TextLengthContent = GameObject.Find("LengthText").GetComponent<TMP_Text>();

        InventoryCanvas = GameObject.Find("CanvasContainer").transform.Find("Inventory Canvas");
        InventoryContent = InventoryCanvas.GetChild(1).GetChild(0).GetChild(0).GetChild(0);

        InventoryCanvas.gameObject.SetActive(false);

        var root = GameObject.Find("UIDocument").GetComponent<UIDocument>().rootVisualElement;

        _bottomList = root.Q<VisualElement>("bottom-list");
        _handleContainer = root.Q<VisualElement>("handle-container");
        _buttonList = root.Q<VisualElement>("button-list");

        _line[1] = root.Q<VisualElement>("line-1");
        _line[2] = root.Q<VisualElement>("line-2");
        _line[3] = root.Q<VisualElement>("line-3");

        _choiceBtn[0] = root.Q<Button>("choice-0");
        _choiceBtn[1] = root.Q<Button>("choice-1");
        _choiceBtn[2] = root.Q<Button>("choice-2");
        _choiceBtn[3] = root.Q<Button>("choice-3");

        _sidemenuBtn = root.Q<VisualElement>("sidemenu-btn");
        _sidemenuBg = root.Q<VisualElement>("menu-background");
        _sidemenu = root.Q<VisualElement>("sidemenu");
        _sidemenuCloseBtn = root.Q<VisualElement>("sidemenu-close");

        _inventoryBtn = root.Q<VisualElement>("inventory-btn");
        _inventoryContainer = root.Q<VisualElement>("inventory-container");
        _inventoryCloseBtn = root.Q<VisualElement>("inventory-close-btn");

        _handleContainer.RegisterCallback<ClickEvent>(OnOffBottom);

        _sidemenuBtn.RegisterCallback<ClickEvent>(OnSideMenu);
        _sidemenuCloseBtn.RegisterCallback<ClickEvent>(OffSideMenu);

        _inventoryBtn.RegisterCallback<ClickEvent>(OnInventoryMenu);
        _inventoryCloseBtn.RegisterCallback<ClickEvent>(OffInventoryMenu);

    }
    

    public void StoryStateSetting()
    {
        _bottomList.RemoveFromClassList("bottomlist--up");
        _bottomList.style.display = DisplayStyle.None;
    }
    public void ChoiceStateSetting()
    {
        _bottomList.style.display = DisplayStyle.Flex;
        _bottomList.AddToClassList("bottomlist--up");
    }

    public void OnOffBottom(ClickEvent evt)
    {
        _bottomList.ToggleInClassList("bottomlist--up");
    }

    public void OnSideMenu(ClickEvent evt)
    {
        _sidemenuBg.AddToClassList("container--open");
        _sidemenu.AddToClassList("sidemenu--open");
    }

    public void OffSideMenu(ClickEvent evt)
    {
        _sidemenuBg.RemoveFromClassList("container--open");
        _sidemenu.RemoveFromClassList("sidemenu--open");
    }
    public void OnInventoryMenu(ClickEvent evt)
    {
        _inventoryContainer.AddToClassList("itemcontainer--open");
        InventoryCanvas.gameObject.SetActive(true);
    }

    public void OffInventoryMenu(ClickEvent evt)
    {
        _inventoryContainer.RemoveFromClassList("itemcontainer--open");
        InventoryCanvas.gameObject.SetActive(false);
    }

    public void ChoiceChange(List<Dictionary<string, object>> choices) //선택지 관리
    {
        StoryStateSetting();
        //모든 버튼 끄기
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                _choiceBtn[i].style.display = DisplayStyle.None;
            }
            else
            {
                _line[i].style.display = DisplayStyle.None;
                _choiceBtn[i].style.display = DisplayStyle.None;
            }
        }

        int choiceCount = 0;

        foreach (Dictionary<string, object> choice in choices)
        {
            //딤드 처리
            string choiceType = (string)choice["Choice_type"];
            if (string.Equals(choiceType, "Dimmed"))
            {
                if (InventoryManager.Instance.IsCondition((string)choice["hidden_Choice_condition_type"], (string)choice["hidden_Choice_condition_Standard"], (string)choice["hidden_Choice_condition_value"]))
                {
                    _choiceBtn[choiceCount].RemoveFromClassList("btn-choice-dimmed");
                }
                else
                {
                    _choiceBtn[choiceCount].AddToClassList("btn-choice-dimmed");
                }
            }
            else
            {
                _choiceBtn[choiceCount].RemoveFromClassList("btn-choice-dimmed");
            }

            //글자 넣기
            _choiceBtn[choiceCount].text = (string)choice["choice_text_ID"];

            //버튼 켜기 + 라인 포함
            if (choiceCount == 0)
            {
                _choiceBtn[choiceCount].style.display = DisplayStyle.Flex;
            }
            else
            {
                _line[choiceCount].style.display = DisplayStyle.Flex;
                _choiceBtn[choiceCount].style.display = DisplayStyle.Flex;
            }

            choiceCount++;
        }
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
    public void RebuildLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRect.content);
    }
    public void SetTextLength(string text)
    {
        TextLengthContent.text = text;
    }


    /*
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
    
   private VisualElement _bottomList;
   private VisualElement _handleContainer;
   private VisualElement _buttonList;

   private VisualElement[] _line = new VisualElement[4];
   private Button[] _choiceBtn = new Button[4];

   public UIManager()
   {
       var root = GameObject.Find("UIDocument").GetComponent<UIDocument>().rootVisualElement;

       _bottomList = root.Q<VisualElement>("bottom-list");
       _handleContainer = root.Q<VisualElement>("handle-container");
       _buttonList = root.Q<VisualElement>("button-list");

       _line[1] = root.Q<VisualElement>("line-1");
       _line[2] = root.Q<VisualElement>("line-2");
       _line[3] = root.Q<VisualElement>("line-3");

       _choiceBtn[0] = root.Q<Button>("choice-0");
       _choiceBtn[1] = root.Q<Button>("choice-1");
       _choiceBtn[2] = root.Q<Button>("choice-2");
       _choiceBtn[3] = root.Q<Button>("choice-3");

       _buttonList.style.display = DisplayStyle.None;
       _handleContainer.RegisterCallback<ClickEvent>(OpenBottom);
   }

   public void StoryStateSetting()
   {

   }
   public void ChoiceStateSetting()
   {

   }

   public void HideBottom(ClickEvent evt) {
       _buttonList.style.display = DisplayStyle.None;
       _handleContainer.RegisterCallback<ClickEvent>(OpenBottom);
   }

   public void OpenBottom(ClickEvent evt)
   {
       _buttonList.style.display = DisplayStyle.Flex;
       _handleContainer.RegisterCallback<ClickEvent>(HideBottom);
   }
   */
}

