using System.Collections.Generic;
using EssentialManagers.Scripts.SO;
using UnityEngine;

namespace EssentialManagers.Scripts.Managers
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [Header("References")] 
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        private AudioDataSO _audioData;
        private bool blockSound; 

        protected override void Awake()
        {
            base.Awake();
            LoadAudioData();
        }

        private void LoadAudioData()
        {
            _audioData = Resources.Load<AudioDataSO>("AudioDataSo");

            if (_audioData == null)
            {
                Debug.LogError("Failed to load AudioDataSO! Make sure it's in the Resources folder and named correctly.");
            }
            else
            {
                Debug.Log("AudioDataSO loaded successfully!");
            }
        }

        public void PlaySound(AudioTag audioTag)
        {
            if (IsSoundBlocked()) return; // Only blocks sound effects

            var clipData = GetAudioClip(audioTag);
            if (clipData != null && clipData.clip != null)
            {
                sfxSource.PlayOneShot(clipData.clip, clipData.volume);
            }
            else
            {
                Debug.LogWarning($"Sound '{audioTag}' not found in AudioData!");
            }
        }

        public void PlayMusic(AudioTag audioTag, bool loop = true)
        {
            if (IsMusicBlocked()) return; // Only blocks music

            var clipData = GetAudioClip(audioTag);
            if (clipData != null && clipData.clip != null)
            {
                musicSource.clip = clipData.clip;
                musicSource.volume = clipData.volume;
                musicSource.loop = loop;
                musicSource.Play();
            }
            else
            {
                Debug.LogWarning($"Music '{audioTag}' not found in AudioData!");
            }
        }


        public void StopMusic()
        {
            musicSource.Stop();
        }

        public void SetMusicVolume(float volume)
        {
            musicSource.volume = volume;
        }

        public void SetSFXVolume(float volume)
        {
            sfxSource.volume = volume;
        }

        private AudioDataSO.AudioClipData GetAudioClip(AudioTag tag)
        {
            if (_audioData == null || _audioData.audioClips == null) return null;

            foreach (var clipData in _audioData.audioClips)
            {
                if (clipData.tag == tag)
                {
                    return clipData;
                }
            }

            return null;
        }

        #region Bools & Setters

        private bool IsMusicBlocked()
        {
            return musicSource.mute;
        }

        private bool IsSoundBlocked()
        {
            return sfxSource.mute;
        }

        public void SetMusicBlocked(bool shouldBlock)
        {
            musicSource.mute = shouldBlock;
        }

        public void SetSoundBlocked(bool shouldBlock)
        {
            sfxSource.mute = shouldBlock;
        }


        #endregion
    }
}
