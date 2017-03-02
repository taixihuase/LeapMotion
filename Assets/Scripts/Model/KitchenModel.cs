namespace Model
{
    public class KitchenModel : Core.MVC.Model
    {
        private bool canShowLightTips = true;

        public bool CanShowLightTips { get { return canShowLightTips; } }

        private bool canShowFireTips = true;

        public bool CanShowFireTips { get { return canShowFireTips; } }

        private bool canShowFridgeTips = true;

        public bool CanShowFridgeTips { get { return canShowFridgeTips; } }

        public override void Reset()
        {
            base.Reset();
            canShowLightTips = true;
            canShowFireTips = true;
            canShowFridgeTips = true;
        }

        public void SetLightTips(bool canShow)
        {
            canShowLightTips = canShow;
        }

        public void SetFireTips(bool canShow)
        {
            canShowFireTips = canShow;
        }

        public void SetFridgeTips(bool canShow)
        {
            canShowFridgeTips = canShow;
        }
    }
}
