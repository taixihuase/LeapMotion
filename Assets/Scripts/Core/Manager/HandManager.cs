using Leap.Unity;
using UnityEngine;

namespace Core.Manager
{
    public class HandManager : Singleton<HandManager>
    {
        private IHandModel leftHand; 

        public IHandModel LeftHand
        {
            get { return leftHand; }
        }

        private IHandModel rightHand;

        public IHandModel RightHand
        {
            get { return rightHand; }
        }

        private IHandModel leftAttachment;

        public IHandModel LeftAttachment
        {
            get { return leftAttachment; }
        }

        private IHandModel rightAttachment;

        public IHandModel RightAttachment
        {
            get { return rightAttachment; }
        }

        private IHandModel leftPhysics;

        public IHandModel LeftPhysics
        {
            get { return leftPhysics; }
        }

        private IHandModel rightPhysics;

        public IHandModel RightPhysics
        {
            get { return rightPhysics; }
        }

        private HandPool pool;

        public HandPool Pool
        {
            get { return pool; }
        }

        public HandManager()
        {
            pool = Object.FindObjectOfType<HandPool>();
            HandPool.ModelGroup graphics = pool.GetGroup("Graphics_Hands");
            leftHand = graphics.LeftModel;
            rightHand = graphics.RightModel;
            HandPool.ModelGroup physics = pool.GetGroup("Physics_Hands");
            leftPhysics = physics.LeftModel;
            rightPhysics = physics.RightModel;
            HandPool.ModelGroup attachments = pool.GetGroup("Attachment_Hands");
            leftAttachment = attachments.LeftModel;
            rightAttachment = attachments.RightModel;
        }
    }
}
