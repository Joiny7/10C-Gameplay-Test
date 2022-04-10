//#define ENABLE_DEBUG_DRAW

using UnityEngine;
using System.Collections.Generic;

public class FlockingMovement : MonoBehaviour, IAgentMovement 
{
	public float m_acceleration = 4.0f;
	public float m_drag = 0.02f;

	private const float INITIAL_SPEED = 10.0f;
	private const float MATCH_VELOCITY_RATE = 1.0f;
	private const float AVOIDANCE_RATE = 2.0f;
	private const float COHERENCE_RATE = 2.0f;
	//private const float ACCELERATION = 4.0f;
	private const float VIEW_RANGE = 5.0f;
	//private const float DRAG = 0.02f;

	private AgentCore m_core;
	private float dif;
	private AgentCore[] cores;
	private float dt;

	public void Setup(AgentCore core) 
	{
		m_core = core;
		m_core.Velocity = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * INITIAL_SPEED;
	}

	private void Update() 
	{
		FindAgentsWithinRadius(VIEW_RANGE, out List<IBoidData> allWithinRadius, out List<IBoidData> sameTeamWithinRadius);
		dt = Time.deltaTime;

		AgentManager.AvoidStaticCollisions(m_core, VIEW_RANGE);

		Boids.MatchVelocity(m_core, sameTeamWithinRadius, MATCH_VELOCITY_RATE, dt);
		Boids.UpdateCoherence(m_core, sameTeamWithinRadius, COHERENCE_RATE, dt);
		Boids.AvoidOthers(m_core, 4.0f, allWithinRadius, AVOIDANCE_RATE, dt);

		// Accelerate
		m_core.Velocity += m_core.Velocity.normalized * m_acceleration * dt;

		// Apply Drag
		m_core.Velocity *= Mathf.Pow(1.0f - m_drag, 30.0f * dt);

		// Look in direction of velocity
		m_core.transform.LookAt(m_core.transform.position + m_core.Velocity);

		// Move
		m_core.Position = m_core.Position + (m_core.Velocity * dt);
	}

	private void FindAgentsWithinRadius(float radius, out List<IBoidData> allWithinRadius, out List<IBoidData> sameTeamWithinRadius) 
	{
		allWithinRadius = new List<IBoidData>();
		sameTeamWithinRadius = new List<IBoidData>();
		cores = ObjectPool.GetActiveCores();

		for (int i = 0; i < cores.Length; i++) 
		{
			if (cores[i] != m_core) 
			{
				dif = (cores[i].Position - this.transform.position).sqrMagnitude;

				if (dif < radius) 
				{
					allWithinRadius.Add(cores[i] as IBoidData);

					if (m_core.m_team == cores[i].m_team) 
					{
						sameTeamWithinRadius.Add(cores[i] as IBoidData);
					}
#if ENABLE_DEBUG_DRAW
					Debug.DrawLine(transform.position, cores[i].Position, Color.green);
#endif
				} else {
#if ENABLE_DEBUG_DRAW
					Debug.DrawLine(transform.position, cores[i].Position, Color.red);
#endif
				}
			}
		}
	}
}