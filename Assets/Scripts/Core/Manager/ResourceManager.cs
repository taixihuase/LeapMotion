﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Define;
using Tool;
using System;

namespace Core.Manager
{
    public sealed class ResourceManager : Singleton<ResourceManager>
    {
#if UNITY_EDITOR
        public bool IsDefaultAsync = false;

        public bool IsDefaultFromServer = false;
#else
        public bool IsDefaultAsync = true;

        public bool IsDefaultFromServer = true;
#endif

        private Dictionary<string, UnityEngine.Object> loadedAssets = new Dictionary<string, UnityEngine.Object>();

        private Dictionary<string, object> loadingAssets = new Dictionary<string, object>();

        public void RegistResource(ResourceType type, string name, UnityEngine.Object res)
        {
            if (res != null && !string.IsNullOrEmpty(name))
            {
                string path = PathHelper.Instance.GetResourcePath(type, name);
                loadedAssets[path] = res;
            }
        }

        public UnityEngine.Object GetResource(ResourceType type, string name)
        {
            UnityEngine.Object obj = null;
            string path;
            if (IsDefaultAsync)
            {
                path = PathHelper.Instance.GetAssetBundlePath(type);
            }
            else
            {
                path = PathHelper.Instance.GetResourcePath(type, name);
            }
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            if (loadedAssets.TryGetValue(path, out obj) == false)
            {
                if (IsDefaultAsync)
                {
                    path = PathHelper.Instance.GetResourcePath(type, name);
                    if (loadedAssets.TryGetValue(path, out obj) == false)
                    {
                        Debug.LogWarning(string.Format("资源 \"{0}\" 尚未加载", name));
                    }
                    if (loadingAssets.ContainsKey(name))
                    {
                        Debug.LogWarning(string.Format("资源 \"{0}\" 仍在加载中", name));
                        return null;
                    }
                }
                else
                {
                    path = PathHelper.Instance.GetAssetBundlePath(type);
                    if (loadedAssets.TryGetValue(path, out obj) == false)
                    {
                        Debug.LogWarning(string.Format("资源 \"{0}\" 尚未加载", name));
                    }
                    if (loadingAssets.ContainsKey(name))
                    {
                        Debug.LogWarning(string.Format("资源 \"{0}\" 仍在加载中", name));
                        return null;
                    }
                }
            }

            if (obj is AssetBundle)
            {
                AssetBundle ab = obj as AssetBundle;
                if (!ab.Contains(name))
                {
                    path = PathHelper.Instance.GetResourcePath(type, name);
                    if (loadedAssets.TryGetValue(path, out obj) == false)
                    {
                        Debug.LogWarning(string.Format("资源 \"{0}\" 尚未加载", name));
                        return null;
                    }
                }
            }
            return obj;
        }

        public WWW GetLoadingWWW(ResourceType type, string name)
        {
            object www = null;
            string path = PathHelper.Instance.GetResourcePath(type, name);
            if (loadingAssets.TryGetValue(path, out www))
            {
                if (www is WWW)
                {
                    return www as WWW;
                }
            }
            return null;
        }

        public AssetBundleCreateRequest GetLoadingRequest(ResourceType type, string name)
        {
            object req = null;
            string path = PathHelper.Instance.GetResourcePath(type, name);
            if (loadingAssets.TryGetValue(path, out req))
            {
                if (req is AssetBundleCreateRequest)
                {
                    return req as AssetBundleCreateRequest;
                }
            }
            return null;
        }

        public void LoadAsset(ResourceType type, string name, Action<UnityEngine.Object> callback = null, bool isAsync = false, bool fromServer = false)
        {
            string path;
            if (isAsync)
            {
                path = PathHelper.Instance.GetAssetBundlePath(type);
            }
            else
            {
                path = PathHelper.Instance.GetResourcePath(type, name);
            }
            if (string.IsNullOrEmpty(path) == false)
            {
                if (loadedAssets.ContainsKey(path))
                {
                    if(callback != null)
                    {
                        callback(loadedAssets[path]);
                    }
                    return;
                }

                if (loadingAssets.ContainsKey(path))
                    return;

                if (isAsync)
                {
                    if (fromServer)
                        CoroutineManager.Instance.StartCoroutine(LoadAssetAsync(name, path, callback));
                    else
                        CoroutineManager.Instance.StartCoroutine(LoadAssetFromFileAsync(name, path, callback));
                }
                else
                {
                    LoadAsset(path, callback);
                }
            }
        }

        private void LoadAsset(string path, Action<UnityEngine.Object> callback)
        {
            AssetBundle ab = null;
            UnityEngine.Object obj = null;
#if !UNITY_EDITOR
            string fullPath = PathHelper.Instance.AddAssetbundlePostfix(PathHelper.Instance.CombineStreamingFile(path)).ToLower();
            ab = AssetBundle.LoadFromFile(fullPath);
#endif
            if (ab != null)
            {
                string[] str = ab.GetAllAssetNames();
                if (str.Length > 0)
                {
                    obj = ab.LoadAsset(str[0]);
                    loadedAssets.Add(path, obj);
                    if (callback != null)
                    {
                        callback(obj);
                    }
                }
                ab.Unload(false);
            }
            else
            {
                obj = Resources.Load(path);
                if (obj == null)
                {
                    Debug.LogError(string.Format("加载资源 \"{0}\" 失败", path));
                    return;
                }
                loadedAssets.Add(path, obj);
                if (callback != null)
                {
                    callback(obj);
                }
            }
        }

        private IEnumerator LoadAssetAsync(string name, string path, Action<UnityEngine.Object> callback)
        {
            string fullPath = PathHelper.Instance.CombineStreamingFile(path).ToLower();
            string url = PathHelper.Instance.AddAssetbundlePostfix(PathHelper.Instance.AddFileProtocol(fullPath));
            using (WWW www = new WWW(url))
            {
                loadingAssets.Add(path, www);
                yield return www;

                if (string.IsNullOrEmpty(www.error))
                {
                    UnityEngine.Object obj = null;
                    AssetBundle ab = AssetBundle.LoadFromMemory(www.bytes);
                    string[] str = ab.GetAllAssetNames();
                    if (str.Length > 0)
                    {
                        obj = ab.LoadAsset(name);
                        loadedAssets.Add(path, ab);
                    }
                    Debug.Log(string.Format("加载资源包 \"{0}\" 成功", path));
                    if (callback != null)
                    {
                        callback(obj);
                    }
                }
                else
                {
                    Debug.LogError(string.Format("加载资源包 \"{0}\" 失败", path));
                }
                loadingAssets.Remove(path);
            }
        }

        private IEnumerator LoadAssetFromFileAsync(string name, string path, Action<UnityEngine.Object> callback)
        {
            string fullPath = PathHelper.Instance.AddAssetbundlePostfix(PathHelper.Instance.CombineStreamingFile(path).ToLower());
            AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(fullPath);
            loadingAssets.Add(path, req);
            yield return req;

            while (req.isDone == false || req.progress < 0.99f)
            {
                Debug.Log(req.progress);
                yield return new WaitForSecondsRealtime(0.1f);
            }

            UnityEngine.Object obj = null;
            AssetBundle ab = req.assetBundle;
            string[] str = ab.GetAllAssetNames();
            if (str.Length > 0)
            {
                obj = ab.LoadAsset(name);
                loadedAssets.Add(path, ab);
            }
            loadingAssets.Remove(path);
            Debug.Log(string.Format("加载资源包 \"{0}\" 成功", path));
            if (callback != null)
            {
                callback(obj);
            }
        }    

        public bool IsResLoading(ResourceType type, string name)
        {
            string path;
            if (IsDefaultAsync)
            {
                path = PathHelper.Instance.GetAssetBundlePath(type);
            }
            else
            {
                path = PathHelper.Instance.GetResourcePath(type, name);
            }
            return loadingAssets.ContainsKey(path);
        }

        public bool IsResLoaded(ResourceType type, string name)
        {
            string path;
            if (IsDefaultAsync)
            {
                path = PathHelper.Instance.GetAssetBundlePath(type);
            }
            else
            {
                path = PathHelper.Instance.GetResourcePath(type, name);
            }
            return loadedAssets.ContainsKey(path);
        }

        public bool RemoveLoadedAsset(string key)
        {
            return loadedAssets.Remove(key);
        }
    }
}