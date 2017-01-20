using UnityEngine;
using Leap.Unity;
using Leap.Unity.Interaction;

namespace Core.Manager
{
    public sealed class InteractionManager : MonoSingleton<InteractionManager>
    {
        private Leap.Unity.Interaction.InteractionManager interaction;

        public Leap.Unity.Interaction.InteractionManager Interaction
        {
            get { return interaction; }
        }

        private LeapHandController handController;

        public LeapHandController HandController
        {
            get { return handController; }
        }

        private LeapProvider provider;

        public LeapProvider Provider
        {
            get { return provider; }
        }

        private HandPool handPool;

        public HandPool HandPool
        {
            get { return handPool; }
        }

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            handController = FindObjectOfType<LeapHandController>();
            provider = FindObjectOfType<LeapProvider>();
            handPool = FindObjectOfType<HandPool>();
            interaction = FindObjectOfType<Leap.Unity.Interaction.InteractionManager>();

            ResourceManager.Instance.LoadAsset(Define.ResourceType.Interaction, "InteractionCube", (obj) =>
            {
                GameObject go = obj as GameObject;
                if (go != null)
                {
                    GameObject instance = Instantiate(go);
                    instance.transform.position = new Vector3(0, 0, 0.3f);
                    instance.GetComponent<InteractionBehaviour>().Manager = interaction;
                }
            }, true, true);
        }
    }
}
