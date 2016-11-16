/*using UnityEngine;
using System.Collections;

public class TransAndRotateForPolygon : MonoBehaviour {
     int speed;
     float friction;
     float lerpSpeed;
     private float xDeg;
     private float yDeg;
     private Quaternion fromRotation;
     private Quaternion toRotation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0))
        {
            xDeg -= Input.GetAxis("Mouse X") * speed * friction;
            yDeg += Input.GetAxis("Mouse Y") * speed * friction;
            fromRotation = transform.rotation;
            toRotation = Quaternion.Euler(yDeg, xDeg, 0);
            transform.rotation = Quaternion.Lerp(fromRotation, toRotation, Time.deltaTime * lerpSpeed);
        }
	}
}*/

using UnityEngine;
using System.Collections;
 
 //[RequireComponent(typeof(MeshCollider))]

public class TransAndRotateForPolygon : MonoBehaviour 
 {
 
     private Vector3 screenPoint;
     private Vector3 offset;
 
     void OnMouseDown()
     {
         if (Input.GetMouseButtonDown(0))
         {
             screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

             offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
         }
         else if (Input.GetMouseButtonDown(1))
         {
             Debug.Log(this.name);
         }
     }
 
     void OnMouseDrag()
     {
         if (Input.GetMouseButton(0))
         {
             Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

             Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
             transform.position = curPosition;
         }
 
     }
 
 }
