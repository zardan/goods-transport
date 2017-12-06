﻿using UnityEngine;
using PM;

public class Scene3Controller : MonoBehaviour, ISceneController, IPMCompilerStopped
{
	[HideInInspector]
	public int carsUnloaded = 0;

	private bool levelShouldBeAnswered = false;

	LevelController levelController;
	Case caseData;

	private void Start()
	{
		levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();
		caseData = levelController.caseData;
	}

	public void OnPMCompilerStopped(HelloCompiler.StopStatus status)
	{
		if (status == HelloCompiler.StopStatus.Finished)
		{
			bool levelShouldBeAnswered = false;

			// Should be moved to PMWrapper
			foreach(Compiler.Function fun in UISingleton.instance.compiler.addedFunctions)
			{
				if (fun.GetType() == new AnswerFunction().GetType())
					levelShouldBeAnswered = true;
			}

			int carsToUnload = caseData.cars.Count;

			if (carsToUnload == carsUnloaded && !levelShouldBeAnswered)
				PMWrapper.SetCaseCompleted();
		}
	}

	public void SetPrecode(Case caseData)
	{
		string precode = "antal_bilar = " + caseData.cars.Count;
		PMWrapper.preCode = precode;
	}
}
