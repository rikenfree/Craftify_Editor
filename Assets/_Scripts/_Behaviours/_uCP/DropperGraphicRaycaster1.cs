using UnityEngine.UI;

namespace uCP
{
	public class DropperGraphicRaycaster1 : GraphicRaycaster
	{
		public int _priority = 1;

		public override int sortOrderPriority => _priority;
	}
}
