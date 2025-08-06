using Main.View;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Main.Controller
{
    public class PencilController1 : SceneElement1
    {
        private ColorController1 color;

        private RootController1 root;

        private Pixel1 lastPixelPainted;

        public Camera cam;

        private void Start()
        {
            color = base.scene.controller.color;
            root = base.scene.controller;
        }

        private void Update()
        {
            if (!IsPointerOverUIObject())
            {
                if (Camera.main != null)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo = default(RaycastHit);
                    if (Input.GetMouseButtonUp(0))
                    {
                        lastPixelPainted = null;
                    }
                    else if (Input.GetMouseButton(0) && root.AllowAction() && Physics.Raycast(ray, out hitInfo) && hitInfo.transform.tag == "Pixel")
                    {
                        Pixel1 component = hitInfo.transform.GetComponent<Pixel1>();
                        SmartPaint(component);
                    }
                }
            }
        }

        private void SmartPaint(Pixel1 pixel)
        {
            if (lastPixelPainted != null && lastPixelPainted != pixel && lastPixelPainted.parent == pixel.parent)
            {
                int num = pixel.i - lastPixelPainted.i;
                int num2 = pixel.j - lastPixelPainted.j;
                int num3 = Mathf.Abs(num);
                int num4 = Mathf.Abs(num2);
                int num5 = (num3 > num4) ? num3 : num4;
                float num6 = (float)num * 1f / (float)num5;
                float num7 = (float)num2 * 1f / (float)num5;
                for (int i = 0; i < num5; i++)
                {
                    int i2 = lastPixelPainted.i + (int)Mathf.Round((float)i * num6);
                    int j = lastPixelPainted.j + (int)Mathf.Round((float)i * num7);
                    try
                    {
                        pixel.parent.GetPixel(i2, j).ChangeColor(color.currentColor);
                    }
                    catch
                    {
                        MonoBehaviour.print("Old Pixel: " + lastPixelPainted.i + "," + lastPixelPainted.j);
                        MonoBehaviour.print("New Pixel: " + pixel.i + "," + pixel.j);
                        MonoBehaviour.print("Steps Count: " + num5);
                        MonoBehaviour.print("Step I: " + num6);
                        MonoBehaviour.print("Step J: " + num7);
                        MonoBehaviour.print("Error at: " + (int)Mathf.Round((float)i * num6) + ", " + (int)Mathf.Round((float)i * num7));
                    }
                }
            }
            pixel.ChangeColor(color.currentColor);
            lastPixelPainted = pixel;
        }

        private bool IsPointerOverUIObject()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);
            List<RaycastResult> list = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, list);
            return list.Count > 0;
        }
    }
}
