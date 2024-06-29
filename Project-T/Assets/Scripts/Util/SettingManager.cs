using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SettingData
{
    public int MASTER_SOUND;
    public int BGM_SOUND;
    public int SFX_SOUND;
    public int VOCIE_SOUND;

    // 설정 값 세이브
    
    public SettingData()
    {
        MASTER_SOUND = 50;
        BGM_SOUND = 50;
        SFX_SOUND = 50;
        VOCIE_SOUND = 50;
    }

    public void SetStting(string a, int b)
    {
        switch (a)
        {
            case "MASTER_SOUND":
                MASTER_SOUND = b;
                break;
            case "BGM_SOUND":
                BGM_SOUND = b;
                break; 
            case "SFX_SOUND":
                SFX_SOUND = b;
                break;
            case "VOCIE_SOUND":
                VOCIE_SOUND = b;
                break;
        }
    }

    public int GetSetting(string a)
    {
        switch (a)
        {
            case "MASTER_SOUND":
                return MASTER_SOUND;
            case "BGM_SOUND":
                return BGM_SOUND;
            case "SFX_SOUND":
                return SFX_SOUND;
            case "VOCIE_SOUND":
                return VOCIE_SOUND;
        }
        return 0;
    }
}

public class SettingManager : SingleTon<SettingManager>
{
    public const string MASTER_SOUND = "MASTER_SOUND";
    public const string BGM_SOUND = "BGM_SOUND";
    public const string SFX_SOUND = "SFX_SOUND";
    public const string VOCIE_SOUND = "VOCIE_SOUND";

    private SettingData settingData = new SettingData();
    private FileStream settingFileR;
    private FileStream settingFileW;

    
    private string SETTING_FILENAME = "SettingFile.save"; // 파일 이름
    private string SETTING_PATH;

    public SettingManager() {
        SETTING_PATH = Path.Combine(Application.persistentDataPath, SETTING_FILENAME);

        ReadSettingFile();
    }

    public void WriteSettingFile() {
        BinaryFormatter formatter = new BinaryFormatter();
        settingFileW = new FileStream(SETTING_PATH, FileMode.Create);

        formatter.Serialize(settingFileW, settingData);
        settingFileW.Close();

        DebugManager.Instance.PrintDebug("셋팅 파일 저장 종료");
        DebugManager.Instance.PrintDrawLine();
    }


    public void ReadSettingFile() {

        if (File.Exists(SETTING_PATH))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            settingFileR = new FileStream(SETTING_PATH, FileMode.Open);

            settingData = formatter.Deserialize(settingFileR) as SettingData;

            settingFileR.Close();

            DebugManager.Instance.PrintDebug("셋팅 파일 로드 완료");
            DebugManager.Instance.PrintDrawLine();
        }

    }


    public int GetSettingValue(string target) {
        return settingData.GetSetting(target);
        
    }
    public void SetSettingValue(string target, int value)
    {
        settingData.SetStting(target, value);
    }

}
