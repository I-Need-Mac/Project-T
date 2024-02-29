using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

using Image = UnityEngine.UI.Image;

public class PlayUI : SingletonBehaviour<PlayUI>
{
    private string storyPath;

    private StoryUI storyUi;
    private InventoryUI inventoryUi;
    private PlayingUITK playingUiTK;

    private Dictionary<string, object> curResourseTable;
    private string curStoryText;
    List<Dictionary<string, object>> curChoiceList;

    private float typingSpeed = 0.03f;
    public bool isTypingSkip = false;

    protected override void Awake()
    {
        storyUi = GetComponentInChildren<StoryUI>();
        inventoryUi = GetComponentInChildren<InventoryUI>();
        playingUiTK = GetComponentInChildren<PlayingUITK>();
    }
        // Start is called before the first frame update
    void Start()
    {
        OffInventoryUI();
        //Instantiate(ResourcesManager.Load<GameObject>(GAMEOVER_UI_PATH + "TextContent"), storyUi.scrollRect.content);
    }

    public void SetTimer(int timer)
    {
        playingUiTK.TimerSetting(timer);
    }

    public void SetStoryName(string storyPath)
    {
        this.storyPath = storyPath;
    }

    #region 스토리 조절
    public void GenerateStoryPrepare(Dictionary<string, object> resourseTable, string storyText, List<Dictionary<string, object>> ChoiceList)
    {
        curResourseTable = resourseTable;
        curStoryText = storyText;
        curChoiceList = ChoiceList;

        playingUiTK.StoryStateSetting();

        LayoutRebuilder.ForceRebuildLayoutImmediate(storyUi.scrollRect.content);

        storyUi.ScrollToBottomSlowly();
    }
    public void SetMoney(string money)
    {
        playingUiTK._money_1.text = playingUiTK._money_2.text = money;
    }
    public void GenerateStory()
    {
        if (!string.Equals(curResourseTable["BGM"].ToString(), ""))
        {
            SoundManager.Instance.Play(Define.SOUND_PATH + storyPath + "/" + curResourseTable["BGM"].ToString(), Define.Sound.Bgm);
            SoundManager.Instance.SetCurentBGM(Define.SOUND_PATH + storyPath + "/" + curResourseTable["BGM"].ToString());
        }
        if (!string.Equals(curResourseTable["Voice"].ToString(), ""))
        {
            SoundManager.Instance.Play(Define.SOUND_PATH + storyPath + "/" + curResourseTable["Voice"].ToString(), Define.Sound.Voice);
        }
        if (!string.Equals(curResourseTable["SFX"].ToString(), ""))
        {
            SoundManager.Instance.Play(Define.SOUND_PATH + storyPath + "/" + curResourseTable["SFX"].ToString(), Define.Sound.SFX);
        }
        if (!string.Equals(curResourseTable["Illustration"].ToString(), ""))
        {
            //일러스트 처리
            ImageContent imageInstance = Instantiate(ResourcesManager.Load<GameObject>(Define.UI_PATH + "ImageContent"), storyUi.scrollRect.content).GetComponent<ImageContent>();
            Sprite imageSprite = ImageLoader.Instance.LoadLocalImageToSprite(curResourseTable["Illustration"].ToString());

            imageInstance.image.sprite = imageSprite;

            float imageRatio = imageSprite.rect.height / imageSprite.rect.width;

            Vector2 sizeDeltaTemp = imageInstance.rectTransform.sizeDelta;

            sizeDeltaTemp.y = sizeDeltaTemp.x * imageRatio;

            imageInstance.rectTransform.sizeDelta = sizeDeltaTemp;

            LayoutRebuilder.ForceRebuildLayoutImmediate(storyUi.scrollRect.content);
        }
        if (!string.Equals(curResourseTable["Toast"].ToString(), ""))
        {
            //Toast 처리
            StartCoroutine(playingUiTK.PopToast(curResourseTable["Toast"].ToString()));
        }

        TextContent textInstance = Instantiate(ResourcesManager.Load<GameObject>(Define.UI_PATH + "TextContent"), storyUi.scrollRect.content).GetComponent<TextContent>();
        
        StartCoroutine(StartTyping(textInstance));
    }

    public IEnumerator StartTyping(TextContent textInstance) //타이핑 IEnumerator
    {
        isTypingSkip = false;

        for (int i = 0; i <= curStoryText.Length && !isTypingSkip; i++)
        {
            textInstance.TextMeshProUGUI.text = curStoryText.Substring(0, i);
            yield return new WaitForSeconds(typingSpeed);
        }
        
        textInstance.TextMeshProUGUI.text = curStoryText;

        textInstance.rectTransform.sizeDelta = new Vector2(textInstance.rectTransform.sizeDelta.x, textInstance.TextMeshProUGUI.preferredHeight);

        LayoutRebuilder.ForceRebuildLayoutImmediate(storyUi.scrollRect.content);

        playingUiTK.ChoiceSetting(curChoiceList);
    }
    #endregion

    #region 인벤토리 조절
    
    public void OnInventoryUI()
    {
        Dictionary<string, Item> itemList = InventoryManager.Instance.GetItemList();

        foreach (KeyValuePair<string, Item> entry in itemList)
        {
            if (entry.Value.Data.type == 0 && entry.Value.Data.isShow == true && entry.Value.Data.amount != 0)
            {
                ItemContent itemInstance = Instantiate(ResourcesManager.Load<GameObject>(Define.UI_PATH + "ItemContent"), inventoryUi.scrollRect.content).GetComponent<ItemContent>();
                itemInstance.itemName.text = entry.Value.Data.itemname;
                if (!string.Equals(entry.Value.Data.imagePath, ""))
                {
                    itemInstance.itemImage.sprite = ImageLoader.Instance.LoadLocalImageToSprite(entry.Value.Data.imagePath);
                }
                itemInstance.itemAmountImage.gameObject.SetActive(false);
            }
            else if (entry.Value.Data.type == 1 && entry.Value.Data.isShow == true && entry.Value.Data.amount != 0)
            {
                ItemContent itemInstance = Instantiate(ResourcesManager.Load<GameObject>(Define.UI_PATH + "ItemContent"), inventoryUi.scrollRect.content).GetComponent<ItemContent>();
                itemInstance.itemName.text = entry.Value.Data.itemname;
                if (!string.Equals(entry.Value.Data.imagePath, ""))
                {
                    itemInstance.itemImage.sprite = ImageLoader.Instance.LoadLocalImageToSprite(entry.Value.Data.imagePath);
                }
                itemInstance.itemAmount.text = entry.Value.Data.amount + "개";
                itemInstance.itemAmountImage.gameObject.SetActive(true);
            }
        }

        inventoryUi.gameObject.SetActive(true);
    }


    public void OffInventoryUI()
    {
        inventoryUi.gameObject.SetActive(false);

        foreach (Transform child in inventoryUi.scrollRect.content.transform)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion

}
