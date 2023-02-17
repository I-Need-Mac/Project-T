using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingManager
{
    private Dictionary<string, int> _settings { get; set; }
    public Dictionary<string, int> settings
    {
        set
        {
            _settings = value;
        }

        get
        {
            return _settings ?? (_settings = new Dictionary<string, int>());
        }
    }

    private FileStream settingFileR;
    private FileStream settingFileW;
    private static SettingManager _instance { get; set; }
    public static SettingManager Instance
    {
        get
        {

            return _instance ?? (_instance = new SettingManager());
        }
    }

    public void WriteSettingFile()
    {
        settingFileW = new FileStream("./setting.txt", FileMode.Create);
        StreamWriter sw = new StreamWriter(settingFileW);
        DebugManager.Instance.PrintDrawLine();
        DebugManager.Instance.PrintDebug("���� ���� ����");
        foreach (KeyValuePair<string, int> item in settings)
        {
            sw.Write(item.Key + "=" + item.Value + "\n");
            DebugManager.Instance.PrintDebug("���� ���� �ۼ�", item.Key + " : " + item.Value);
        }
        sw.Close();
        DebugManager.Instance.PrintDebug("���� ���� ���� ����");
        DebugManager.Instance.PrintDrawLine();
    }


    public void ReadSettingFile()
    {
        settingFileR = new FileStream("./setting.txt", FileMode.Open);
        //settingFileR = new FileStream("./Assets/Resources/setting.txt", FileMode.Open);
        StreamReader sr = new StreamReader(settingFileR);

        DebugManager.Instance.PrintDrawLine();
        DebugManager.Instance.PrintDebug("���� ���� �ε�");

        string source = sr.ReadLine();
        string[] values;
        while (source != null)
        {
            values = source.Split('=');  // ��ǥ�� �����Ѵ�. ����ÿ� ��ǥ�� �����Ͽ� �����Ͽ���.           
            if (values.Length == 0)
            {
                sr.Close();
                return;
            }
            settings.Add(values[0], int.Parse(values[1]));
            source = sr.ReadLine();    // ���� �д´�.        
        }
        sr.Close();

        foreach (KeyValuePair<string, int> item in settings)
        {
            DebugManager.Instance.PrintDebug(item.Key, item.Value);
        }

        DebugManager.Instance.PrintDebug("���� ���� �ε� �Ϸ�");
        DebugManager.Instance.PrintDrawLine();
    }


    public int GetSettingValue(string target)
    {
        if (settings.TryGetValue(target, out int value))
        {
            DebugManager.Instance.PrintDrawLine();
            DebugManager.Instance.PrintDebug("Setting ���� �� ȣ��", target + " : " + value);
            DebugManager.Instance.PrintDrawLine();
            return value;
        }
        return -1;

    }
    public bool SetSettingValue(string target, int value)
    {
        if (settings[target] != null)
        {
            settings[target] = value;
            DebugManager.Instance.PrintDrawLine();
            DebugManager.Instance.PrintDebug("Setting ���� �� ����", target + " : " + value);
            DebugManager.Instance.PrintDrawLine();
            return true;
        }
        return false;

    }

}