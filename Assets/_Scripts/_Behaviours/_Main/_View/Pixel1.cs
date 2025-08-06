using UnityEngine;

namespace Main.View
{
	public class Pixel1 : SceneElement1
	{
		public float r;

		public float g;

		public float b;

		public float a;

		public float r0;

		public float g0;

		public float b0;

		public float a0;

		public int i;

		public int j;

		public BodyPart1 parent;

		public bool isClothing;

		private Mesh myMesh;

		private MeshRenderer myRenderer;

		private void Awake()
		{
			myMesh = GetComponent<MeshFilter>().mesh;
			myRenderer = GetComponent<MeshRenderer>();
		}

		public Color GetColor()
		{
			return new Color(r, g, b, a);
		}

		public void ChangeColor(Color color)
		{
			r = color.r;
			g = color.g;
			b = color.b;
			a = color.a;
			myMesh.colors = new Color[4]
			{
				color,
				color,
				color,
				color
			};
			myRenderer.enabled = ((double)color.a >= 1.0);
		}

		public void SetDefaultColor(Color color)
		{
			r0 = color.r;
			g0 = color.g;
			b0 = color.b;
			a0 = color.a;
		}

		public void RandomColor()
		{
			float num = Random.Range(0f, 1f);
			float num2 = Random.Range(0f, 1f);
			float num3 = Random.Range(0f, 1f);
			Color color = new Color(num, num2, num3);
			ChangeColor(color);
		}

		public void Erase()
		{
			
				ChangeColor(new Color(0f, 0f, 0f, 0f));
			
		}
	}
}
