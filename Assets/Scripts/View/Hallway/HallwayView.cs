using Core.Manager;
using Core.MVC;

namespace View.Hallway
{
    public class HallwayView : SceneEntityView
    {
        void Start()
        {
            if (GlobalManager.Instance.SceneMode == GlobalManager.Mode.PracticeMode)
            {

            }
            else
            {
                PlayEnvironmentSounds("HallwayT");
            }
        }
    }
}
