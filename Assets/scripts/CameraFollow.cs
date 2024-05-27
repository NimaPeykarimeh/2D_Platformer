using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] Transform objectToFollow;
    [SerializeField] Vector3 offset = new Vector3(0f,0f,-10f);

    [Range(0f,1f)]
    [SerializeField] float speed = 0.125f;

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position ,objectToFollow.position + offset,speed);
    }
}
