using UnityEngine;
using System.Collections;

public class DotFactionizer : MonoBehaviour
{
    public int faction = 0;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var dot = collision.GetComponent<DotMazer>();
        if (dot != null)
        {
            dot.SetFaction(this.faction); // change dot to my colour
        }
    }
}
