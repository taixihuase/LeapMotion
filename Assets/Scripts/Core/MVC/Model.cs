using System;

namespace Core.MVC
{
    public abstract class Model : Notifier
    {
        protected string name;

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = GetHashCode().ToString();
                }
                return name;
            }
        }

        public void Refresh(Enum attribute, params object[] e)
        {
            RaiseEvent(string.Format("{0}{1}", Name, attribute), e);
        } 
    }
}