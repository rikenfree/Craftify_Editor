using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AvocadoShark
{
    public class Scene_Select : MonoBehaviour
    {
        public void SwitchScene(string scene_name)
        {
            SceneManager.LoadScene(scene_name);
        }
    }
}
