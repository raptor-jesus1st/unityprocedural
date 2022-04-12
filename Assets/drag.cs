using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drag : MonoBehaviour
{
    public LayerMask dragmask;
    GameObject selectedobject;
    bool isdragging;
    public GameObject cube;

    void Start()
    {
        isdragging = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           

            if (Physics.Raycast(ray, out hit))
            {
                
                Debug.Log(hit.collider.gameObject.name);
                selectedobject = hit.collider.gameObject;
                isdragging = true;
            }
        }
        if (isdragging == true)
        {
            Vector3 pos = mousepos();
            selectedobject.transform.position = pos;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isdragging = false;
        }
    }
    Vector3 mousepos()
    {
        return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
    }


}
