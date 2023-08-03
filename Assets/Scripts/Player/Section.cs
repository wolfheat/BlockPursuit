using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    [field: SerializeField] public List<GameTile> GameTiles { get; set; }
    //public List<Vector2Int> Occupying { get; private set; }

    public TileType TileType { get; set; }
    public Vector2Int Position { get; set; }
    public int Rotation{ get; set; }

    [SerializeField] GameTile floorPrefab;
    private int pickedupRotationIndex = 0;
    [SerializeField] private GameObject originalPivot;
    [SerializeField] private GameObject pivot;
    [SerializeField] private GameObject[] visualTypes;

    public GameObject OriginalPivot { get => originalPivot; private set { originalPivot = value;} }

    public GameObject TileHolder { get; private set; }

    private Coroutine shake;

    public void ShakeTile()
    {
        if (shake != null || !SavingUtility.playerGameData.gameEffectsSettings.UseShake) return;
        shake = StartCoroutine(Shake());
    }

    public void InterruptShakeIfShaking()
    {
        if (shake == null) return;

        StopCoroutine(shake);
        shake = null;
        transform.position = startPosition;
    }


    private const float ShakeDistance = 0.15f;
    private const float MinShakeDistance = 0.004f;
    private const float ShakeDampening = 0.85f;
    private const float ShakeSpeed = 14f;
    private const float ShakeTime = 0.25f;
    private Vector3 startPosition;

    private IEnumerator Shake()
    {
        // Need to exit shake if lifting it?

        startPosition = transform.position;
        // Shake tile Right To left
        // Make tile move shaking Right to left
        float timer = 0;
        float displacement = 0;
        float CurrentShakeDistance = ShakeDistance;
        int dir = 1;
        while(timer < ShakeTime && CurrentShakeDistance > MinShakeDistance)
        {
            float moveDistance = ShakeSpeed * Time.deltaTime * dir;
            timer += Time.deltaTime;
            displacement += moveDistance;

            transform.position += new Vector3(moveDistance,0,0);    

            if((dir==1 && displacement > CurrentShakeDistance)|| (dir == -1 && displacement < -CurrentShakeDistance))
            {
                //Change direction
                dir *= -1;
                // DampenDistance
                CurrentShakeDistance *= ShakeDampening;
            }
            yield return null;
        }

        //reset to start position
        transform.position = startPosition;

        shake = null;

        //if (timer >= ShakeTime) Debug.Log("Shake Timed Out");
        //else if(CurrentShakeDistance <= MinShakeDistance) Debug.Log("Shake Exited due to being small");
    }

    public void SetHolder(GameObject tileHolder)
    {
        TileHolder = tileHolder;
        pivot.transform.SetParent(tileHolder.transform, true);
    }

    public void PlaceAt(Vector2Int target, int rotationIndex)
    {
        transform.SetParent(pivot.transform,true);
        
        Position = target;
        Vector3 targetPosition = new Vector3(target.x, target.y, pivot.transform.position.z);

        //Debug.Log("Setting Visuals to rotation: " + ((4 + rotationIndex - pickedupRotationIndex) % 4 + " since indexes is " + rotationIndex + "," + pickedupRotationIndex));

        Rotation = (rotationIndex - pickedupRotationIndex + Rotation) %4;


        Quaternion targetRotation = Quaternion.Euler(0, 0, Rotation * 90f);
        pivot.transform.SetLocalPositionAndRotation(targetPosition, targetRotation);

        UpdateTilePos();
    }

    internal void PickedUpAtRotationIndex(int rotationIndex)
    {
        pickedupRotationIndex = rotationIndex;
    }

    internal void SetPivotPosition(GameTile pickedTile)
    {
        transform.SetParent(pivot.transform.parent, true);
        pivot.transform.position = pickedTile.transform.position;
        transform.SetParent(pivot.transform, true);
    }

    internal void Held(bool held, bool valid)
    {

        //Debug.Log("Update Valid: "+valid);
        visualTypes[0].gameObject.SetActive(!held);
        visualTypes[1].gameObject.SetActive(held && valid);
        visualTypes[2].gameObject.SetActive(held && !valid);        
    }

    internal void Used(bool used)
    {
        visualTypes[0].gameObject.SetActive(used);
        visualTypes[1].gameObject.SetActive(false);
        visualTypes[2].gameObject.SetActive(false);        
    }

    internal void SetVisualTo(Vector2Int target, int rotationIndex)
    {
        transform.SetParent(pivot.transform, true);

        Vector3 targetPosition = new Vector3(target.x, target.y, pivot.transform.position.z);

        Quaternion targetRotation = Quaternion.Euler(0, 0, (4 + rotationIndex - pickedupRotationIndex+ Rotation) % 4 * 90f);
        pivot.transform.SetLocalPositionAndRotation(targetPosition,targetRotation);
        UpdateTilePos();
    }

    internal void UpdateTilePos()
    {
        // Set tiles POS
        foreach (GameTile tile in GameTiles)
        {
            tile.Pos = new Vector2Int(Mathf.RoundToInt(tile.transform.position.x), Mathf.RoundToInt(tile.transform.position.y));
            //Debug.Log("Setting tile position to: " + tile.Pos);
            tile.section = this;
        }
    }

    internal void DestroyParts()
    {
        for (int i = GameTiles.Count-1; i >= 0; i--)
        {
            Destroy(GameTiles[i].gameObject);
        }
        Destroy(pivot.gameObject);
    }
}
