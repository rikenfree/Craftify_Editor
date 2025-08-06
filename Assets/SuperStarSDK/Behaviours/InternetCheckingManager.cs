using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class InternetCheckingManager : MonoBehaviour
{
    public static InternetCheckingManager Instance;
    public bool isinternetavailable;
    public GameObject NoInternetPopUp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InternetCheckInvoke();
    }

    public void InternetCheckInvoke()
    {
        StartCoroutine(CheckInternetConnection((isConnected) =>
        {
            if (isConnected)
            {
                isinternetavailable = true;
                NoInternetPopUp.SetActive(false);
            }
            else
            {
                isinternetavailable = false;
                NoInternetPopUp.SetActive(true);
            }

            Invoke("InternetCheckInvoke", 5); // Re-check internet every 5 seconds
        }));
    }

    private IEnumerator CheckInternetConnection(System.Action<bool> callback)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            callback(false);
            yield break;
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Get("https://www.google.com"))
        {
            webRequest.timeout = 5; // Timeout in seconds
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                callback(false);
            }
            else
            {
                callback(true);
            }
        }
    }
}
