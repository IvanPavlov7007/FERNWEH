using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Sailboat.UI
{
    public class ResolutionSelector : MonoBehaviour
    {

        [System.Serializable]
        public struct ResolutionOption
        {
            public int width;
            public int height;
        }

        [Header("Setup")]
        public ResolutionOption[] resolutions = new ResolutionOption[]
{
    new ResolutionOption { width = 1920, height = 1080 },
    new ResolutionOption { width = 1600, height = 900 },
    new ResolutionOption { width = 1366, height = 768 },
    new ResolutionOption { width = 1280, height = 720 },

    new ResolutionOption { width = 1920, height = 1200 },
    new ResolutionOption { width = 1680, height = 1050 },
    new ResolutionOption { width = 1440, height = 900 },
    new ResolutionOption { width = 1280, height = 800 },

    new ResolutionOption { width = 2560, height = 1080 },
    new ResolutionOption { width = 3440, height = 1440 },

    new ResolutionOption { width = 1600, height = 1200 },
    new ResolutionOption { width = 1024, height = 768 },
};    // Fill this list in Inspector
        public Button buttonPrefab;              // Assign a UI Button prefab
        public Transform contentParent;          // Where to place the buttons

        private void Start()
        {
            if (buttonPrefab == null || contentParent == null)
            {
                Debug.LogError("ResolutionSelector: Missing buttonPrefab or contentParent");
                return;
            }

            foreach (var res in resolutions)
            {
                CreateResolutionButton(res);
            }
        }

        private void CreateResolutionButton(ResolutionOption res)
        {
            Button btn = Instantiate(buttonPrefab, contentParent);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{res.width} x {res.height}";
            btn.onClick.AddListener(() => ApplyResolution(res.width, res.height));
        }

        private void ApplyResolution(int width, int height)
        {
            Screen.SetResolution(width, height,Screen.fullScreenMode);
            Debug.Log($"Resolution set to {width} x {height}");
        }
    }
}