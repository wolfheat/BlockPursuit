using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{

    [SerializeField] private List<LevelDefinition> levelsEasy = new List<LevelDefinition>();
    [SerializeField] private List<LevelDefinition> levelsMedium = new List<LevelDefinition>();
    [SerializeField] private List<LevelDefinition> levelsHard = new List<LevelDefinition>();

    public static List<LevelDefinition>[] LevelDefinitions { get; private set; }

    private void Awake()
    {
        LevelDefinitions = new List<LevelDefinition>[3] { levelsEasy, levelsMedium, levelsHard };
    }


}
