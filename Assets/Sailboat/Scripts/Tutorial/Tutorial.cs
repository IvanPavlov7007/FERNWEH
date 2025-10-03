using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using System;

namespace Sailboat.Tutorial
{

    [CreateAssetMenu(menuName ="Game/Tutorial")]
    public class Tutorial : ScriptableObject
    {
        public TutorialNode[] nodes;
        public float transitionTime = 1f;
        public float textTransitionTime = 0.5f;
    }


    [System.Serializable]
    public class TutorialNode
    {
        public TutorialAction actions;
        public float shownTime;
        public LocalizedString stringReference;
    }

    [System.Serializable] [Flags]
    public enum TutorialAction
    {
        None = 0,
        showHint = 1 << 0, 
        OverviewCamera = 1 << 1,
        FocusCamera = 1 << 2,
        CustomA = 1 << 3,
        CustomB = 1 << 4
    }
}