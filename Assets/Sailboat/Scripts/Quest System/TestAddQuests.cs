using System.Collections.Generic;
using UnityEngine;

namespace Sailboat
{
    public class TestAddQuests : MonoBehaviour
    {

        [ContextMenu("Add wood")]
        public void AddWood()
        {
            G.Instance.inventory.AddItem("wood",  8, true);
        }

        public void AddFish()
        {
            G.Instance.inventory.AddItem("normal_fish", 10, true);
        }
    }
}