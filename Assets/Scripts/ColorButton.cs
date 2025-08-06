using UnityEngine;

public class ColorButton : MonoBehaviour
{
    public int myIndex;
    public GetIndexScript getIndexScript;

    public void OnButtonClick()
    {
        getIndexScript.GetIndex(myIndex, this.gameObject);
    }
}
