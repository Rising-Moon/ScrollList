using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSwtichShader : MonoBehaviour{
    private Material mat;

    private void Awake(){
        mat = gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update(){
        Debug.Log("启用test关键字");
        if (Input.GetKeyDown("s")) {
            mat.EnableKeyword("test");
        }
    }
}