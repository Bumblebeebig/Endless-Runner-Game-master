using System;
using Bumblebee_Asset.Scripts.Store;
using GoogleMobileAds.Api;
using UnityEngine;

namespace Bumblebee_Asset.Scripts.Game
{
    public class AdManager : MonoBehaviour
    {
        private BannerView _bannerView;
        private InterstitialAd _interstitial;
        private RewardBasedVideoAd _rewardBasedVideo;
        private bool _rewarded;

        [Obsolete]
        public void Start()
        {
       
            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(InitializationStatus => { });
        
            // Get singleton reward based video ad reference.
            this._rewardBasedVideo = RewardBasedVideoAd.Instance;
        
            // RewardBasedVideoAd is a singleton, so handlers should only be registered once.
            this._rewardBasedVideo.OnAdRewarded += this.HandleRewardBasedVideoRewarded;
            this._rewardBasedVideo.OnAdClosed += this.HandleRewardBasedVideoClosed;
        
            this.RequestRewardBasedVideo();
     
        }

        private void Update()
        {
            if (_rewarded)
            {
                _rewarded = false;
                ShopManager shopManager = FindObjectOfType<ShopManager>();
                if (shopManager != null)
                    shopManager.RewardWithAD();
            }
        }

        // Returns an ad request with custom ad targeting.
        private AdRequest CreateAdRequest()
        {
            return new AdRequest.Builder().Build();
        }

        public void RequestBanner()
        {
            // These ad units are configured to always serve test ads.
            string adUnitId = "ca-app-pub-3940256099942544/6300978111";

            // Clean up banner ad before creating a new one.
            if (this._bannerView != null)
            {
                this._bannerView.Destroy();
            }

            // Create a 320x50 banner at the top of the screen.
            this._bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);

            // Load a banner ad.
            this._bannerView.LoadAd(this.CreateAdRequest());
        }

        public void RequestInterstitial()
        {
            // These ad units are configured to always serve test ads.
            string adUnitId = "ca-app-pub-3940256099942544/1033173712";


            // Clean up interstitial ad before creating a new one.
            if (this._interstitial != null)
            {
                this._interstitial.Destroy();
            }

            // Create an interstitial.
            this._interstitial = new InterstitialAd(adUnitId);

            // Load an interstitial ad.
            this._interstitial.LoadAd(this.CreateAdRequest());
        }

        public void RequestRewardBasedVideo()
        {
            string adUnitId = "ca-app-pub-3940256099942544/5224354917";

            this._rewardBasedVideo.LoadAd(this.CreateAdRequest(), adUnitId);
        }

        public void ShowInterstitial()
        {
            if (this._interstitial.IsLoaded())
            {
                this._interstitial.Show();
            }
            else
            {
                Debug.Log("Interstitial is not ready yet");
            }
        }

        public void ShowRewardBasedVideo()
        {
            if (this._rewardBasedVideo.IsLoaded())
            {
                this._rewardBasedVideo.Show();
            }
            else
            {
                MainMenu menu = FindObjectOfType<MainMenu>();
                if (menu != null)
                    menu.messageAnim.SetTrigger("show");

                Debug.Log("Video is not ready yet");
            }
        }


        #region RewardBasedVideo callback handlers

        public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
        {
            this.RequestRewardBasedVideo();
        }

        public void HandleRewardBasedVideoRewarded(object sender, Reward args)
        {
            _rewarded = true;
        }

        #endregion
    }
}