namespace Main.Controller
{
	public class BodyPartsController : SceneElement
	{
		public void OpenBodyPartsViewer()
		{
			base.scene.view.bodyPartsViewer.gameObject.SetActive(value: true);
			base.scene.view.bodyPartsViewer.Refresh();
		}

		public void CloseBodyPartsViewer()
		{
			base.scene.view.bodyPartsViewer.gameObject.SetActive(value: false);
		}
	}
}
