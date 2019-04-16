using System.Collections.Generic;

namespace GameData
{
	public class SectionData
	{
		public string type { get; set; }
		public int itemCount { get; set; }
		public int rows { get; set; }
	}

	public class CarData
	{
		public List<SectionData> sections { get; set; }
		public int batteryLevel { get; set; }
		public string color { get; set; }
	}

	public class TypeDefinitionData
	{
		public string name { get; set; }
		public List<string> types { get; set; }
	}

	public class CorrectSortedQueueData
	{
		public int lowerBound { get; set; }
		public int upperBound { get; set; }

		public string type { get; set; }
	}

	public class SortingData
	{
		public List<TypeDefinitionData> typeDefinitions { get; set; }

		public CorrectSortedQueueData leftQueue { get; set; }
		public CorrectSortedQueueData rightQueue { get; set; }
		public CorrectSortedQueueData forwardQueue { get; set; }
	}

	public class GoodsCaseDefinition : CaseDefinition
	{
		public List<CarData> cars { get; set; }
		public SortingData correctSorting { get; set; }
		public int answer { get; set; }
		public int chargeBound { get; set; }
	}
}
