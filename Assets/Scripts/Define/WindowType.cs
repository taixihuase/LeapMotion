﻿using System.ComponentModel;

namespace Define
{
    public enum WindowType
    {
        [Description("Menu")]
        Menu = 0,

        [Description("LivingRoom")]
        LivingRoom = 1,

        [Description("Bathroom")]
        Bathroom = 2,

        [Description("Kitchen")]
        Kitchen = 3,

        [Description("Hallway")]
        Hallway = 4,
    }
}
