﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AwaitFail2
{



class AsyncCoordinator : INotifyCompletion
{
private Queue<Action> actions = new Queue<Action>();				// actions a executar
private Queue<Action> actionsToBeContinued = new Queue<Action>();	// quan s'acaben d'executar, es tornen a posar a actions2 si cal

public delegate Task AsyncSequenceFunction (AsyncCoordinator coordinator);


public void scheduleCoroutine (AsyncSequenceFunction func)
{
	actions.Enqueue (
		async ()=>
		{
			await func (this);
		});
}

public bool RunStep ()
{

	if (actions.Count<=0 && actionsToBeContinued.Count<=0) return false;

	while (actions.Count>0)
	{
		// Debug note: Es impossible fer un step in cap a dins, pero podem posar un breakpoint al OnComplete
		// per saber on es amb el stack trace
		var act=actions.Dequeue();
		act.Invoke(); 
	}

	// swap
	swap (ref actions,ref actionsToBeContinued);


	return true;

}


void swap<T> (ref T x,ref T y)
{
	T tmp=x;
	x=y;
	y=tmp;
}


#region await stuff



public void OnCompleted (Action continuation)
{
	actionsToBeContinued.Enqueue (continuation);
}


public AsyncCoordinator GetAwaiter ()
{
	return this;
}


public bool IsCompleted { get { return false; }}


public void GetResult ()
{
	// we have no result.
}

#endregion


}

}
