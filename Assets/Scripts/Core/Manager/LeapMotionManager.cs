using UnityEngine;
using Leap.Unity;
using Leap.Unity.Interaction;

namespace Core.Manager
{
    public sealed class LeapMotionManager : MonoSingleton<LeapMotionManager>
    {
        private InteractionManager interaction;

        public InteractionManager Interaction
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

        protected override void Init()
        {
            handController = FindObjectOfType<LeapHandController>();
            provider = FindObjectOfType<LeapProvider>();
            handPool = FindObjectOfType<HandPool>();
            interaction = FindObjectOfType<InteractionManager>();
        }
    }
}
