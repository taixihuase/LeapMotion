using UnityEngine;

namespace Core.Manager
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private GameObject uiCamera;

        public GameObject UICamera
        {
            get { return uiCamera; }
        }

        private Transform root;

        public Transform Root
        {
            get
            {
                return root;
            }
        }

        private void Awake()
        {
            GameObject root = new GameObject("UIRoot");
            root.transform.parent = transform;
            root.transform.localPosition = new Vector3(9999, 9999, 0);
            root.layer = LayerMask.NameToLayer("UI");
            this.root = root.transform;

            UIRoot uiRoot = root.AddComponent<UIRoot>();
            uiRoot.scalingStyle = UIRoot.Scaling.Constrained;
            uiRoot.manualWidth = 1366;
            uiRoot.manualHeight = 768;
            uiRoot.fitWidth = true;
            uiRoot.fitHeight = false;

            root.AddComponent<UIPanel>();

            Rigidbody rigidbody = root.AddComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;

            uiCamera = new GameObject("UICamera");
            uiCamera.transform.parent = root.transform;
            uiCamera.transform.localPosition = Vector3.zero;
            uiCamera.layer = root.layer;

            Camera camera = uiCamera.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.Depth;
            camera.cullingMask = 1 << uiCamera.layer;
            camera.orthographic = true;
            camera.orthographicSize = 1;
            camera.depth = 1;
            camera.farClipPlane = 1;
            camera.nearClipPlane = -1;

            uiCamera.AddComponent<UICamera>();
        }

        public void Init()
        {

        }
    }
}
