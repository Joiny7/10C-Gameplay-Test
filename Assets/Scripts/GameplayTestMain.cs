//#define ENABLE_DEBUG_DRAW
using UnityEngine;

public class GameplayTestMain : MonoBehaviour
{
	private static SceneSetup[] ms_sceneSetups = new SceneSetup[] {
		new SceneSetup { Size = new Vector3(30, 30, 30), BoidCount = 50},
		new SceneSetup { Size = new Vector3(40, 40, 40), BoidCount = 150},
		new SceneSetup { Size = new Vector3(50, 50, 50), BoidCount = 400},
		new SceneSetup { Size = new Vector3(70, 70, 70), BoidCount = 1000},
	};

	private void Start()
	{
		AgentManager.ResetScene(ms_sceneSetups[0]);
		this.enabled = true;
	}

	private void Update()
	{
		CheckInput();
	}

	private void CheckInput()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			AgentManager.ResetScene(ms_sceneSetups[0]);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
			AgentManager.ResetScene(ms_sceneSetups[1]);

		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
			AgentManager.ResetScene(ms_sceneSetups[2]);

		}
		else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
			AgentManager.ResetScene(ms_sceneSetups[3]);

		}
	}
}