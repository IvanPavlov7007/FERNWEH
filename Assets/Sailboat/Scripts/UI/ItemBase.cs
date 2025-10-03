using System.Collections;
using UnityEngine;

namespace Sailboat
{
    [CreateAssetMenu(menuName = "Game/ItemBase")]
    public class ItemBase : ScriptableObject
    {
        public string itemId;
        public string displayName;
        public string description;
        public Sprite icon;
        public string iconName;
    }
}