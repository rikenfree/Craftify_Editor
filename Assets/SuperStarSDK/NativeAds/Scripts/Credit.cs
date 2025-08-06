using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AvocadoShark
{
    public class Credit : MonoBehaviour
    {
        public void Start()
        {
            Application.targetFrameRate = 60;
        }
        public void openurl()
        {
            Application.OpenURL("https://u3d.as/2TS2");
        }
    }
}
