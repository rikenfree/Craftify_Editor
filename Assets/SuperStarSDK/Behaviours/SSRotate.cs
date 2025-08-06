using System.Collections;
using System.Collections.Generic;
using UnityEngine;
   
namespace SuperStarSdk
{
  
    public class SSRotate : MonoBehaviour
    {
		public Space Space { set { space = value; } get { return space; } }
		[SerializeField] private Space space = Space.Self;

		/// <summary>The position will be incremented by this each second.</summary>
		public Vector3 PerSecond { set { perSecond = value; } get { return perSecond; } }
		[SerializeField] private Vector3 perSecond;

		void Update()
		{
			transform.Rotate(perSecond * Time.deltaTime, space);
		}
	}
}