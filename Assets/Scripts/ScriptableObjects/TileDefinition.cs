using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileDefinition",menuName ="New Tile")]
public class TileDefinition : ScriptableObject
{
    [SerializeField] string tileName;
    [SerializeField] Sprite sprite;

}
