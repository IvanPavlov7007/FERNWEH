using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sailboat.UI
{
    public class MainMenu : MonoBehaviour
    {
        private IEnumerator Start()
        {
            TransitionUI.Instance.FadeOut();
            yield return null;
        }

        IEnumerator End()
        {
            TransitionUI.Instance.FadeIn();
            yield return new WaitForSeconds(0.5f);
            loadFirstScene();
        }

        public void StartGame()
        {
            StartCoroutine(End());
        }

        private void loadFirstScene()
        {
            TransitionScene.transitionIndex = 0;
            SceneManager.LoadScene(TransitionScene.SCENE_NAME);
        }
    }
}