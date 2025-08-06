using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHighlighter : MonoBehaviour
{
    [SerializeField] public List<Button> buttons;

    void Start()
    {
        foreach (var button in buttons)
        {
            SetHighlight(button, false);
            button.onClick.AddListener(() => OnButtonClick(button));
        }
    }

    void OnButtonClick(Button clickedButton)
    {
        foreach (var button in buttons)
        {
            SetHighlight(button, button == clickedButton);
        }
    }

    void SetHighlight(Button button, bool active)
    {
        if (button.transform.childCount > 1)
        {
            button.transform.GetChild(1).gameObject.SetActive(active);
        }
        else
        {
            Debug.LogWarning("Button doesn't have a second child (index 1) for highlight image.");
        }
    }
}
