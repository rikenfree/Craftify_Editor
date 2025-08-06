using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
	public class CircleButton : SceneElement
	{
		public Image selectorRing;
        public Color selectColor;
        public Color defultColorColor;


		// public void MarkSelected()
		// {
        //     selectorRing.gameObject.GetComponent<Image>().color = selectColor;

        //     //selectorRing.gameObject.SetActive(value: true);
		// }

		// public void MarkDeselected()
		// {
        //     selectorRing.gameObject.GetComponent<Image>().color = defultColorColor;
        //   //  selectorRing.gameObject.SetActive(value: false);
		// }
	}
}
