using Main.View;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main.Controller
{
	public class BucketController1 : SceneElement1
	{
		private RootController1 root;

		private void Start()
		{
			root = base.scene.controller;
		}

		private void Update()
		{
			if (!IsPointerOverUIObject())
			{
				if (Camera.main != null)
				{
					Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
					RaycastHit hitInfo = default(RaycastHit);
					if (Input.GetMouseButton(0) && root.AllowAction() && Physics.Raycast(ray, out hitInfo) && hitInfo.transform.tag == "Pixel")
					{
						hitInfo.transform.GetComponent<Pixel1>().parent.PainAllPixels(base.scene.controller.color.currentColor);
					}
				}
			}
		}

		private bool IsPointerOverUIObject()
		{
			PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
			pointerEventData.position = new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);
			List<RaycastResult> list = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerEventData, list);
			return list.Count > 0;
		}
	}
}
