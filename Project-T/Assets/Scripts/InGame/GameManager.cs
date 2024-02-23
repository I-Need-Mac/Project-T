using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

using Image = UnityEngine.UI.Image;
using Button = UnityEngine.UIElements.Button;

public class GameManager : SingletonBehaviour<GameManager>
{
    public string gameFileName = "Test";
    public string gameStoryid = "Story_0000";

    private int timerInterval = 30;
    private int countdownTimer = 30;

    protected override void Awake()
    {
        StoryManager.Instance.SetStoryId(gameStoryid);
        StoryManager.Instance.StoryManagerInit(gameFileName);
        InventoryManager.Instance.SetItemList(new Dictionary<string, Item>());
        InventoryManager.Instance.InventoryManagerInit(gameFileName);

    }

    void Start()
    {
        if (SaveLoadManager.Instance.IsSaveData())
        {
            SaveLoadManager.Instance.LoadData();
        }

        StoryManager.Instance.StoryUpdate();
        StartCoroutine("Save30Sec");
    }

    IEnumerator Save30Sec()
    {
        while (true)
        {
            // 1초 대기
            yield return new WaitForSeconds(1f);

            // 타이머 감소
            countdownTimer -= 1;

            // UI 업데이트
            PlayUI.Instance.SetTimer(countdownTimer);

            // 시간이 0보다 작거나 같으면 30초로 재설정하고 저장 함수 호출
            if (countdownTimer <= 0)
            {
                countdownTimer = timerInterval;
                SaveLoadManager.Instance.SaveData();
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
