using Leap.Unity;
using System;
using UnityEngine;

namespace Core.Manager
{
    public class HandManager : MonoSingleton<HandManager>
    {
        private IHandModel leftHand; 

        public IHandModel LeftHand
        {
            get { return leftHand; }
            private set
            {
                if (value != null)
                {
                    if (!value.isActiveAndEnabled)
                    {
                        GameObject clone = GameObject.Find("PepperBaseCutLeftHand(Clone)");
                        if (clone != null && leftHand != clone)
                        {
                            leftHand = clone.GetComponent<IHandModel>();
                            if (OnHandChanged != null)
                            {
                                OnHandChanged.Invoke(leftHand, 0);
                            }
                        }
                    }
                    else if(value != leftHand)
                    {
                        leftHand = value;
                        if (OnHandChanged != null)
                        {
                            OnHandChanged.Invoke(leftHand, 0);
                        }
                    }
                }
            }
        }

        private IHandModel rightHand;

        public IHandModel RightHand
        {
            get { return rightHand; }
            private set
            {
                if (value != null)
                {
                    if (!value.isActiveAndEnabled)
                    {
                        GameObject clone = GameObject.Find("PepperBaseCutRightHand(Clone)");
                        if (clone != null && rightHand != clone)
                        {
                            rightHand = clone.GetComponent<IHandModel>();
                            if(OnHandChanged != null)
                            {
                                OnHandChanged.Invoke(rightHand, 1);
                            }
                        }
                    }
                    else if (value != rightHand)
                    {
                        rightHand = value;
                        if (OnHandChanged != null)
                        {
                            OnHandChanged.Invoke(rightHand, 1);
                        }
                    }
                }
            }
        }

        public Action<IHandModel, int> OnHandChanged;

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

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            pool = LeapMotionManager.Instance.HandPool;
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

        private void Update()
        {
            if(!LeftHand.isActiveAndEnabled || !RightHand.isActiveAndEnabled)
            {
                HandPool.ModelGroup graphics = pool.GetGroup("Graphics_Hands");
                LeftHand = graphics.LeftModel;
                RightHand = graphics.RightModel;
            }
        }
    }
}