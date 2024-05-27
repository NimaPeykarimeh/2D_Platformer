using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    [SerializeField] Movement2D movement2D;

    public void SetPosition()
    {
        movement2D.UpdateLedgeClimbPosition();
    }
}
