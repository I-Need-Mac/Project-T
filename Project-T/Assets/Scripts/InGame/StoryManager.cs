using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : SingleTon<StoryManager>
{
    private Dictionary<string, Dictionary<string, object>> story_load;
    private Dictionary<string, Dictionary<string, object>> story;
    private Dictionary<string, Dictionary<string, object>> choice;
    private Dictionary<string, Dictionary<string, object>> item;
    private Dictionary<string, Dictionary<string, object>> resource;

    public StoryManager()
    {
        SetStoryManager();
    }

    public void SetStoryManager() {
        SetCSVRead();
    }

    public void SetCSVRead()
    {
        story_load = CSVReader.Read("Story_load");
        DebugManager.Instance.PrintDebug("story_load");

        story = CSVReader.Read("Story");
        DebugManager.Instance.PrintDebug("story");

        choice = CSVReader.Read("Choice");
        DebugManager.Instance.PrintDebug("choice");

        item = CSVReader.Read("Item_Definition");
        DebugManager.Instance.PrintDebug("Item_Definition");

        resource = CSVReader.Read("Resource");
        DebugManager.Instance.PrintDebug("Resource");

    }

    public Dictionary<string, object> GetStory_load(string storyID)
    {
        return story_load[storyID];
    }

    public Dictionary<string, object> GetStory(string storyID)
    {
        return story[storyID];
    }

    public Dictionary<string, object>[] GetChoice(string choiceID)
    {
        int countChoice = 0;
        for(int i = 1; i <= 4; i++)
        {
            if(choice.ContainsKey(choiceID + i))
            {
                countChoice++;
            }
        }

        Dictionary<string, object>[] Choices = new Dictionary<string, object>[countChoice];
        for(int i = 1; i <= countChoice; i++)
        {
            Choices[i - 1] = choice[choiceID + i];
        }

        return Choices;
    }

    public Dictionary<string, object> GetItem(string itemID)
    {
        return item[itemID];
    }

    public Dictionary<string, object> GetResource(string storyID)
    {
        return resource[storyID];
    }
}
