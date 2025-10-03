using System.Collections;
using UnityEngine;
using Pixelplacement;

namespace Sailboat
{
    public class GameCanvas : Singleton<GameCanvas>
    {
        public Canvas canvas { get; private set; }

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
        }

    }
}