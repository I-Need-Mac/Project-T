using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private VisualElement _bottomList;
    private VisualElement _handleContainer;
    private VisualElement _buttonList;

    private VisualElement[] _line = new VisualElement[4];
    private Button[] _choiceBtn = new Button[4];

    private void Start()
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

        _handleContainer.RegisterCallback<ClickEvent>(OnOffBottom);

    }
    public void OnOffBottom(ClickEvent evt)
    {
        _bottomList.ToggleInClassList("bottomlist--up");
    }

    public void StoryStateSetting()
    {

    }
    public void ChoiceStateSetting()
    {

    }
    public void ChoiceChange(List<Dictionary<string, object>> choices) //선택지 관리
    {
        //모든 버튼 끄기
        for(int i = 0; i < 4; i++)
        {
            if(i == 0)
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
                if(InventoryManager.Instance.IsCondition((string)choice["hidden_Choice_condition_type"], (string)choice["hidden_Choice_condition_Standard"], (string)choice["hidden_Choice_condition_value"]))
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

        _bottomList.AddToClassList("bottomlist--up");
    }


}
