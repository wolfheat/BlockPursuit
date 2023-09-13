using UnityEditor;
using UnityEngine;

namespace MenuTool
{
    [CustomEditor(typeof(BasePanel),true)]
    public class EnableDisableMenuTool : Editor
    {
        Texture2D buttonTexture;
        private void ToggleMenu(BasePanel panel) => panel.TogglePanel();
        public override void OnInspectorGUI()
        {
            BasePanel panel = (BasePanel)target;

            var style = new GUIStyle() { 
                alignment = TextAnchor.MiddleCenter,
                normal = new GUIStyleState() {background = Texture2D.grayTexture, textColor = Color.black} ,
                fixedHeight = 25,
                fixedWidth = 55
            };
            
            EditorGUILayout.BeginHorizontal();
            GUI.color = Color.white;
            GUILayout.Space(10);
            if (GUILayout.Button("Toggle", style))
                ToggleMenu(panel);
            EditorGUILayout.EndHorizontal();
            DrawDefaultInspector();
        }
    }
}
