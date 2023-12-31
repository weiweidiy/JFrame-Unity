﻿using System;
using System.Collections.Generic;

namespace JFrame.Common
{
    public class Unsubscriber<T> : IDisposable
    {
        private List<IObserver<T>> _observers;
        private IObserver<T> _observer;

        internal Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }


    public class UnSubScriberProd : IDisposable
    {
        private List<ProdObserver> _observers;
        private ProdObserver _observer;

        internal UnSubScriberProd(List<ProdObserver> observers, ProdObserver observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }

        public bool HasAction(Action<string, ProdInfo, string> action)
        {
            return this._observer.HasAction(action);
        }
    }

}