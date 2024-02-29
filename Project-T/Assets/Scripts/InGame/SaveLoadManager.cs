using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using BFM;

[System.Serializable] // 직렬화 해야 한 줄로 데이터들이 나열되어 저장 장치에 읽고 쓰기가 쉬워진다.
public class SaveData
{
    public string saveId;

    // 인벤토리 세이브.
    public List<string> saveInvenItemid = new List<string>();
    public List<int> saveInvenItemAmount = new List<int>();

    public string saveBGM;

    // 설정 값 세이브
    
}
public class SaveLoadManager : SingleTon<SaveLoadManager>
{
    private SaveData saveData = new SaveData();

    private string SAVE_DATA_DIRECTORY;  // 저장할 폴더 경로
    private string SAVE_FILENAME = "/SaveFile.txt"; // 파일 이름


    public SaveLoadManager()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + @"\SAVE\";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY)) // 해당 경로가 존재하지 않는다면
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); // 폴더 생성(경로 생성)
    }

    public void SaveData()
    {
        saveData.saveId = StoryManager.Instance.curStoryId;

        saveData.saveInvenItemid = new List<string>();
        saveData.saveInvenItemAmount = new List<int>();

        Dictionary<string, Item> itemList = InventoryManager.Instance.GetItemList();
        foreach (var entry in itemList)
        {
                saveData.saveInvenItemid.Add(entry.Key);
                saveData.saveInvenItemAmount.Add(entry.Value.Data.amount);
        }

        saveData.saveBGM = SoundManager.Instance.GetCurentBGM();

        // 최종 전체 저장
        string json = JsonUtility.ToJson(saveData); // 제이슨화

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

        Debug.Log("저장 완료");
        Debug.Log(json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            // 전체 읽어오기
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            StoryManager.Instance.curStoryId = saveData.saveId;


            // 인벤토리 로드
            for (int i = 0; i < saveData.saveInvenItemid.Count; i++)
                InventoryManager.Instance.LoadItemList(saveData.saveInvenItemid[i], saveData.saveInvenItemAmount[i]);

            SoundManager.Instance.Play(saveData.saveBGM, Define.Sound.Bgm);
            SoundManager.Instance.SetCurentBGM(saveData.saveBGM);

            Debug.Log("로드 완료");
        }
        else
            Debug.Log("세이브 파일이 없습니다.");
    }
    
    public bool IsSaveData()
    {
        return File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
    }
    public void ResetData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            File.Delete(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
        }
    }
}
