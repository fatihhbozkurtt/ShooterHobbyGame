using System;
using UnityEngine;

namespace EssentialManagers.Scripts.SO
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Audio/Audio Data")]
    public class AudioDataSO : ScriptableObject
    {
        [Serializable]
        public class AudioClipData
        {
            public AudioTag tag; // Using enum instead of string
            public AudioClip clip;
            [Range(0f, 1f)] public float volume = 1f;
        }

        public AudioClipData[] audioClips;

        public AudioClipData GetAudioClipData(AudioTag tag)
        {
            foreach (var data in audioClips)
            {
                if (data.tag == tag)
                    return data;
            }

            Debug.LogWarning($"Audio tag '{tag}' not found!");
            return null;
        }
    }

    [Serializable]
    public enum AudioTag
    {
        None,
        ButtonClick,
        BackgroundMusic,
        Jump,
        Explosion,
        Win,
        Lose
    }
}