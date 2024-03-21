using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bumblebee_Asset.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        public static bool IsGameOver;
        public GameObject gameOverPanel;

        public static bool IsGameStarted;
        public GameObject startingText;
        public GameObject newRecordPanel;

        public static int Score;
        public Text scoreText;
        public TextMeshProUGUI gemsText;
        public TextMeshProUGUI newRecordText;

        public static bool IsGamePaused;
        public GameObject[] characterPrefabs;

        private AdManager _adManager;


        private void Awake()
        {
            int index = PlayerPrefs.GetInt("SelectedCharacter");
            GameObject character = Instantiate(characterPrefabs[index], transform.position, Quaternion.identity);
            _adManager = FindObjectOfType<AdManager>();
        }

        void Start()
        {
            Score = 0;
            Time.timeScale = 1;
            IsGameOver = IsGameStarted = IsGamePaused = false;

            _adManager.RequestBanner();
            _adManager.RequestInterstitial();
            _adManager.RequestRewardBasedVideo();
        }

        void Update()
        {
            //Update UI
            gemsText.text = PlayerPrefs.GetInt("TotalGems", 0).ToString();
            scoreText.text = Score.ToString();

            //Game Over
            if (IsGameOver)
            {
                Time.timeScale = 0;
                if (Score > PlayerPrefs.GetInt("HighScore", 0))
                {
                    newRecordPanel.SetActive(true);
                    newRecordText.text = "New \nRecord\n" + Score;
                    PlayerPrefs.SetInt("HighScore", Score);
                }
                else
                {
                    int i = Random.Range(0, 6);
                    if (i == 0)
                        _adManager.ShowInterstitial();
                    else if (i == 3)
                        _adManager.ShowRewardBasedVideo();
                }

                gameOverPanel.SetActive(true);
                Destroy(gameObject);
            }

            //Start Game
            if (SwipeManager.IsTap && !IsGameStarted)
            {
                IsGameStarted = true;
                Destroy(startingText);
            }
        }
    }
}