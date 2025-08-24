using TMPro;
using UnityEngine;

namespace EventBus
{
    public class UIController : MonoBehaviour
    {

        [SerializeField] TMP_Text tMP_Text;
        private void OnEnable()
        {
            EventBus.Subscribe<EventBusTestEvent>(ForTest);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<EventBusTestEvent>(ForTest);
        }

        private  void ForTest(EventBusTestEvent testEvent)
        {
            tMP_Text.text = "Success";
        }
    }
}

