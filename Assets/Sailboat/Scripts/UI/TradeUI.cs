using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sailboat.UI
{
    public class TradeUI : MonoBehaviour
    {

        [Header("References")]
        public Transform itemsContainer;
        public Button itemRowPrefab;

        private TradeOffer currentOffer;

        public void OpenTrade(TradeOffer offer)
        {
            currentOffer = offer;
            

            RefreshUI();
            gameObject.SetActive(true);
        }

        private void RefreshUI()
        {
            foreach (Transform child in itemsContainer)
                Destroy(child.gameObject);

            foreach (var entry in currentOffer.tradeEntries)
            {
                var row = Instantiate(itemRowPrefab, itemsContainer);
                row.GetComponentInChildren<TextMeshProUGUI>().text =
                    $"{entry.giveItemId} x{entry.giveAmount} - {entry.getItemId} x {entry.getItemAmount}";

                row.onClick.AddListener(() =>
                {
                    TryBuy(entry);
                });
            }

            //close
            var closeButton = Instantiate(itemRowPrefab, itemsContainer);
            closeButton.GetComponentInChildren<TextMeshProUGUI>().text ="Close trade";

            closeButton.onClick.AddListener(() =>
            {
                CloseTrade();
            });
        }

        private void TryBuy(TradeEntry entry)
        {
            if (G.Instance.inventory.HasItem(entry.giveItemId, entry.giveAmount))
            {
                G.Instance.inventory.RemoveItem(entry.giveItemId, entry.giveAmount);
                G.Instance.inventory.AddItem(entry.getItemId, entry.getItemAmount);
                RefreshUI();
            }
            else
            {
                Debug.Log($"Not enough {entry.giveItemId}!");
            }
        }

        public void CloseTrade()
        {
            gameObject.SetActive(false);
        }
    }
}