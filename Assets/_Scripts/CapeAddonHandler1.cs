using Assets.SimpleZip;
using Main.Controller;
using Newtonsoft.Json;
using SuperStarSdk;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ImageCropper;

public class CapeAddonHandler1 : MonoBehaviour
{
    public static CapeAddonHandler1 Instance;

    [Header("Screen Reference")]
    public GameObject PickImageScreen;
    public GameObject ViewImageScreen;
    public GameObject HDTextureScreen;
    public GameObject[] TutorialScreen;
    public GameObject TutorialParent;

    [Header("Create Texture")]
    public RawImage rawImageViewCroppedImage;
    public RawImage rawImageForMakeTexture;
    public Animator CharacterAnim;
    public GameObject MainCharacter;
    public bool IsCustomCapeAddon = false;

    public Texture2D pikedImageTexture;
    public Texture2D coppedImageTexture;
    public Texture2D coppedImageTextureForDummyModel;
    public Material bodyMaterial;
    public ScreenshotHandler ScreenshotHandler;

    [Header("Create Addon")]
    public string ProductName = "CapeEditorMCPE";
    private string MAINDOWNLOADPATHDIRECT;
    public Texture2D packIconPng;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        IsCustomCapeAddon = true;

        if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image) == NativeGallery.Permission.Denied)
        {
            NativeGallery.RequestPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
        }
        else if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image) == NativeGallery.Permission.ShouldAsk)
        {
            NativeGallery.RequestPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
        }

        CharacterAnim.SetBool("Anim2", false);

    }







    private void OnEnable()
    {
#if UNITY_EDITOR || UNITY_IOS
        MAINDOWNLOADPATHDIRECT = Path.Combine(Application.persistentDataPath, ProductName);
#elif UNITY_ANDROID
        MAINDOWNLOADPATHDIRECT = "/storage/emulated/0/Download/" + ProductName;
#endif

        if (!Directory.Exists(MAINDOWNLOADPATHDIRECT))
            Directory.CreateDirectory(MAINDOWNLOADPATHDIRECT);
    }

    public void PickImage()
    {
        SoundController1.Instance.PlayClickSound();

        NativeGallery.GetImageFromGallery((path) =>
        {
            if (string.IsNullOrEmpty(path)) return;

            Texture2D texture = NativeGallery.LoadImageAtPath(path, 1024, false);
            if (texture == null)
            {
                Debug.Log("Couldn't load texture from " + path);
                return;
            }

            pikedImageTexture = texture;

            ImageCropper.Instance.Show(pikedImageTexture, (bool result, Texture originalImage, Texture2D croppedImage) =>
            {
                if (result)
                {
                    rawImageViewCroppedImage.texture = croppedImage;
                    rawImageForMakeTexture.texture = croppedImage;
                    coppedImageTextureForDummyModel = croppedImage;
                    coppedImageTexture = croppedImage;
                    ViewImageScreen.SetActive(true);
                }
            });
        }, "Select a PNG image", "image/*");
    }

    public void MakeTexture()
    {
        if (SuperStarAd.Instance.NoAds == 0)
        {
            SuperStarAd.Instance.ShowForceInterstitialWithLoader((k) =>
            {
                SoundController1.Instance.PlayClickSound();
                ScreenshotHandler.takePics();
                bodyMaterial.mainTexture = coppedImageTextureForDummyModel;
                CharacterAnim.SetBool("Anim2", true);
                HDTextureScreen.SetActive(true);
                PickImageScreen.SetActive(false);
                ViewImageScreen.SetActive(false);
                MainCharacter.transform.rotation = Quaternion.identity;
            }, 3);
        }
        else
        {
            SoundController1.Instance.PlayClickSound();
            ScreenshotHandler.takePics();
            bodyMaterial.mainTexture = coppedImageTextureForDummyModel;
            CharacterAnim.SetBool("Anim2", true);
            HDTextureScreen.SetActive(true);
            PickImageScreen.SetActive(false);
            ViewImageScreen.SetActive(false);
            MainCharacter.transform.rotation = Quaternion.identity;
        }

    }

    public void CreateDirectImmportPack()
    {
        SoundController1.Instance.PlayClickSound();
        SuperStarAd.Instance.ShowRewardVideo(rewarded =>
        {
            if (!rewarded)
            {
                ToastManager.Instance.ShowToast("Watch the full video to unlock export.");
                return;
            }
            if (coppedImageTexture == null)
            {
                ToastManager.Instance.ShowToast("Please crop and apply cape before exporting.");
                return;
            }
            CreateAddonForCape();
        });
    }

    //    public void CreateAddonForCape()
    //    {
    //        string uniqueID = DateTime.Now.ToString("yyyyMMdd_HHmmss");
    //        string rootFolder = Path.Combine("/storage/emulated/0/Download/CapeEditorMCPE", $"VelocityCapeAddon_{uniqueID}");
    //        string zipPath = Path.Combine("/storage/emulated/0/Download/CapeEditorMCPE", $"VelocityCape_{uniqueID}.mcpack");

    //        try
    //        {
    //            // Create folder structure
    //            Directory.CreateDirectory(rootFolder);
    //            Directory.CreateDirectory(Path.Combine(rootFolder, "models/entity"));
    //            Directory.CreateDirectory(Path.Combine(rootFolder, "entity"));
    //            Directory.CreateDirectory(Path.Combine(rootFolder, "render_controllers"));
    //            Directory.CreateDirectory(Path.Combine(rootFolder, "textures"));

    //            // Save manifest.json with UTF-8 without BOM
    //            string manifestPath = Path.Combine(rootFolder, "manifest.json");
    //            File.WriteAllText(manifestPath, GenerateManifestJson(), new System.Text.UTF8Encoding(false));

    //            // Save pack_icon.png
    //            string base64Icon = GetPackIconBase64();
    //            if (!string.IsNullOrEmpty(base64Icon))
    //                SaveBase64ToPng(base64Icon, Path.Combine(rootFolder, "pack_icon.png"));

    //            // Save geometry, entity, render controller
    //            File.WriteAllText(Path.Combine(rootFolder, "models/entity/Cape.json"), GetCapeGeometryJson());
    //            File.WriteAllText(Path.Combine(rootFolder, "entity/player.entity.json"), GetCapeEntityJson());
    //            File.WriteAllText(Path.Combine(rootFolder, "render_controllers/custom.cape.json"), GetCapeRendererJson());

    //            // Save cape texture
    //            SaveTextureAsPNG(coppedImageTexture, "cape.png", Path.Combine(rootFolder, "textures"));

    //            // Clean up accidental duplicate JSON if exists
    //            string duplicateEntity = Path.Combine(rootFolder, "entity/player.entity(1).json");
    //            if (File.Exists(duplicateEntity))
    //                File.Delete(duplicateEntity);

    //            // Delete old zip if it exists
    //            if (File.Exists(zipPath))
    //                File.Delete(zipPath);

    //            // ✅ Zip all files inside rootFolder (no outer folder)
    //            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
    //            {
    //                var files = Directory.GetFiles(rootFolder, "*", SearchOption.AllDirectories);
    //                foreach (var file in files)
    //                {
    //                    string relativePath = Path.GetRelativePath(rootFolder, file).Replace("\\", "/");
    //                    archive.CreateEntryFromFile(file, relativePath);
    //                }
    //            }

    //#if UNITY_ANDROID
    //            if (File.Exists(zipPath))
    //            {
    //                ToastManager.Instance.ShowTost("Exported: " + Path.GetFileName(zipPath));
    //                AndroidContentOpenerWrapper.OpenContent(zipPath);

    //                // ✅ Delete temporary addon folder after successful export
    //                try
    //                {
    //                    Directory.Delete(rootFolder, true);
    //                    Debug.Log("Temporary addon folder deleted: " + rootFolder);
    //                }
    //                catch (Exception e)
    //                {
    //                    Debug.LogWarning("Could not delete temp folder: " + e.Message);
    //                }
    //            }
    //            else
    //            {
    //                ToastManager.Instance.ShowTost("Addon file not found.");
    //            }
    //#endif
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.LogError("Export failed: " + ex.Message);
    //            ToastManager.Instance.ShowTost("Export failed. Try again.");
    //        }
    //    }

    public void CreateAddonForCape()
    {
        string uniqueID = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string rootFolder = Path.Combine("/storage/emulated/0/Download/CapeEditorMCPE", $"VelocityCapeAddon_{uniqueID}");
        string resourcePack = Path.Combine(rootFolder, "resource_pack");
        string behaviorPack = Path.Combine(rootFolder, "behavior_pack");
        string zipPath = Path.Combine("/storage/emulated/0/Download/CapeEditorMCPE", $"VelocityCape_{uniqueID}.mcaddon");

        try
        {
            // Create folder structure
            Directory.CreateDirectory(Path.Combine(resourcePack, "models/entity"));
            Directory.CreateDirectory(Path.Combine(resourcePack, "textures"));
            Directory.CreateDirectory(Path.Combine(resourcePack, "render_controllers"));
            Directory.CreateDirectory(Path.Combine(resourcePack, "entity"));
            Directory.CreateDirectory(Path.Combine(behaviorPack, "entities"));

            // Save manifest.json
            File.WriteAllText(Path.Combine(resourcePack, "manifest.json"), GenerateManifestJson("resources"), new System.Text.UTF8Encoding(false));
            File.WriteAllText(Path.Combine(behaviorPack, "manifest.json"), GenerateManifestJson("data"), new System.Text.UTF8Encoding(false));

            // Save cape entity (behavior)
            File.WriteAllText(Path.Combine(behaviorPack, "entities/cape_entity.json"), GetCapeEntityJson());

            // Save resource files
            File.WriteAllText(Path.Combine(resourcePack, "models/entity/Cape.json"), GetCapeGeometryJson());
            File.WriteAllText(Path.Combine(resourcePack, "render_controllers/custom.cape.json"), GetCapeRendererJson());
            SaveTextureAsPNG(coppedImageTexture, "cape.png", Path.Combine(resourcePack, "textures"));
            SaveBase64ToPng(GetPackIconBase64(), Path.Combine(resourcePack, "pack_icon.png"));

            // Delete existing zip if needed
            if (File.Exists(zipPath))
                File.Delete(zipPath);

            // Create mcaddon zip (includes both packs)
            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                var files = Directory.GetFiles(rootFolder, "*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    string relativePath = Path.GetRelativePath(rootFolder, file).Replace("\\", "/");
                    archive.CreateEntryFromFile(file, relativePath);
                }
            }

#if UNITY_ANDROID
            if (File.Exists(zipPath))
            {
                ToastManager.Instance.ShowToast("Exported: " + Path.GetFileName(zipPath));
                AndroidContentOpenerWrapper.OpenContent(zipPath);

                // Delete temp addon folder
                try
                {
                    Directory.Delete(rootFolder, true);
                    Debug.Log("Temporary addon folder deleted: " + rootFolder);
                }
                catch (Exception e)
                {
                    Debug.LogWarning("Could not delete temp folder: " + e.Message);
                }
            }
            else
            {
                ToastManager.Instance.ShowToast("Addon file not found.");
            }
#endif
        }
        catch (Exception ex)
        {
            Debug.LogError("Export failed: " + ex.Message);
            ToastManager.Instance.ShowToast("Export failed. Try again.");
        }
    }



    //private string GenerateManifestJson()
    //{
    //    string headerUUID = Guid.NewGuid().ToString();
    //    string moduleUUID = Guid.NewGuid().ToString();

    //    var manifest = new
    //    {
    //        format_version = 2,
    //        header = new
    //        {
    //            name = "Velocity Cape",
    //            description = "A custom cape addon for Minecraft Bedrock",
    //            uuid = headerUUID,
    //            version = new[] { 1, 0, 0 },
    //            min_engine_version = new[] { 1, 13, 0 }
    //        },
    //        modules = new[]
    //        {
    //        new
    //        {
    //            type = "resources",
    //            description = "Velocity Cape Resources",
    //            uuid = moduleUUID,
    //            version = new[] { 1, 0, 0 }
    //        }
    //    }
    //    };

    //    return JsonConvert.SerializeObject(manifest, Formatting.Indented);
    //}

    private string GenerateManifestJson(string type)
    {
        string headerUUID = Guid.NewGuid().ToString();
        string moduleUUID = Guid.NewGuid().ToString();

        var manifest = new
        {
            format_version = 2,
            header = new
            {
                name = $"Velocity Cape ({type})",
                description = $"Velocity Cape Addon {type}",
                uuid = headerUUID,
                version = new[] { 1, 0, 0 },
                min_engine_version = new[] { 1, 13, 0 }
            },
            modules = new[]
            {
            new
            {
                type = type == "resources" ? "resources" : "data",
                description = $"Velocity Cape {type}",
                uuid = moduleUUID,
                version = new[] { 1, 0, 0 }
            }
        }
        };

        return JsonConvert.SerializeObject(manifest, Formatting.Indented);
    }



    private void SaveBase64ToPng(string base64Data, string filePath)
    {
        byte[] imageBytes = Convert.FromBase64String(base64Data);
        File.WriteAllBytes(filePath, imageBytes);
    }


    private string GetPackIconBase64()
    {
        if (packIconPng == null)
        {
            Debug.LogWarning("packIconPng is null!");
            return null;
        }

        byte[] imageData = packIconPng.DeCompress().EncodeToPNG();
        return Convert.ToBase64String(imageData);
    }




    private string GetCapeGeometryJson()
    {
        return @"{
  ""format_version"": ""1.12.0"",
  ""minecraft:geometry"": [
    {
      ""description"": {
        ""identifier"": ""geometry.cape"",
        ""texture_width"": 64,
        ""texture_height"": 32
      },
      ""bones"": [
        {
          ""name"": ""body"",
          ""pivot"": [0, 0, 0],
          ""cubes"": []
        },
        {
          ""name"": ""cape"",
          ""pivot"": [0, 24, 1],
          ""cubes"": [
            {
              ""origin"": [-5, 0, 1],
              ""size"": [10, 16, 1],
              ""uv"": [0, 0]
            }
          ]
        }
      ]
    }
  ]
}";
    }


    private string GetCapeEntityJson()
    {
        return @"{
  ""format_version"": ""1.10.0"",
  ""minecraft:client_entity"": {
    ""description"": {
      ""identifier"": ""cape:cape_entity"",
      ""materials"": {
        ""default"": ""cape""
      },
      ""geometry"": {
        ""default"": ""geometry.cape""
      },
      ""textures"": {
        ""default"": ""textures/cape""
      },
      ""render_controllers"": [""controller.render.default""]
    }
  }
}";
    }

    private string GetCapeRendererJson()
    {
        return @"{
  ""format_version"": ""1.10.0"",
  ""render_controllers"": {
    ""controller.render.default"": {
      ""geometry"": ""geometry.cape"",
      ""materials"": [""material.default""],
      ""textures"": [""textures/cape""]
    }
  }
}";
    }

    public void SaveTextureAsPNG(Texture2D texture, string filename, string folderPath)
    {
        // ✅ Resize to 64x64 for Minecraft compatibility
        Texture2D resizedTexture = ResizeTexture(texture, 64, 64);

        // 🔄 Optional: Decompress before encoding
        Texture2D decompressedTexture = resizedTexture.DeCompress();

        // ✅ Encode to PNG
        byte[] imageData = decompressedTexture.EncodeToPNG();

        // 📁 Save to path
        string path = Path.Combine(folderPath, filename);
        File.WriteAllBytes(path, imageData);

        Debug.Log("✅ Saved Image (64x64) to: " + path);
    }


    public void ChangeScreen(int screenNo)
    {
        if (SuperStarAd.Instance.NoAds == 0)
        {
            SuperStarAd.Instance.ShowForceInterstitialWithLoader((k) =>
            {
                SoundController1.Instance.PlayClickSound();
                CloseTutorial();

                if (screenNo == 0)
                {
                    HDTextureScreen.SetActive(false);
                    TutorialParent.SetActive(true);
                    TutorialScreen[0].SetActive(true);
                }
                else if (screenNo >= 1 && screenNo <= 4)
                {
                    TutorialScreen[screenNo].SetActive(true);
                }
                else if (screenNo == 5)
                {
                    TutorialParent.SetActive(false);
                    PickImageScreen.SetActive(true);
                }
            }, 3);
        }
        else
        {
            SoundController1.Instance.PlayClickSound();
            CloseTutorial();

            if (screenNo == 0)
            {
                HDTextureScreen.SetActive(false);
                TutorialParent.SetActive(true);
                TutorialScreen[0].SetActive(true);
            }
            else if (screenNo >= 1 && screenNo <= 4)
            {
                TutorialScreen[screenNo].SetActive(true);
            }
            else if (screenNo == 5)
            {
                TutorialParent.SetActive(false);
                PickImageScreen.SetActive(true);
            }
        }
    }

    public void CloseTutorial()
    {
        foreach (var screen in TutorialScreen)
            screen.SetActive(false);
    }

    public void MainScreenCloseButton()
    {
        SuperStarAd.Instance.ShowInterstitialTimer((o) =>
        {
            SoundController1.Instance.PlayClickSound();
            IsCustomCapeAddon = false;
            SceneManager.LoadScene(0);
        });
    }

    public void TextureCloseButton()
    {
        SuperStarAd.Instance.ShowInterstitialTimer((o) =>
        {
            SoundController1.Instance.PlayClickSound();
            ViewImageScreen.SetActive(false);
            PickImageScreen.SetActive(true);
        });
    }

    public void PreviewCloseButton()
    {
        SuperStarAd.Instance.ShowInterstitialTimer((o) =>
        {
            SoundController1.Instance.PlayClickSound();
            HDTextureScreen.SetActive(false);
            PickImageScreen.SetActive(true);
        });
    }

    public void BackScreen(int ScreenNo)
    {
        SuperStarAd.Instance.ShowInterstitialTimer((o) =>
        {
            SoundController1.Instance.PlayClickSound();
            CloseTutorial();

            switch (ScreenNo)
            {
                case 0:
                    TutorialParent.SetActive(false);
                    HDTextureScreen.SetActive(true);
                    MainCharacter.transform.rotation = Quaternion.identity;
                    break;
                case 1:
                    TutorialScreen[0].SetActive(true);
                    break;
                case 2:
                    TutorialScreen[1].SetActive(true);
                    break;
                case 3:
                    TutorialScreen[2].SetActive(true);
                    break;
                case 4:
                    TutorialScreen[3].SetActive(true);
                    break;
            }
        });
    }

    public void RateUs()
    {
        SoundController1.Instance.PlayClickSound();
        SuperStarSdkManager.Instance.Rate();
    }

    public static Texture2D ResizeTexture(Texture2D source, int width, int height)
    {
        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        Graphics.Blit(source, rt);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        Texture2D result = new Texture2D(width, height);
        result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        result.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);
        return result;
    }

    public void OnHomeButtonClick()
    {
        SceneManager.LoadScene(3);
    }

}
[Serializable]
public class ManifestHeader1
{
    public string description;
    public string name;
    public string uuid;
    public List<int> version;
}

[Serializable]
public class ManifestModule1
{
    public string description;
    public string type;
    public string uuid;
    public List<int> version;
}

[Serializable]
public class ManifestRoot1
{
    public int format_version;
    public ManifestHeader header;
    public List<ManifestModule> modules;
}

public static class ExtensionMethod
{
    public static Texture2D DeCompress(this Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}