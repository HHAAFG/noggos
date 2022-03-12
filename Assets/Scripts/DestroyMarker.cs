using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMarker : MonoBehaviour
{
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
