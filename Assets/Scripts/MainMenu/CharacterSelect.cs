using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WSP.Units.Characters;

namespace WSP.MainMenu
{
    public class CharacterSelect : MonoBehaviour
    {
        [SerializeField] CharacterButton characterPrefab;
        [SerializeField] Transform buttonParent;

        [SerializeField] Button startButton;

        CharacterButton selected;

        void Start()
        {
            startButton.interactable = false;

            var characters = CharacterDatabase.Characters;
            for (var i = 0; i < characters.Length; i++)
            {
                var button = Instantiate(characterPrefab, buttonParent);
                button.Set(i);
                button.OnSelected += OnCharacterSelected;
            }

            return;

            void OnCharacterSelected(CharacterButton button)
            {
                if (selected) selected.SetSelected(false);
                selected = button;
                button.SetSelected(true);
                startButton.interactable = true;
            }
        }

        void OnEnable()
        {
            if (selected) selected.SetSelected(false);

            selected = null;
            startButton.interactable = false;
        }

        public void Play()
        {
            if (!selected) return;

            PlayerPrefs.SetInt("Character", selected.CharacterIndex);

            SceneManager.LoadScene(1);
        }
    }

}