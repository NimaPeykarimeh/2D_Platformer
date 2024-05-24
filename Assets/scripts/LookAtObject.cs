using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class LookAtObject : MonoBehaviour
{
    [SerializeField] Transform objectToRotate;
    [SerializeField] Transform objectToLook;
    [SerializeField] float speed;

    [Header("Ray")]
    [SerializeField] float rayDistance = 10f;

    private void Start()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector3 _dir = objectToLook.position - transform.position;
        _dir.z = 0;
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        objectToRotate.right = Vector3.MoveTowards(objectToRotate.right, _dir, speed * Time.deltaTime);

        objectToLook.transform.position = mousePos;

        RaycastHit2D rayHit2D = Physics2D.Raycast(cameraPos, mousePos - cameraPos, rayDistance);
        if (rayHit2D)
        {
            
            Debug.DrawRay(cameraPos, mousePos - cameraPos * rayHit2D.distance,color: UnityEngine.Color.green);
        }
        else 
        {
            Debug.DrawRay(cameraPos, mousePos - cameraPos * 5f, color: UnityEngine.Color.red);
        }
    }
}
