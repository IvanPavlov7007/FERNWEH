using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Sailboat
{

    public class UIElementSound : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
    {
        public string clickSound, selectSound;
        public float volume = 0.6f;

        public void OnPointerClick(PointerEventData eventData)
        {
            AudioController.Instance.PlaySoundFlat(clickSound, volume);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioController.Instance.PlaySoundFlat(selectSound, volume);
        }

        //private void Awake()
        //{
        //    button = GetComponent<Button>();
        //}

    }
}