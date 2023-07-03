using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TileDefinition",menuName ="New Tile")]
public class TileDefinition : ScriptableObject
{
    [SerializeField] string tileName;
    public Sprite sprite;

}