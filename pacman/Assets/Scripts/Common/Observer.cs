using UnityEngine;
using System;
using System.Collections.Generic;

namespace Common
{
	public interface IObserver<T>
	{
		void OnCompleted();

		void OnError(Exception exception);

		void OnNext(T value);
	}
 
	public interface IObservable<T>
	{
		IDisposable Subscribe(IObserver<T> observer);
	}
	
	internal class Unsubscriber<T> : IDisposable
	{
		private List<IObserver<T>> observers;
		private IObserver<T> observer;

		internal Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
		{
			this.observers = observers;
			this.observer = observer;
		}

		public void Dispose()
		{
			if (observers.Contains(observer))
				observers.Remove(observer);
		}
	}
}