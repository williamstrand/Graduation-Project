using UnityEngine;
using UnityEngine.SceneManagement;

namespace WSP.MainMenu
{
    public class MenuButtons : MonoBehaviour
    {
        [SerializeField] GameObject main;
        [SerializeField] GameObject controls;

        GameObject currentMenu;

        void Start()
        {
            SetMenu(main);
        }

        void SetMenu(GameObject menu)
        {
            if (currentMenu) currentMenu.SetActive(false);
            currentMenu = menu;
            currentMenu.SetActive(true);
        }

        public void StartGame()
        {
            SceneManager.LoadScene(1);
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void OpenControls()
        {
            SetMenu(controls);
        }
        
        public void OpenMain()
        {
            SetMenu(main);
        }
    }
}