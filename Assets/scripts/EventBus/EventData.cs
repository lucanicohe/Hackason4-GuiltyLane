namespace EventBus
{
    public class EventData
    {
        //should be empty
        public float _coolDown { get; private set; }

        public EventData(float coolDown = 0)
        {
            _coolDown = coolDown;
        }
    }
}


