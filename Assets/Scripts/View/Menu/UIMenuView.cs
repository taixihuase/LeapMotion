using Core.Manager;
using Core.MVC;
using UnityEngine;

namespace View.Menu
{
    public class UIMenuView : UIView
    {
        private int currentPanel = 0;

        [SerializeField]
        GameObject quitPanel;

        public void OnQuit()
        {
            quitPanel.SetActive(true);
        }

        public void OnGuide()
        {
            currentPanel = 1;
            pos[0].SetActive(false);
            pos[1].SetActive(true);
        }
      
        public void OnSelectMode()
        {
            currentPanel = 2;
            pos[0].SetActive(false);
            pos[2].SetActive(true);
        }

        public void OnSelectMode1()
        {
            GlobalManager.Instance.SceneMode = GlobalManager.Mode.PracticeMode;
            CameraManager.Instance.ChangeScene(0.5f, 0.2f, 0.5f, () =>
            {
                UIManager.Instance.CloseSceneWindows(Define.SceneType.MenuScene);
                SceneManager.Instance.LoadSceneAsync(Define.SceneType.MainScene, UnityEngine.SceneManagement.LoadSceneMode.Single, null);
            });
        }

        public void OnSelectMode2()
        {
            GlobalManager.Instance.SceneMode = GlobalManager.Mode.ThrillingMode;
            CameraManager.Instance.ChangeScene(0.5f, 0.2f, 0.5f, () =>
            {
                UIManager.Instance.CloseSceneWindows(Define.SceneType.MenuScene);
                SceneManager.Instance.LoadSceneAsync(Define.SceneType.MainScene, UnityEngine.SceneManagement.LoadSceneMode.Single, null);
            });
        }

        public void OnCancelQuit()
        {
            quitPanel.SetActive(false);
        }

        public void OnSubmitQuit()
        {
            Application.Quit();
        }

        public void OnReturn()
        {
            if(currentPanel != 0)
            {
                pos[currentPanel].SetActive(false);
                currentPanel = 0;
                pos[0].SetActive(true);
            }
        }
    }
}
