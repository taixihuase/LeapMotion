using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Manager
{
    public class SoundManager : MonoSingleton<SoundManager>
    {
        private AudioSource environmentSound;

        private List<AudioSource> effectSound;

        private Dictionary<int, IEnumerator> coroutines;

        private int effectAmount = 5;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            environmentSound = gameObject.AddComponent<AudioSource>();
            effectSound = new List<AudioSource>();
            for (int i = 0; i < effectAmount; i++)
            {
                effectSound.Add(gameObject.AddComponent<AudioSource>());
            }
            coroutines = new Dictionary<int, IEnumerator>();
        }

        private void PlayEnvironmentSound(AudioClip clip, bool isLoop = false, float volumn = 1f, bool breakCoroutine = false)
        {
            if (clip == null)
                return;

            StopEnvironmentSound(breakCoroutine);
            environmentSound.volume = volumn;
            environmentSound.clip = clip;
            environmentSound.loop = isLoop;
            environmentSound.Play();
        }

        public void PlayEnvironmentSound(string name, bool isLoop = false, float volumn = 1f)
        {
            Object res = ResourceManager.Instance.GetResource(Define.ResourceType.Sound, name);
            AudioClip clip;
            if (res == null)
            {
                ResourceManager.Instance.LoadAsset(Define.ResourceType.Sound, name, (o) =>
                {
                    if (o is AssetBundle)
                    {
                        clip = (o as AssetBundle).LoadAsset(name) as AudioClip;
                    }
                    else
                    {
                        clip = o as AudioClip;
                    }
                    if (clip != null)
                    {
                        PlayEnvironmentSound(clip, isLoop, volumn, true);
                    }
                }, ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);
            }
            else
            {
                if (res is AssetBundle)
                {
                    clip = (res as AssetBundle).LoadAsset(name) as AudioClip;
                }
                else
                {
                    clip = res as AudioClip;
                }
                PlayEnvironmentSound(clip, isLoop, volumn, true);
            }
        }

        public void RestartEnvironmentSound()
        {
            if (environmentSound.clip != null && environmentSound.isPlaying == false)
            {
                environmentSound.Play();
            }
        }

        public void StopEnvironmentSound(bool breakCoroutine = false)
        {
            if(environmentSound.clip != null)
            {
                environmentSound.Stop();
            }
            if (breakCoroutine)
            {
                if (coroutines.ContainsKey(-1))
                {
                    CoroutineManager.Instance.StopCoroutine(coroutines[-1]);
                    coroutines.Remove(-1);
                }
            }
        }

        private void PlayEffectSound(AudioClip clip, bool isLoop = false, float volumn = 1f, int soundIndex = 0, bool breakCoroutine = false)
        {
            if (clip == null)
                return;

            StopEffectSound(soundIndex, breakCoroutine);
            effectSound[soundIndex].volume = volumn;
            effectSound[soundIndex].clip = clip;
            effectSound[soundIndex].loop = isLoop;
            effectSound[soundIndex].Play();
        }

        public void PlayEffectSound(string name, bool isLoop = false, float volumn = 1f, int soundIndex = 0)
        {
            Object res = ResourceManager.Instance.GetResource(Define.ResourceType.Sound, name) as AudioClip;
            AudioClip clip;
            if (res == null)
            {
                ResourceManager.Instance.LoadAsset(Define.ResourceType.Sound, name, (o) =>
                {
                    if (o is AssetBundle)
                    {
                        clip = (o as AssetBundle).LoadAsset(name) as AudioClip;
                    }
                    else
                    {
                        clip = o as AudioClip;
                    }
                    if (clip != null)
                    {
                        PlayEffectSound(clip, isLoop, volumn, soundIndex, true);
                    }
                }, ResourceManager.Instance.IsDefaultAsync, ResourceManager.Instance.IsDefaultFromServer);
            }
            else
            {
                if (res is AssetBundle)
                {
                    clip = (res as AssetBundle).LoadAsset(name) as AudioClip;
                }
                else
                {
                    clip = res as AudioClip;
                }
                PlayEffectSound(clip, isLoop, volumn, soundIndex, true);
            }
        }

        public void RestartEffectSound(int soundIndex = 0)
        {
            if (effectSound[soundIndex].clip != null && effectSound[soundIndex].isPlaying == false)
            {
                effectSound[soundIndex].Play();
            }
        }

        public void StopEffectSound(int soundIndex = 0, bool breakCoroutine = false)
        {
            if (effectSound[soundIndex].clip != null)
            {
                effectSound[soundIndex].Stop();
            }
            if (breakCoroutine)
            {
                if (coroutines.ContainsKey(soundIndex))
                {
                    CoroutineManager.Instance.StopCoroutine(coroutines[soundIndex]);
                    coroutines.Remove(soundIndex);
                }
            }
        }

        public void PlayScheduledEnvironmentSounds(List<ScheduledSound> soundClips, bool isLoopFromStart = false)
        {
            if(soundClips.Count == 0)
                return;

            StopEnvironmentSound(true);
            var coroutine = PlayScheduledSounds(true, soundClips, isLoopFromStart, -1);
            coroutines.Add(-1, coroutine);
            CoroutineManager.Instance.StartCoroutine(coroutine);
        }

        public void PlayScheduledEffectSounds(List<ScheduledSound> soundClips, bool isLoopFromStart = false, int soundIndex = 0)
        {
            if (soundClips.Count == 0)
                return;

            StopEffectSound(soundIndex, true);
            var coroutine = PlayScheduledSounds(false, soundClips, isLoopFromStart, soundIndex);
            coroutines.Add(soundIndex, coroutine);
            CoroutineManager.Instance.StartCoroutine(coroutine);
        }

        IEnumerator PlayScheduledSounds(bool isEnvironment, List<ScheduledSound> soundClips, bool isLoopFromStart = false, int soundIndex = -1)
        {
            AudioClip[] clips = new AudioClip[soundClips.Count];
            float totalEndTime = 0;
            int lastAvaliableClip = -1;
            for (int i = 0; i < clips.Length; i++)
            {
                clips[i] = ResourceManager.Instance.GetResource(Define.ResourceType.Sound, soundClips[i].ClipName) as AudioClip;
                if (clips[i] == null)
                {
                    ResourceManager.Instance.LoadAsset(Define.ResourceType.Sound, soundClips[i].ClipName, (o) =>
                    {
                        if (o is AssetBundle)
                        {
                            clips[i] = (o as AssetBundle).LoadAsset(soundClips[i].ClipName) as AudioClip;
                        }
                        else
                        {
                            clips[i] = o as AudioClip;
                        }
                    }, false, false);

                    if (clips[i] == null)
                    {
                        continue;
                    }
                }
                lastAvaliableClip = i;
                yield return null;
            }

            if (lastAvaliableClip == -1)
            {
                coroutines.Remove(soundIndex);
                yield break;
            }

            totalEndTime = soundClips[lastAvaliableClip].DelayTime + clips[lastAvaliableClip].length;
            while (true)
            {
                float timer = 0;
                int current = 0;
                float availableLength = 0;
                float lengthCounter = 0;
                while (timer < totalEndTime)
                {
                    if (current <= lastAvaliableClip && timer > soundClips[current].DelayTime)
                    {
                        availableLength = soundClips[current].AvailableLength;
                        lengthCounter = 0;
                        if (isEnvironment)
                        {
                            PlayEnvironmentSound(clips[current], soundClips[current].IsLoop, soundClips[current].Volumn);
                        }
                        else
                        {
                            PlayEffectSound(clips[current], soundClips[current].IsLoop, soundClips[current].Volumn, soundIndex);
                        }
                        current++;
                    }
                    if (availableLength > 0 && lengthCounter > availableLength)
                    {
                        if (isEnvironment)
                        {
                            StopEnvironmentSound();
                        }
                        else
                        {
                            StopEffectSound(soundIndex);
                        }
                        availableLength = 0;
                    }
                    timer += Time.deltaTime;
                    lengthCounter += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                if (isLoopFromStart == false)
                {
                    coroutines.Remove(soundIndex);
                    yield break;
                }
            }
        }

        public void ResetSceneSound()
        {
            StopEnvironmentSound(true);
            for (int i = 0; i < effectSound.Count; i++)
            {
                StopEffectSound(i, true);
            }
        }

        [System.Serializable]
        public class ScheduledSound
        {
            [SerializeField] private string clipName;

            public string ClipName { get { return clipName; } }

            [SerializeField] private float delayTime;

            public float DelayTime { get { return delayTime; } }

            [SerializeField] private float availableLength;

            public float AvailableLength { get { return availableLength; } }

            [SerializeField] private float volumn = 1f;

            public float Volumn { get { return volumn; } }

            [SerializeField] private bool isLoop = false;

            public bool IsLoop { get { return isLoop; } }
        }
    }
}
