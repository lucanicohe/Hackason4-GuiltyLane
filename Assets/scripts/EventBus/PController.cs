using UnityEngine;

namespace EventBus
{
    public class PController : MonoBehaviour
    {
        [SerializeField] ParticleSystem effect;
        private void OnEnable()
        {
            EventBus.Subscribe<EventBusTestEvent>(ForTest);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<EventBusTestEvent>(ForTest);
        }

        private void ForTest(EventBusTestEvent eventBus)
        {
            effect.Play();
        }
    }
}

