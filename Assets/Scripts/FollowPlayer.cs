using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] GameObject following;
    [SerializeField] Vector2 followingOffsetDistance = new Vector2Int(0,-5);
    private Vector2 followingOffset = Vector2.zero;
    private bool tweening;
    private int id = 0;
    private Vector3[] camPos;
    private Vector3 startPosition;
    private Vector3 targetPosition;

    private const float TweenTime = 0.6f;

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
        id = SavingUtility.gameSettingsData.CameraPos;
    }

    private void Start()
    {
        camPos = new Vector3[4] { new Vector3(0, 0,-14.73f), new Vector3(0,-5f, -14.73f), new Vector3(0,0, -25f), new Vector3(0,0, -35f) };            
    }

    void Update()
    {
        if (tweening) return;

        Vector3 newCameraPosition = new Vector3(camPos[id].x+following.transform.position.x, camPos[id].y + following.transform.position.y, camPos[id].z + following.transform.position.z);

        transform.position = newCameraPosition;
        // Look at player
        transform.LookAt(following.transform,Vector3.back);
    }

    public void ChangeView()
    {
        tweening = true;
        startPosition = transform.position;
        
        id = (id + 1)%camPos.Length;
        SavingUtility.gameSettingsData.ChangeCameraPos(id);
        targetPosition = camPos[id] + following.transform.position; 
        StartCoroutine(TweenToPosition());
    }


    private IEnumerator TweenToPosition()
    {
        float timer = 0;
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, targetPosition, timer/TweenTime);
            transform.LookAt(following.transform, Vector3.back);
            yield return null;
        }
        tweening = false;
    }
}
