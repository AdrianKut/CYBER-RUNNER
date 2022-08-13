using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour
{
    public static AdsInitializer Instance;

    public static string gameId = "4878800";
    public static string intersititalAd = "Interstitial_Android";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        InitializeAds();
    }

    private void InitializeAds()
    {
        Advertisement.Initialize(gameId,true);
        Advertisement.Load(intersititalAd);
    }

    public void ShowIntersitialAd()
    {
        if (Advertisement.IsReady())
            Advertisement.Show(intersititalAd);
    }
   
}