using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Sound
    {
        Bgm,
        Voice,
        SFX,
        MaxCount,  // 아무것도 아님. 그냥 Sound enum의 개수 세기 위해 추가. (0, 1, '2' 이렇게 2개) 
    }

    public enum Volume
    {
        Master,
        Bgm,
        SFX,
    }

    public const string UI_PATH = "Prefabs/UI/";

    public const string SOUND_PATH = "Sound/";
}
