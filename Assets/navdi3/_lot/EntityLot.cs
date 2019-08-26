using UnityEngine;
using System.Collections;

public class EntityLot : MonoBehaviour
{
    public static EntityLot NewEntLot(Transform parent = null, string name = "unnamed entlot")
    {
        var gob = new GameObject(name);
        gob.transform.SetParent(parent);
        gob.transform.localPosition = Vector3.zero;
        return gob.AddComponent<EntityLot>();
    }
}
