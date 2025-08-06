using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneSignalSDK;

public class OneSignalManager : MonoBehaviour
{
    public static OneSignalManager Instance;
    public string signalID;
    public string signalIDIos;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {

#if UNITY_IOS
     //   OneSignal.Initialize(signalIDIos);
#elif UNITY_ANDROID
        //OneSignal.Initialize(signalID);

#endif
    }
}
