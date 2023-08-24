using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AchievementsDefinitions))]
public class AchievementsDefinitionsEditor : Editor
{
    /*public override void OnInspectorGUI()
    {
        AchievementsDefinitions main = (AchievementsDefinitions)target;

        // Display foldout with a custom layout
        //definitions.foldout = EditorGUILayout.Foldout(definitions.foldout, "Achievement Definitions");
        if (true)
        {
            for (int i = 0; i < main.definitions.Length; i++)
            {

                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical();
                main.definitions[i].completedSprite = (Sprite)EditorGUILayout.ObjectField(main.definitions[i].completedSprite, typeof(Sprite), false);
                main.definitions[i].achievementName = EditorGUILayout.TextField(main.definitions[i].achievementName);
                GUILayout.BeginHorizontal();
                main.definitions[i].type = (UnlockRequirementType)EditorGUILayout.EnumPopup(main.definitions[i].type);
                main.definitions[i].value = EditorGUILayout.IntField(main.definitions[i].value);
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
                main.definitions[i].description = EditorGUILayout.TextArea(main.definitions[i].description, GUILayout.Width(150), GUILayout.Height(60));
                GUILayout.EndHorizontal();
                GUILayout.Space(5);

            }
        }
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+",GUILayout.Width(20)))
        {
            main.AddToArray(1);
        }
        if (GUILayout.Button("-", GUILayout.Width(20)))
        {
            main.AddToArray(-1);
        }
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }*/
}