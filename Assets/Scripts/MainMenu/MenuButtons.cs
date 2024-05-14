using UnityEngine;

namespace WSP.MainMenu
{
    public class MenuButtons : MonoBehaviour
    {
        [SerializeField] GameObject main;
        [SerializeField] GameObject controls;
        [SerializeField] GameObject characterSelect;

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

        public void OpenCharacterSelect()
        {
            SetMenu(characterSelect);
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