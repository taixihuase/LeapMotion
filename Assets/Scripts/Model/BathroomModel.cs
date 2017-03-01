namespace Model
{
    public class BathroomModel : Core.MVC.Model
    {
        private bool canShowPourTips = true;

        public bool CanShowPourTips { get { return canShowPourTips; } }

        private bool canShowFillTips = true;

        public bool CanShowFillTips { get { return canShowFillTips; } }

        public override void Reset()
        {
            base.Reset();
            canShowPourTips = true;
            canShowFillTips = true;
        }

        public void SetPourTips(bool canShow)
        {
            canShowPourTips = canShow;
        }

        public void SetFillTips(bool canShow)
        {
            canShowFillTips = canShow;
        }
    }
}