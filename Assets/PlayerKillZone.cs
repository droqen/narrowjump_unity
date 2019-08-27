using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillZone : MonoBehaviour
{
    public bool killPlayer = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BouncyPlayerScript>())
        {
            killPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BouncyPlayerScript>())
        {
            killPlayer = false;
        }
    }
}
