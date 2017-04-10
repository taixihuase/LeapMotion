using System.ComponentModel;

namespace Define
{
    public enum SceneType
    {
        [Description("")]
        None = -1,

        [Description("StartScene")]
        StartScene = 1,

        [Description("MenuScene")]
        MenuScene = 2,

        [Description("MainScene")]
        MainScene = 3,
    }
}
