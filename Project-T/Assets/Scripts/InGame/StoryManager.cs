using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : SingletonBehaviour<StoryManager>
{
    private Dictionary<string, Dictionary<string, object>> storyLoadTable;
    private Dictionary<string, Dictionary<string, object>> storyTable;
    private Dictionary<string, Dictionary<string, object>> choiceTable;
    private Dictionary<string, Dictionary<string, object>> itemTable;
    private Dictionary<string, Dictionary<string, object>> resourceTable;

    private List<Dictionary<string, object>> outputChoices;
    public string curStoryId { get; set; }
    protected override void Awake()
    {

    }
        public void StoryManagerInit(string storyPath)
    {
        curStoryId = "Story_0000";
        storyLoadTable = CSVReader.Read(storyPath + "/Story_load");
        storyTable = CSVReader.Read(storyPath + "/Story");
        choiceTable = CSVReader.Read(storyPath + "/Choice");
        resourceTable = CSVReader.Read(storyPath + "/Resource");
    }

    public void StoryUpdate()
    {
        Dictionary<string, object> curStory = storyTable[curStoryId];
        Dictionary<string, object> curStoryLoad = storyLoadTable[curStoryId];
        Dictionary<string, object> curResource = resourceTable[curStoryId];

        string curStoryText = curStory["story"].ToString().Replace("\\c", ",").Replace("\\n", "\n").Replace("\\q", "\"");
        Dictionary<string, object>[] curChoices = GetChoices(curStoryLoad["choice_group_ID"].ToString());
        InventoryManager.Instance.ItemChange(curStoryLoad);

        outputChoices = new List<Dictionary<string, object>>();

        //불만족 히든 걸러내기
        for (int i = 0; i < curChoices.Length; i++)
        {
            string choiceType = (string)curChoices[i]["Choice_type"];
            if (string.Equals(choiceType, "Hidden"))
            {
                if (InventoryManager.Instance.IsCondition(curChoices[i]["hidden_Choice_condition_type"].ToString(), 
                    curChoices[i]["hidden_Choice_condition_Standard"].ToString(), curChoices[i]["hidden_Choice_condition_value"].ToString()))
                {
                    outputChoices.Add(curChoices[i]);
                }
            }
            else
            {
                outputChoices.Add(curChoices[i]);
            }

        }

        PlayUI.Instance.GenerateStoryPrepare(curResource, curStoryText, outputChoices);
    }

    private Dictionary<string, object>[] GetChoices(string choiceID)
    {
        int countChoice = 0;
        for (int i = 1; i <= 4; i++)
        {
            if (choiceTable.ContainsKey(choiceID + i))
            {
                countChoice++;
            }
        }

        Dictionary<string, object>[] Choices = new Dictionary<string, object>[countChoice];
        for (int i = 1; i <= countChoice; i++)
        {

            Choices[i - 1] = choiceTable[choiceID + i];
        }

        return Choices;
    }

    public void SelectChoice(int btnNum)
    {

        bool isCondition1 = InventoryManager.Instance.IsCondition((string)outputChoices[btnNum]["condition1_type"], (string)outputChoices[btnNum]["condition1_standard"], (string)outputChoices[btnNum]["condition1_value"]);
        bool isCondition2 = InventoryManager.Instance.IsCondition((string)outputChoices[btnNum]["condition2_type"], (string)outputChoices[btnNum]["condition2_standard"], (string)outputChoices[btnNum]["condition2_value"]);

        if (isCondition1)
        {
            if (isCondition2)
            {
                curStoryId = ResultDecode(outputChoices[btnNum]["And_result"].ToString());
            }
            else
            {
                curStoryId = ResultDecode(outputChoices[btnNum]["Condition1_result"].ToString());
            }
        }
        else
        {
            if (isCondition2)
            {
                curStoryId = ResultDecode(outputChoices[btnNum]["Condition2_result"].ToString());
            }
            else
            {
                curStoryId = ResultDecode(outputChoices[btnNum]["fail_result"].ToString());
            }
        }

        StoryUpdate();
    }

    public string ResultDecode(string resultText)
    {
        string[] resultSplit = resultText.Split("_");

        int randomNum = Random.Range(0, 100);

        for (int i = 0; i < resultText.Length / 2; i++)
        {
            randomNum -= int.Parse(resultSplit[i * 2 + 1].Substring(1));
            if (randomNum < 0)
            {
                return resultSplit[i * 2].Replace("R", "Story_");
            }
        }
        return "오류";
    }

}
