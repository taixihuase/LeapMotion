using Core.Manager;
using Core.MVC;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

        }

        public void OnSelectMode2()
        {
            SceneManager.Instance.LoadSceneAsync(Define.SceneType.MainScene, UnityEngine.SceneManagement.LoadSceneMode.Single, (sc, mode) =>
            {
                UIManager.Instance.CloseSceneWindows(Define.SceneType.MenuScene);
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
                currentPanel = 0;
                pos[currentPanel].SetActive(false);
                pos[0].SetActive(true);
            }
        }
    }
}
