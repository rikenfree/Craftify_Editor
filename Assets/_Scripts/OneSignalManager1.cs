using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneSignalSDK;

public class OneSignalManager1 : MonoBehaviour
{
    public static OneSignalManager1 Instance;
    public string oneSignalId;

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
        OneSignal.Initialize(oneSignalId);
    }
}
