using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sailboat
{
    public class GlobalFunctionButtonUI : MonoBehaviour
    {
        Button button;

        public ButtonFunction function;

        private void Awake()
        {
            button = GetComponent<Button>();
            switch (function)
            {
                case ButtonFunction.Exit:
                    button.onClick.AddListener(Exit);
                    break;
                case ButtonFunction.Restart:
                    button.onClick.AddListener(Restart);
                    break;
                case ButtonFunction.MainMenu:
                    button.onClick.AddListener(MainMenu);
                    break;
                default:
                    break;
            }
        }

        [System.Serializable]
        public enum ButtonFunction { Exit, Restart, MainMenu }

        public void Exit()
        {
            GameManager.ExitGame();
        }

        public void Restart()
        {
            GameManager.RestartGame();
        }

        public void MainMenu()
        {
            GameManager.GoToMainMenu();
        }
    }
}