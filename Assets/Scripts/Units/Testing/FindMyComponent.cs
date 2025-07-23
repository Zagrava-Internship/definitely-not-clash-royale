using Targeting;
using UnityEngine;

namespace Units.Testing
{
    public class FindMyComponent : MonoBehaviour
    {
        // Можешь нажать на эту кнопку в инспекторе в Play Mode,
        // чтобы запустить поиск в любой момент.
        [ContextMenu("Run Search")]
        public void SearchForTargetables()
        {
            Debug.Log("--- НАЧАЛО ПОИСКА КОМПОНЕНТОВ ---", this.gameObject);

            // Ищем ВСЕ компоненты типа TargetableBase (и его наследников, как Unit)
            // в этом объекте и во всех его дочерних объектах.
            // Параметр 'true' означает, что ищем даже на неактивных GameObject'ах.
            var components = GetComponentsInChildren<TargetableBase>(true);

            if (components.Length == 0)
            {
                Debug.LogWarning("Ни одного компонента типа TargetableBase или его наследников не найдено.");
            }
            else
            {
                Debug.Log($"Найдено компонентов: {components.Length}");
                foreach (var component in components)
                {
                    // Выводим имя GameObject'а и точный тип найденного компонента
                    Debug.Log($"Объект: '{component.gameObject.name}' содержит компонент типа: '{component.GetType().Name}'", component.gameObject);
                }
            }

            Debug.Log("--- КОНЕЦ ПОИСКА ---", this.gameObject);
        }

        // Также можно запустить поиск один раз при старте
        void Start()
        {
            SearchForTargetables();
        }
    }
}