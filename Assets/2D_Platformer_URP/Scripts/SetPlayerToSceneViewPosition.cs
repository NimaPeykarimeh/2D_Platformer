#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SetPlayerToSceneViewPosition : MonoBehaviour
{
#if UNITY_EDITOR

    // This script sets your player's position to match the Scene View camera's position when you start the game in the Unity Editor.
    // It's helpful for quickly testing your player from a specific location without manually moving the player each time.

    [Tooltip("Determines whether the player will start the game at the center of the Scene View.")]
    [SerializeField] bool SetToView;

    [Tooltip("The color of the crosshair that will be drawn in the Scene View.")]
    [SerializeField] Color CrossColor = Color.white;

    [Tooltip("The size of the crosshair that will be drawn in the Scene View.")]
    [SerializeField] float crossSize = 0.5f;

    Vector3 offset = new Vector3(0f, 0f, 1f);


    void Awake()
    {
        if (SetToView)
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView != null)
            {
                Vector3 pos = sceneView.camera.transform.position;
                if (sceneView.in2DMode) pos.z = 0;
                transform.position = pos;
            }
        }
    }
    private void OnDrawGizmos()
    {

        if (SetToView)
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView.in2DMode)
            {
                Vector3 centerPosition = sceneView.camera.transform.position + offset;
            
                Gizmos.color = CrossColor;

                Gizmos.DrawLine(centerPosition + Vector3.down * crossSize, centerPosition + Vector3.up * crossSize);
                Gizmos.DrawLine(centerPosition + Vector3.left * crossSize, centerPosition + Vector3.right * crossSize);
            }

        }

    }

#endif
}