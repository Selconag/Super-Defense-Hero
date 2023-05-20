using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 [RequireComponent(typeof(Rigidbody))]

public class PickUp : MonoBehaviour
{
    public Transform theDest;
    public GameObject pickedUpItem;
    Vector3 mousePressPos;
    Vector3 mouseReleasePos;

    bool isClicking = false;
    //bool isClicking = false;

    private void Start()
    {
        pickedUpItem = this.gameObject;
    }

    private void Update()
    {
        if (Input.touchCount > 0 && !isClicking)
        {
            mousePressPos = Input.GetTouch(0).position;
            Debug.Log("Pressed");
            isClicking = true;


            //mousePressPos = Input.mousePosition;
        }
        else if (Input.touchCount == 0 && isClicking)
        {
            mouseReleasePos = Input.GetTouch(0).position;
            Debug.Log("Released");
            isClicking = false;

            //Do the math for throw

            //mousePressPos = Input.mousePosition;
        }
    //    else if (Input.touchCount > 0 && isClicking)
    //    {
    //        mousePressPos = Input.GetTouch(0).position;
    //        Debug.Log("Pressed");
    //        isClicking = true;


    //        //mousePressPos = Input.mousePosition;
    //    }
    }
    //For picking up
    private void OnMouseDown()
    {
        GetComponent<Rigidbody>().useGravity = false;
        this.transform.position = theDest.position;
        this.transform.parent = theDest.transform;

    }

    //For Aiming
    private void OnMouseDrag()
    {
        //Do calculations here
        //Destroy(this.gameObject);
        Debug.Log("Moving");
    }

    
    //For shooting
    private void OnMouseUp()
    {
        GetComponent<Rigidbody>().useGravity = true;
        //this.transform.position = theDest.position;
        this.transform.parent = null;
    }

}
