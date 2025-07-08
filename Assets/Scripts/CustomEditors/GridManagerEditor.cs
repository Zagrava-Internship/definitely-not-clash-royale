using Grid;
using Grid.Obstacles;
using Maps;
using UnityEditor;
using UnityEngine;

namespace CustomEditors
{
    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor:Editor
    {
        public override void OnInspectorGUI()
        {
            var gm = (GridManager)target;
            gm.obstacleData = (MapObstacleData)EditorGUILayout.ObjectField("Obstacle Data",
                gm.obstacleData, typeof(MapObstacleData), false);
            gm.usePreloadedSettings = EditorGUILayout.Toggle("Use Preloaded Settings", gm.usePreloadedSettings);
            if (gm.usePreloadedSettings)
            {
                gm.gridSettingsData = (MapGridSettingsData)EditorGUILayout.ObjectField("Grid Settings Data",
                    gm.gridSettingsData, typeof(MapGridSettingsData), false);
                if (gm.gridSettingsData == null)
                {
                    EditorGUILayout.HelpBox("Please assign a MapGridSettingsData asset!", MessageType.Warning);
                }
                else
                {
                    EditorGUILayout.HelpBox("Using settings from MapGridSettingsData asset.", MessageType.Info);
                }
            }
            else
            {
                gm.width = (uint)EditorGUILayout.IntField("Width", (int)gm.width);
                gm.height = (uint)EditorGUILayout.IntField("Height", (int)gm.height);
                gm.cellWidth = EditorGUILayout.FloatField("Cell Width", gm.cellWidth);
                gm.cellHeight = EditorGUILayout.FloatField("Cell Height", gm.cellHeight);
                gm.originPosition = EditorGUILayout.Vector3Field("Origin Position", gm.originPosition);
                gm.autoRegenerate = EditorGUILayout.Toggle("Auto Regenerate", gm.autoRegenerate);
                gm.regenInterval = EditorGUILayout.FloatField("Regen Interval", gm.regenInterval);
            }
            if (GUI.changed)
            {
                EditorUtility.SetDirty(gm);
            }
        }
    }
}