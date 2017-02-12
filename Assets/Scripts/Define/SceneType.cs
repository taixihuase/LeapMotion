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

        [Description("MainScene")]
        MainScene = 2,
    }
}
