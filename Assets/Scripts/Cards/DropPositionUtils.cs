using Maps.MapManagement.Grid;
using UnityEngine;

namespace Cards
{
    public static class DropPositionUtils
    {
        public static Vector3 ScreenToSnappedWorld(Vector2 screenPosition, Camera cam, float z)
        {
            var camZ = Mathf.Abs(cam.transform.position.z);
            var pos = cam.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, camZ));
            pos.z = z;
            var node = GridManager.Instance.GetNodeFromWorldPoint(pos);
            return node?.WorldPosition ?? pos;
        }
        public static bool IsValidDropScreenPosition(Vector2 screenPosition, Camera cam, float z)
        {
            var worldPos = ScreenToSnappedWorld(screenPosition, cam, z);
            var node = GridManager.Instance.GetNodeFromWorldPoint(worldPos);
            return node is { IsWalkable: true };
        }
    }
}