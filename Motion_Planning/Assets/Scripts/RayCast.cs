/*using UnityEngine;
using System.Collections;

public class RayCast : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
*/
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

    void Update()
    {
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButton(0) && Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.red, 0.1f, true);
            Debug.Log(hit.transform.name);
        }*/

        //メインカメラ上のマウスカーソルのある位置からRayを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //レイヤーマスク作成
        //int layerMask = LayerMaskNo.DEFAULT;
        int layerMask = -1;

        //Rayの長さ
        float maxDistance = 10;

        RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction, maxDistance, layerMask);

        //なにかと衝突した時だけそのオブジェクトの名前をログに出す
        if (Input.GetMouseButtonDown(0) && hit.collider)
        {
            IsHit = true; //表示有用左鍵點擊到物件
            objIsHit = hit.collider.gameObject; //紀錄點擊到哪個物件
            Debug.Log(hit.collider.gameObject.name);
            screenPoint = Camera.main.WorldToScreenPoint(hit.collider.gameObject.transform.position);

            offset = objIsHit.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
         }
        else if (Input.GetMouseButton(0) && IsHit)
        {
            //Debug.Log(hit.collider.gameObject.name);
            //Debug.Log(Input.GetAxis("Mouse X"));
            //hit.collider.gameObject.transform.Translate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse X"), 0.0f);

            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            objIsHit.transform.position = curPosition;
        }
        else if (Input.GetMouseButtonUp(0) && IsHit)
        {
            //Debug.Log(hit.collider.gameObject.name);
            //Debug.Log(Input.GetAxis("Mouse X"));
            //hit.collider.gameObject.transform.Translate(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse X"), 0.0f);

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
            //Debug.Log(Input.GetAxis("Mouse X"));
            //hit.collider.gameObject.transform.Rotate(0.0f, 0.0f, 10.0f);

            startDragDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - objIsHit.transform.position;
            initialRotation = objIsHit.transform.rotation;

        }
        else if(Input.GetMouseButton(1) && IsHit)
        {

            currentDragDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - objIsHit.transform.position;
            //gives you the angle in degrees the mouse has rotated around the object since starting to drag
            angleFromStart = Vector3.Angle(startDragDir, currentDragDir);
            //Debug.Log(angleFromStart);
            objIsHit.transform.rotation = initialRotation;
            objIsHit.transform.Rotate(0.0f, 0.0f, -angleFromStart);

        }
        else if (Input.GetMouseButtonUp(1) && IsHit)
        {
            IsHit = false;
            //Debug.Log(objIsHit.transform.rotation);
        }
        
    }
}