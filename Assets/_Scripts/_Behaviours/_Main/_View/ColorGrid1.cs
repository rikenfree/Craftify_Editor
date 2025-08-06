using UnityEngine;

namespace Main.View
{
	public class ColorGrid1 : SceneElement1
	{
		public ColorCell1 colorCellPrefab;

		private void Start()
		{
			string[] array = new string[56]
			{
				"651819",
				"9d2529",
				"cd3134",
				"000000",
				"3e2310",
				"3c1f10",
				"371c12",
				"7c5011",
				"ba7f1b",
				"ed5823",
				"3b3b3b",
				"795936",
				"785034",
				"734534",
				"a5a620",
				"fff938",
				"fdf95f",
				"57565e",
				"986d44",
				"996150",
				"905849",
				"2d7b2d",
				"43bc47",
				"77d77a",
				"737371",
				"c08d5f",
				"be7b59",
				"976051",
				"297f58",
				"46bc99",
				"79d5b2",
				"8e8f91",
				"dca96e",
				"de9774",
				"d48972",
				"2c627c",
				"4496ba",
				"79b7d7",
				"aaaaaa",
				"edbe8b",
				"f0ad83",
				"eaa285",
				"2a4877",
				"406fbb",
				"7799d3",
				"c7c7c7",
				"fed1a0",
				"fec0a2",
				"fab69a",
				"7a2f7d",
				"bc46bc",
				"d478d5",
				"fffeff",
				"fee7ca",
				"fedfc8",
				"fed6c7"
			};
			for (int i = 0; i < array.Length; i++)
			{
				Color color = default(Color);
				ColorUtility.TryParseHtmlString("#" + array[i], out color);
				ColorCell1 colorCell = Object.Instantiate(colorCellPrefab);
				colorCell.SetColor(color);
				colorCell.transform.SetParent(base.gameObject.transform, worldPositionStays: false);
			}
		}
	}
}
