using UnityEngine;

public interface IAgentMovement 
{
	void Setup(AgentCore core);
}

public enum eAgentTeam 
{
	Red,
	Blue,
	Green,
}

public class AgentCore : MonoBehaviour, IBoidData 
{
	public eAgentTeam m_team;
	public IAgentMovement m_movement;

	public Vector3 Position 
	{
		set {
			transform.position = value;
		}
		get {
			return transform.position;
		}
	}
	public Vector3 Velocity { get; set; }

	public void Setup() 
	{
		m_movement = GetComponent<IAgentMovement>();
		if (m_movement != null) {
			m_movement.Setup(this);
		} else {
			Debug.LogError("AgentCore is missing a movement component!", this.gameObject);
		}
	}
}