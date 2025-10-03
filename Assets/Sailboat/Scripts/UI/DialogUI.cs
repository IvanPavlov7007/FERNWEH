using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sailboat.UI
{
    public class DialogUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI dialogText;
        [SerializeField] private Transform choicesContainer;
        [SerializeField] private Button choiceButtonPrefab;
        [SerializeField] private Button itemChoiceButtonPrefab;

        private DialogNode currentNode;
        private DialogChoice implicitChoice; // auto-advance if no button

        public void StartDialog(Dialog dialog)
        {
            G.Instance.gameManager.WorldInteractive = false;
            gameObject.SetActive(true);
            ShowNode(dialog.startNode);
        }

        private void ShowNode(DialogNode node)
        {
            currentNode = node;
            dialogText.text = node.text;

            G.Instance.inventoryUI.gameObject.SetActive(node.openInventory);

            // Clear old choices
            foreach (Transform child in choicesContainer)
                Destroy(child.gameObject);

            implicitChoice = null;

            if (node.choices != null)
            {
                foreach (var choice in node.choices)
                {
                    // Item choice
                    if (choice.requiredItem != null && !string.IsNullOrEmpty(choice.requiredItem.itemId))
                    {
                        CreateItemButtonChoice(choice);
                        continue;
                    }

                    // Implicit auto-choice (empty text)
                    if (string.IsNullOrEmpty(choice.text))
                    {
                        if (implicitChoice != null)
                            Debug.LogWarning($"Multiple implicit choices in node {node}");
                        implicitChoice = choice;
                        continue;
                    }

                    // Normal choice
                    CreateAnswerButtonChoice(choice);
                }
            }

            // Always enable blocking button to prevent re-interaction
            G.Instance.blockingButton.Show(HandleOutsideClick);
        }

        private void HandleOutsideClick()
        {
            if (implicitChoice != null)
            {
                HandleChoice(implicitChoice);
            }
            else if (currentNode.choices == null || currentNode.choices.Count == 0)
            {
                // End dialog
                close();
            }
            else
            {
                // Node has choices, so ignore outside clicks (stay blocked)
                G.Instance.blockingButton.Show(HandleOutsideClick);
            }
        }

        private void CreateAnswerButtonChoice(DialogChoice choice)
        {
            var btn = Instantiate(choiceButtonPrefab, choicesContainer);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;
            btn.onClick.AddListener(() => HandleChoice(choice));
        }

        private void CreateItemButtonChoice(DialogChoice choice)
        {
            var item = choice.requiredItem;
            var text = $" Give {UIManager.stringIconAndAmount(item.itemId, item.amount)}";

            var btn = Instantiate(itemChoiceButtonPrefab, choicesContainer);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = text;

            if (!G.Instance.inventory.HasItem(choice.requiredItem.itemId, choice.requiredItem.amount))
                btn.interactable = false;

            btn.onClick.AddListener(() => HandleChoice(choice));
        }

        private void close()
        {
            gameObject.SetActive(false);
            G.Instance.gameManager.WorldInteractive = true;
        }

        private void HandleChoice(DialogChoice choice)
        {
            if (choice == null)
            {
                close();
                return;
            }

            if (choice.requiredItem != null && choice.requiredItem.notEmpty())
            {
                G.Instance.inventory.RemoveItem(choice.requiredItem.itemId, choice.requiredItem.amount);
                AudioController.Instance.PlaySoundFlat("handleCoins2");
            }

            // Quest logic
            if (choice.questToStart != null)
                QuestManager.Instance.StartQuest(choice.questToStart.questId);

            if (!string.IsNullOrEmpty(choice.phraseId))
                GameEvents.PhraseSeen(choice.phraseId);

            // Trade logic
            if (choice.tradeToOpen != null)
            {
                close();
                G.Instance.tradeUI.OpenTrade(choice.tradeToOpen);
                return;
            }

            // Next node or close
            if (choice.nextNode != null)
                ShowNode(choice.nextNode);
            else
            {
                close();
            }
        }
    }
}
