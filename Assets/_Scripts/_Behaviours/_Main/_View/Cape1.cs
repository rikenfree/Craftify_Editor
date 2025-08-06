
using I2.Loc;
using Main.Controller;
using System;
using System.Collections;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Main.View
{
    public enum CapeResolution
    {
        C2217 = 0,
        C6432 = 1,
        C512256 = 2,
        Elytra6432 = 3
    }

    public class Cape1 : SceneElement1
    {

        public int LastMode
        {
            get { return PlayerPrefs.GetInt("LastMode", 0); }

            set { PlayerPrefs.SetInt("LastMode", value); }
        }


        [Header("Cape")]
        public Texture2D skin;
        public Texture2D capeTexture;

        public GameObject capeObject;
        public GameObject elytraObject;

        private BodyPart1[] bodyCapeParts;
        private BodyPart1[] bodyelytraParts;

        public bool tryAutoLoad;
        public Color[] colorMaps;

        public CapeResolution CurrentResolution;

        public Texture2D capeTexture2217;
        public Texture2D capeTexture6432;
        public Texture2D capeTexture512256;
        public Texture2D elytraTexture6432;
        public Texture2D AutoSavedTexture;


        [Header("Elytra")]
        public Texture2D elytraTexture;
        public Texture2D subElytraTexture;


        public GameObject[] GridObjects;


        private string mcpeCustomSkinPath = "games/com.mojang/minecraftpe/custom.png";
        private string mcpeCustomSkinFolderPath = "games/com.mojang/minecraftpe";

        public string CurrentHeaderName;
        private void Awake()
        {
            StartCoroutine(InitDefaultSkin());


            InitCapeBodyParts(capeObject.transform);
            InitElytraBodyParts(elytraObject.transform);
            if (LastMode == 0 || LastMode == 1)
            {
                if (LastMode == 0)
                {
                    CurrentResolution = CapeResolution.C2217;
                }
                else
                {
                    CurrentResolution = CapeResolution.C6432;
                }
                capeObject.SetActive(true);
                elytraObject.SetActive(false);
            }
            else if (LastMode == 3)
            {
                CurrentResolution = CapeResolution.Elytra6432;
                capeObject.SetActive(false);
                elytraObject.SetActive(true);
            }
            //   tryAutoLoad = false;


        }
        public void RefreshModels()
        {

            if (LastMode == 0)
            {
                capeObject.SetActive(true);
                elytraObject.SetActive(false);
                CurrentResolution = CapeResolution.C2217;
            }
            else if (LastMode == 1)
            {
                capeObject.SetActive(true);
                elytraObject.SetActive(false);
                CurrentResolution = CapeResolution.C6432;
            }
            else if (LastMode == 3)
            {
                CurrentResolution = CapeResolution.Elytra6432;
                capeObject.SetActive(false);
                elytraObject.SetActive(true);
            }
        }

        public void OnClickGridOnOff(bool selected)
        {

            for (int i = 0; i < GridObjects.Length; i++)
            {
                GridObjects[i].SetActive(selected);
            }
        }


        public void NewSetup(CapeResolution cape)
        {
            UndoRedoController1.Instance.InitStacks();

            if (cape == CapeResolution.C2217)
            {
                CurrentResolution = CapeResolution.C2217;
                LastMode = 0;
                capeObject.SetActive(true);
                elytraObject.SetActive(false);
                LoadSkinWithTexture(Resources.Load<Texture2D>("cape/e0"));

                CurrentHeaderName = "Cape 22x17";
                base.scene.view.masterCavas.HeaderLocalize.Term = "Cape_Header_22x17";
                LocalizationManager.LocalizeAll();
            }
            else if (cape == CapeResolution.C6432)
            {
                CurrentResolution = CapeResolution.C6432;
                LastMode = 1;
                capeObject.SetActive(true);
                elytraObject.SetActive(false);
                LoadSkinWithTexture(Resources.Load<Texture2D>("cape/e1"));

                CurrentHeaderName = "Cape 64x32";
                base.scene.view.masterCavas.HeaderLocalize.Term = "Cape_Header_64x32";
                LocalizationManager.LocalizeAll();
            }
            else if (cape == CapeResolution.C512256)
            {
                CurrentResolution = CapeResolution.C512256;
                LastMode = 2;
                LoadSkinWithTexture(Resources.Load<Texture2D>("cape/e2"));
                // If this needs a header too, add it!
                base.scene.view.masterCavas.HeaderLocalize.Term = "Cape_Header_512x256";
                LocalizationManager.LocalizeAll();
            }
            else if (cape == CapeResolution.Elytra6432)
            {
                CurrentResolution = CapeResolution.Elytra6432;
                LastMode = 3;
                capeObject.SetActive(false);
                elytraObject.SetActive(true);
                LoadElytraWithTexture(Resources.Load<Texture2D>("cape/e3"));

                CurrentHeaderName = "Elytra 64x32";
                base.scene.view.masterCavas.HeaderLocalize.Term = "Cape_Header_Elytra_64x32";
                LocalizationManager.LocalizeAll();
            }
        }

        private void Start()
        {
            SaveImageOnPersistentDataPathFromResource("cape/e0", "e0.png");
            SaveImageOnPersistentDataPathFromResource("cape/e1", "e1.png");
            SaveImageOnPersistentDataPathFromResource("cape/e2", "e2.png");
            SaveImageOnPersistentDataPathFromResource("cape/e3", "e3.png");
            StartCoroutine(LoadGalleryPickedTexture("gload.png"));
            StartCoroutine(LoadGalleryPickedTexturep1("e0.png"));
            StartCoroutine(LoadGalleryPickedTexturep2("e1.png"));
            StartCoroutine(LoadGalleryPickedTexturep3("e2.png"));
            StartCoroutine(LoadGalleryPickedElytraTexturep("e3.png"));

            if (skin == null)
            {
                skin = Resources.Load<Texture2D>("cape/e0");
            }

            if (capeTexture == null)
            {
                capeTexture = Resources.Load<Texture2D>("cape/e0");
            }

            LoadSkinWithTexture(capeTexture);
        }

        public IEnumerator LoadAutoSavedTexture(string textureFileName)
        {
            string path = Path.Combine(Application.persistentDataPath, textureFileName);
            Debug.Log("DataPath ::> " + path);


            if (File.Exists(path))
            {
                Debug.Log("File gload  Exist");
                using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + path))
                {
                    // Send the request and yield until it's done.
                    yield return www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("Failed to load texture: " + www.error);
                    }
                    else
                    {
                        // Get the loaded texture.
                        AutoSavedTexture = DownloadHandlerTexture.GetContent(www);
                    }
                }
            }
            else
            {

                Debug.LogError("File Not Exist");
            }
            // Create a UnityWebRequest to get the texture.

        }
        public IEnumerator LoadGalleryPickedTexture(string textureFileName)
        {
            string path = Path.Combine(Application.persistentDataPath, textureFileName);
            Debug.LogError("LoadGalleryPickedTexture" + path);


            if (File.Exists(path))
            {
                Debug.Log("File gload  Exist");
                using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + path))
                {
                    // Send the request and yield until it's done.
                    yield return www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("Failed to load texture: " + www.error);
                    }
                    else
                    {
                        // Get the loaded texture.
                        galleryPickedTexture = DownloadHandlerTexture.GetContent(www);
                    }
                }
            }
            else
            {

                Debug.LogError("File Not Exist");
            }
            // Create a UnityWebRequest to get the texture.

        }
        public IEnumerator LoadGalleryPickedTexturep1(string textureFileName)
        {
            string path = Path.Combine(Application.persistentDataPath, textureFileName);


            if (File.Exists(path))
            {
                Debug.Log("File gload  Exist");
                using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + path))
                {
                    // Send the request and yield until it's done.
                    yield return www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("Failed to load texture: " + www.error);
                    }
                    else
                    {
                        // Get the loaded texture.
                        capeTexture2217 = DownloadHandlerTexture.GetContent(www);
                    }
                }
            }
            else
            {

                Debug.LogError("File Not Exist");
            }
            // Create a UnityWebRequest to get the texture.

        }
        public IEnumerator LoadGalleryPickedTexturep2(string textureFileName)
        {
            string path = Path.Combine(Application.persistentDataPath, textureFileName);


            if (File.Exists(path))
            {
                Debug.Log("File gload  Exist");
                using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + path))
                {
                    // Send the request and yield until it's done.
                    yield return www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("Failed to load texture: " + www.error);
                    }
                    else
                    {
                        // Get the loaded texture.
                        capeTexture6432 = DownloadHandlerTexture.GetContent(www);


                    }
                }
            }
            else
            {

                Debug.LogError("File Not Exist");
            }
            // Create a UnityWebRequest to get the texture.

        }
        public IEnumerator LoadGalleryPickedTexturep3(string textureFileName)
        {
            string path = Path.Combine(Application.persistentDataPath, textureFileName);


            if (File.Exists(path))
            {
                Debug.Log("File gload  Exist");
                using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + path))
                {
                    // Send the request and yield until it's done.
                    yield return www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("Failed to load texture: " + www.error);
                    }
                    else
                    {
                        // Get the loaded texture.
                        capeTexture512256 = DownloadHandlerTexture.GetContent(www);
                    }
                }
            }
            else
            {

                Debug.LogError("File Not Exist");
            }
            // Create a UnityWebRequest to get the texture.

        }

        public IEnumerator LoadGalleryPickedElytraTexturep(string textureFileName)
        {
            string path = Path.Combine(Application.persistentDataPath, textureFileName);
            Debug.LogError("LoadGalleryPickedElytraTexturep => " + path);


            if (File.Exists(path))
            {
                Debug.Log("File gload  Exist" + path);
                using (UnityWebRequest www = UnityWebRequestTexture.GetTexture("file://" + path))
                {
                    // Send the request and yield until it's done.
                    yield return www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError("Failed to load texture: " + www.error);
                    }
                    else
                    {
                        // Get the loaded texture.
                        elytraTexture6432 = DownloadHandlerTexture.GetContent(www);
                    }
                }
            }
            else
            {
                Debug.LogError("File Not Exist");
            }
            // Create a UnityWebRequest to get the texture.

        }


        private IEnumerator InitDefaultSkin()
        {

            string path2 = Application.persistentDataPath + "/autosave.png";
            if (File.Exists(path2))
            {
                yield return StartCoroutine(LoadAutoSavedTexture("autosave.png"));
                Debug.Log("Do Auto Load");
                if (LastMode == 0 || LastMode == 1)
                {
                    LoadSkinWithTexture(AutoSavedTexture);
                }
                else if (LastMode == 3)
                {
                    LoadElytraWithTexture(AutoSavedTexture);
                }
            }
            else if (LastMode == 0)
            {
                CurrentHeaderName = "Cape 22x17";
                base.scene.view.masterCavas.HeaderLocalize.Term = "Cape_Header_22x17";
                LocalizationManager.LocalizeAll();

                skin = new Texture2D(22, 17, TextureFormat.ARGB32, false);
                LoadDefaultCapeSkin();
            }
            else if (LastMode == 3)
            {
                CurrentHeaderName = "Elytra 64x32";
                base.scene.view.masterCavas.HeaderLocalize.Term = "Cape_Header_Elytra_64x32";
                LocalizationManager.LocalizeAll();

                skin = new Texture2D(24, 22, TextureFormat.ARGB32, false);
                LoadDefaultElytraSkin();
            }
        }
        public Color GetColor(int i, int j)
        {
            //Debug.LogError("GetColor:" + i + " :j:" + j + " Final => " + j * skin.width + i);
            return colorMaps[j * skin.width + i];
        }
        public void LoadDefaultElytraSkin()
        {
            if (elytraTexture == null)
            {
                Debug.LogError("elytra Texture is  null");
                return;
            }

            if (elytraTexture.width == 64 && elytraTexture.height == 32)
            {

                Debug.LogError("texture is available");  //Debug.Break();
                subElytraTexture = TextureUtility1.GetSubTexture(elytraTexture, new Vector2(22, 10), new Vector2(24, 22));
                skin = new Texture2D(24, 22, TextureFormat.ARGB32, false); ;
                // Paint2422TextureOnSkin(pickedTexture2D2217);
                //   tempRawImage.texture = skin;
                colorMaps = subElytraTexture.GetPixels();
                TempLoadElytra();
            }
            else
            {
                Debug.LogError("elytra not supported");
            }

        }

        public void LoadElytraWithTexture(Texture2D elytra)
        {
            CurrentHeaderName = "Cape 64x32";
            base.scene.view.masterCavas.HeaderLocalize.Term = "Cape_Header_64x32";
            LocalizationManager.LocalizeAll();

            elytraTexture = elytra;

            if (elytraTexture.width == 64 && elytraTexture.height == 32)
            {
                Debug.LogError("texture is available");
                subElytraTexture = TextureUtility1.GetSubTexture(elytraTexture, new Vector2(22, 10), new Vector2(24, 22));
                skin = new Texture2D(24, 22, TextureFormat.ARGB32, false);
                colorMaps = subElytraTexture.GetPixels();
                TempLoadElytra();
            }
            else if (elytraTexture.width == 24 && elytraTexture.height == 22)
            {
                subElytraTexture = elytra;
                skin = new Texture2D(24, 22, TextureFormat.ARGB32, false);
                colorMaps = subElytraTexture.GetPixels();
                TempLoadElytra();
            }
            else
            {
                Debug.LogError("elytra not supported");
            }
        }
        public void LoadDefaultCapeSkin()
        {
            //capeTexture = Resources.Load<Texture2D>("cape/empty00");

            capeTexture = CapeController.Instance.capeTextures[CapeController.Instance.currentCapInt];
            Paint22x17TextureOnSkin(capeTexture);
            //   tempRawImage.texture = skin;
            Debug.LogError("SetColor0:" + skin.GetPixel(1, 1) + "  :raw:" + capeTexture.GetPixel(1, 1));
            //Debug.Break();
            colorMaps = skin.GetPixels();
            //  tempcolorMaps = capeTexture.GetPixels();
            // tempcolorMaps123 = capeTexture.GetPixels32();
        }

        public void LoadSkinWithTexture(Texture2D cape)
        {
            capeTexture = cape;

            if (cape.width == 22 && cape.height == 17)
            {
                CurrentHeaderName = "Cape 22x17";
                base.scene.view.masterCavas.HeaderLocalize.Term = "Cape_Header_22x17";
                LocalizationManager.LocalizeAll();

                skin = new Texture2D(22, 17, TextureFormat.ARGB32, false);
                Debug.LogError("2217 " + CurrentHeaderName);
                galleryPickedTexture = cape;
                Paint22x17TextureOnSkin(capeTexture);

                colorMaps = skin.GetPixels();
            }
            else if (cape.width == 64 && cape.height == 32)
            {
                CurrentHeaderName = "Cape 64x32";
                base.scene.view.masterCavas.HeaderLocalize.Term = "Cape_Header_64x32";
                LocalizationManager.LocalizeAll();

                skin = new Texture2D(64, 32, TextureFormat.ARGB32, false);

                Debug.LogError("6432");
                SaveTexureOnPersistentDataPath(cape, "gload.png");
                galleryPickedTexture = cape;
                Texture2D pickedTexture2D2217 = TextureUtility1.GetSubTexture(cape, new Vector2(0, 15), new Vector2(22, 17));
                skin = new Texture2D(64, 32, TextureFormat.ARGB32, false);
                capeTexture = pickedTexture2D2217;
                Paint22x17TextureOnSkin(capeTexture);
                colorMaps = skin.GetPixels();
            }
            else if (cape.width == 512 && cape.height == 256)
            {
                Debug.LogError("512256");
                base.scene.view.masterCavas.HeaderLocalize.Term = "Cape_Header_512x256";
            }
            else
            {
                skin = new Texture2D(22, 17, TextureFormat.ARGB32, false);

                CurrentHeaderName = "Cape 22x17";
                base.scene.view.masterCavas.HeaderLocalize.Term = "Cape_Header_22x17";
                LocalizationManager.LocalizeAll();

                Debug.LogError("default 2217");
                Paint22x17TextureOnSkin(cape);
                Debug.LogError("SetColor0:" + skin.GetPixel(1, 1) + "  :raw:" + capeTexture.GetPixel(1, 1));
                colorMaps = skin.GetPixels();
            }

            TempLoadCape();
        }

        public void LoadCapeSkinFromGallery(Texture2D pickedTexture2D)
        {
            SaveTexureOnPersistentDataPath(pickedTexture2D, "gload.png");
            galleryPickedTexture = pickedTexture2D;
            capeTexture = pickedTexture2D;
            Paint22x17TextureOnSkin(capeTexture);
            //  tempRawImage.texture = skin;
            Debug.LogError("SetColor0:" + skin.GetPixel(1, 1) + "  :raw:" + capeTexture.GetPixel(1, 1));
            //Debug.Break();
            colorMaps = skin.GetPixels();
            // tempcolorMaps = capeTexture.GetPixels();
            //  tempcolorMaps123 = capeTexture.GetPixels32();
        }


        public Texture2D galleryPickedTexture;

        public void LoadCapeSkinFromGallery6432(Texture2D pickedTexture2D)
        {
            // ReplaceTextureInPersistentDataPath("p2.png", pickedTexture2D);
            SaveTexureOnPersistentDataPath(pickedTexture2D, "gload.png");
            galleryPickedTexture = pickedTexture2D;
            Texture2D pickedTexture2D2217 = TextureUtility1.GetSubTexture(pickedTexture2D, new Vector2(0, 15), new Vector2(22, 17));
            skin = new Texture2D(22, 17, TextureFormat.ARGB32, false); ;
            Debug.Log("LoadCapeSkinFromGallery6432");
            capeTexture = pickedTexture2D2217;
            Paint22x17TextureOnSkin(capeTexture);
            Debug.LogError("SetColor0:" + skin.GetPixel(1, 1) + "  :raw:" + capeTexture.GetPixel(1, 1)); //Debug.Break();
            colorMaps = skin.GetPixels();
            // tempcolorMaps = capeTexture.GetPixels();
            //  tempcolorMaps123 = capeTexture.GetPixels32();
            Debug.Log("LoadCapeSkinFromGallery6432 111 ");
        }

        public void LoadElytraSkinFromGallery6432(Texture2D pickedTexture2D)
        {
            // ReplaceTextureInPersistentDataPath("p2.png", pickedTexture2D);
            SaveTexureOnPersistentDataPath(pickedTexture2D, "gload.png");
            galleryPickedTexture = pickedTexture2D;
            Texture2D pickedTexture2D2217 = TextureUtility1.GetSubTexture(pickedTexture2D, new Vector2(0, 15), new Vector2(22, 17));
            skin = new Texture2D(22, 17, TextureFormat.ARGB32, false); ;
            Debug.Log("LoadCapeSkinFromGallery6432");
            capeTexture = pickedTexture2D2217;
            Paint22x17TextureOnSkin(capeTexture);
            Debug.LogError("SetColor0:" + skin.GetPixel(1, 1) + "  :raw:" + capeTexture.GetPixel(1, 1)); //Debug.Break();
            colorMaps = skin.GetPixels();
            // tempcolorMaps = capeTexture.GetPixels();
            //  tempcolorMaps123 = capeTexture.GetPixels32();
            Debug.Log("LoadCapeSkinFromGallery6432 111 ");
        }

        public static void OverrideTextureWithSubtexture(Texture2D mainTexture, Texture2D subTexture, Vector2 offset)
        {
            // Check to make sure that the main texture and subtexture are valid.
            if (mainTexture == null || subTexture == null)
            {
                return;
            }

            // Check to make sure that the offset is valid.
            if (offset.x < 0 || offset.y < 0 || offset.x + subTexture.width > mainTexture.width || offset.y + subTexture.height > mainTexture.height)
            {
                return;
            }

            // Copy the pixels from the subtexture to the main texture.
            for (int x = 0; x < subTexture.width; x++)
            {
                for (int y = 0; y < subTexture.height; y++)
                {
                    mainTexture.SetPixel(x + (int)offset.x, y + (int)offset.y, subTexture.GetPixel(x, y));
                }
            }
            mainTexture.Apply();
        }

        public static void ReplaceTextureInPersistentDataPath(string fileName, Texture2D newTexture)
        {
            // Check to make sure that the resource name and file name are valid.
            if (string.IsNullOrEmpty(fileName) || newTexture == null)
            {
                return;
            }
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            Debug.Log(filePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            //Texture2D texture = Resources.Load<Texture2D>(resourceName);
            if (newTexture != null)
            {

                // Convert the Texture to PNG format
                byte[] imageBytes = newTexture.EncodeToPNG();

                // Write bytes to file
                File.WriteAllBytes(filePath, imageBytes);

                Debug.Log($"Image saved at: {filePath}");

            }
            else
            {
                Debug.Log("Texture not found");
            }


        }
        public static void SaveImageOnPersistentDataPathFromResource(string resourceName, string fileName)
        {

            // Check to make sure that the resource name and file name are valid.
            if (resourceName == null || fileName == null)
            {
                return;
            }
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            Debug.Log(filePath);
            if (!File.Exists(filePath))
            {
                Texture2D texture = Resources.Load<Texture2D>(resourceName);
                if (texture != null)
                {
                    // Create a readable and uncompressed copy
                    Texture2D readableTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
                    readableTexture.SetPixels(texture.GetPixels());
                    readableTexture.Apply();

                    byte[] imageBytes = readableTexture.EncodeToPNG();

                    File.WriteAllBytes(filePath, imageBytes);
                    Debug.Log($"Image saved at: {filePath}");
                }
            }
            else
            {
                Debug.LogError("File is already there");
            }

            // replace "YourImageName" with the name of your image file


        }

        public static void SaveTexureOnPersistentDataPath(Texture2D resource, string fileName)
        {

            // Check to make sure that the resource name and file name are valid.
            if (resource == null || fileName == null)
            {
                return;
            }
            string filePath = Path.Combine(Application.persistentDataPath, fileName);
            Debug.Log(filePath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if (!File.Exists(filePath))
            {


                byte[] imageBytes = resource.EncodeToPNG();

                // Write bytes to file
                File.WriteAllBytes(filePath, imageBytes);

                Debug.Log($"Image saved at: {filePath}");


            }
            else
            {
                Debug.LogError("File is already there");
            }

            // replace "YourImageName" with the name of your image file


        }



        public void InitCapeBodyParts(Transform parent)
        {
            Component[] componentsInChildren = parent.GetComponentsInChildren(typeof(BodyPart1), includeInactive: true);
            bodyCapeParts = new BodyPart1[componentsInChildren.Length];
            for (int i = 0; i < bodyCapeParts.Length; i++)
            {
                bodyCapeParts[i] = componentsInChildren[i].GetComponent<BodyPart1>();
            }
            UndoRedoController1.Instance.bodyCapeParts = bodyCapeParts;
        }

        public void InitElytraBodyParts(Transform parent)
        {
            Component[] componentsInChildren = parent.GetComponentsInChildren(typeof(BodyPart1), includeInactive: true);
            bodyelytraParts = new BodyPart1[componentsInChildren.Length];
            for (int i = 0; i < bodyelytraParts.Length; i++)
            {
                bodyelytraParts[i] = componentsInChildren[i].GetComponent<BodyPart1>();
            }
            UndoRedoController1.Instance.bodyelytraParts = bodyelytraParts;

        }

        public void AutoSave()
        {
            //AndroidNativeUtility.ExternalStoragePathLoaded += AutoSaveCallBack;
            //SA_Singleton<AndroidNativeUtility>.instance.GetExternalStoragePath();

            string savePath = Application.persistentDataPath;
            AutoSaveCallBack(savePath);
        }



        public void TempLoadCape()//After change cap texture refresh color
        {
            BodyPart1[] array = bodyCapeParts;
            foreach (BodyPart1 bodyPart in array)
            {
                if (bodyPart.HasPixelsData())
                {
                    bodyPart.RefreshSkinData();
                }
            }
        }

        public void TempLoadElytra()//After change cap texture refresh color
        {
            BodyPart1[] array = bodyelytraParts;
            foreach (BodyPart1 bodyPart in array)
            {
                if (bodyPart.HasPixelsData())
                {
                    bodyPart.RefreshElytraData();
                }
            }
        }

        //private void AutoLoadCallBack(string path)
        //{
        //    //AndroidNativeUtility.ExternalStoragePathLoaded -= AutoLoadCallBack;
        //    string path2 = path + "/autosave.png";
        //    if (File.Exists(path2))
        //    {
        //        Debug.LogError("AutoLoad:" + path2);
        //        byte[] data = File.ReadAllBytes(path2);
        //        Texture2D texture2D = Resources.Load<Texture2D>("Skins/disk_tmp");
        //        texture2D.LoadImage(data);
        //        //Paint64x64TextureOnSkin(texture2D);
        //        Paint22x17TextureOnSkin(texture2D);
        //        Debug.LogError("SetColor1");
        //        colorMaps = skin.GetPixels();
        //        try
        //        {
        //            if (LastMode==0)
        //            {
        //                TempLoadCape();
        //            }
        //            else if (LastMode==1)
        //            {
        //                TempLoadElytra();
        //            }

        //        }
        //        catch (Exception)
        //        {
        //            //base.scene.view.errorCanvas.ShowCustomError("Attention", "Can't load autosaved skin.");
        //        }
        //    }
        //    else
        //    {
        //        print("Can't find autosaved file.");
        //    }
        //}

        private void AutoSaveCallBack(string path)
        {
            //AndroidNativeUtility.ExternalStoragePathLoaded -= AutoSaveCallBack;

            UpdateSkinFromPixels(CurrentResolution);
            byte[] bytes = skin.EncodeToPNG();
            //Debug.LogError("AutoSavePath:" + (path + "/autosave.png"));
            File.WriteAllBytes(path + "/autosave.png", bytes);
        }

        //private void SaveGalleryCallBack(GallerySaveResult result)
        //{
        //	SA_Singleton<AndroidCamera>.instance.OnImageSaved -= SaveGalleryCallBack;
        //	if (result.IsSucceeded)
        //	{
        //		base.scene.view.infoCanvas.ShowGalleryExportSuccessInfo();
        //	}
        //	else
        //	{
        //		base.scene.view.errorCanvas.ShowCantSaveGallerySkinError();
        //	}
        //}

        private void SaveMCPECallBack(string path)
        {
            //AndroidNativeUtility.ExternalStoragePathLoaded -= SaveMCPECallBack;
            string text = SearchMCPEFolder(path);
            if (text != null)
            {
                UpdateSkinFromPixels(CurrentResolution);
                byte[] bytes = skin.EncodeToPNG();
                File.WriteAllBytes(text + "/custom.png", bytes);
                base.scene.view.infoCanvas.ShowMCPEExportSuccessInfo();
            }
            else
            {
                base.scene.view.errorCanvas.ShowCantSaveMCPESkinError();
            }
        }

        public void SaveSkinToPath(string path)
        {
            byte[] bytes = skin.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
        }

        //private void UpdateSkinFromPixels(CapeResolution cape)
        //{
        //    Color[,] array = CurrentPixelColors();
        //    for (int i = 0; i < 64; i++)
        //    {
        //        for (int j = 0; j < 64; j++)
        //        {
        //            skin.SetPixel(i, j, array[i, j]);
        //        }
        //    }
        //    Debug.LogError("SetColor2");
        //    colorMaps = skin.GetPixels();
        //}

        private void UpdateSkinFromPixels(CapeResolution cape)
        {
            Color[,] array = CurrentPixelColors(cape);
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    skin.SetPixel(i, j, array[i, j]);
                }
            }
            // Debug.LogError("SetColor3");
            colorMaps = skin.GetPixels();
        }

        private Color[,] CurrentPixelColors(CapeResolution capeResolution)
        {

            if (capeResolution == CapeResolution.C2217 || capeResolution == CapeResolution.C6432)
            {

                Color[,] result = new Color[22, 17];
                BodyPart1[] array = bodyCapeParts;
                foreach (BodyPart1 bodyPart in array)
                {
                    for (int j = 0; j < bodyPart.width; j++)
                    {
                        for (int k = 0; k < bodyPart.height; k++)
                        {
                            Colorize(result, bodyPart, j, k);
                        }
                    }
                }
                return result;
            }
            else if (capeResolution == CapeResolution.Elytra6432)
            {
                Color[,] result = new Color[24, 22];
                BodyPart1[] array = bodyelytraParts;
                foreach (BodyPart1 bodyPart in array)
                {
                    for (int j = 0; j < bodyPart.width; j++)
                    {
                        for (int k = 0; k < bodyPart.height; k++)
                        {

                            //  Debug.Log("j => "+j+ "  k=> "+k);
                            Colorize(result, bodyPart, j, k);
                        }
                    }
                }
                return result;
            }
            return null;

        }

        //private Color[,] CurrentPixelColors()
        //{
        //    Color[,] result = new Color[64, 64];
        //    BodyPart[] array = bodyCapeParts;
        //    foreach (BodyPart bodyPart in array)
        //    {
        //        for (int j = 0; j < bodyPart.width; j++)
        //        {
        //            for (int k = 0; k < bodyPart.height; k++)
        //            {
        //                Colorize(result, bodyPart, j, k);
        //            }
        //        }
        //    }
        //    return result;
        //}

        private void Colorize(Color[,] skin, BodyPart1 bp, int i, int j)
        {
            Color pixelColor = bp.GetPixelColor(i, j);
            if (!(pixelColor == default(Color)))
            {
                //   Debug.Log("BP => "+bp.transform.name);
                //   Debug.Log("bp.skinX + i => " + (bp.skinX + i));
                //    Debug.Log("bp.skinY + j=> " + (bp.skinY + j));

                skin[(bp.skinX + i), (bp.skinY + j)] = pixelColor;
            }
        }

        private void LoadMCPECallBack(string path)
        {
            //AndroidNativeUtility.ExternalStoragePathLoaded -= LoadMCPECallBack;
            string text = SearchMCPESkin(path);
            if (text != null)
            {
                LoadTextureFromDisk(text);
            }
            else
            {
                base.scene.view.errorCanvas.ShowCantFindMCPESkinError();
            }
        }

        private string SearchMCPESkin(string dataPath)
        {
            string[] array = dataPath.Split('/');
            string text = "/";
            if (FoundMCPESkin(text))
            {
                return text + mcpeCustomSkinPath;
            }
            for (int i = 0; i < array.Length; i++)
            {
                text = text + array[i] + "/";
                if (FoundMCPESkin(text))
                {
                    return text + mcpeCustomSkinPath;
                }
            }
            return null;
        }

        private string SearchMCPEFolder(string dataPath)
        {
            string[] array = dataPath.Split('/');
            string text = "/";
            if (FoundMCPEFolder(text))
            {
                return text + mcpeCustomSkinFolderPath;
            }
            for (int i = 0; i < array.Length; i++)
            {
                text = text + array[i] + "/";
                if (FoundMCPEFolder(text))
                {
                    return text + mcpeCustomSkinFolderPath;
                }
            }
            return null;
        }

        private void LoadTextureFromDisk(string texturePath)
        {
            try
            {
                byte[] data = File.ReadAllBytes(texturePath);
                Texture2D texture2D = Resources.Load<Texture2D>("Skins/disk_tmp");
                texture2D.LoadImage(data);
                HandleTexture(texture2D);
            }
            catch (Exception)
            {
                base.scene.view.errorCanvas.ShowCantFindMCPESkinError();
            }
        }

        private bool FoundMCPESkin(string path)
        {
            return File.Exists(path + mcpeCustomSkinPath);
        }

        private bool FoundMCPEFolder(string path)
        {
            return Directory.Exists(path + mcpeCustomSkinFolderPath);
        }

        private IEnumerator Load(string url, bool random)
        {
            WWW www = new WWW(url);
            Texture2D tmpTexture = Resources.Load<Texture2D>("Skins/internet_tmp");
            yield return www;
            if (www.error == null)
            {
                www.LoadImageIntoTexture(tmpTexture);
                HandleTexture(tmpTexture);
            }
            else
            {
                base.scene.view.errorCanvas.ShowLoadingSkinOnlineError(random);
            }
            base.scene.view.waitingCanvas.Hide();
        }

        public void HandleTexture(Texture2D texture)
        {
            if (texture.width == 64 && texture.height == 32)
            {
                Paint64x32TextureOnSkin(texture);
                Debug.LogError("SetColor4");
                colorMaps = skin.GetPixels();
                base.scene.view.newCharacter.ReloadBodyPartsData();
                MonoBehaviour.print("New Skin Loaded: 64x32");
            }
            else if (texture.width == 64 && texture.height == 64)
            {
                Paint64x64TextureOnSkin(texture);
                Debug.LogError("SetColor5");
                colorMaps = skin.GetPixels();
                base.scene.view.newCharacter.ReloadBodyPartsData();
                MonoBehaviour.print("New Skin Loaded: 64x64");
            }
            else
            {
                base.scene.view.errorCanvas.ShowSkinWrongSizeError();
                MonoBehaviour.print("Incorrect Size");
            }
        }

        private void Paint64x64TextureOnSkin(Texture2D s)
        {
            if (s.width != 64 || s.height != 64)
            {
                return;
            }
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    skin.SetPixel(i, j, s.GetPixel(i, j));
                }
            }
        }

        private void Paint22x17TextureOnSkin(Texture2D s)
        {
            if (s.width != 22 || s.height != 17)
            {
                Debug.LogError("return kare che");
                return;
            }
            for (int i = 0; i < 22; i++)
            {
                for (int j = 0; j < 17; j++)
                {
                    skin.SetPixel(i, j, s.GetPixel(i, j));
                    skin.Apply();
                }
            }
        }

        private void Paint64x32TextureOnSkin(Texture2D s)
        {
            if (s.width != 64 || s.height != 32)
            {
                return;
            }
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    Color color = default(Color);
                    color.r = 0f;
                    color.g = 0f;
                    color.b = 0f;
                    color.a = 0f;
                    skin.SetPixel(i, j, color);
                }
            }
            for (int k = 0; k < 64; k++)
            {
                for (int l = 0; l < 32; l++)
                {
                    skin.SetPixel(k, 32 + l, s.GetPixel(k, l));
                }
            }
            for (int m = 0; m < 4; m++)
            {
                for (int n = 0; n < 12; n++)
                {
                    skin.SetPixel(27 - m, n, s.GetPixel(m, n));
                }
            }
            for (int num = 4; num < 8; num++)
            {
                for (int num2 = 0; num2 < 16; num2++)
                {
                    skin.SetPixel(27 - num, num2, s.GetPixel(num, num2));
                }
            }
            for (int num3 = 8; num3 < 12; num3++)
            {
                for (int num4 = 0; num4 < 12; num4++)
                {
                    skin.SetPixel(27 - num3, num4, s.GetPixel(num3, num4));
                }
            }
            for (int num5 = 12; num5 < 16; num5++)
            {
                for (int num6 = 0; num6 < 12; num6++)
                {
                    skin.SetPixel(43 - num5, num6, s.GetPixel(num5, num6));
                }
            }
            for (int num7 = 8; num7 < 12; num7++)
            {
                for (int num8 = 12; num8 < 16; num8++)
                {
                    skin.SetPixel(35 - num7, num8, s.GetPixel(num7, num8));
                }
            }
            for (int num9 = 40; num9 < 44; num9++)
            {
                for (int num10 = 0; num10 < 12; num10++)
                {
                    skin.SetPixel(83 - num9, num10, s.GetPixel(num9, num10));
                }
            }
            for (int num11 = 44; num11 < 48; num11++)
            {
                for (int num12 = 0; num12 < 16; num12++)
                {
                    skin.SetPixel(83 - num11, num12, s.GetPixel(num11, num12));
                }
            }
            for (int num13 = 48; num13 < 52; num13++)
            {
                for (int num14 = 0; num14 < 12; num14++)
                {
                    skin.SetPixel(83 - num13, num14, s.GetPixel(num13, num14));
                }
            }
            for (int num15 = 52; num15 < 56; num15++)
            {
                for (int num16 = 0; num16 < 12; num16++)
                {
                    skin.SetPixel(99 - num15, num16, s.GetPixel(num15, num16));
                }
            }
            for (int num17 = 48; num17 < 52; num17++)
            {
                for (int num18 = 12; num18 < 16; num18++)
                {
                    skin.SetPixel(91 - num17, num18, s.GetPixel(num17, num18));
                }
            }
        }

        public void SaveMCPE()
        {
            string path = new AndroidJavaClass("android.os.Environment").CallStatic<AndroidJavaObject>("getExternalStorageDirectory", Array.Empty<object>()).Call<string>("getPath", Array.Empty<object>());
            string mcpePath = path + "/" + mcpeCustomSkinFolderPath;
            Debug.Log("path :" + mcpePath);
            SaveMCPECallBack(mcpePath);
            //SA_Singleton<AndroidNativeUtility>.instance.GetExternalStoragePath();
            if (Main.Controller.RatingController1.instance.Rate == 0)
            {
                Main.Controller.RatingController1.instance.rateCanvas.gameObject.SetActive(true);
            }
        }

        public IEnumerator WaitDelay(Texture2D image, string name)
        {
            yield return new WaitForSeconds(.2f);
            NativeGallery.SaveImageToGallery(image, name, "Skin" + LastSavedIndex, null);
        }
        public int LastSavedIndex
        {
            get { return PlayerPrefs.GetInt("LastSavedIndex", 0); }

            set { PlayerPrefs.SetInt("LastSavedIndex", value); }
        }

        public void SaveGallery(int index)
        {
            Debug.LogError("SaveGallery");
            LastSavedIndex++;
            if (index == 0)
            {
                Debug.LogError("2217");
                UpdateSkinFromPixels(CurrentResolution);
                OverrideTextureWithSubtexture(capeTexture2217, skin, new Vector2(0, 0));
                StartCoroutine(WaitDelay(capeTexture2217, "3DCapeEditor2217"));
                //NativeGallery.SaveImageToGallery(capeTexture2217, "3DCapeEditor2217", "Skin" + LastSavedIndex, null);
            }
            else if (index == 1)
            {
                Debug.LogError("6432");
                //skin = galleryPickedTexture;

                OverrideTextureWithSubtexture(capeTexture6432, skin, new Vector2(0, 15));
                StartCoroutine(WaitDelay(capeTexture6432, "3DCapeEditor6432"));
                // NativeGallery.SaveImageToGallery(capeTexture6432, "3DCapeEditor6432", "Skin" + LastSavedIndex, null);
            }
            else if (index == 2)
            {
                Debug.LogError("512256");

                OverrideTextureWithSubtexture(capeTexture512256, skin, new Vector2(0, 15));
                StartCoroutine(WaitDelay(capeTexture512256, "3DCapeEditor512256"));
                // NativeGallery.SaveImageToGallery(capeTexture512256, "3DCapeEditor512256", "Skin" + LastSavedIndex, null);
            }
            else
            {
                Debug.LogError("default  2217" + elytraTexture6432);

                OverrideTextureWithSubtexture(elytraTexture6432, skin, new Vector2(22, 10));
                Debug.LogError("next1 " + skin);
                StartCoroutine(WaitDelay(elytraTexture6432, "3DCapeEditor512256"));
                // NativeGallery.SaveImageToGallery(elytraTexture6432, "3DCapeEditor512256", "Skin" + LastSavedIndex, null);
                Debug.LogError("Skin" + LastSavedIndex);
            }





            //else 
            //{ 

            //if (galleryPickedTexture.width == 22 && galleryPickedTexture.height == 17)
            //{
            //    Debug.LogError("2217");
            //    UpdateSkinFromPixels22();
            //    NativeGallery.SaveImageToGallery(skin, "3DCapeEditor2217", "Skin", null);
            //}
            //else if (galleryPickedTexture.width == 64 && galleryPickedTexture.height == 32)
            //{
            //    Debug.LogError("6432");
            //    //skin = galleryPickedTexture;
            //    OverrideTextureWithSubtexture(galleryPickedTexture,skin,new Vector2(0,15));
            //    NativeGallery.SaveImageToGallery(galleryPickedTexture, "3DCapeEditor6432", "Skin", null);


            //}
            //else if (galleryPickedTexture.width == 512 && galleryPickedTexture.height == 256)
            //{
            //    Debug.LogError("512256");
            //    NativeGallery.SaveImageToGallery(galleryPickedTexture, "3DCapeEditor512256", "Skin", null);
            //}
            //else {
            //    Debug.LogError("default  2217");
            //}
            //}


            //Media.Gallery.Pick(PickFromGalleryCallback)
            //SA_Singleton<AndroidCamera>.instance.OnImageSaved += SaveGalleryCallBack;
            //Media.Gallery.SaveImage(skin, "test", ImageFormat.PNG, SaveImageCallback);
            //SA_Singleton<AndroidCamera>.instance.SaveImageToGalalry(skin);


            if (Main.Controller.RatingController1.instance.Rate == 0)
            {
                Main.Controller.RatingController1.instance.rateCanvas.gameObject.SetActive(true);
            }
            Main.Controller.RatingController1.instance.succesfullPooup.SetActive(true);
        }
    }
}