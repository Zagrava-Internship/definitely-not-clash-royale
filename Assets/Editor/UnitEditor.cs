using Units;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(Unit))]
    public class UnitEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var unit = (Unit)target;
            EditorGUILayout.Space();
            var currentState = unit.GetState();
            EditorGUILayout.LabelField("Current State", 
                currentState != null ? currentState.DisplayName : "None");
        }
    }
}
