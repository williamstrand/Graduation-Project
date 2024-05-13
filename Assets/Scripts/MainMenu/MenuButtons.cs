using UnityEngine;
using UnityEngine.SceneManagement;

namespace WSP.MainMenu
{
    public class MenuButtons : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void OpenSettings()
        {
            // Open settings menu
        }
    }
}