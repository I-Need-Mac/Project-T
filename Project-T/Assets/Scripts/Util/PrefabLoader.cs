using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PrefabLoader : SingleTon<PrefabLoader>
{
    private Dictionary<string, GameObject> cachedPrefab = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> cachedMap = new Dictionary<string, GameObject>();

    const string MAP_HOME = "Maps/";
    const string PREFAB_HOME = "Prefabs/";

    public PrefabLoader()
    {
        cachedPrefab.Add("Dummy", Resources.Load(PREFAB_HOME + "Dummy", typeof(GameObject)) as GameObject);
        cachedMap.Add("Dummy", Resources.Load(MAP_HOME + "Dummy", typeof(GameObject)) as GameObject);
    }


    public GameObject LoadPrefabToGameObject(string path)
    {
        GameObject returnPrefab;
        if (cachedPrefab.ContainsKey(path))
        {
            DebugManager.Instance.PrintDebug("[PrefabLoader] Cached Prefab : " + path);
            returnPrefab = cachedPrefab[path];
        }
        else
        {
            DebugManager.Instance.PrintDebug("[PrefabLoader] New Prefab : " + path);

            returnPrefab = Resources.Load(PREFAB_HOME + path, typeof(GameObject)) as GameObject;
            if (returnPrefab == null)
            {
                DebugManager.Instance.PrintDebug("[PrefabLoader] Dummy Prefab : 경로 이상");
                returnPrefab = cachedPrefab["Dummy"];
            }
            else { cachedPrefab.Add(path, returnPrefab); }

        }
        return returnPrefab;
    }

    public GameObject LoadMapToGameObject(string path)
    {
        GameObject returnPrefab;
        if (cachedMap.ContainsKey(path))
        {
            DebugManager.Instance.PrintDebug("[PrefabLoader] Cached Map : " + path);
            returnPrefab = cachedMap[path];
        }
        else
        {
            DebugManager.Instance.PrintDebug("[PrefabLoader] New Map : " + path);

            returnPrefab = Resources.Load(MAP_HOME + path, typeof(GameObject)) as GameObject;
            if (returnPrefab == null)
            {
                DebugManager.Instance.PrintDebug("[PrefabLoader] Dummy Prefab : 경로 이상");
                returnPrefab = cachedMap["Dummy"];
            }
            else { cachedMap.Add(path, returnPrefab); }

        }
        return returnPrefab;
    }

}
