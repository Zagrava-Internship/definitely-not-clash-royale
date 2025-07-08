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
            EditorGUILayout.LabelField("Current State", unit.GetState().DisplayName ?? "None");
        }
    }
}
