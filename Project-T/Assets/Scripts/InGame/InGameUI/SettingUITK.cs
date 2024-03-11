using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using Button = UnityEngine.UIElements.Button;

using UnityEngine.SceneManagement;

public class SettingUITK : MonoBehaviour
{
    public Button[] _minusBtn = new Button[3];
    public Label[] _volumeLabel = new Label[3];
    public Button[] _plusBtn = new Button[3];

    public TextField _cheatInput;
    public Button _cheatBtn;

    public TextField _cheatItemInput;
    public TextField _cheatAmountInput;
    public Button _cheatItemBtn;

    public Button _closebtn;

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _minusBtn[0] = root.Q<Button>("masterM");
        _volumeLabel[0] = root.Q<Label>("masterLabel");
        _plusBtn[0] = root.Q<Button>("masterP");

        _minusBtn[1] = root.Q<Button>("BGMM");
        _volumeLabel[1] = root.Q<Label>("BGMLabel");
        _plusBtn[1] = root.Q<Button>("BGMP");

        _minusBtn[2] = root.Q<Button>("SFXM");
        _volumeLabel[2] = root.Q<Label>("SFXLabel");
        _plusBtn[2] = root.Q<Button>("SFXP");

        _closebtn = root.Q<Button>("closeSetting");

        _minusBtn[0].RegisterCallback<ClickEvent>(MasterDown);
        _plusBtn[0].RegisterCallback<ClickEvent>(MasterUp);

        _minusBtn[1].RegisterCallback<ClickEvent>(BGMDown);
        _plusBtn[1].RegisterCallback<ClickEvent>(BGMUp);

        _minusBtn[2].RegisterCallback<ClickEvent>(SFXDown);
        _plusBtn[2].RegisterCallback<ClickEvent>(SFXUp);

        _closebtn.RegisterCallback<ClickEvent>(CloseSetting);

        SetVolumeSetting();

        _cheatInput = root.Q<TextField>("cheatInput");
        _cheatBtn = root.Q<Button>("cheatBtn");

        _cheatItemInput = root.Q<TextField>("cheatItem");
        _cheatAmountInput = root.Q<TextField>("cheatAmount");
        _cheatItemBtn = root.Q<Button>("cheatItemBtn");

        _cheatBtn.RegisterCallback<ClickEvent>(GotoCheat);
        _cheatItemBtn.RegisterCallback<ClickEvent>(ItemCheat);
    }

    public void GotoCheat(ClickEvent evt)
    {
        if (_cheatInput.value.Contains("Story_"))
        {
            SaveLoadManager.Instance.cheatData(_cheatInput.value);
            SceneManager.LoadScene("Loading");
        }
    }

    public void ItemCheat(ClickEvent evt)
    {
        if (FindObjectOfType<GameManager>() != null)
        {
            InventoryManager.Instance.Add(_cheatItemInput.value, _cheatAmountInput.value);
            InventoryManager.Instance.SettingMoney();
        }
    }

    public void CloseSetting(ClickEvent evt)
    {
        SettingManager.Instance.WriteSettingFile();
        Destroy(gameObject);
    }

    public void SetVolumeSetting()
    {
        int[] value = new int[3];
        value[0] = SettingManager.Instance.GetSettingValue(SettingManager.MASTER_SOUND);
        value[1] = SettingManager.Instance.GetSettingValue(SettingManager.BGM_SOUND);
        value[2] = SettingManager.Instance.GetSettingValue(SettingManager.SFX_SOUND);

        for (int i = 0; i < 3; i++)
        {
            _volumeLabel[i].text = value[i].ToString();
            if (value[i] == 0)
            {
                _minusBtn[i].AddToClassList("minusBtnDisable");
                _plusBtn[i].RemoveFromClassList("plusBtnDisable");
            }
            else if (value[i] == 10)
            {
                _minusBtn[i].RemoveFromClassList("minusBtnDisable");
                _plusBtn[i].AddToClassList("plusBtnDisable");
            }
            else
            {
                _minusBtn[i].RemoveFromClassList("minusBtnDisable");
                _plusBtn[i].RemoveFromClassList("plusBtnDisable");
            }
        }
        
    }
    public void MasterDown(ClickEvent evt)
    {
        if(!string.Equals(_volumeLabel[0].text, "0"))
        {
            int i = int.Parse(_volumeLabel[0].text) - 1;
            _volumeLabel[0].text = i.ToString();
            SoundManager.Instance.SetVolume("Master", i);
            SettingManager.Instance.SetSettingValue(SettingManager.MASTER_SOUND, i);

            if(i == 0)
            {
                _minusBtn[0].AddToClassList("minusBtnDisable");
            }
            if (i == 9)
            {
                _plusBtn[0].RemoveFromClassList("plusBtnDisable");
            }
        } 
    }
    public void MasterUp(ClickEvent evt)
    {
        if (!string.Equals(_volumeLabel[0].text, "10"))
        {
            int i = int.Parse(_volumeLabel[0].text) + 1;
            _volumeLabel[0].text = i.ToString();
            SoundManager.Instance.SetVolume("Master", i);
            SettingManager.Instance.SetSettingValue(SettingManager.MASTER_SOUND, i);

            if (i == 10)
            {
                _plusBtn[0].AddToClassList("plusBtnDisable");
            }
            if (i == 1)
            {
                _minusBtn[0].RemoveFromClassList("minusBtnDisable");
            }
        }
    }

    public void BGMDown(ClickEvent evt)
    {
        if (!string.Equals(_volumeLabel[1].text, "0"))
        {
            int i = int.Parse(_volumeLabel[1].text) - 1;
            _volumeLabel[1].text = i.ToString();
            SoundManager.Instance.SetVolume("BGM", i);
            SettingManager.Instance.SetSettingValue(SettingManager.BGM_SOUND, i);

            if (i == 0)
            {
                _minusBtn[1].AddToClassList("minusBtnDisable");
            }
            if (i == 9)
            {
                _plusBtn[1].RemoveFromClassList("plusBtnDisable");
            }
        }
    }
    public void BGMUp(ClickEvent evt)
    {
        if (!string.Equals(_volumeLabel[1].text, "10"))
        {
            int i = int.Parse(_volumeLabel[1].text) + 1;
            _volumeLabel[1].text = i.ToString();
            SoundManager.Instance.SetVolume("BGM", i);
            SettingManager.Instance.SetSettingValue(SettingManager.BGM_SOUND, i);

            if (i == 10)
            {
                _plusBtn[1].AddToClassList("plusBtnDisable");
            }
            if (i == 1)
            {
                _minusBtn[1].RemoveFromClassList("minusBtnDisable");
            }
        }
    }

    public void SFXDown(ClickEvent evt)
    {
        if (!string.Equals(_volumeLabel[2].text, "0"))
        {
            int i = int.Parse(_volumeLabel[2].text) - 1;
            _volumeLabel[2].text = i.ToString();
            SoundManager.Instance.SetVolume("SFX", i);
            SettingManager.Instance.SetSettingValue(SettingManager.SFX_SOUND, i);

            if (i == 0)
            {
                _minusBtn[2].AddToClassList("minusBtnDisable");
            }
            if (i == 9)
            {
                _plusBtn[2].RemoveFromClassList("plusBtnDisable");
            }
        }
    }
    public void SFXUp(ClickEvent evt)
    {
        if (!string.Equals(_volumeLabel[2].text, "10"))
        {
            int i = int.Parse(_volumeLabel[2].text) + 1;
            _volumeLabel[2].text = i.ToString();
            SoundManager.Instance.SetVolume("SFX", i);
            SettingManager.Instance.SetSettingValue(SettingManager.SFX_SOUND, i);

            if (i == 10)
            {
                _plusBtn[2].AddToClassList("plusBtnDisable");
            }
            if (i == 1)
            {
                _minusBtn[2].RemoveFromClassList("minusBtnDisable");
            }
        }
    }

}
