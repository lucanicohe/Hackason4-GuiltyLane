using UnityEngine;

namespace EventBus
{
    public class CubeController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                EventBusTestEvent eventBusTestEvent = new EventBusTestEvent();
                EventBus.Raise(eventBusTestEvent);
            }
        }
    }
}

