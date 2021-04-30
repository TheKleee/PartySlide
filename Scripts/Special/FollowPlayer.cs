using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Target To Follow:")]
    public Transform target;

    void FixedUpdate()
    {
        if (target != null)
            transform.position = Vector3.Lerp(transform.position, target.position, 35 * Time.deltaTime);
        else
            Destroy(gameObject);
    }
}
