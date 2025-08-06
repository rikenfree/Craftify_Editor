using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Main.View
{
    public class Elytra1 : MonoBehaviour
    {
      //  public static Elytra Instance;

        public Texture2D skin;
        public Texture2D elytraTexture;
        public Texture2D subElytraTexture;
        public Transform ElytraTrans;
        public BodyPart1[] bodyParts;
        public Color[] colorMaps;
       
        public bool tryAutoLoad;
        private void Awake()
        {
            //if (Instance==null)
            //{
            //    Instance = this;
            //}

            InitDefaultSkin();
            InitBodyParts();
            tryAutoLoad = false;
        }

        private void InitDefaultSkin()
        {
            skin = Resources.Load<Texture2D>("elytra/empty");
           
            LoadDefaultElytraSkin();
        }

        public void InitBodyParts()
        {
            Component[] componentsInChildren = GetComponentsInChildren(typeof(BodyPart1), includeInactive: true);
            bodyParts = new BodyPart1[componentsInChildren.Length];
            for (int i = 0; i < bodyParts.Length; i++)
            {
                bodyParts[i] = componentsInChildren[i].GetComponent<BodyPart1>();
            }
        }
        //private void Update()
        //{
        //    if (!tryAutoLoad)
        //    {
        //        AutoLoad();
        //        tryAutoLoad = true;
        //    }
        //}
        public void LoadDefaultElytraSkin()
        {

            if (elytraTexture==null)
            {
                Debug.LogError("elytra Texture is  null");
                return;
            }

            if (elytraTexture.width == 64 && elytraTexture.height == 32)
            {

                Debug.LogError("texture is available");  //Debug.Break();
                subElytraTexture = TextureUtility1.GetSubTexture(elytraTexture, new Vector2(22, 10), new Vector2(24, 22));
                skin = subElytraTexture;
                  // Paint2422TextureOnSkin(pickedTexture2D2217);
                  //   tempRawImage.texture = skin;
                colorMaps = subElytraTexture.GetPixels();
                TempLoad();
            }
            else 
            {
                Debug.LogError("elytra not supported");
            }

            //  skin = elytraTexture;
         //   tempcolorMaps = elytraTexture.GetPixels();
          //  tempcolorMaps123 = elytraTexture.GetPixels32();
        }

        public void TempLoad()//After change cap texture refresh color
        {
            BodyPart1[] array = bodyParts;
            foreach (BodyPart1 bodyPart in array)
            {
                if (bodyPart.HasPixelsData())
                {
                    bodyPart.RefreshElytraData();
                }
            }
        }

        public Color GetColor(int i, int j)
        {
            Debug.Log("GetColor:" + i + " :j:" + j +"  final =>"+ j * skin.width + i);
            return colorMaps[j * skin.width + i];
        }
        private void Paint2422TextureOnSkin(Texture2D s)
        {
            if (s.width != 24 || s.height != 22)
            {
                Debug.LogError("return kare che");
                return;
            }
            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 22; j++)
                {
                    skin.SetPixel(i, j, s.GetPixel(i, j));
                    skin.Apply();
                }
            }
        }
    }
}