using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WSP.Units;

namespace WSP.MainMenu
{
    public class CharacterSelect : MonoBehaviour
    {
        [SerializeField] Character[] characters;
        [SerializeField] CharacterButton characterPrefab;
        [SerializeField] Transform buttonParent;

        [SerializeField] Button startButton;

        CharacterButton selected;

        void Start()
        {
            startButton.interactable = false;

            for (var i = 0; i < characters.Length; i++)
            {
                var button = Instantiate(characterPrefab, buttonParent);
                button.Set(Instantiate(characters[i]));
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

            var character = JsonUtility.ToJson(selected.Character);
            PlayerPrefs.SetString("Character", character);

            SceneManager.LoadScene(1);
        }
    }

}