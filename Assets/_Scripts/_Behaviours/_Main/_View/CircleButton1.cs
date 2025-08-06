using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
	public class CircleButton1 : SceneElement1
	{
		public Image selectorRing;
        public Color selectColor;
        public Color defultColorColor;

		public bool isMarked = false;

		public void MarkSelected()
		{
			//selectorRing.gameObject.GetComponent<Image>().color = selectColor;
			isMarked = true;
			//selectorRing.gameObject.SetActive(value: true);
		}

		public void MarkDeselected()
		{
			isMarked = false;
			//selectorRing.gameObject.GetComponent<Image>().color = defultColorColor;
			//  selectorRing.gameObject.SetActive(value: false);
		}
	}
}
