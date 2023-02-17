using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizeManager : SingleTon<LocalizeManager>
{

    private string[] LANGUAGE = new string[] { "Korean", "English", "Japanese" };                                                           //언어 칼럼명
    private int langType;                                                                                                                   //언어 넘버링 0 한국어, 1 영어, 2 일본어
    private Dictionary<string, Dictionary<string, object>> localTableData = new Dictionary<string, Dictionary<string, object>>();           //로컬라이즈 ID 딕셔너리 아이디값으로 모든 데이터를 넣음

    public LocalizeManager()
    {                                                                                                              //로컬라이즈 매니저 생성
        SetLocalizeManager();                                                                                                               //로컬라이즈 기본 설정
    }
    public void SetLocalizeManager()
    {                                                                                                       //로컬라이스 셋팅 함수 (설정에서 언어 변경 시 해줘야함)
        SetLangType();
        GetLocalizeTable();
    }
    public void SetLangType()
    {                                                                                                             //설정파일에서 데이터를 읽어와서 기준 언어로 세팅
        langType = SettingManager.Instance.GetSettingValue("lang");
    }

    public void GetLocalizeTable()
    {                                                                                                        //로컬라이즈 데이터를 테이블에서 로드
        localTableData = CSVReader.Read("Localize");
    }

    public string GetText(string targetID)
    {                                                                                                //아이디로 데이터를 반환함
        return Convert.ToString(localTableData[targetID][LANGUAGE[langType]]);
    }
}
