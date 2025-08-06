using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;

using System.IO;

public class PostBuildStep
{
    // Set the IDFA request description:
    const string k_TrackingDescription = "This lets us provide you with a more personalized experience, relevant content, and promotions.";

    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToXcode)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            AddPListValues(pathToXcode);
            ModifySwiftAndBitcode(pathToXcode);
        }
    }

    // Implement a function to read and write values to the plist file:
    static void AddPListValues(string pathToXcode)
    {
        // Retrieve the plist file from the Xcode project directory:
        string plistPath = pathToXcode + "/Info.plist";
        PlistDocument plistObj = new PlistDocument();


        // Read the values from the plist file:
        plistObj.ReadFromString(File.ReadAllText(plistPath));

        // Set values from the root object:
        PlistElementDict plistRoot = plistObj.root;

        // Set the description key-value in the plist:
        plistRoot.SetString("NSUserTrackingUsageDescription", k_TrackingDescription);

        // Save changes to the plist:
        File.WriteAllText(plistPath, plistObj.WriteToString());
    }

    public static void PostExport(string buildPath)
    {
        //DisableBitcode(buildPath);
        ModifySwiftAndBitcode(buildPath);
        LinkLibraries(buildPath);
        ModifyPList(buildPath);
        AddCapabilities(buildPath);

       // AdjustEditor.OnPostprocessBuild(BuildTarget.iOS, buildPath);

       // Debug.Log("BuildPostProcessor Build Path: " + buildPath);
       // Debug.Log("BuildPostProcessor Finished");
    }

    public static void LinkLibraries(string path)
    {
        // linked library
        string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);
        string target = proj.GetUnityMainTargetGuid();

        //proj.AddFrameworkToProject(target, "CoreTelephony.framework", true);
        //proj.AddFrameworkToProject(target, "VideoToolbox.framework", true);
        //proj.AddFrameworkToProject(target, "libresolv.tbd", true);
        //proj.AddFrameworkToProject(target, "libiPhone-lib.a", true);
        //proj.AddFrameworkToProject(target, "CoreText.framework", true);
        proj.AddFrameworkToProject(target, "Metal.framework", false);
        //proj.AddFrameworkToProject(target, "CoreML.framework", true);
        //proj.AddFrameworkToProject(target, "Accelerate.framework", true);

        proj.AddFrameworkToProject(target, "iAd.framework", true);
        proj.AddFrameworkToProject(target, "AdSupport.framework", true);
        //proj.AddFrameworkToProject(target, "AppTrackingTransparency.framework", true); //May need for iOS14 when the time comes
        proj.AddFrameworkToProject(target, "StoreKit.framework", true);
        proj.AddFrameworkToProject(target, "AdSupport.framework", true);
        proj.AddFrameworkToProject(target, "CoreTelephony.framework", true);

        File.WriteAllText(projPath, proj.WriteToString());
    }

    public static void ModifyPList(string buildPath)
    {
        //string plistPath = Application.dataPath.Replace("/Assets", "") + "/Builds/iOS/Info.plist";
        string plistPath = buildPath + "/Info.plist";
        //Debug.Log("plist start" + plistPath);

        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        int indexEnd = PlayerSettings.bundleVersion.LastIndexOf(".") + 1;
        string modifiedVersionNumber = PlayerSettings.bundleVersion.Substring(0, indexEnd) + PlayerSettings.iOS.buildNumber;

        PlistElementDict rootDic = plist.root;
        var cameraPermission = "NSCameraUsageDescription";
        var micPermission = "NSMicrophoneUsageDescription";
        rootDic.SetString(cameraPermission, "Video need to use camera");
        rootDic.SetString(micPermission, "Voice call need to user mic");

        rootDic.SetString("MinimumOSVersion", "10.0");

        rootDic.SetString("CFBundleShortVersionString", modifiedVersionNumber);

        rootDic.SetString("NSPhotoLibraryUsageDescription", "Choose a new Profile image");

        rootDic.CreateArray("UIRequiredDeviceCapabilities");

        PlistElementDict dict = null;
        if (rootDic.values.ContainsKey("NSAppTransportSecurity"))
        {
            //Debug.Log("get NSAppTransportSecurity");
            dict = rootDic.values["NSAppTransportSecurity"].AsDict();
        }
        else
        {
           // Debug.Log("create NSAppTransportSecurity");
            dict = rootDic.CreateDict("NSAppTransportSecurity");
        }

        if (!dict.values.ContainsKey("NSAllowsArbitraryLoads"))
        {
          //  Debug.Log("create NSAllowsArbitraryLoads");
            dict.CreateDict("NSAllowsArbitraryLoads");
        }
        dict.SetBoolean("NSAllowsArbitraryLoads", true);

        // Set encryption usage boolean
        string encryptKey = "ITSAppUsesNonExemptEncryption";
        plist.root.SetBoolean(encryptKey, false);

        // remove exit on suspend if it exists.
        string exitsOnSuspendKey = "UIApplicationExitsOnSuspend";
        if (plist.root.values.ContainsKey(exitsOnSuspendKey))
        {
            plist.root.values.Remove(exitsOnSuspendKey);
        }

        File.WriteAllText(plistPath, plist.WriteToString());
    }

    public static void AddCapabilities(string buildPath)
    {
        string pbxPath = PBXProject.GetPBXProjectPath(buildPath);
        var capManager = new ProjectCapabilityManager(pbxPath, "ios.entitlements", "Unity-iPhone");
        capManager.AddSignInWithApple();
        //capManager.AddKeychainSharing(new string[] { "$(AppIdentifierPrefix)com.gametaco.keys" });
        capManager.AddPushNotifications(false); //bool is to note if it's in development mode.
        capManager.WriteToFile();
    }

    public static void DisableBitcode(string projPath)
    {
        string projectPath = PBXProject.GetPBXProjectPath(projPath);

        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);

#if UNITY_2019_3_OR_NEWER
        var targetGuid = pbxProject.GetUnityMainTargetGuid();
#else
        var targetName = PBXProject.GetUnityTargetName();
        var targetGuid = pbxProject.TargetGuidByName(targetName);
#endif
        pbxProject.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
        pbxProject.WriteToFile(projectPath);

        var projectInString = File.ReadAllText(projectPath);

       // Debug.Log("BuildPostProcessor -- projectInString: " + projectInString);

        projectInString = projectInString.Replace("ENABLE_BITCODE = YES;", $"ENABLE_BITCODE = NO;");
        File.WriteAllText(projectPath, projectInString);
    }

    public static void ModifySwiftAndBitcode(string path)
    {
        string projPath = PBXProject.GetPBXProjectPath(path);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

        string targetGuid = proj.GetUnityMainTargetGuid();

        foreach (var framework in new[] { targetGuid, proj.GetUnityFrameworkTargetGuid() })
        {
            proj.SetBuildProperty(framework, "ENABLE_BITCODE", "NO");
          //  proj.SetBuildProperty(framework, "EMBEDDED_CONTENT_CONTAINS_SWIFT", "YES");
         //   proj.SetBuildProperty(framework, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
         //   proj.SetBuildProperty(framework, "SWIFT_VERSION", "5.1"); //Seem to have to change this for new versions of the FB SDK
        }

      //  Debug.Log("BuildPostProcessor -- ModifySwiftAndBitcode -- projectString: " + proj);

        proj.WriteToFile(projPath);
    }
}
#endif