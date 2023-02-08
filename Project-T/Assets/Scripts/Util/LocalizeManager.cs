using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizeManager : SingleTon<LocalizeManager>
{

    private string[] LANGUAGE = new string[] { "Korean", "English", "Japanese" };                                                           //��� Į����
    private int langType;                                                                                                                   //��� �ѹ��� 0 �ѱ���, 1 ����, 2 �Ϻ���
    private Dictionary<string, Dictionary<string, object>> localTableData = new Dictionary<string, Dictionary<string, object>>();           //���ö����� ID ��ųʸ� ���̵����� ��� �����͸� ����

    public LocalizeManager()
    {                                                                                                              //���ö����� �Ŵ��� ����
        SetLocalizeManager();                                                                                                               //���ö����� �⺻ ����
    }
    public void SetLocalizeManager()
    {                                                                                                       //���ö��̽� ���� �Լ� (�������� ��� ���� �� �������)
        SetLangType();
        GetLocalizeTable();
    }
    public void SetLangType()
    {                                                                                                             //�������Ͽ��� �����͸� �о�ͼ� ���� ���� ����
        langType = SettingManager.Instance.GetSettingValue("lang");
    }

    public void GetLocalizeTable()
    {                                                                                                        //���ö����� �����͸� ���̺��� �ε�
        localTableData = CSVReader.Read("Localize");
    }

    public string GetText(string targetID)
    {                                                                                                //���̵�� �����͸� ��ȯ��
        return Convert.ToString(localTableData[targetID][LANGUAGE[langType]]);
    }
}
