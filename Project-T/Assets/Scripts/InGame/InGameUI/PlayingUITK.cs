using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using Button = UnityEngine.UIElements.Button;

public class PlayingUITK : MonoBehaviour
{

    public VisualElement _bottomList;
    public VisualElement _handleContainer;
    public VisualElement _buttonList;

    public VisualElement _sidemenuBtn;
    public VisualElement _sidemenuBg;
    public VisualElement _sidemenu;
    public VisualElement _sidemenuCloseBtn;

    public VisualElement _inventoryBtn;
    public VisualElement _inventoryContainer;
    public VisualElement _inventoryCloseBtn;

    public Label _toast;
    public Label _money_1;
    public Label _money_2;

    public VisualElement[] _line = new VisualElement[4];
    public Button[] _choiceBtn = new Button[4];

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

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

        _toast = root.Q<Label>("toast");
        _money_1 = root.Q<Label>("money_1");
        _money_2 = root.Q<Label>("money_2");

        _handleContainer.RegisterCallback<ClickEvent>(OnOffBottom);

        _sidemenuBtn.RegisterCallback<ClickEvent>(OnSideMenu);
        _sidemenuCloseBtn.RegisterCallback<ClickEvent>(OffSideMenu);

        _inventoryBtn.RegisterCallback<ClickEvent>(OnInventoryMenu);
        _inventoryCloseBtn.RegisterCallback<ClickEvent>(OffInventoryMenu);
    }


    public IEnumerator PopToast(string toastmessage)
    {
        _toast.text = toastmessage;
        _toast.RemoveFromClassList("toastsetting--pop");
        yield return new WaitForSeconds(0.4f);
        _toast.AddToClassList("toastsetting--pop");
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
        PlayUI.Instance.OnInventoryUI();
    }

    public void OffInventoryMenu(ClickEvent evt)
    {
        _inventoryContainer.RemoveFromClassList("itemcontainer--open");
        PlayUI.Instance.OffInventoryUI();
    }


    #region 선택지 관리
    public void StoryStateSetting()
    {
        _bottomList.style.display = DisplayStyle.None;
        _bottomList.RemoveFromClassList("bottomlist--up");
    }
    public void ChoiceStateSetting()
    {
        _bottomList.style.display = DisplayStyle.Flex;
        _bottomList.AddToClassList("bottomlist--up");
    }

    public void SelectChoice1(ClickEvent evt)
    {
        StoryManager.Instance.SelectChoice(0);
    }
    public void SelectChoice2(ClickEvent evt)
    {
        StoryManager.Instance.SelectChoice(1);
    }
    public void SelectChoice3(ClickEvent evt)
    {
        StoryManager.Instance.SelectChoice(2);
    }
    public void SelectChoice4(ClickEvent evt)
    {
        StoryManager.Instance.SelectChoice(3);
    }

    public void ChoiceSetting(List<Dictionary<string, object>> choices) //선택지 관리
    {
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
                if (InventoryManager.Instance.IsCondition(choice["hidden_Choice_condition_type"].ToString(),
                    choice["hidden_Choice_condition_Standard"].ToString(), choice["hidden_Choice_condition_value"].ToString()))
                {
                    SelectChoiceInt(choiceCount);
                    _choiceBtn[choiceCount].RemoveFromClassList("btn-choice-dimmed");
                }
                else
                {
                    UnSelectChoiceInt(choiceCount);
                    _choiceBtn[choiceCount].AddToClassList("btn-choice-dimmed");
                }
            }
            else
            {
                SelectChoiceInt(choiceCount);
                _choiceBtn[choiceCount].RemoveFromClassList("btn-choice-dimmed");
            }

            //글자 넣기
            _choiceBtn[choiceCount].text = choice["choice_text_ID"].ToString().Replace("\\c", ",").Replace("\\n", "\n");

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

        ChoiceStateSetting();
    }

    public void SelectChoiceInt(int i)
    {
        switch (i)
        {
            case 0:
                _choiceBtn[0].RegisterCallback<ClickEvent>(SelectChoice1);
                break;
            case 1:
                _choiceBtn[1].RegisterCallback<ClickEvent>(SelectChoice2);
                break;
            case 2:
                _choiceBtn[2].RegisterCallback<ClickEvent>(SelectChoice3);
                break;
            case 3:
                _choiceBtn[3].RegisterCallback<ClickEvent>(SelectChoice4);
                break;
        }
    }

    public void UnSelectChoiceInt(int i)
    {
        switch (i)
        {
            case 0:
                _choiceBtn[0].UnregisterCallback<ClickEvent>(SelectChoice1);
                break;
            case 1:
                _choiceBtn[1].UnregisterCallback<ClickEvent>(SelectChoice2);
                break;
            case 2:
                _choiceBtn[2].UnregisterCallback<ClickEvent>(SelectChoice3);
                break;
            case 3:
                _choiceBtn[3].UnregisterCallback<ClickEvent>(SelectChoice4);
                break;
        }
    }
    #endregion

}
