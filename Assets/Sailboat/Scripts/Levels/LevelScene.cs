using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sailboat
{

    public class LevelScene : MonoBehaviour
    {
        protected IEnumerator MoveToTheNextScene()
        {
            TransitionUI.Instance.FadeIn();
            yield return new WaitForSeconds(0.5f);

            int currentNumber = GameManager.CurrentNumber();
            string sceneName = TransitionScene.SCENE_NAME;
            TransitionScene.transitionIndex = currentNumber + 1;
            SceneManager.LoadScene(sceneName);
            yield return null;
        }

        protected virtual IEnumerator Start()
        {
            TransitionUI.Instance.FadeOut();
            yield return new WaitForSeconds(0.5f);
        }

        protected void playTargetReachedSound()
        {
            AudioController.Instance.PlaySoundFlat("impact1", 2f);
        }
    }
}
