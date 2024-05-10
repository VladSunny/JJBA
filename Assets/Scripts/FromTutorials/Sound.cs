using UnityEngine.Audio;
using UnityEngine;

namespace JJBA.Audio
{
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume;
        [Range(.1f, 3f)]
        public float pitch;
        [Range(0f, 1f)]
        public float spatialBlend;

        public bool loop;
        public bool playOnAwake;

        [HideInInspector]
        public AudioSource source;
    }
}