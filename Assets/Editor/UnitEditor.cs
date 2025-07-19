using Units;
using UnityEditor;
using UnityEngine;

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
            if (unit.StateMachine is null)
            {
                EditorGUILayout.HelpBox("State Machine is not initialized.", MessageType.Warning);
                return;
            }
            else
            {
                if (GUILayout.Button("Initialize State Machine"))
                {
                    unit.StateMachine.Initialize(unit);
                }
            }
            var currentState = unit.StateMachine.GetState();
            EditorGUILayout.LabelField("Current State", 
                currentState != null ? currentState.DisplayName : "None");
        }
    }
}
