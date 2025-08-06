using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public string url;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Application.OpenURL(url);
        }
    }
}
