using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace AwaitFail2
{

static class AwaiterCoordinatorTest2
{
static AsyncCoordinator coordinator;

//=============================================================================
/// <summary></summary>
public static void run ()
{
	coordinator=new AsyncCoordinator();

	coordinator.scheduleCoroutine (w_doJob1);

	while (RunStep())
	{
		//Console.WriteLine ("Step ran");
	}

	Console.WriteLine ("No more steps to run");
}

//=============================================================================
/// <summary></summary>
static bool RunStep ()
{
	return coordinator.RunStep();
}

//=============================================================================
/// <summary></summary>
static async Task extraAwaitableFunc (AsyncCoordinator coord)
{
int i;

	for (i=0;i<2;i++)
	{
		Console.WriteLine ("**** subwait "+i);
		await coord;
	}
}


static async Task w_doJob1 (AsyncCoordinator coord)
{
	Console.WriteLine ("**   step 1");
	await coord;

	Console.WriteLine ("**   step 2");
	await extraAwaitableFunc(coord);

	Console.WriteLine ("**   step 3");
	await coord;
}


}



}
