using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using System.Linq;
using System;

namespace Sailboat
{
    public class ProgressionManager : Singleton<ProgressionManager>
    {
        public List<ProgressionState> progressionStates = new List<ProgressionState>();

        ProgressionState currentState;
        ProgressionState nextState;
        public ProgressionState CurrentState => currentState;
        public ProgressionState NextState => nextState;

        public void LoadAllStates()
        {
            progressionStates.AddRange(Resources.LoadAll<ProgressionState>("Scriptable Objects/Progression States/"));
            progressionStates = progressionStates.OrderBy(x=>x.index).ToList();
            currentState = progressionStates[0];
            nextState = progressionStates[1];
        }

        public void Progress()
        {
            Progress(nextState);
        }

        public void Progress(ProgressionState state)
        {
            int currentIndex = progressionStates.IndexOf(state);
            currentState = state;
            Player.Instance.switchVehicle(state.vehicleType);

            foreach (var it in state.requiredItems)
            {
                G.Instance.inventory.RemoveItem(it.itemId, it.amount);
            }

            if (currentIndex < progressionStates.Count - 1)
                nextState = progressionStates[currentIndex + 1];
            else
                nextState = null;

            GameEvents.ProgressionReaced(state.name);
        }

    }
}