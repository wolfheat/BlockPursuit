using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject following;

    void Update()
    {
        transform.position = new Vector3(following.transform.position.x, following.transform.position.y, transform.position.z);
    }
}
