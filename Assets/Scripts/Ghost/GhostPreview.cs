using Units;
using UnityEngine;

namespace Ghost
{
    public class GhostPreview
    {
        private GameObject _instance;

        public void Create(UnitConfig unitConfig, Vector3 position)
        {
            Destroy();
            if (unitConfig == null || unitConfig.ghostPrefab == null) return;
            _instance = Object.Instantiate(unitConfig.ghostPrefab, position, Quaternion.identity);
        }

        public void Move(Vector3 position)
        {
            if (_instance != null)
                _instance.transform.position = position;
        }

        public void Destroy()
        {
            if (_instance != null)
                Object.Destroy(_instance);
            _instance = null;
        }

    }
}