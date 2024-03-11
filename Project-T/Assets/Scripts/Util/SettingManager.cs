using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingManager : SingleTon<SettingManager>
{
    public const string MASTER_SOUND = "MASTER_SOUND";
    public const string BGM_SOUND = "BGM_SOUND";
    public const string SFX_SOUND = "SFX_SOUND";
    public const string VOCIE_SOUND = "VOCIE_SOUND";

    private Dictionary<string, int> _settings { get; set; }
    public Dictionary<string, int> settings
    {
        set { 
            _settings = value;
        }

        get { 
            return _settings ??( _settings= new Dictionary<string, int>());
        }
    }

    private FileStream settingFileR;
    private FileStream settingFileW;

    public SettingManager() { 
        ReadSettingFile();
    }

    public void WriteSettingFile() {
        settingFileW = new FileStream("./Assets/Resources/setting.txt", FileMode.Create);
        StreamWriter sw = new StreamWriter(settingFileW);
        DebugManager.Instance.PrintDrawLine();
        DebugManager.Instance.PrintDebug("셋팅 파일 저장");
        foreach (KeyValuePair<string, int> item in settings)
        {
            sw.Write(item.Key + "=" + item.Value+"\n");
            DebugManager.Instance.PrintDebug("셋팅 파일 작성", item.Key+" : "+item.Value);
        }
        sw.Close();
        DebugManager.Instance.PrintDebug("셋팅 파일 저장 종료");
        DebugManager.Instance.PrintDrawLine();
    }


    public void ReadSettingFile() {
        settingFileR = new FileStream("./Assets/Resources/setting.txt", FileMode.Open);
        //settingFileR = new FileStream("./Assets/Resources/setting.txt", FileMode.Open);
        StreamReader sr = new StreamReader(settingFileR);

        DebugManager.Instance.PrintDrawLine();
        DebugManager.Instance.PrintDebug("셋팅 파일 로드");
        
        string source = sr.ReadLine();       
        string [] values;
        while (source != null)
        {
            values = source.Split('=');  // 쉼표로 구분한다. 저장시에 쉼표로 구분하여 저장하였다.           
            if( values.Length == 0 ){               
                sr.Close();                
                return;            
            }
            if (!settings.ContainsKey(values[0])) { 
                settings.Add(values[0],int.Parse(values[1]));
            }
            else {
                settings[values[0]]= int.Parse(values[1]);

            }
            source = sr.ReadLine();    // 한줄 읽는다.        
        }
        sr.Close();

        foreach(KeyValuePair<string,int> item in settings)
        {
            DebugManager.Instance.PrintDebug(item.Key,item.Value);
        }

        DebugManager.Instance.PrintDebug("셋팅 파일 로드 완료");
        DebugManager.Instance.PrintDrawLine();
    }


    public int GetSettingValue(string target) { 
        if(settings.TryGetValue(target, out int value)){
            DebugManager.Instance.PrintDrawLine();
            DebugManager.Instance.PrintDebug("Setting 파일 값 호출", target + " : " + value);
            DebugManager.Instance.PrintDrawLine();
            return value;
        }
        return -1;
        
    }
    public bool SetSettingValue(string target, int value)
    {
        if (settings[target]!= null)
        {
            settings[target] = value;
            DebugManager.Instance.PrintDrawLine();
            DebugManager.Instance.PrintDebug("Setting 파일 값 세팅", target + " : " + value);
            DebugManager.Instance.PrintDrawLine();

            WriteSettingFile();
            return true;
        }
        return false;

    }

}
