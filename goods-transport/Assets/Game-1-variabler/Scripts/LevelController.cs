﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	[Header("Prefabs")]
	public GameObject carPrefab;
	public GameObject boxRowPrefab;
	public GameObject palmPrefab;
	public GameObject treePrefab;
	public GameObject lampPrefab;
	public GameObject tablePrefab;
	public GameObject chairPrefab;

	[Header("Distances")]
	public float carSpacing = 1;
	public float boxSpacing = 0.5f;
	public float carPadding = 0.4f;
	private float boxLength = 1; // is set from CreateAssets()

	[HideInInspector]
	public Case caseData;

	[HideInInspector]
	public Queue<GameObject> activeCars = new Queue<GameObject>();

	private Dictionary<string, GameObject> itemType;
	private void BuildItemDictionary()
	{
		if (itemType == null)
		{
			itemType = new Dictionary<string, GameObject>();

			itemType.Add("palmer", palmPrefab);
			itemType.Add("granar", treePrefab);
			itemType.Add("bord", tablePrefab);
			itemType.Add("lampor", lampPrefab);
			itemType.Add("stolar", tablePrefab);
		}
	}

	public void LoadLevel(Case caseData)
	{
		this.caseData = caseData;

		BuildItemDictionary();
		RemoveOldAssets();
		CreateAssets();
		SetPrecodeAndAnswer();
	}

	private void RemoveOldAssets()
	{
		foreach (GameObject obj in activeCars)
		{
			Destroy(obj);
		}
		activeCars.Clear();
	}

	private void CreateAssets()
	{
		float boxLength = boxRowPrefab.GetComponentInChildren<Renderer>().bounds.size.x;
		float previousCarPosition = 0;

		foreach (Car car in caseData.cars)
		{
			GameObject carObj = Instantiate(carPrefab);
			activeCars.Enqueue(carObj);

			RescaleCar(car, carObj);

			// Position car in queue
			float carPosX = previousCarPosition > 0 ? previousCarPosition + carSpacing : 0;
			carObj.transform.position = new Vector3(carPosX, 0, 0);

			// Place the rows of boxes and their items in car
			Bounds carBounds = carObj.GetComponent<Renderer>().bounds;

			float carLength = carBounds.size.x;
			float carWidthCenter = carBounds.center.z;
			float carLeftEnd = carBounds.min.x;
			float sectionLeftEnd = carLeftEnd;

			for (int i = 0; i < car.sections.Count; i++)
			{
				Section section = car.sections[i];
				Vector3[,] itemPositions = new Vector3[section.rows, 4];

				for (int j = 1; j <= section.rows; j++)
				{
					GameObject boxRow = Instantiate(boxRowPrefab);
					boxRow.transform.SetParent(carObj.transform);

					float rowTopEnd = boxRow.GetComponentInChildren<Renderer>().bounds.max.y;
					float rowCenter = sectionLeftEnd + carPadding + ((2 * (float)j - 1) / 2) * boxLength + (j - 1) * boxSpacing;
					
					for (int k = 1; k <= 4; k++)
					{
						float colCenter = carBounds.min.z + carPadding + ((2 * (float)k - 1) / 2) * boxLength + (k - 1) * boxSpacing;
						itemPositions[j-1, k-1] = new Vector3(rowCenter, rowTopEnd, colCenter);
					}

					boxRow.transform.position = new Vector3(rowCenter, 0.5f, carWidthCenter);
				}
				PlaceItems(itemPositions, carObj, itemType[section.type], section.itemCount);

				float sectionLength = section.rows * boxLength + (section.rows - 1) * boxSpacing  + 2 * carPadding;
				sectionLeftEnd += sectionLength;
			}
			previousCarPosition = carObj.transform.position.x + carLength + carSpacing;
		}
	}

	private void SetPrecodeAndAnswer()
	{
		ISceneController[] sceneControllers = PM.UISingleton.FindInterfaces<ISceneController>();
		if (sceneControllers.Length > 1)
			throw new Exception("There are more than one class that implements ISceneController in current scene.");
		if (sceneControllers.Length < 1)
			throw new Exception("Could not find any class that implements ISceneController.");

		sceneControllers[0].SetLevelAnswer(caseData);
		sceneControllers[0].SetPrecode(caseData);
	}

	private void RescaleCar(Car carData, GameObject carObj)
	{
		Vector3 carSize = carObj.GetComponent<Renderer>().bounds.size;

		int sectionCount = 0;
		int rowsInCar = 0;
		int boxSpacingsNeeded = 0;

		foreach (Section section in carData.sections)
		{
			rowsInCar += section.rows;
			if (section.itemCount > 0)
			{
				sectionCount++;
				boxSpacingsNeeded += section.rows - 1;
			}
		}
		float newCarWidth = 4 * boxLength + 3 * boxSpacing + 2 * carPadding;
		float newCarLength = rowsInCar * boxLength + boxSpacingsNeeded * boxSpacing + 2 * sectionCount * carPadding;
		
		float scaleFactorLength = newCarLength / carSize.x;
		float scaleFactorWidth = newCarWidth / carSize.z;

		carObj.transform.localScale = new Vector3(scaleFactorLength, 0.4f, scaleFactorWidth);
	}
	private void PlaceItems(Vector3[,] positionMatrix, GameObject parent, GameObject itemPrefab, int itemCount)
	{
		if (itemCount > positionMatrix.Length)
			throw new Exception("There are too few rows in comparison to the itemCount in car.");

		int a = 0;
		int b = 0;
		for (int i = 0; i < itemCount; i++)
		{
			GameObject obj = Instantiate(itemPrefab);
			obj.transform.position = positionMatrix[a, b];
			obj.transform.SetParent(parent.transform);

			a += 1;

			if (a >= positionMatrix.GetLength(0))
			{
				a = 0;
				b += 1;
			}
		}
	}
}
