using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Title : SingletonBehaviour<Title>
{
    public VisualElement _loadGameBtn;
    public VisualElement _newGameBtn;
    public VisualElement _storeBtn;
    public VisualElement _settingBtn;
    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _loadGameBtn = root.Q<VisualElement>("loadGameBtn");
        _newGameBtn = root.Q<VisualElement>("newGameBtn");
        _storeBtn = root.Q<VisualElement>("storeBtn;");
        _settingBtn = root.Q<VisualElement>("settingBtn");

        _loadGameBtn.RegisterCallback<ClickEvent>(LoadGame);
        _newGameBtn.RegisterCallback<ClickEvent>(NewGame);
        //_storeBtn.RegisterCallback<ClickEvent>(OpenStore);
        //_settingBtn.RegisterCallback<ClickEvent>(OpenSetting);

        if (SaveLoadManager.Instance.IsSaveData())
        {
            _loadGameBtn.style.display = DisplayStyle.Flex;
            _newGameBtn.RemoveFromClassList("titleTopBtn");
            _newGameBtn.AddToClassList("titleBtn");
            
        }
        else
        {
            _loadGameBtn.style.display = DisplayStyle.None;
            _newGameBtn.RemoveFromClassList("titleBtn");
            _newGameBtn.AddToClassList("titleTopBtn");
        }
    }

    public void LoadGame(ClickEvent evt)
    {
        SceneManager.LoadScene("Loading");
    }
    public void NewGame(ClickEvent evt)
    {
        SaveLoadManager.Instance.ResetData();
        SceneManager.LoadScene("Loading");
    }
    public void OpenStore(ClickEvent evt)
    {
        
    }
    public void OpenSetting(ClickEvent evt)
    {
        
    }
}
