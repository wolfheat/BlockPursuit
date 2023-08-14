using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject following;
    [SerializeField] Vector2 followingOffsetDistance = new Vector2Int(0,-5);
    private Vector2 followingOffset = Vector2.zero;

    private int id = 0;
    private Vector3[] camPos;

    private void OnEnable()
    {
        SavingUtility.LoadingComplete += SetActiveControlFromSave;

    }
    private void OnDisable()
    {
        SavingUtility.LoadingComplete -= SetActiveControlFromSave;
    }

    private void SetActiveControlFromSave()
    {
        id = SavingUtility.playerGameData.CameraPos;
    }

    private void Start()
    {
        camPos = new Vector3[4] { new Vector3(0, 0,-14.73f), new Vector3(0,-5f, -14.73f), new Vector3(0,0, -25f), new Vector3(0,0, -35f) };            
    }

    void Update()
    {
        Vector3 newCameraPosition = new Vector3(camPos[id].x+following.transform.position.x, camPos[id].y + following.transform.position.y, camPos[id].z + following.transform.position.z);

        transform.position = newCameraPosition;
        // Look at player
        transform.LookAt(following.transform,Vector3.back);
    }

    public void ChangeView()
    {
        id = (id + 1)%camPos.Length;
        SavingUtility.playerGameData.ChangeCameraPos(id);
    }


}
