﻿using Compiler;
using UnityEngine;

public class DriveRight : Function
{
	public DriveRight()
	{
		name = "kör_höger";
		inputParameterAmount.Add(0);
		hasReturnVariable = false;
		pauseWalker = true;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
		SortedQueue.RightQueue.Add(CarQueue.GetFirstCar());

		CarQueue.DriveQueueForward();
		CarQueue.DriveFirstCarRight();

		return new Variable();
	}
}
