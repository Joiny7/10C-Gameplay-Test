using UnityEngine;
using System.Collections.Generic;

public struct SceneSetup {
	public Vector3 Size;
	public int BoidCount;
}

public class AgentManager : MonoBehaviour 
{
	public static AgentManager Instance;
	private CompositeCollider m_worldCollider;
	private List<CollisionPoint> m_tempHits = new List<CollisionPoint>();

	private void Awake() {
		Instance = this;
	}

	public static void ResetScene(SceneSetup sceneSetup) 
	{
		ObjectPool.DeactivateBoids();
		// Static collision
		Instance.m_worldCollider = new CompositeCollider();
		Instance.m_worldCollider.Add(new AABBInsideCollider(new Vector3(0, 0, 0), sceneSetup.Size));
		ObjectPool.ActivateBoids(sceneSetup.BoidCount);
	}

	public static void AvoidStaticCollisions(IBoidData boid, float closestDist) {
		Instance.m_tempHits.Clear();
		Instance.m_worldCollider.TestSphere(boid.Position, closestDist, Instance.m_tempHits);

		if(Instance.m_tempHits.Count > 0)
			Boids.AvoidCollisions(boid, 1f, Instance.m_tempHits);
	}
}