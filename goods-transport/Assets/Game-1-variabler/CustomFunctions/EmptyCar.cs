﻿using System.Collections.Generic;
using UnityEngine;

public class EmptyCar : Compiler.Function
{
	public EmptyCar()
	{
		this.name = "töm_bilen";
		this.inputParameterAmount.Add(0);
		this.hasReturnVariable = false;
		this.pauseWalker = true;
	}

	public override Compiler.Variable runFunction(Compiler.Scope currentScope, Compiler.Variable[] inputParas, int lineNumber)
	{
		LinkedList<GameObject> carsToUnload = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>().activeCars;

		GameObject currentCar = carsToUnload.First.Value;

		foreach (UnloadableItem item in currentCar.GetComponentsInChildren<UnloadableItem>())
		{
			if (item != null)
				item.isUnloading = true;
		}
		GameObject.FindGameObjectWithTag("SceneController").GetComponent<Scene3Controller>().carsUnloaded += 1;

		return new Compiler.Variable();
	}
}
