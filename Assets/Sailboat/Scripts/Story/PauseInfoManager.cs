using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using TMPro;
using System;

namespace Sailboat.PauseInfo
{
    public class PauseInfoManager : MonoBehaviour
    {
        [SerializeField] private LocalizeStringEvent storyStringEvent;
        [SerializeField] private LocalizeStringEvent controlsStringEvent;
        [SerializeField] private Image hintImage;
        [SerializeField] private bool loadInfoOnAwake = true;

        public PauseInfo currentPauseInfo { get; private set; }

        private const string PATH = "Scriptable Objects/Pause Infos/";
        private const string PauseInfoName = "Level ";

        private void Awake()
        {
            if(loadInfoOnAwake)
            {
                LoadPauseInfo();
            }
        }

        private void LoadPauseInfo()
        {
            currentPauseInfo = Resources.Load<PauseInfo>(PATH + PauseInfoName + GameManager.CurrentNumber().ToString());
            if (currentPauseInfo == null)
            {
                Debug.LogError(gameObject.name + ": Couldnt find the Pause Info for this level " + currentPauseInfo);
                return;
            }

            storyStringEvent.StringReference = currentPauseInfo.story;
            controlsStringEvent.StringReference = currentPauseInfo.controls;
            hintImage.sprite = currentPauseInfo.vehicleDisplayHint;
        }
    }
}