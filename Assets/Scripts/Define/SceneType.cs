using System.ComponentModel;

namespace Define
{
    public enum SceneType
    {
        [Description("")]
        None = -1,

        [Description("TestScene")]
        TestScene = 0,

        [Description("TestLoad")]
        TestLoad = 1,

        [Description("StartScene")]
        StartScene = 2,

        [Description("MenuScene")]
        MenuScene = 3,

        [Description("MainScene")]
        MainScene = 4,
    }
}
