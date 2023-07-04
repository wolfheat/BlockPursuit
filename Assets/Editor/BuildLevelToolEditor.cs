using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

[CustomEditor(typeof(BuildLevelTool))]
public class BuildLevelToolEditor : Editor
{

    public string path = "Assets/Scripts/ScriptableObjects/Levels/";
    private int counter = 10;

    public override void OnInspectorGUI()
    {
        BuildLevelTool tool = (BuildLevelTool)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate Level Definition"))
        {
            Debug.Log("Generating Level Definition");
            GenerateLevelDefinition();
        }
    }

    private void GenerateLevelDefinition()
    {
        LevelDefinition newLevelDefinition = CreateInstance<LevelDefinition>();
        LevelCreator levelCreator = FindObjectOfType<LevelCreator>();
        PlayerController player = FindObjectOfType<PlayerController>();

        counter++;
        
        newLevelDefinition.SetLevelDefinition("Level", levelCreator.GetTilePlacementList(), levelCreator.GetFillAreaPositions(), player.Position);

        AssetDatabase.CreateAsset(newLevelDefinition, path + "Level"+ counter + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
