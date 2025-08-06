using Main.View;
using UnityEngine;

namespace Main.Controller
{
	public class CameraController1 : SceneElement1
	{
		public MasterCanvas1 MC;
		public Camera Cam;

		public float distance = 10f;

		public float xSpeed = 80f;

		public float ySpeed = 200f;

		public float yMinLimit = -90f;

		public float yMaxLimit = 90f;

		public float distanceMin = 0.5f;

		public float distanceMax = 15f;

		private bool justTouched = true;

		private float x;

		private float y;

		private Transform mainCamera;

		private Character1 character;

		private RootController1 root;

		private void Start()
		{
			InitData();
			UpdateCamera();
		}

		public void ZoomIn()
		{
			distance -= 1f;
			UpdateCamera();
		}

		public void ZoomOut()
		{
			distance += 1f;
			UpdateCamera();
		}

		public bool isAbleToRotate = false;

		private void LateUpdate()
		{
            if (Input.GetMouseButtonDown(0))
            {
				isAbleToRotateCape();

			}

			if (Input.GetMouseButtonUp(0))
			{
				isAbleToRotate = false;

			}

			if ((bool)character)
			{
				PerformRotate();
			}
		}

		private void InitData()
		{
			mainCamera = base.scene.view.camera;
			character = base.scene.view.character;
			root = base.scene.controller;
			x = mainCamera.eulerAngles.y;
			y = mainCamera.eulerAngles.x;
		}

		private void UpdateCamera()
		{
			Quaternion rotation = Quaternion.Euler(y, x, 0f);
			Vector3 point = new Vector3(0f, 0f, 0f - distance);
			Vector3 position = rotation * point + character.transform.position;
			base.scene.view.camera.rotation = rotation;
			base.scene.view.camera.position = position;
		}

		private void PerformRotate()
		{
			if (Application.platform == RuntimePlatform.Android|| Application.platform == RuntimePlatform.IPhonePlayer)
			{
				RotateOnMobile();
			}
			else
			{
				RotateOnDesktop();
			}
		}

		private void RotateOnDesktop()
		{
			if (Input.GetMouseButton(0) && isAbleToRotate)
			{
                /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 25))
                {
                    if (hit.collider != null && hit.collider.gameObject.tag == "Pixel")
                    {
                        return;
                    }
                }*/
                Rotate();
			}
		}

		public LayerMask layerMask;
		public void isAbleToRotateCape() {


			Vector3 mousePosFar = new Vector3(Input.mousePosition.x,
				Input.mousePosition.y, Cam.farClipPlane);

			Vector3 mousePosNear = new Vector3(Input.mousePosition.x,
				Input.mousePosition.y, Cam.nearClipPlane);

			Vector3 mousePosF = Cam.ScreenToWorldPoint(mousePosFar);
			Vector3 mousePosN = Cam.ScreenToWorldPoint(mousePosNear);

			
			RaycastHit hit;

			// Only return true if the ray hits a collider in layerMask
			if (Physics.Raycast(mousePosN,mousePosF-mousePosN, out hit,200) && MC.isEditorShowing)
			{
				//Debug.Log("You hit: " + hit.collider.name);

				if (hit.collider.CompareTag("Pixel"))
				{

					isAbleToRotate = false;
				}
				else {
					isAbleToRotate = true;
				}

			}
			else
			{
				isAbleToRotate = false;
				Debug.Log("You didn't hit anything on the specified layer");
			}
			
		}




		private void RotateOnMobile()
		{
			if (UnityEngine.Input.touchCount > 0 && isAbleToRotate)
			{
				Touch touch = UnityEngine.Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began)
				{
					justTouched = true;
				}
				else if (touch.phase == TouchPhase.Moved && justTouched)
				{
					justTouched = false;
				}
				else if (touch.phase == TouchPhase.Moved)
				{
					Rotate();
				}
			}
		}

		private void Rotate()
		{
			UpdateXYVariables();
			UpdateCamera();
		}

		private void UpdateXYVariables()
		{
			x += UnityEngine.Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
			y -= UnityEngine.Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			y = ClampAngle(y, yMinLimit, yMaxLimit);
		}

		public static float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
			return Mathf.Clamp(angle, min, max);
		}
	}
}
