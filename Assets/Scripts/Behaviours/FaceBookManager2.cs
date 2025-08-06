//using System.Collections.Generic;
//using UnityEngine;
//using Facebook.Unity;
//using UnityEngine.UI;
//using System.Collections;
//using System.IO;
//using Facebook.MiniJSON;

//public class FaceBookManager2 : MonoBehaviour
//{
//    public static FaceBookManager Instance;
//    //  public Text FriendsText;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        if (!FB.IsInitialized)
//        {
//            FB.Init(() =>
//            {
//                if (FB.IsInitialized)
//                    FB.ActivateApp();
//                else
//                    Debug.LogError("Couldn't initialize");
//            },
//            isGameShown =>
//            {
//                if (!isGameShown)
//                    Time.timeScale = 0;
//                else
//                    Time.timeScale = 1;
//            });
//        }
//        else
//            FB.ActivateApp();
//    }
//    private void Start()
//    {
//        if (GuiManager.instance.Login ==1)
//        {
//            var path = Path.Combine(Application.persistentDataPath + "/ProfileImages/", name + "fb" + ".png");
//            GetTexture(path);
            
//        }
       
//    }

//    #region Login / Logout
//    public void FacebookLogin()
//    {
//        var permissions = new List<string>() { "public_profile", "email" };
//        FB.LogInWithReadPermissions(permissions, AuthCallback);
//    }

//    public void FacebookLogout()
//    {
//        FB.LogOut();
       
//    }
//    #endregion

//    //public void FacebookShare()
//    //{
//    //    FB.ShareLink(new System.Uri("https://resocoder.com"), "Check it out!",
//    //        "Hexa Blast",
//    //        new System.Uri("https://resocoder.com/wp-content/uploads/2017/01/logoRound512.png"));
//    //}

//    #region Inviting
//    public void FacebookGameRequest()
//    {
//        FB.AppRequest("Hey! Come and play this awesome game!", title: "Hexa Blast");
//    }

//    private void AuthCallback(ILoginResult result)
//    {
//        if (FB.IsLoggedIn)
//        {
//            // AccessToken class will have session details
//            var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
//            Debug.Log(aToken.UserId);
//            FetchFBProfile();
//            GetMyProfilePicture();
//            //Print current access token's granted permissions
//            foreach (string perm in aToken.Permissions)
//            {
//                Debug.Log(perm);
//            }
//        }
//        else
//        {
//            Debug.Log("User cancelled login");
//        }
//        Debug.LogError(result.AccessToken);
//        Debug.LogError(result.Cancelled);
//        Debug.LogError(result.Error);
//        Debug.LogError(result.RawResult);
//    }

//    private void FetchFBProfile()
//    {
//        FB.API("/me?fields=first_name,last_name,email,picture", HttpMethod.GET, FetchProfileCallback, new Dictionary<string, string>() { });
//    }

//    private void FetchProfileCallback(IGraphResult result)
//    {
//        if (result.Error == null)
//        {
//           // playerInfo.Instance.PlayerAsGuest = 2;
           
//            Debug.Log(result.RawResult);
//            Dictionary<string, object> FBUserDetails = (Dictionary<string, object>)result.ResultDictionary;

//            string id = FBUserDetails["id"].ToString();
//            GuiManager.instance.PlayerName = FBUserDetails["first_name"].ToString();
//            GuiManager.instance.LoginScreen.SetActive(false);
//            GuiManager.instance.LoginAsGuest = 1;
//            GuiManager.instance.Login = 1;
//            // playerInfo.Instance.MainPlayer.playerID = id;


//            // StartCoroutine(DownloadImageFromFBUrl(id, playerInfo.Instance.MainPlayer.playerName));
//        }
//        else
//        {
//            Debug.LogError("Error Ocuured ");
//        }

//        ////StartCoroutine(ApiController.api_Instance.fbPlayerRegistration ());
//        //StartCoroutine(gameObject.GetComponent<ApiController>().fbPlayerRegistration());

//        //StartCoroutine(gameObject.GetComponent<ProfileImageManager>().DownloadImageFromFBUrl(GameSettings.fbUserID, GameSettings.name));
//        //NetworkManagerDebug.s_Singleton.ChangeTo(NetworkManagerDebug.s_Singleton.playPanel);
//    }

//    public void FacebookInvite()
//    {
//        FB.AppRequest("Come play this great game!",null, null, null, null, null, null,delegate (IAppRequestResult result)
//        {
//            Debug.Log(result.RawResult);
//        });
//    }
//    #endregion

//    //void LoginCallback2(FBResult result)
//    //{
//    //    if (result.Error != null)
//    //        lastResponse = "Error Response:\n" + result.Error;
//    //    else if (!FB.IsLoggedIn)
//    //        lastResponse = "Login cancelled by Player";
//    //    else
//    //    {
//    //        IDictionary dict = Facebook.MiniJSON.Json.Deserialize(result.Text) as IDictionary;
//    //        fbname = dict["first_name"].ToString();
//    //        print("your name is: " + fbname);
//    //    }
//    //}
//    //public void GetFriendsPlayingThisGame()
//    //{
//    //    string query = "/me/friends";
//    //    FB.API(query, HttpMethod.GET, result =>
//    //    {
//    //        var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
//    //        var friendsList = (List<object>)dictionary["data"];
//    //        FriendsText.text = string.Empty;
//    //        foreach (var dict in friendsList)
//    //            FriendsText.text += ((Dictionary<string, object>)dict)["name"];
//    //    });
//    //}

//    public IEnumerator DownloadImageFromFBUrl(string id, string name)
//    {
//        Debug.LogError("DownLoadFromServer facebook" + id);
//        string url = "https" + "://graph.facebook.com/" + id + "/picture?type=large";
//        WWW download = new WWW(url);

//        Debug.LogError("profile url : " + url);
//        //  Texture2D textFb2 = new Texture2D(256, 256, TextureFormat.DXT1, false); //TextureFormat must be DXT5
//        yield return download;
//        if (download.texture != null)
//        {
//            //Debug.LogError ("DownLoad facebook texture not null");
//            if (!Directory.Exists(Application.persistentDataPath + "/" + "ProfileImages"))
//                Directory.CreateDirectory(Application.persistentDataPath + "/" + "ProfileImages");

//            var path = Path.Combine(Application.persistentDataPath + "/ProfileImages/", name + "fb" + ".png");

//            File.WriteAllBytes(path, download.bytes);
//            yield return new WaitForSeconds(1);
//            Debug.Log("Download done : " + path);
//            //UiManager.Instance.path = path;
//            //   UiManager.Instance.GetImage();
//        }
//        // yield return new WaitForSeconds(0.5f);
//        // StopCoroutine("DownloadImageFromFBUrl");
//    }


//    public IEnumerator loadOpponentImage(string id)
//    {
//        string url = "https" + "://graph.facebook.com/" + id + "/picture?type=large";
//        WWW download = new WWW(url);

//        Debug.LogError("profile url : " + url);
//        //  Texture2D textFb2 = new Texture2D(256, 256, TextureFormat.DXT1, false); //TextureFormat must be DXT5
//        yield return download;

//        Texture tex = download.texture;
//        Sprite sp = Sprite.Create((Texture2D)tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f); ;

//        //UiManager.Instance.UpdateAiImage(sp);
//        //profileimage1.sprite = sp;
//    }

//    public void GetMyProfilePicture()
//    {
//        FB.API("/me?fields=picture.width(200).height(200)", Facebook.Unity.HttpMethod.GET, delegate (IGraphResult result)
//        {
//            if (result.Error == null)
//            {
//                Dictionary<string, object> reqResult = Json.Deserialize(result.RawResult) as Dictionary<string, object>;

//                if (reqResult == null) Debug.Log("JEST NULL"); else Debug.Log("nie null");

//                string MyFbAvatarUrl = ((reqResult["picture"] as Dictionary<string, object>)["data"] as Dictionary<string, object>)["url"] as string;
//                StartCoroutine(loadImageMy(MyFbAvatarUrl));
//            }
//            else
//            {
//                Debug.Log("Error retreiving image: " + result.Error);
//            }
//        });
//    }
   
//    public IEnumerator loadImageMy(string url)
//    {
//        Debug.LogError("DownloadAvtar3");
//        WWW www = new WWW(url);
//        yield return www;
//        Sprite profilePic = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f), 32);
//        GuiManager.instance.playerProfileImage.sprite = profilePic;

//        if (www.texture != null)
//        {
//            if (!Directory.Exists(Application.persistentDataPath + "/" + "ProfileImages"))
//                Directory.CreateDirectory(Application.persistentDataPath + "/" + "ProfileImages");

//            var path = Path.Combine(Application.persistentDataPath + "/ProfileImages/", name + "fb" + ".png");

//            File.WriteAllBytes(path, www.bytes);
           
//        }
     
//    }
//    public Texture2D GetTexture(string path)
//    {
//        string finalPath = path;
//        if (!File.Exists(finalPath))
//        {
//            finalPath = path;
//        }
//        byte[] pngBytes = File.ReadAllBytes(finalPath);
//        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
//        tex.LoadImage(pngBytes);
//        tex.wrapMode = TextureWrapMode.Repeat;
//        tex.filterMode = FilterMode.Bilinear;
//        Sprite profilePic = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 32);
//        GuiManager.instance.playerProfileImage.sprite = profilePic;
//        return tex;
//    }

//    //prite sp = Sprite.Create((Texture2D)tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f); ;
//    //profileimage1.sprite = sp;
//}