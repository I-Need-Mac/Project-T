using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    private const bool _isPrintDebug = true;
    public bool isPrintDebug
    {
        get
        {
            return _isPrintDebug;
        }
    }
    private static DebugManager _instance { get; set; }
    public static DebugManager Instance
    {
        get
        {
            return _instance ?? (_instance = new DebugManager());
        }
    }

    public void PrintDebug(object target)
    {
        if (isPrintDebug)
            Debug.Log(target);

    }
    public void PrintDebug(object target, object value)
    {
        if (isPrintDebug)
            Debug.Log(target + " : " + value);

    }
    public void PrintDebug(string target)
    {
        if (isPrintDebug)
            Debug.Log(target);



    }

    public void PrintDrawLine()
    {
        if (isPrintDebug)
            Debug.Log("------------------------------------------------------------------");
    }
}
