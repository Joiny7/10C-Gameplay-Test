using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static List<AgentCore> AllAgentCores = new List<AgentCore>();
	public GameObject[] m_agentPrefabs;
	public static ObjectPool Instance;
	private static int LastAmount;

	private void Awake()
	{
		Instance = this;
		SpawnAllBoids();
		System.GC.Collect();
	}

	public static AgentCore[] GetActiveCores()
    {
        AgentCore[] cores = new AgentCore[LastAmount];

        for (int i = 0; i < LastAmount; i++)
        {
			cores[i] = AllAgentCores[i];
        }

		return cores;
    }

	public static void SpawnAgent(GameObject prefab, Vector3 position)
	{
		AgentCore core = Instantiate(prefab, position, Quaternion.identity).GetComponent<AgentCore>();
		core.Setup();
		AllAgentCores.Add(core);
		core.gameObject.SetActive(false);
	}

	private void SpawnAllBoids()
    {
        for (int i = 0; i < 1000; i++)
        {
			SpawnAgent(m_agentPrefabs[Random.Range(0, m_agentPrefabs.Length)], Vector3.zero);
		}
	}

	public static void ActivateBoids(int amount)
    {
		LastAmount = amount;

		if (AllAgentCores != null)
		{
			for (int i = 0; i < amount; i++)
			{
				AllAgentCores[i].gameObject.SetActive(true);
			}
		}
	}

	public static void DeactivateBoids()
    {
		System.GC.Collect();
		if (AllAgentCores != null)
		{
			for (int i = 0; i < AllAgentCores.Count; i++)
			{
				var b = AllAgentCores[i];
				b.transform.position = Vector3.zero;
				b.gameObject.SetActive(false);
			}
		}
	}
}