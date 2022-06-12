using UnityEngine;
using System.Collections.Generic;

namespace Dyspra
{
    public interface ISubject
    {
        void NotifyObservers(GameObject entity, E_Event eventToTrigger);
        void AddObserver(ref AbstractObserver observer);
        void RemoveObserver(ref AbstractObserver observer);
    }

    public class Subject : ISubject
    {
        private List<AbstractObserver> _observers = new List<AbstractObserver>();
        public void NotifyObservers(GameObject entity, E_Event eventToTrigger)
        {
            foreach (AbstractObserver item in _observers)
                item.OnNotify(entity, eventToTrigger);
        }

        public void AddObserver(ref AbstractObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(ref AbstractObserver observer)
        {
            _observers.Remove(observer);
        }
    }
}