using System.Collections;
using UnityEngine;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using System;

namespace Sailboat
{
    public class SoundEnvironment : MonoBehaviour
    {
        public bool overtaking = true;
        public AudioClip environmentLoop;
        public StayArea stayArea;
        AudioSource aud;
        

        public event Action<SoundEnvironment> onStarted;
        public event Action<SoundEnvironment> onEnded;

        private void Awake()
        {
            if (stayArea != null)
            {
                stayArea.onStay.AddListener(areaEntered);
                stayArea.onExited.AddListener(areaExited);
            }
            onStarted += SoundManager.Instance.EnironmentStarted;
            onEnded += SoundManager.Instance.EnvironmentEndend;

            aud = SoundManager.Instance.newEnvironmentSource();
            aud.clip = environmentLoop;
            aud.loop = true;
            aud.Play();
            aud.volume = 0f;
        }

        TweenBase fading;

        void areaEntered(Entity entity)
        {
            if (!G.Instance.player.entityIsPlayer(entity))
                return;
            Play();
        }

        void areaExited(Entity entity)
        {
            if (!G.Instance.player.entityIsPlayer(entity))
                return;
            Stop();
        }


        public void Play()
        {
            
            if (fading != null)
                fading.Stop();
            fading = Tween.Volume(aud, 1f, 1f, 0f);
            onStarted?.Invoke(this);
        }

        public void Stop()
        {
            fading = Tween.Volume(aud, 0f, 1f, 0f);
            onEnded?.Invoke(this);
        }

    }
}