using DG.Tweening;
using I2.Loc;
using Main.Controller;
using Main.View;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Launguage2
{
    public string Name;
    public string Code;
}

public class LaunguageLocalization2 : MonoBehaviour
{
    public static LaunguageLocalization2 instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        InitializeLanguageButtons();
        ApplyInitialLanguageSelection();
    }

    public string currentlanguage
    {
        get { return PlayerPrefs.GetString("language", "en"); }
        set
        {
            PlayerPrefs.SetString("language", value);
            PlayerPrefs.Save();
        }
    }

    [Header("I2 Localize Reference")]
    public Transform languagebtnparent;
    public List<Launguage> Language = new List<Launguage>();

    private int oldSelectedNob = -1;

    public GameObject LanguagePanel;

    private void InitializeLanguageButtons()
    {
        for (int i = 0; i < Language.Count; i++)
        {
            Transform t = languagebtnparent.GetChild(i);

            // Use Localize instead of setting .text directly
            var localize = t.GetChild(0).GetComponent<Localize>();
            if (localize != null)
            {
                localize.Term = Language[i].Name;
                localize.OnLocalize();
            }

            Button button = t.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            int index = i;
            button.onClick.AddListener(() => AddEvent(index));
        }
    }

    private void ApplyInitialLanguageSelection()
    {
        string currentLang = currentlanguage;
        for (int i = 0; i < Language.Count; i++)
        {
            if (Language[i].Code == currentLang)
            {
                oldSelectedNob = i;
                UpdateLanguageSelection(i, true);
                return;
            }
        }

        oldSelectedNob = 0;
        UpdateLanguageSelection(0, true);
    }

    public void AddEvent(int index)
    {
        Debug.Log("AddEvent => " + index);
        SoundManager2.instance.PlayButtonSound();
        OnLanguageSelect(index);
    }

    public void OnLanguageSelect(int index)
    {
        Debug.Log("OnLanguageSelect");
        if (oldSelectedNob != index)
        {
            UpdateLanguageSelection(index);
        }
    }

    private void UpdateLanguageSelection(int index, bool initialSetup = false)
    {
        if (oldSelectedNob >= 0 && oldSelectedNob < languagebtnparent.childCount)
        {
            Transform oldT = languagebtnparent.GetChild(oldSelectedNob);
            oldT.GetChild(1).gameObject.SetActive(false);
        }

        Transform newT = languagebtnparent.GetChild(index);
        if (!initialSetup)
        {
            newT.DOScale(new Vector3(1.05f, 1.05f, 1.05f), 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        }
        newT.GetChild(1).gameObject.SetActive(true);

        currentlanguage = Language[index].Code;
        LocalizationManager.CurrentLanguageCode = currentlanguage;

        oldSelectedNob = index;
    }

    public void SetToDefault()
    {
        SoundManager2.instance.PlayButtonSound();
        OnLanguageSelect(0);
    }

    public void LanguagePanelClose()
    {
        SoundManager2.instance.PlayButtonSound();
        LanguagePanel.SetActive(false);
    }
}
