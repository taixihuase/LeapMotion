﻿using Core.Manager;
using Core.MVC;
using Tool;
using Define;
using UnityEngine;

namespace View.Hallway
{
    public class UIHallwayView : Core.MVC.View
    {
        public void OnClickToOtherScene(string sceneName)
        {
            GameObject hallway = FindObjectOfType<HallwayView>().gameObject;
            Object scene = ResourceManager.Instance.GetResource(ResourceType.Scene, sceneName);
            if (hallway != null)
            {
                CameraManager.Instance.ChangeScene(0.5f, 0.2f, 0.5f, () =>
                {
                    Destroy(hallway);
                    GameObject obj = Instantiate(scene) as GameObject;
                    Transform startPos = obj.GetComponent<EntityView>().GetStartPos();
                    CameraManager.Instance.MoveAndRotate(startPos);
                    UIManager.Instance.CloseWindow(SceneType.MainScene, WindowType.Hallway);
                    UIManager.Instance.OpenWindow(SceneType.MainScene, EnumDescriptionTool.GetEnum<WindowType>(sceneName));
                });
            }
        }
    }
}