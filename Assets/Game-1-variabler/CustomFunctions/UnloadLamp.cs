﻿using Mellis;
using Mellis.Core.Interfaces;
using UnityEngine;

public class UnloadLamp : ClrYieldingFunction
{
	public UnloadLamp() : base("lasta_av_lampa")
	{
	}

	public override void InvokeEnter(params IScriptType[] arguments)
	{
		var lamp = GameObject.FindGameObjectWithTag("Lamp");

		if (lamp == null)
		{
			PMWrapper.RaiseError("Hittade ingen lampa att lasta av.");
		}

		lamp.GetComponent<UnloadableItem>().isUnloading = true;
		GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController1_2>().itemsUnloaded += 1;
	}
}
