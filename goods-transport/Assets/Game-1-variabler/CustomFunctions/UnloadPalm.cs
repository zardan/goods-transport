﻿using UnityEngine;
using PM;

public class UnloadPalm : Compiler.Function
{	
	public UnloadPalm()
	{
		this.name = "lasta_av_palm";
		this.inputParameterAmount.Add(0);
		this.hasReturnVariable = false;
		this.pauseWalker = true;
	}

	public override Compiler.Variable runFunction(Compiler.Scope currentScope, Compiler.Variable[] inputParas, int lineNumber)
	{
		GameObject.FindGameObjectWithTag("Palm").GetComponent<UnloadableItem>().isUnloading = true;
		GameObject.FindGameObjectWithTag("SceneController").GetComponent<Scene2Controller>().itemsUnloaded += 1;
		return new Compiler.Variable();
	}
}
