using System.Collections;
using UnityEngine;
using Pixelplacement;
namespace Sailboat
{
    public class MinimapAnimation : MonoBehaviour
    {
        Vector3 endPos;
        Vector3 endScale;

        [SerializeField]
        Vector3 centerPos;
        [SerializeField]
        Vector3 centerScale;

        [SerializeField]
        float delay = 2f, duration = 1.5f;

        private void Awake()
        {
            endPos = (transform as RectTransform).anchoredPosition3D;
            endScale = transform.localScale;
        }

        private IEnumerator Start()
        {
            yield return StartCoroutine(animation());
        }

        IEnumerator animation()
        {
            Tween.AnchoredPosition(transform as RectTransform, centerPos,endPos, duration, delay, Tween.EaseOut);
            Tween.LocalScale(transform, centerScale, endScale, duration, delay, Tween.EaseOut);
            yield return new WaitForSeconds(duration);
        }
    }
}