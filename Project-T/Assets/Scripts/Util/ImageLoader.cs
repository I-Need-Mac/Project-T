using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ImageLoader : SingleTon<ImageLoader>
{
    private const String DEFULT_PATH = "Arts/";

    private Dictionary<string, Texture2D> cachedImageTexture2D = new Dictionary<string, Texture2D>();
    private Dictionary<string, Sprite> cachedImageSprite = new Dictionary<string, Sprite>();

    public ImageLoader()
    {
        cachedImageTexture2D.Add("Dummy", Resources.Load(DEFULT_PATH + "Dummy", typeof(Texture2D)) as Texture2D);
        cachedImageSprite.Add("Dummy", Resources.Load(DEFULT_PATH + "Dummy", typeof(Sprite)) as Sprite);
    }



    public Texture2D LoadLocalImageToTexture2D(string path)
    {
        Texture2D return2D;
        if (cachedImageTexture2D.ContainsKey(DEFULT_PATH + path))
        {
            DebugManager.Instance.PrintDebug("[ImageLoader] 캐싱된 이미지 : " + path);
            return2D = cachedImageTexture2D[DEFULT_PATH + path];
        }
        else
        {
            DebugManager.Instance.PrintDebug("[ImageLoader] 신규 이미지 : " + path);

            return2D = Resources.Load(DEFULT_PATH + path, typeof(Texture2D)) as Texture2D;
            if (return2D == null)
            {
                DebugManager.Instance.PrintDebug("[ImageLoader] Dummy 이미지 : 경로 이상");
                return2D = cachedImageTexture2D["Dummy"];
            }
            else { cachedImageTexture2D.Add(DEFULT_PATH + path, return2D); }

        }
        return return2D;
    }

    public Sprite LoadLocalImageToSprite(string path)
    {
        Sprite returnSprite;
        if (cachedImageSprite.ContainsKey(DEFULT_PATH + path))
        {
            DebugManager.Instance.PrintDebug("[ImageLoader] 캐싱된 이미지 : " + path);
            returnSprite = cachedImageSprite[DEFULT_PATH + path];
        }
        else
        {
            DebugManager.Instance.PrintDebug("[ImageLoader] 신규 이미지 : " + path);

            returnSprite = Resources.Load(DEFULT_PATH + path, typeof(Sprite)) as Sprite;
            if (returnSprite == null)
            {
                DebugManager.Instance.PrintDebug("[ImageLoader] Dummy 이미지 : 경로 이상");
                returnSprite = cachedImageSprite[DEFULT_PATH + "Dummy"];
            }
            else
            {
                cachedImageSprite.Add(DEFULT_PATH + path, returnSprite);
            }
        }
        return returnSprite;
    }
}