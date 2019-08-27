using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectabledot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BouncyPlayerScript>())
        {
            Object.Destroy(this.gameObject);
        }
    }
}
