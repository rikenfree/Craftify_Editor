using UnityEngine;

public class XScaleModifayer1 : MonoBehaviour
{
	public float XMaxSize = 10f;

	public bool scaleDownOnly;

	public bool calulateStartOnly;

	private void Awake()
	{
		Calculate();
	}

	private void FixedUpdate()
	{
		if (!calulateStartOnly)
		{
			Calculate();
		}
	}

	public void Calculate()
	{
		Rect objectBounds = PreviewScreenUtil1.getObjectBounds(base.gameObject);
		float num = (float)Screen.width / 100f * XMaxSize;
		if (!(objectBounds.width < num) || !scaleDownOnly)
		{
			float d = num / objectBounds.width;
			base.transform.localScale = base.transform.localScale * d;
		}
	}
}
