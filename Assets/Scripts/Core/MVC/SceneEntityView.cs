using Core.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.MVC
{
    public class SceneEntityView : EntityView
    {
        [SerializeField]
        protected Transform[] pos;

        protected void MoveCamera(params object[] arg1)
        {
            int index = (int)arg1[0];
            Action callback = null;
            if (arg1.Length >= 2)
            {
                callback = (Action)arg1[1];
            }
            CameraManager.Instance.MoveAndRotate(pos[index], callback);
        }

        public Transform GetStartPos()
        {
            return pos[0];
        }

        [Serializable]
        protected class SceneSoundGroup
        {
            [SerializeField] private string groupName;

            public string GroupName
            {
                get { return groupName; }
            }

            [SerializeField] private bool isLoopFromStart = false;

            public bool IsLoopFromStart
            {
                get { return isLoopFromStart; }
            }

            [SerializeField] private int audioSourceIndex;

            public int AudioSourceIndex
            {
                get { return audioSourceIndex; }
            }

            [SerializeField] private List<SoundManager.ScheduledSound> sounds;

            public List<SoundManager.ScheduledSound> Sounds
            {
                get { return sounds; }
            }
        }

        [SerializeField]
        private List<SceneSoundGroup> sounds;

        protected Dictionary<string, SceneSoundGroup> soundDict;

        protected void PlayEffectSounds(string soundGroupName)
        {
            if (soundDict.ContainsKey(soundGroupName))
            {
                var s = soundDict[soundGroupName];
                SoundManager.Instance.PlayScheduledEffectSounds(s.Sounds, s.IsLoopFromStart, s.AudioSourceIndex);
            }
        }

        protected void PlayEnvironmentSounds(string soundGroupName)
        {
            if (soundDict.ContainsKey(soundGroupName))
            {
                var s = soundDict[soundGroupName];
                SoundManager.Instance.PlayScheduledEnvironmentSounds(s.Sounds, s.IsLoopFromStart);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            soundDict = new Dictionary<string, SceneSoundGroup>();
            for (int i = 0; i < sounds.Count; i++)
            {
                string name = sounds[i].GroupName;
                if (string.IsNullOrEmpty(name))
                    continue;

                if (soundDict.ContainsKey(name) == false)
                    soundDict.Add(name, sounds[i]);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            SoundManager.Instance.ResetSceneSound();
        }
    }
}
