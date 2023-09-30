#if UNITY_IOS
 
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
//20221206ATTのFrameとinfo.plistを自動で追加するスクリプト
 
public static class ATTPostProcessBuild
{
    [PostProcessBuild]//ビルド時に実行する
    private static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
    {
        // Info.plist に Privacy - Tacking Usage Description(NSUserTrackingUsageDescription)を追加する（ステップ２）
        string infoPlistPath = buildPath + "/Info.plist";
        PlistDocument infoPlist = new PlistDocument();
        infoPlist.ReadFromFile(infoPlistPath);
        PlistElementDict root = infoPlist.root;
        root.SetString("NSUserTrackingUsageDescription", "あなたの好みに合わせた広告を表示するために使用されます");
        infoPlist.WriteToFile(infoPlistPath);
 
        // PBXProjectクラスというのを用いてAppTrackingTransparency.frameworkを追加していきます（ステップ３）
        string pbxProjectPath = PBXProject.GetPBXProjectPath(buildPath);
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(pbxProjectPath);
        string targetGuid = pbxProject.GetUnityFrameworkTargetGuid();
        pbxProject.AddFrameworkToProject(targetGuid, "AppTrackingTransparency.framework", true);
        pbxProject.WriteToFile(pbxProjectPath);
    }
}
 
#endif