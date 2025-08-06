
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSkinManager2 : MonoBehaviour
{

    public static RandomSkinManager2 Instance;
    public GameObject RandomSkinPrefab;
    public Transform RandomSkinParent;
    public int RandomSkinItemsCount = 50;
    public List<int> RandomSkinItemsList = new List<int>();
    public int MaxSkins = 780;
    private void Awake()
    {
        if (Instance==null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (IsInitialized==0)
        {
            InitializeItems();
        }

        GenerateRandomNumbers(RandomSkinItemsCount);
        SetUpRandomSkinObjects();
    }
    public float probabilityOfFree = 0.2f;


    public int IsInitialized {

        get { 
            return PlayerPrefs.GetInt("IsInitialized", 0); 
        }

        set {
            PlayerPrefs.SetInt("IsInitialized",value);
        }
    }

    void InitializeItems()
    {
        // Set the first 'freeItems' to free (0)
        for (int i = 0; i < 20; i++)
        {
            PlayerPrefs.SetInt("Item_" + i, 0);
        }

        // Set the remaining items to 0 or 1 based on the probability
        for (int i = 20; i < MaxSkins; i++)
        {
            int value = Random.value < probabilityOfFree ? 0 : 1;
            PlayerPrefs.SetInt("Item_" + i, value);
        }

        // Save PlayerPrefs
        PlayerPrefs.Save();
        IsInitialized = 1;
    }

    public void GenerateRandomNumbers(int count)
    {
      
        HashSet<int> uniqueNumbers = new HashSet<int>();
        System.Random random = new System.Random();

        while (uniqueNumbers.Count < count)
        {
            uniqueNumbers.Add(random.Next(0, MaxSkins)); // Change the range of random numbers here as needed
        }

        RandomSkinItemsList.AddRange(uniqueNumbers);
       
    }


    public int GiveMeuniqueIndex() 
    {
      //  System.Random random = new System.Random();
        int Rindex = Random.Range(0, MaxSkins);

        if (!RandomSkinItemsList.Contains(Rindex))
        {
            RandomSkinItemsList.Add(Rindex);
            return Rindex;
        }
        else 
        {
            return GiveMeuniqueIndex();
        }

        return 0;
    }
    public void SetUpRandomSkinObjects() 
    {

        foreach (Transform item in RandomSkinParent)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < RandomSkinItemsList.Count; i++)
        {
         GameObject RObject  = Instantiate(RandomSkinPrefab, RandomSkinParent);
         RObject.GetComponent<RandomSkinItem2>().CurrentIndex = RandomSkinItemsList[i];
        }
    
    }
}
