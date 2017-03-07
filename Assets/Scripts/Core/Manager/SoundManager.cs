using UnityEngine;

namespace Core.Manager
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        private AudioSource environmentSound;

        public AudioSource EnvironmentSound
        {
            get { return environmentSound; }
        }

        private AudioSource effectSound;

        public AudioSource EffectSound
        {
            get { return effectSound; }
        }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            environmentSound = gameObject.AddComponent<AudioSource>();
            effectSound = gameObject.AddComponent<AudioSource>();
        }

        public void PlayEnvironmentSound(string name, bool isLoop = false, float volumn = 1f)
        {
            AudioClip clip = ResourceManager.Instance.GetResource(Define.ResourceType.Sound, name) as AudioClip;
            if(clip == null)
            {
                ResourceManager.Instance.LoadAsset(Define.ResourceType.Sound, name, (o) =>
                {
                    clip = o as AudioClip;
                }, false, false);

                if(clip == null)
                {
                    return;
                }
            }

            environmentSound.volume = volumn;
            environmentSound.clip = clip;
            environmentSound.loop = isLoop;
            environmentSound.Play();
        }

        public void RestartEnvironmentSound()
        {
            if (environmentSound.clip != null && environmentSound.isPlaying == false)
            {
                environmentSound.Play();
            }
        }

        public void StopEnvironmentSound()
        {
            if(environmentSound.clip != null)
            {
                environmentSound.Stop();
            }
        }

        public void PlayEffectSound(string name, bool isLoop = false, float volumn = 1f)
        {
            AudioClip clip = ResourceManager.Instance.GetResource(Define.ResourceType.Sound, name) as AudioClip;
            if (clip == null)
            {
                ResourceManager.Instance.LoadAsset(Define.ResourceType.Sound, name, (o) =>
                {
                    clip = o as AudioClip;
                }, false, false);

                if (clip == null)
                {
                    return;
                }
            }

            effectSound.volume = volumn;
            effectSound.clip = clip;
            effectSound.loop = isLoop;
            effectSound.Play();
        }

        public void RestartEffectSound()
        {
            if (effectSound.clip != null && effectSound.isPlaying == false)
            {
                effectSound.Play();
            }
        }

        public void StopEffectSound()
        {
            if (effectSound.clip != null)
            {
                effectSound.Stop();
            }
        }
    }
}
