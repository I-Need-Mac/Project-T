using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StoryUI : MonoBehaviour, IPointerClickHandler
{
    public ScrollRect scrollRect;
    public RectTransform contentRect;
    private float scrollDuration = 0.3f;

    private void Awake()
    {
        scrollRect = GetComponentInChildren<ScrollRect>();
        contentRect = scrollRect.content.GetComponent<RectTransform>();
    }

    public void ScrollToBottomSlowly()
    {
        // contentRect의 높이를 가져옴
        float contentHeight = contentRect.rect.height;

        // 스크롤 영역의 높이를 가져옴
        float scrollHeight = 749f;

        // 만약 스크롤 뷰의 높이가 컨텐츠 높이보다 작을 경우 스크롤이 필요 없으므로 리턴
        if (scrollHeight >= contentHeight) {
            PlayUI.Instance.GenerateStory();
        
            return;
        }

        StartCoroutine(ScrollSmoothly());
    }

    public IEnumerator ScrollSmoothly()
    {
        Vector2 startPosition = scrollRect.normalizedPosition;
        Vector2 targetPosition = new Vector2(0f,0f);

        float elapsedTime = 0f;

        while (elapsedTime < scrollDuration)
        {
            float t = elapsedTime / scrollDuration;
            scrollRect.normalizedPosition = Vector2.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        scrollRect.normalizedPosition = targetPosition;

        PlayUI.Instance.GenerateStory();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayUI.Instance.isTypingSkip = true;
    }
}
