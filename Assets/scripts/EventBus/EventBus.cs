using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventBus
{
    public static class EventBus
    {
        private static Dictionary<Type, Delegate> eventTable = new();

        public static void Subscribe<T>(Action<T> listener) where T : EventData
        {
            var type = typeof(T);
            if (eventTable.TryGetValue(type, out var existing))
            {
                eventTable[type] = Delegate.Combine(existing, listener);
            }
            else
            {
                eventTable[type] = listener;
            }
        }

        public static void Unsubscribe<T>(Action<T> listener) where T : EventData
        {
            var type = typeof(T);
            if (eventTable.TryGetValue(type, out var existing))
            {
                eventTable[type] = Delegate.Remove(existing, listener);
            }
        }

        public static void Raise<T>(T eventData) where T : EventData
        {
            var type = typeof(T);
            if (eventTable.TryGetValue(type, out var del))
            {
                if (del is Action<T> action)
                {
                    action.Invoke(eventData);
                }
            }
        }

        public static void Clear()
        {
            eventTable.Clear();
        }
    }
}

