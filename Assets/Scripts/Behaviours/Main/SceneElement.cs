using UnityEngine;

namespace Main
{
	public class SceneElement : MonoBehaviour
	{
		public Scene scene => UnityEngine.Object.FindObjectOfType<Scene>();
	}
}
