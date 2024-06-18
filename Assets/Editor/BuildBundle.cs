using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildBundle : MonoBehaviour
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        //创建一个文件路径
        SetBundleName();
        string dir = "AssetBundles";
        if (Directory.Exists(dir) == false)
        {
            Directory.CreateDirectory(dir);
        }
        //输出路径,BuildAssetBundleOptions,平台
        BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        //AddVUP(dir);
        
    }
    [MenuItem("Tool/SetFileBundleName")]
    static void SetBundleName()
    {

        #region 设置资源的AssetBundle的名称和文件扩展名
        UnityEngine. Object[] selects = Selection.objects;
        foreach (UnityEngine. Object selected in selects)
        {
            string path = AssetDatabase.GetAssetPath(selected);
            AssetImporter asset = AssetImporter .GetAtPath(path);
            asset.assetBundleName = selected.name; //设置Bundle文件的名称
            asset.assetBundleVariant = "vup";//设置Bundle文件的扩展名
            asset.SaveAndReimport();
            
        }
        AssetDatabase .Refresh();
        #endregion
    }
}
