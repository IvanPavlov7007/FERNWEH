using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Text;

namespace Sailboat
{
    public class ProgressionUI : MonoBehaviour
    {
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI headerText;
        public Image image;
        public Button upgradeButton;
        public TextMeshProUGUI buttonText;

        private void Awake()
        {
            upgradeButton.onClick.AddListener(buttonClicked);
        }

        private void OnEnable()
        {
            GameEvents.Instance.OnProgressionStateReached += stringDelegate;
            GameEvents.Instance.OnItemCollected += Redraw;
            Redraw();
        }

        private void OnDisable()
        {
            if (GameEvents.Instance == null)
                return;
            GameEvents.Instance.OnItemCollected -= Redraw;
            GameEvents.Instance.OnProgressionStateReached -= stringDelegate;
        }

        void stringDelegate(string st)
        {
            Redraw();
        }

        public void buttonClicked()
        {
            ProgressionManager.Instance.Progress();
        }

        public void Redraw(string _ = "", int __ = 0)
        {
            //crutch
            if (ProgressionManager.Instance.CurrentState == null)
                return;
            var state = ProgressionManager.Instance.CurrentState;
            descriptionText.text = state.description;
            headerText.text = state.header;
            image.sprite = state.vehicleDisplayHint;
            var nextState = ProgressionManager.Instance.NextState;
            if (nextState != null)
            {
                upgradeButton.gameObject.SetActive(true);


                StringBuilder sb = new StringBuilder();
                foreach (var it in nextState.requiredItems)
                {
                    sb.Append(UIManager.stringIconAndAmount(it.itemId, it.amount) + " ");
                }

                buttonText.text = sb.ToString();

                if (nextState.requiredItems.TrueForAll(x => G.Instance.inventory.HasItem(x.itemId, x.amount)))
                {
                    upgradeButton.interactable = true;
                }
                else
                {
                    upgradeButton.interactable = false;
                }
            }
            else
            {
                upgradeButton.gameObject.SetActive(false);
            }
        }


    }
}