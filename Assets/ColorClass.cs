using System.Collections.Generic;
using UnityEngine;

public class ColorClass : MonoBehaviour
{
    public static ColorClass instance;
    public int currentThemeIndex = 0;

    [SerializeField]
    public List<colorCombination> colors = new List<colorCombination>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SetThemeColors(); // Set colors on startup
        }
        else
        {
            Destroy(gameObject);
        }
        SetThemeColors();

    }

    private void SetThemeColors()
    {
        colors = new List<colorCombination>()
        {
            new colorCombination() { // Theme 0: Jet Black
                backGroundcolor = new Color32(18, 18, 18, 255),
                buttoncolor = new Color32(36, 36, 36, 255),
                topBarcolor = new Color32(28, 28, 28, 255),
                headerTextcolor = Color.white,
                buttonTextcolor = Color.white,
                normalTextcolor = new Color32(200, 200, 200, 255)
            },
            new colorCombination() { // Theme 1: Midnight Navy
                backGroundcolor = new Color32(15, 22, 35, 255),
                buttoncolor = new Color32(34, 46, 67, 255),
                topBarcolor = new Color32(20, 29, 47, 255),
                headerTextcolor = Color.white,
                buttonTextcolor = Color.white,
                normalTextcolor = new Color32(210, 220, 240, 255)
            },
            new colorCombination() { // Theme 2: Space Grey
                backGroundcolor = new Color32(30, 30, 30, 255),
                buttoncolor = new Color32(50, 50, 50, 255),
                topBarcolor = new Color32(45, 45, 45, 255),
                headerTextcolor = Color.white,
                buttonTextcolor = new Color32(240, 240, 240, 255),
                normalTextcolor = new Color32(200, 200, 200, 255)
            },
            new colorCombination() { // Theme 3: Deep Teal
                backGroundcolor = new Color32(10, 25, 28, 255),
                buttoncolor = new Color32(25, 55, 60, 255),
                topBarcolor = new Color32(15, 40, 44, 255),
                headerTextcolor = Color.white,
                buttonTextcolor = new Color32(220, 255, 255, 255),
                normalTextcolor = new Color32(180, 220, 220, 255)
            },
            new colorCombination() { // Theme 4: Coffee Dark
                backGroundcolor = new Color32(35, 27, 24, 255),
                buttoncolor = new Color32(55, 43, 36, 255),
                topBarcolor = new Color32(42, 33, 30, 255),
                headerTextcolor = new Color32(255, 248, 240, 255),
                buttonTextcolor = new Color32(255, 230, 210, 255),
                normalTextcolor = new Color32(220, 200, 180, 255)
            },
            new colorCombination() { // Theme 5: Plum Night
                backGroundcolor = new Color32(24, 18, 28, 255),
                buttoncolor = new Color32(42, 30, 56, 255),
                topBarcolor = new Color32(36, 24, 46, 255),
                headerTextcolor = new Color32(255, 240, 255, 255),
                buttonTextcolor = new Color32(230, 200, 255, 255),
                normalTextcolor = new Color32(200, 180, 210, 255)
            },
            new colorCombination() { // Theme 6: Forest Shadow
                backGroundcolor = new Color32(18, 28, 20, 255),
                buttoncolor = new Color32(28, 48, 34, 255),
                topBarcolor = new Color32(22, 36, 26, 255),
                headerTextcolor = new Color32(220, 255, 220, 255),
                buttonTextcolor = new Color32(180, 230, 180, 255),
                normalTextcolor = new Color32(160, 210, 160, 255)
            },
            new colorCombination() { // Theme 7: Burnt Clay
                backGroundcolor = new Color32(33, 24, 20, 255),
                buttoncolor = new Color32(61, 37, 29, 255),
                topBarcolor = new Color32(47, 33, 27, 255),
                headerTextcolor = Color.white,
                buttonTextcolor = new Color32(255, 220, 200, 255),
                normalTextcolor = new Color32(210, 180, 160, 255)
            },
            new colorCombination() { // Theme 8: Dracula Dark
                backGroundcolor = new Color32(32, 31, 40, 255),
                buttoncolor = new Color32(52, 51, 66, 255),
                topBarcolor = new Color32(42, 41, 56, 255),
                headerTextcolor = new Color32(240, 240, 255, 255),
                buttonTextcolor = new Color32(200, 200, 255, 255),
                normalTextcolor = new Color32(180, 180, 210, 255)
            },
            new colorCombination() { // Theme 9: Slate Shadow
                backGroundcolor = new Color32(26, 28, 30, 255),
                buttoncolor = new Color32(46, 48, 50, 255),
                topBarcolor = new Color32(38, 40, 42, 255),
                headerTextcolor = new Color32(230, 230, 230, 255),
                buttonTextcolor = Color.white,
                normalTextcolor = new Color32(190, 190, 190, 255)
            }
        }; 
    }
}

[System.Serializable] 
public class colorCombination
{
    public Color backGroundcolor;
    public Color buttoncolor;
    public Color topBarcolor;
    public Color headerTextcolor;
    public Color buttonTextcolor;
    public Color normalTextcolor;
}
