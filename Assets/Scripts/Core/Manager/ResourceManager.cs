using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Define;
using Tool;

namespace Core.Manager
{
    public class ResourceManager
    {
        private Dictionary<string, Object> loadedAssets = new Dictionary<string, Object>();

        private Dictionary<string, WWW> loadingAssets = new Dictionary<string, WWW>();

        private Dictionary<ResourceType, List<string>> allAssets = new Dictionary<ResourceType, List<string>>();

        public Object GetResource(ResourceType type, string name)
        {
            Object obj = null;
            string path = PathHelper.Instance.GetResourcePath(type, name);
            if (loadedAssets.TryGetValue(path, out obj) == false)
            {
                Debug.LogWarning(string.Format("资源 \"{0}\" 尚未加载", name));
                if (loadingAssets.ContainsKey(name))
                {
                    Debug.LogWarning(string.Format("资源 \"{0}\" 仍在加载中", name));
                }
            }
            return obj;
        }

        public void LoadAssetAsync(ResourceType type, string name)
        {
            string path = PathHelper.Instance.GetResourcePath(type, name);
            if (string.IsNullOrEmpty(path) == false)
            {
                if (loadedAssets.ContainsKey(path))
                    return;

                if (loadingAssets.ContainsKey(path))
                    return;

                CoroutineManager.Instance.StartCoroutine(LoadAssetAsync(path, typeof(Object)));
            }
        }

        private IEnumerator LoadAssetAsync(string path, System.Type type)
        {
            string fullPath = PathHelper.Instance.CombineStreamingFile(path);
            string url = PathHelper.Instance.AddFileProtocol(fullPath);
            WWW www = new WWW(url);
            loadingAssets.Add(path, www);
            yield return www;

            if(string.IsNullOrEmpty(www.error))
            {
                AssetBundle ab = AssetBundle.LoadFromMemory(www.bytes);
                loadedAssets.Add(path, ab);
                loadingAssets.Remove(path);
                Debug.Log(string.Format("加载资源包 \"{0}\" 成功", path));
            }
            else
            {
                loadingAssets.Remove(path);
                Debug.LogError(string.Format("加载资源包 \"{0}\" 失败", path));
            }
        }

        public bool IsResLoading(ResourceType type, string name)
        {
            string path = PathHelper.Instance.GetResourcePath(type, name);
            return loadingAssets.ContainsKey(path);
        }

        public bool IsResLoaded(ResourceType type, string name)
        {
            string path = PathHelper.Instance.GetResourcePath(type, name);
            return loadedAssets.ContainsKey(path);
        }

        public WWW GetLoadingWWW(ResourceType type, string name)
        {
            WWW www = null;
            string path = PathHelper.Instance.GetResourcePath(type, name);
            loadingAssets.TryGetValue(path, out www);
            return www;
        }

        private string GetAssetName(string fullPath)
        {
            int index = fullPath.LastIndexOf('/');
            return fullPath.Substring(index + 1, fullPath.Length - index - 1);
        }
    }
}