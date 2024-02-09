using System;
using UniRx;
using UnityEngine;

namespace Core
{
    public class DisposableBehaviour<T> : MonoBehaviour where T : class
    {
        protected CompositeDisposable Disposables;
        protected T ActiveModel { get; private set; }

        public IDisposable Init(T model)
        {
            Disposables = new CompositeDisposable();

            ActiveModel = model;

            OnInit();

            return Disposables;
        }

        protected virtual void OnInit()
        {
        }
    }
}