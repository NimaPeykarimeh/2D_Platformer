using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class ActiveCameraManager : MonoBehaviour
{
    [SerializeField] CinemachineCamera[] cameras;
    CinemachineCamera thisCam;
    private void Start()
    {
        cameras = FindObjectsOfType<CinemachineCamera>();
        thisCam = GetComponent<CinemachineCamera>();
    }

    void ActivateCamera()
    {
        foreach (CinemachineCamera cam in cameras)
        {
            cam.enabled = cam == thisCam;
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActivateCamera();
        }
    }

}
