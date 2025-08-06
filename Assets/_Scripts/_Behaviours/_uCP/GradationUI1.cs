using UnityEngine;
using UnityEngine.UI;

namespace uCP
{
	[AddComponentMenu("uCP/Gradation UI")]
	public class GradationUI1 : BaseMeshEffect
	{
		public bool useAlpha;

		public Color[] colors = new Color[4]
		{
			Color.black,
			Color.white,
			Color.red,
			Color.black
		};

		public override void ModifyMesh(VertexHelper help)
		{
			if (!IsActive() || help == null)
			{
				return;
			}
			Rect pixelAdjustedRect = base.graphic.GetPixelAdjustedRect();
			UIVertex vertex = UIVertex.simpleVert;
			for (int i = 0; i < help.currentVertCount; i++)
			{
				help.PopulateUIVertex(ref vertex, i);
				Vector2 vector = Rect.PointToNormalized(pixelAdjustedRect, vertex.position);
				vertex.color = Color.Lerp(Color.Lerp(colors[0], colors[1], vector.y), Color.Lerp(colors[3], colors[2], vector.y), vector.x);
				if (!useAlpha)
				{
					vertex.color.a = byte.MaxValue;
				}
				help.SetUIVertex(vertex, i);
			}
		}

		public virtual void UpdateColors()
		{
			base.graphic.SetVerticesDirty();
		}
	}
}
