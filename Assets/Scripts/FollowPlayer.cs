using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject following;
    [SerializeField] Vector2 followingOffsetDistance = new Vector2Int(0,-5);
    private Vector2 followingOffset = Vector2.zero;


    void Update()
    {
        transform.position = new Vector3(following.transform.position.x+followingOffset.x, following.transform.position.y+followingOffset.y, transform.position.z);

        // Look at player
        transform.LookAt(following.transform,Vector3.back);
    }

    public void ChangeView()
    {
        if (followingOffset == Vector2.zero) followingOffset = followingOffsetDistance;
        else followingOffset = Vector2.zero;
    }


}
