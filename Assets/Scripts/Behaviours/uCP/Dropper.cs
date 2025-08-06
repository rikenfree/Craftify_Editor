//using System;
//using UnityEngine;
//using UnityEngine.UI;

//namespace uCP
//{
//	[RequireComponent(typeof(Button))]
//	public class Dropper : MonoBehaviour
//	{
//		public GameObject dropperCanvas;

//		public Color ActiveColor = new Color(96f, 96f, 96f);

//		[NonSerialized]
//		public Color NormalColor;

//		[NonSerialized]
//		public Button DropperButton;

//		[NonSerialized]
//		public Graphic graph;

//		[NonSerialized]
//		public ColorPicker _colorPicker;

//		public ColorPicker colorPicker
//		{
//			get
//			{
//				if (_colorPicker == null)
//				{
//					_colorPicker = GetComponentInParent<ColorPicker>();
//				}
//				return _colorPicker;
//			}
//		}

//		protected virtual void Awake()
//		{
//			graph = GetComponent<Graphic>();
//			NormalColor = graph.color;
//			DropperButton = GetComponent<Button>();
//			DropperButton.onClick.AddListener(OnDropperButton);
//		}

//		public virtual void OnDropperButton()
//		{
//			graph.color = ActiveColor;
//			DropperGetColor component = UnityEngine.Object.Instantiate(dropperCanvas).GetComponent<DropperGetColor>();
//			UnityEngine.Debug.Log(component);
//			component.OnGetColor.AddListener(OnGetColorCallback);
//		}

//		public virtual void OnGetColorCallback(Color c)
//		{
//			graph.color = NormalColor;
//			colorPicker.color = c;
//			colorPicker.UpdateUI();
//		}
//	}
//}
