#if UNITY_ANDROID 

using System.IO;
using UnityEditor;
using UnityEditor.Android;

public class AndroidAppBundlePostProcessBuild : IPostGenerateGradleAndroidProject
{
    public int callbackOrder { get { return 0; } }

    void IPostGenerateGradleAndroidProject.OnPostGenerateGradleAndroidProject(string path)
    {
        if (EditorUserBuildSettings.buildAppBundle)
        {
            Directory.Delete(Path.Combine(path, @"../UnityDataAssetPack/src/main/assets/bin/Data/UnitySubsystems"), true);
        }
    }
}

#endif