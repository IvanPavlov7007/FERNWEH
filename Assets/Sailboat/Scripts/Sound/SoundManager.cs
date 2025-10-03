using UnityEngine;
using UnityEngine.Audio;
using Pixelplacement;
using System.Collections.Generic;

namespace Sailboat
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField]
        AudioSource baseAudioSource;
        [SerializeField]
        AudioSource baseSFXAudioSource;
        List<AudioSource> sources = new List<AudioSource>();
        List<SoundEnvironment> currentEnvironments = new List<SoundEnvironment>();
        [SerializeField]
        SoundEnvironment defaultEnvironment;

        private List<AudioClip> sfx = new List<AudioClip>();
        private List<AudioClip> loops = new List<AudioClip>();

        private void Awake()
        {
            foreach (object o in Resources.LoadAll("Audio/SFX"))
            {
                sfx.Add((AudioClip)o);
            }
            foreach (object o in Resources.LoadAll("Audio/Loops"))
            {
                loops.Add((AudioClip)o);
            }
        }

        public AudioSource playSound(string soundId, float volume = 1f, float skipToTime = 0f)
        {
            AudioClip clip = GetAudioClip(soundId);
            return playSound(clip, volume, skipToTime);
        }

        public AudioSource playSound(AudioClip clip, float volume = 1f, float skipToTime = 0f)
        {
            baseSFXAudioSource.PlayOneShot(clip,volume);
            return baseSFXAudioSource;
        }

        public AudioClip GetAudioClip(string soundId)
        {
            return sfx.Find(x => x.name.ToLowerInvariant() == soundId.ToLowerInvariant());
        }

        private void Start()
        {
            defaultEnvironment.Play();
        }

        public AudioSource newEnvironmentSource()
        {
            var source = Instantiate(baseAudioSource.gameObject, transform).GetComponent<AudioSource>();
            sources.Add(source);
            return source;
        }
        public void removeSource(AudioSource source)
        {
            sources.Remove(source);
            Destroy(source.gameObject);
        }

        public void EnironmentStarted(SoundEnvironment env)
        {
            if (env != defaultEnvironment)
            {
                currentEnvironments.Add(env);
                defaultEnvironment.Stop();
            }
        }

        public void EnvironmentEndend(SoundEnvironment env)
        {
            if (env != defaultEnvironment)
            {
                currentEnvironments.Remove(env);
                defaultEnvironment.Play();
            }
        }
    }
}