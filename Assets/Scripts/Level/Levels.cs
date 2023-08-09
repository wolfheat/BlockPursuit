using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour
{

    [SerializeField] private List<LevelDefinition> levelsEasy = new List<LevelDefinition>();
    [SerializeField] private List<LevelDefinition> levelsMedium = new List<LevelDefinition>();
    [SerializeField] private List<LevelDefinition> levelsHard = new List<LevelDefinition>();
    [SerializeField] private List<LevelDefinition> levelsABC = new List<LevelDefinition>();

    public static List<LevelDefinition>[] LevelDefinitions { get; private set; }

    private void Awake()
    {
        LevelDefinitions = new List<LevelDefinition>[4] { levelsEasy, levelsMedium, levelsHard, levelsABC };

        DefineLevelIndexes();
    }

    private void DefineLevelIndexes()
    {
        for (int i = 0; i < LevelDefinitions.Length; i++)
        {
            for(int j = 0; j< LevelDefinitions[i].Count; j++)
            {
                LevelDefinitions[i][j].SetLevelIndex(i,j);
            }
        }
    }
}
