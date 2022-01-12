using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerNigger : MonoBehaviour
{
    Transform target;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.position - offset;
    }
}
