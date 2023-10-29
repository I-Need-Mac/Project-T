using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITextFontSetter : MonoBehaviour
{
    public TMP_FontAsset newFont;
    void Start()
    {
        // 모든 TMP 텍스트 요소를 찾아 리스트에 저장
        List<TextMeshProUGUI> textElements = new List<TextMeshProUGUI>(FindObjectsOfType<TextMeshProUGUI>());

        // 각 TMP 텍스트 요소에 새로운 폰트 설정
        foreach (TextMeshProUGUI textElement in textElements)
        {
            textElement.font = newFont;
        }
    }
}
