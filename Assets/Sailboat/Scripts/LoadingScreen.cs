using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

namespace Sailboat
{

    public class LoadingScreen : MonoBehaviour
    {
        public float minTime = 5f;
        public string mainSceneName;


        AsyncOperation asyncOperation;

        private IEnumerator Start()
        {
            AudioController.Instance.PlaySoundFlat("intro");
            asyncOperation = SceneManager.LoadSceneAsync(mainSceneName);
            asyncOperation.allowSceneActivation = false;
            yield return new WaitForSeconds(minTime);
            yield return End();
        }

        IEnumerator End()
        {
            TransitionUI.Instance.FadeIn();
            yield return new WaitForSeconds(0.5f);
            asyncOperation.allowSceneActivation = true;
        }

    }
}