namespace Model
{
    public class LivingRoomModel : Core.MVC.Model
    {
        private bool canShowClickTips = true;

        public bool CanShowClickTips { get { return canShowClickTips; } }

        private bool canShowSocketTips = true;

        public bool CanShowSocketTips { get { return canShowSocketTips; } }

        private bool canShowWarningTips = true;

        public bool CanShowWarningTips { get { return canShowWarningTips; } }

        public override void Reset()
        {
            base.Reset();
            canShowClickTips = true;
            canShowSocketTips = true;
            canShowWarningTips = true;
        }

        public void SetClickTips(bool canShow)
        {
            canShowClickTips = canShow;
        }

        public void SetSocketTips(bool canShow)
        {
            canShowSocketTips = canShow;
        }

        public void SetWarningTips(bool canShow)
        {
            canShowWarningTips = canShow;
        }
    }
}
