using UnityEngine;

namespace Maps
{
    [CreateAssetMenu(fileName = "MapGridSettingsData", menuName = "Game/MapGridSettingsData")]
    public class MapGridSettingsData: ScriptableObject
    {
        [Header("Grid Settings")] 
        public uint width;
        public uint height;
        [Header("Cell Settings")] 
        public float cellWidth;
        public float cellHeight;
        [Header("Origin Position")]
        public Vector3 originPosition = Vector3.zero;
        [Header("Debug Grid Regeneration")] 
        public bool autoRegenerate;
        public float regenInterval;
    }
}