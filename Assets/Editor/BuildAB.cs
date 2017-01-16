using UnityEditor;
using Tool;

namespace MyEditor
{
    public class BuildAB
    {
        [MenuItem("AssetBundle/Build Windows AssetBundle")]
        static void BuildWinAB()
        {
            BuildPipeline.BuildAssetBundles(PathHelper.Instance.AssetBundlePath, BuildAssetBundleOptions.ForceRebuildAssetBundle | BuildAssetBundleOptions.IgnoreTypeTreeChanges | BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
            AssetDatabase.Refresh();
        }
    }
}