using Core;
using Define;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Tool
{
    public class PathHelper : Singleton<PathHelper>
    {
        private Dictionary<ResourceType, string> resPath = new Dictionary<ResourceType, string>();

        public const string FilePrefix = "file:///";

        public string AddFileProtocol(string path)
        {
            if(string.IsNullOrEmpty(path))
            {
                return null;
            }
            else if(path.Contains(FilePrefix) == false)
            {
                return FilePrefix + path;
            }
            else
            {
                return path;
            }
        }

        private string GetAssetName(string fullPath)
        {
            int index = fullPath.LastIndexOf('/');
            return fullPath.Substring(index + 1, fullPath.Length - index - 1);
        }

        public const string WindowPath = "UI/Window";

        public const string ScenePath = "Scene";

        public string AssetBundlePath
        {
            get
            {
                return Application.streamingAssetsPath;
            }
        }

        public string CombineStreamingFile(string path)
        {
            if(string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            #if UNITY_ANDROID && !UNITY_EDITOR
            return Path.Combine(Application.dataPath + "!assets", path).Replace("\\", "/");
            #else
            return Path.Combine(AssetBundlePath, path).Replace("\\", "/");
            #endif
        }

        public string CombineLocalFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            return Path.Combine(Application.dataPath + "/Resources", path).Replace("\\", "/");
        }

        public string GetResourcePath(ResourceType type, string name)
        {
            if(string.IsNullOrEmpty(name) || resPath.ContainsKey(type) == false)
            {
                return null;
            }
            else
            {
                return string.Format("{0}/{1}", resPath[type], name);
            }
        }


    }
}