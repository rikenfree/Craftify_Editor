using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AvocadoShark
{
    public class Go_Back : MonoBehaviour
    {
        public void MainScene()
        {
            SceneManager.LoadScene("Menu Showcase");
        }
    }
}
