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
        _settingBtn.RegisterCallback<ClickEvent>(OpenSetting);

        SoundManager.Instance.Init();
        SoundManager.Instance.Clear();

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

    private void Start()
    {
        SoundSetting();
    }

    public void SoundSetting()
    {
        int value = SettingManager.Instance.GetSettingValue(SettingManager.MASTER_SOUND);
        
        if(value != -1)
        {
            SoundManager.Instance.SetVolume("Master", value);
        }
        else
        {
            SoundManager.Instance.SetVolume("Master", 10);
            SettingManager.Instance.SetSettingValue(SettingManager.MASTER_SOUND, 10);
        }

        value = SettingManager.Instance.GetSettingValue(SettingManager.BGM_SOUND);

        if (value != -1)
        {
            SoundManager.Instance.SetVolume("BGM", value);
        }
        else
        {
            SoundManager.Instance.SetVolume("BGM", 10);
            SettingManager.Instance.SetSettingValue(SettingManager.BGM_SOUND, 10);
        }

        value = SettingManager.Instance.GetSettingValue(SettingManager.SFX_SOUND);

        if (value != -1)
        {
            SoundManager.Instance.SetVolume("SFX", value);
        }
        else
        {
            SoundManager.Instance.SetVolume("SFX", 10);
            SettingManager.Instance.SetSettingValue(SettingManager.SFX_SOUND, 10);
        }

        SettingManager.Instance.WriteSettingFile();
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
        Instantiate(ResourcesManager.Load<GameObject>(Define.UI_PATH + "SettingUI"));
    }
}
