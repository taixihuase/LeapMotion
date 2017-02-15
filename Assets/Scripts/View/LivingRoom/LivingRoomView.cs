using Controller;
using Core.MVC;
using Leap;
using Leap.Unity.Interaction;
using System;
using UnityEngine;

namespace View.LivingRoom
{
    public class LivingRoomView : EntityView
    {
        void Start()
        {
            Init(LivingRoomCtrl.Instance.Model);
            Bind(Define.EventType.MoveCameraPos, MoveCamera);
            Bind(Define.EventType.InsertPlug, InsertPlug);
            Bind(Define.EventType.PutPlugOut, PutPlugOut);
        }

        [SerializeField]
        Transform InsertPos;

        [SerializeField]
        Transform Plug;

        [SerializeField]
        InteractionBehaviour PlugInteraction;

        Action<Hand> insertFunc = null;

        bool isInsert = false;

        float cd = 1f;

        float timer = 0;

        private void InsertPlug(params object[] arg1)
        {
            insertFunc = (h) =>
            {
                isInsert = true;
                PlugInteraction.isKinematic = true;
                PlugInteraction.useGravity = false;
                Plug.transform.localPosition = InsertPos.transform.localPosition;
                Plug.transform.localRotation = InsertPos.transform.localRotation;
            };
            PlugInteraction.OnHandReleasedEvent += insertFunc;
        }

        private void PutPlugOut(params object[] arg1)
        {
            if (insertFunc != null)
            {
                isInsert = false;
                timer = 0;
                PlugInteraction.useGravity = true;
                PlugInteraction.OnHandReleasedEvent -= insertFunc;
            }
        }

        void Update()
        {
            if(isInsert)
            {
                if(timer < cd)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    PlugInteraction.isKinematic = false;
                }
            }
        }
    }
}
