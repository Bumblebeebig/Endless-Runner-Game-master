using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Bumblebee_Asset.Scripts.Game
{
    public class LevelEvent : MonoBehaviour
    {
        public GameObject gamePausedPanel;
        public Button pauseButton;

        private void Update()
        {
            if (!GameManager.IsGameStarted)
                return;

            if (GameManager.IsGameOver)
            {
                pauseButton.interactable = false;
                return;
            }


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameManager.IsGamePaused)
                {
                    ResumeGame();
                    gamePausedPanel.SetActive(false);
                }
                else
                {
                    PauseGame();
                    gamePausedPanel.SetActive(true);
                }
            }
        }

        public void ReplayGame()
        {
            SceneManager.LoadScene("Level");
        }

        public void GoToMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void PauseGame()
        {
            if (!GameManager.IsGamePaused && !GameManager.IsGameOver)
            {
                Time.timeScale = 0;
                GameManager.IsGamePaused = true;
            }
        }

        public void ResumeGame()
        {
            if (GameManager.IsGamePaused)
            {
                Time.timeScale = 1;
                GameManager.IsGamePaused = false;
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}