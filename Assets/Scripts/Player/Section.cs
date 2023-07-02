using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class Section : MonoBehaviour
{
    [field: SerializeField] public List<GameTile> GameTiles { get; set; }
    //public List<Vector2Int> Occupying { get; private set; }
    int rotation = 0;
    [SerializeField] GameTile floorPrefab;
    private int pickedupRotationIndex = 0;
    [SerializeField] private GameObject pivot;
    [SerializeField] private GameObject[] visualTypes;


    public GameObject LevelHolder { get; private set; }

    public void SetLevelHolder(GameObject levelHolder)
    {
        LevelHolder = levelHolder;
        pivot.transform.SetParent(levelHolder.transform, true);
    }

    public void PlaceAt(Vector2Int target, int rotationIndex)
    {
        transform.SetParent(pivot.transform,true);
        
        Vector3 targetPosition = new Vector3(target.x, target.y, pivot.transform.position.z);

        //Debug.Log("Setting Visuals to rotation: " + ((4 + rotationIndex - pickedupRotationIndex) % 4 + " since indexes is " + rotationIndex + "," + pickedupRotationIndex));

        rotation = (rotationIndex - pickedupRotationIndex + rotation)%4;

        Quaternion targetRotation = Quaternion.Euler(0, 0, rotation * 90f);
        pivot.transform.SetLocalPositionAndRotation(targetPosition, targetRotation);

        UpdateTilePos();
    }

    internal void PickedUpAtRotationIndex(int rotationIndex)
    {
        Debug.Log("Setting Pickeduprotation to :" + rotationIndex);
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

        Quaternion targetRotation = Quaternion.Euler(0, 0, (4 + rotationIndex - pickedupRotationIndex+rotation) % 4 * 90f);
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
    }
}
