using System.ComponentModel;

namespace Define
{
    public enum WindowType
    {
        [Description("WinTest")]
        WinTest = 0,

        [Description("UGUITest")]
        UGUITest = 1,

        [Description("LivingRoom")]
        LivingRoom = 2,

        [Description("Bathroom")]
        Bathroom = 3,

        [Description("Kitchen")]
        Kitchen = 4,

        [Description("Hallway")]
        Hallway = 5,
    }
}
