using System.Collections;
using UnityEngine;
using Pixelplacement;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

namespace Sailboat
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        bool worldInteractive = true;

        public static string MAIN_MENU = "Main Menu";

        public bool WorldInteractive
        {
            get
            {
                return worldInteractive;
            }
            set
            {
                worldInteractive = value;
            }
        }

        private void Awake()
        {
            G.Instance.uIInputController.onPaused += onPaused;
            G.Instance.uIInputController.onResumed += onResumed;
        }

        void onPaused()
        {
            if (worldInteractive)
            {
                G.Instance.playerInput.SwitchCurrentActionMap("Menu");
                PauseUI.Instance.Show();
            }
            Time.timeScale = 0f;
        }

        public void TutorialOn(bool on)
        {
            if (on)
            {
                G.Instance.playerInput.SwitchCurrentActionMap("Tutorial");
            }
            else
            {
                G.Instance.playerInput.SwitchCurrentActionMap("Sailing");

            }
        }

        void onResumed()
        {
            if (worldInteractive)
            {
                G.Instance.playerInput.SwitchCurrentActionMap("Sailing");
                PauseUI.Instance.Hide();
            }
            Time.timeScale = 1f;
        }

        public void interactedWithNPC(Interactable interactable)
        {
            if (!WorldInteractive)
                return;

            NPC npc = interactable as NPC;

            AudioController.Instance.PlaySoundFlat("drawKnife1");
            
            GameEvents.NPCInteractedWith(npc.npcName);

            if (npc.currentDefaultDialog != null)
            {
                G.Instance.dialogUI.StartDialog(npc.currentDefaultDialog);
            }
            else if (npc.currentTradeOffer != null)
            {
                G.Instance.tradeUI.OpenTrade(npc.currentTradeOffer);
            }
            else
            {
                Debug.Log("NPC has nothing to say");
            }
        }

        public static void RestartGame()
        {
            Instance.onResumed();
            TransitionScene.transitionIndex = CurrentNumber();
            SceneManager.LoadScene(TransitionScene.SCENE_NAME);
        }

        public static void ExitGame()
        {
            Instance.onResumed();
            Application.Quit();
        }

        public static void GoToMainMenu()
        {
            Instance.onResumed();
            SceneManager.LoadScene(MAIN_MENU);
        }

        public static int CurrentNumber()
        {
            return int.Parse(Regex.Match(SceneManager.GetActiveScene().name, @"\d+").Value);
        }
    }
}