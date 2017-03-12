using System.Collections;
using Boo.Lang;
using DG.Tweening;
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

        private void PlayEnvironmentSound(AudioClip clip, bool isLoop = false, float volumn = 1f)
        {
            if (clip == null)
                return;

            StopEnvironmentSound();
            environmentSound.volume = volumn;
            environmentSound.clip = clip;
            environmentSound.loop = isLoop;
            environmentSound.Play();
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

            PlayEnvironmentSound(clip, isLoop, volumn);
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

        private void PlayEffectSound(AudioClip clip, bool isLoop = false, float volumn = 1f)
        {
            if (clip == null)
                return;

            StopEffectSound();
            effectSound.volume = volumn;
            effectSound.clip = clip;
            effectSound.loop = isLoop;
            effectSound.Play();
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

            PlayEffectSound(clip, isLoop, volumn);
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

        public void PlayScheduledEnvironmentSounds(List<ScheduledSound> soundClips, bool isLoopFromStart = false)
        {
            if(soundClips.Count == 0)
                return;

            CoroutineManager.Instance.StartCoroutine(PlayScheduledSounds(true, soundClips, isLoopFromStart));
        }

        public void PlayScheduledEffectSounds(List<ScheduledSound> soundClips, bool isLoopFromStart = false)
        {
            if (soundClips.Count == 0)
                return;

            CoroutineManager.Instance.StartCoroutine(PlayScheduledSounds(false, soundClips, isLoopFromStart));
        }

        IEnumerator PlayScheduledSounds(bool isEnvironment, List<ScheduledSound> soundClips, bool isLoopFromStart = false)
        {
            AudioClip[] clips = new AudioClip[soundClips.Count];
            float endTime = 0;
            int lastAvaliableClip = -1;
            for (int i = 0; i < clips.Length; i++)
            {
                clips[i] = ResourceManager.Instance.GetResource(Define.ResourceType.Sound, soundClips[i].ClipName) as AudioClip;
                if (clips[i] == null)
                {
                    ResourceManager.Instance.LoadAsset(Define.ResourceType.Sound, soundClips[i].ClipName, (o) =>
                    {
                        clips[i] = o as AudioClip;
                    }, false, false);

                    if (clips[i] == null)
                    {
                        continue;
                    }
                    lastAvaliableClip = i;
                }
                yield return null;
            }
            if (lastAvaliableClip == -1)
                yield break;

            endTime = soundClips[lastAvaliableClip].DelayTime + clips[lastAvaliableClip].length;
            while (true)
            {
                float timer = 0;
                int current = 0;
                while (timer < endTime)
                {
                    if (current <= lastAvaliableClip && timer > soundClips[current].DelayTime)
                    {
                        if (isEnvironment)
                        {
                            PlayEnvironmentSound(clips[current], false, soundClips[current].Volumn);
                        }
                        else
                        {
                            PlayEffectSound(clips[current], false, soundClips[current].Volumn);
                        }
                        current++;
                    }
                    timer += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                if (isLoopFromStart == false)
                {
                    yield break;
                }
            }
        }

        public class ScheduledSound
        {
            private string clipName;

            public string ClipName { get { return clipName; } }

            private float delayTime;

            public  float DelayTime { get { return delayTime; } }

            private float volumn;

            public float Volumn { get { return volumn; } }

            public ScheduledSound(string clipName, float delay, float volumn = 1f)
            {
                this.clipName = clipName;
                delayTime = delay;
                this.volumn = volumn;
            }
        }
    }
}
