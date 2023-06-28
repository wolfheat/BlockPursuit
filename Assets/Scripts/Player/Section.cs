using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class Section : MonoBehaviour
{
    int[,] cubesID;
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

    internal void CreateAsSectionType(SectionType type)
    {
        //Debug.Log("Creating as section type: position is: "+ transform.position+ "at parent "+transform.parent);
        switch (type)
        {
            case SectionType.I:
                cubesID = new int[4, 1] { { 1 }, { 1 }, { 1 }, { 1 }};
                break;
            case SectionType.O:
                cubesID = new int[2, 2] { { 1, 1 }, { 1, 1 }};
                break;
            case SectionType.L:
                cubesID = new int[3, 2] { { 1, 1 }, { 1, 0 }, { 1, 0}};
                break;
            default:
                break;
        }
        Create();
    }

    private void Create()
    {
        GameTiles = new List<GameTile>();
        for (int i = 0; i < cubesID.GetLength(0); i++) 
        {
            for (int j = 0; j < cubesID.GetLength(1); j++)
            {
                if (cubesID[i, j] == 0) continue;

                GameTile newFloor = Instantiate(floorPrefab,transform,false);
                newFloor.transform.localPosition = new Vector3(i, j, 0);
                GameTiles.Add(newFloor);
            }
        }
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

    internal void SetVisualTo(Vector2Int target, int rotationIndex)
    {
        transform.SetParent(pivot.transform, true);

        Vector3 targetPosition = new Vector3(target.x, target.y, pivot.transform.position.z);

        //Debug.Log("Setting Visuals to rotation: "+ ((4 + rotationIndex - pickedupRotationIndex)%4+" since indexes is "+rotationIndex+","+pickedupRotationIndex));

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
