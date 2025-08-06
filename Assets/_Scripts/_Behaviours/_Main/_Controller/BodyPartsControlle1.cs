namespace Main.Controller
{
	public class BodyPartsController1 : SceneElement1
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
