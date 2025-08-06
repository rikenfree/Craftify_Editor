using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace uCP
{
	public class DropperGetColor : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		[Serializable]
		public class GetColorEvent : UnityEvent<Color>
		{
		}

		[SerializeField]
		public GetColorEvent OnGetColor;

		[NonSerialized]
		public Texture2D texture = new Texture2D(1, 1, TextureFormat.RGB24, mipChain: false);

		[NonSerialized]
		public bool isClicked;

		[NonSerialized]
		public Vector2 ClickedPosition = Vector2.zero;

		public void OnGUI()
		{
			if (isClicked)
			{
				texture.ReadPixels(new Rect(ClickedPosition.x, ClickedPosition.y, 1f, 1f), 0, 0);
				OnGetColor.Invoke(texture.GetPixel(0, 0));
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		public void OnPointerClick(PointerEventData p)
		{
			isClicked = true;
			ClickedPosition = p.position;
		}
	}
}
