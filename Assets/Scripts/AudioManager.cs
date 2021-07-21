using UnityEngine.Audio;
using UnityEngine;
using System.Collections.Generic;

/*
THANK YOU BRACKEYS
*/

namespace AudioManagement
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        public List<Sound> sounds = new List<Sound>();
        public float masterVolumeMusic = 1f;
        public float masterVolumeSFX = 1f;

        void Awake()
        {
            if (AudioManager.instance != null)
            {
                Destroy(gameObject);
                Debug.LogError("There can only be one AudioManager");
                return;
            }

            DontDestroyOnLoad(gameObject);

            AudioManager.instance = this;

            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume * (sound.type == AudioType.MUSIC ? masterVolumeMusic : masterVolumeSFX);
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
                if (sound.playOnAwake)
                    sound.source.Play();
            }
        }

        public void Play(string _name)
        {
            Sound sound = sounds.Find(sounds => sounds.name == _name);
            if (sound != null)
                sound.source.Play();
            else
                Debug.LogError("No sound with name " + _name + " exists.");
        }

        public void Pause(string _name)
        {
            Sound sound = sounds.Find(sounds => sounds.name == _name);
            if (sound != null)
                sound.source.Pause();
            else
                Debug.LogError("No sound with name " + _name + " exists.");
        }

        public void Stop(string _name)
        {
            Sound sound = sounds.Find(sounds => sounds.name == _name);
            if (sound != null)
                sound.source.Stop();
            else
                Debug.LogError("No sound with name " + _name + " exists.");
        }

        public void SetVolume(string _name, float _volume)
        {
            Sound sound = sounds.Find(sounds => sounds.name == _name);
            if (sound != null)
                sound.volume = _volume;
            else
                Debug.LogError("No sound with name " + _name + " exists.");

            UpdateVolumes(sound.type);
        }

        public void SetMasterVolume(float _volume, AudioType _type)
        {
            if (_type == AudioType.MUSIC)
                masterVolumeMusic = _volume;
            else
                masterVolumeSFX = _volume;

            UpdateVolumes(_type);
        }

        public void UpdateVolumes(AudioType _type)
        {
            foreach (Sound sound in sounds)
            {
                if (sound.type == _type)
                    sound.source.volume = sound.volume * (sound.type == AudioType.MUSIC ? masterVolumeMusic : masterVolumeSFX);
            }
        }
    }
}
