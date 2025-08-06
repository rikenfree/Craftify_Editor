using UnityEngine;

namespace Main
{
	public class SceneElement1 : MonoBehaviour
	{
		public Scene1 scene => UnityEngine.Object.FindObjectOfType<Scene1>();
	}
}
