using UnityEngine;

namespace ImageCropperNamespace
{
	public class SizeChangeListener : MonoBehaviour
	{
		private RectTransform rectTransform;
		public RectTransform target;
		
		public System.Action<Vector2> onSizeChanged;

		private void Awake()
		{
			rectTransform = (RectTransform) transform;
		}

		private void Start()
		{
			OnRectTransformDimensionsChange();
            
			rectTransform = target;
        }

		private void OnRectTransformDimensionsChange()
		{
			if( onSizeChanged != null && rectTransform != null )
				onSizeChanged( rectTransform.rect.size );


			
		}
	}
}