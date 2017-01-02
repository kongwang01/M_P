using UnityEngine;
public class RayCast : MonoBehaviour
{
    GameObject objIsHit;
    private bool IsHit = false;
    private Vector3 screenPoint;
    private Vector3 offset;

    int speed;
    float friction;
    float lerpSpeed;
    private float xDeg;
    private float yDeg;
    private Quaternion fromRotation;
    private Quaternion toRotation;

    Vector3 startDragDir;
    Vector3 currentDragDir;
    Quaternion initialRotation;
    float angleFromStart;

	// Use this for initialization
	void Start () {

	}

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //int layerMask = LayerMaskNo.DEFAULT;
        int layerMask = -1;
        float maxDistance = 10;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, maxDistance, layerMask);


        if (Input.GetMouseButtonDown(0) && hit.collider)
        {
			//Debug.Log ("mouse :" +Input.mousePosition + " , ray :" + (Vector2)ray.origin + " , D :" + (Vector2)ray.direction);
            IsHit = true; //表示有用左鍵點擊到物件
            objIsHit = hit.collider.gameObject; //紀錄點擊到哪個物件
            Debug.Log(hit.collider.gameObject.name);
            screenPoint = Camera.main.WorldToScreenPoint(hit.collider.gameObject.transform.position);

            offset = objIsHit.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
         }
        else if (Input.GetMouseButton(0) && IsHit)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            objIsHit.transform.position = curPosition;
        }
        else if (Input.GetMouseButtonUp(0) && IsHit)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            objIsHit.transform.position = curPosition;
            IsHit = false;
            Debug.Log(curPosition);
        }
        else if (Input.GetMouseButtonDown(1) && hit.collider)
        {
            IsHit = true; //表示有用右鍵點擊到物件
            objIsHit = hit.collider.gameObject;

            Debug.Log(hit.collider.gameObject.name);

            startDragDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - objIsHit.transform.position;
            initialRotation = objIsHit.transform.rotation;

        }
        else if(Input.GetMouseButton(1) && IsHit)
        {

            currentDragDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - objIsHit.transform.position;
            //gives you the angle in degrees the mouse has rotated around the object since starting to drag
            angleFromStart = Vector3.Angle(startDragDir, currentDragDir);
			//Vector3 judgePosOrNeg = Vector3.Cross(startDragDir, currentDragDir);
			//if (judgePosOrNeg.y < 0)
			//	angleFromStart = -angleFromStart;


            //Debug.Log(angleFromStart);
            objIsHit.transform.rotation = initialRotation;

            objIsHit.transform.Rotate(0.0f, 0.0f, angleFromStart);
			
			initialRotation = objIsHit.transform.rotation;
			startDragDir = currentDragDir;
        }
        else if (Input.GetMouseButtonUp(1) && IsHit)
        {
            IsHit = false;
			//objIsHit.transform.Rotate(0.0f, 0.0f, 30.0f);
            Debug.Log(objIsHit.transform.rotation.eulerAngles);
        }
        
    }
}