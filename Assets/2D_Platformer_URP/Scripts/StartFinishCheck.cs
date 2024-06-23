using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartFinishCheck : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Start"))
        {
            RaceTimer.instance.StartRace();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            RaceTimer.instance.FinishRace();
        }
    }
}
