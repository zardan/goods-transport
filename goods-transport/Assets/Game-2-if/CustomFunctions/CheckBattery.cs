﻿using Compiler;
using UnityEngine;

public class CheckBattery : Compiler.Function
{
	public CheckBattery()
	{
		this.name = "kolla_batterinivå";
		this.inputParameterAmount.Add(0);
		this.hasReturnVariable = true;
		this.pauseWalker = true;
	}

	public override Variable runFunction(Scope currentScope, Variable[] inputParas, int lineNumber)
	{
		int batteryLevel = CarQueue.GetFirstCar().GetComponent<CarInfo>().BatteryLevel;

		ChargeStation.Instance.CheckBattery(batteryLevel);

		return new Variable("BatteryLevel", batteryLevel);
	}
}
