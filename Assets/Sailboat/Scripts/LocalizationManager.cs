using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using TMPro;

namespace Sailboat
{
    public class LocalizationManager : MonoBehaviour
    {
        public static int chosenLocale = 0;
        [SerializeField]
        TMPro.TMP_Dropdown languageDropdown;

        private IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
            var myLocale = LocalizationSettings.SelectedLocale;
            var currentId = LocalizationSettings.AvailableLocales.Locales.IndexOf(myLocale);
            languageDropdown.value = currentId;
            //languageDropdown.value = chosenLocale;
            languageDropdown.onValueChanged.AddListener(onDropdownSelect);
        }

        void onDropdownSelect(int index)
        {
            StartCoroutine(SetLocale(index));
        }

        IEnumerator SetLocale(int localeId)
        {
            yield return LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
        }

    }
}