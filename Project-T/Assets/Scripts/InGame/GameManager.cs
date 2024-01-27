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

    protected override void Awake()
    {
        StoryManager.Instance.StoryManagerInit("Test");
        InventoryManager.Instance.InventoryManagerInit("Test");
    }

    void Start()
    {
        StoryManager.Instance.StoryUpdate();
    }
    
}
