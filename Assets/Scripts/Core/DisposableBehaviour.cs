using System;
using UniRx;
using UnityEngine;

namespace Core
{
    public class DisposableBehaviour<T> : MonoBehaviour
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