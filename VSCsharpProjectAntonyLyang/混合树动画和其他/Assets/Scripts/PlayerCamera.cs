using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    public Transform target;

    public float distance = 8.0f;
    public float height = -1.0f;

    void LateUpdate()
    {
        if (!target)
            return;

        transform.position = target.position;
        //Z轴间隔距离
        transform.position -= Vector3.forward * distance;
        //高度调整
        transform.position = new Vector3(transform.position.x, target.transform.position.y+height, transform.position.z);
    }
}
