using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] Transform objectToFollow;
    [SerializeField] Vector3 offset = new Vector3(0f,0f,-10f);
    [SerializeField] Vector3 speedMultiplier = new Vector3(1f,0.1f,1f);
    [Range(0f,1f)]
    [SerializeField] float speed = 0.125f;

    private void LateUpdate()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = objectToFollow.position + offset;

        startPos.x = Mathf.Lerp(startPos.x, endPos.x, speed * speedMultiplier.x) ;
        startPos.y = Mathf.Lerp(startPos.y, endPos.y, speed * speedMultiplier.y);
        startPos.z = Mathf.Lerp(startPos.z, endPos.z, speed * speedMultiplier.z);

        transform.position = startPos;
    }
}
