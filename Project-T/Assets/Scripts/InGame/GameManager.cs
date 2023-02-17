using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text StoryText;
    public TMP_Text Choice_1;
    public TMP_Text Choice_2;
    public TMP_Text Choice_3;
    public TMP_Text Choice_4;

    // Start is called before the first frame update
    void Start()
    {
        Dictionary<string, Dictionary<string, object>> story_load = CSVReader.Read("Story_load");
        DebugManager.Instance.PrintDebug("story_load");

        Dictionary<string, Dictionary<string, object>> story = CSVReader.Read("Story");
        DebugManager.Instance.PrintDebug("story");
        //Dictionary<string, Dictionary<string, object>> choice = CSVReader.Read("Choice");

        DebugManager.Instance.PrintDebug(story_load["RYTA1001"]["choice_group_ID"]);

        DebugManager.Instance.PrintDebug(story);

        DebugManager.Instance.PrintDebug((string)(story["RYTA1001"]["story"]));

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StoryChange(string story)
    {
        StoryText.text = story;
    }
    public void choiceChange(string story)
    {
        //오류 있어서 좀 물어봐야함.
        StoryText.text = story;
    }
}
