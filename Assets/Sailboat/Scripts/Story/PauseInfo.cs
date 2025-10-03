using System.Collections;
using UnityEngine;
using UnityEngine.Localization;

namespace Sailboat.PauseInfo
{
    [CreateAssetMenu(menuName ="Game/PauseInfo")]
    public class PauseInfo : ScriptableObject
    {
        public LocalizedString story;
        public LocalizedString controls;
        public Sprite vehicleDisplayHint;
    }
}