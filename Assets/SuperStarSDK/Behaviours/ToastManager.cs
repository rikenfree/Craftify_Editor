// Ensure you have DOTween installed for animations
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastManager : MonoBehaviour
{
    public static ToastManager Instance;

    [Header("Toast Settings")]
    public GameObject toastPrefab;
    public Transform toastParent;
    public Vector2 defaultPosition = new Vector2(0, 300);
    public int poolSize = 10;

    private Queue<GameObject> toastPool = new Queue<GameObject>();

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ValidateReferences();
        InitializePool();
    }

    private void ValidateReferences()
    {
        if (toastPrefab == null)
            Debug.LogError("Toast Prefab is not assigned.");
        if (toastParent == null)
            Debug.LogError("Toast Parent is not assigned.");
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject toast = Instantiate(toastPrefab, toastParent);
            toast.SetActive(false);
            toastPool.Enqueue(toast);
        }
    }

    private GameObject GetToast()
    {
        if (toastPool.Count > 0)
        {
            GameObject toast = toastPool.Dequeue();
            toast.SetActive(true);
            return toast;
        }
        else
        {
            Debug.LogWarning("Toast pool is empty, instantiating a new toast.");
            return Instantiate(toastPrefab, toastParent);
        }
    }

    private void ReturnToast(GameObject toast)
    {
        toast.SetActive(false);
        toastPool.Enqueue(toast);
    }

    public void ShowToast(string message, float duration = 1f, Vector2? position = null)
    {
        GameObject toast = GetToast();
        RectTransform rectTransform = toast.GetComponent<RectTransform>();
        CanvasGroup canvasGroup = toast.GetComponent<CanvasGroup>();

        if (canvasGroup == null)
        {
            Debug.LogError("Toast Prefab requires a CanvasGroup component for fade animations.");
            return;
        }

        rectTransform.anchoredPosition = position ?? defaultPosition;
        toast.GetComponentInChildren<Text>().text = message;

        // Fade in animation
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.3f).OnComplete(() =>
        {
            // Fade out animation after duration
            canvasGroup.DOFade(0, 0.3f).SetDelay(duration).OnComplete(() =>
            {
                ReturnToast(toast);
            });
        });
    }

    public void ShowNoInternetToast()
    {
        ShowToast("Please check your connection.");
    }

    public void ShowDataLoadingToast()
    {
        ShowToast("Data is loading...");
    }

    // Optional: Testing input (remove for production)
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ShowNoInternetToast();
        }
    }
}
