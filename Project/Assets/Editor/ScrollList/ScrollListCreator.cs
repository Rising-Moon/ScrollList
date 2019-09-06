using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScrollListEditor : MonoBehaviour
{
    [MenuItem("GameObject/UI/Scroll List", false, 2063)]
    static public void CreateScrollList()
    {
        GameObject scrollList = Resources.Load<GameObject>("Prefabs/ScrollList");
        GameObject canvas = GameObject.Find("Canvas");
        GameObject instance = Instantiate(scrollList);
        if (canvas != null)
        {
            instance.transform.SetParent(canvas.transform);
            instance.transform.localPosition = Vector3.zero;
            instance.name = "Scroll List";
        }
    }
}