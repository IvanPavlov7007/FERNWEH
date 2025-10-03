using System.Collections;
using UnityEngine;
using Pixelplacement;
using TMPro;

namespace Sailboat
{
    [RequireComponent(typeof(StayArea))]
    public class BoundariesManager : MonoBehaviour
    {
        [SerializeField]
        GameObject fogAsset;
        [SerializeField]
        GameObject textHintAsset;

        StayArea stayArea;
        GameObject fogInstance;
        GameObject textHintInstance;

        ITransparencyController fogTransparency;
        ITransparencyController textTransparency;

        [SerializeField]
        float appearTime, dissapearTime;

        [SerializeField]
        bool stayingInTheArea = true;

        SimpleTimer showTimer, hideTimer;
        float a, b;
        float currentAlpha;

        private void Awake()
        {
            stayArea = GetComponent<StayArea>();
        }

        void Start()
        {
            var playerTr = G.Instance.player.bodyTransform;
            if (fogInstance == null)
                fogInstance = Instantiate(fogAsset, playerTr.position,Quaternion.identity,playerTr);
            if (textHintInstance == null)
                textHintInstance = Instantiate(textHintAsset, GameCanvas.Instance.transform);

            fogTransparency = Transparency.GetController(getPS());
            textTransparency = Transparency.GetController(text());
            fogTransparency.Alpha = 0f;
            textTransparency.Alpha = 0f;
            currentAlpha = 0f;
        }

        private void OnEnable()
        {
            stayArea.onExited.AddListener(onExited);
            stayArea.onStay.AddListener(onStayed);
        }

        private void OnDisable()
        {
            if (stayArea == null)
                return;
            stayArea.onExited.RemoveListener(onExited);
            stayArea.onStay.RemoveListener(onStayed);
        }

        void setTransparency(float alpha)
        {
            currentAlpha = alpha;
            fogTransparency.Alpha = alpha;
            textTransparency.Alpha = alpha;
        }

        private void Update()
        {
            if(showTimer != null)
            {
                if (!showTimer.tick(Time.deltaTime))
                {
                    setTransparency(
                        Mathf.Lerp(a, b, showTimer.currentTime / showTimer.timeoutTime.Value));
                }
                else
                {
                    setTransparency(1f);
                    showTimer = null;
                }
            }

            if (hideTimer != null )
            {
                if (!hideTimer.tick(Time.deltaTime))
                {
                    setTransparency(
                        Mathf.Lerp(a, b, hideTimer.currentTime / hideTimer.timeoutTime.Value));
                }
                else
                {
                    setTransparency(0f);
                    hideTimer = null;
                }
            }
        }

        void onStayed(Entity entity)
        {
            if (!Player.Instance.entityIsPlayer(entity))
                return;

            if (!stayingInTheArea)
            {
                hideTimer = new SimpleTimer(dissapearTime);
                showTimer = null;
                a = currentAlpha;
                b = 0f;
            }
            stayingInTheArea = true;
        }

        private ParticleSystem getPS()
        {
            return fogInstance.GetComponent<ParticleSystem>();
        }

        private TextMeshProUGUI text()
        {
            return textHintInstance.GetComponent<TextMeshProUGUI>();
        }

        void onExited(Entity entity)
        {
            if (!Player.Instance.entityIsPlayer(entity))
                return;

            stayingInTheArea = false;
            hideTimer = null;
            showTimer = new SimpleTimer(appearTime);
            a = currentAlpha;
            b = 1f;
        }
    }
}