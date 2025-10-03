using System.Collections.Generic;
using UnityEngine;

namespace Sailboat
{
    [CreateAssetMenu(menuName ="Game/Progression State")]
    public class ProgressionState : ScriptableObject
    {
        public int index;
        public VehicleType vehicleType;
        public Sprite vehicleDisplayHint;
        public string header;
        public string description;
        public List<DialogItem> requiredItems;
        public bool requiresHome;
    }
}