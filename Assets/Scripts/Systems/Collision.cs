using System.Collections.Generic;
using UnityEngine;

public struct CollisionPoint {
    public Vector3 Position;
    public Vector3 Normal;
}

public interface ICollider 
{
    void TestSphere(Vector3 pt, float radius, List<CollisionPoint> outHits);
}

public class CompositeCollider: ICollider 
{

    public void Add(ICollider collider) {
        m_staticColliders.Add(collider);
    }

    public void TestSphere(Vector3 pt, float radius, List<CollisionPoint> outHits) {
        for (int i = 0; i < m_staticColliders.Count; ++i) {
            m_staticColliders[i].TestSphere(pt, radius, outHits);
        }
    }

    List<ICollider> m_staticColliders = new List<ICollider>();
}

public class SphereCollider : ICollider 
{
    private Vector3 m_center;
    private float m_radius;
    private float collisionDist;
    private float distSqr;
    private Vector3 localPt;

    public SphereCollider(Vector3 center, float radius) 
    {
        m_center = center;
        m_radius = radius;
    }

    public void TestSphere(Vector3 pt, float radius, List<CollisionPoint> outHits) 
    {
        collisionDist = radius + m_radius;
        localPt = pt - m_center;
        distSqr = localPt.sqrMagnitude;

        if (distSqr < collisionDist * collisionDist) 
        {
            var normal = localPt.normalized;
            outHits.Add(new CollisionPoint {
                Position = m_center + (normal * m_radius),
                Normal = normal,
            });
        }
    }
}

public class AABBInsideCollider : ICollider 
{
    private Vector3 m_center;
    private Vector3 m_halfSize;
    private Vector3 m_invHalfSize;

    public AABBInsideCollider(Vector3 center, Vector3 size) 
    {
        m_center = center;
        m_halfSize = size * .5f;
        m_invHalfSize.Set(2.0f / size.x, 2.0f / size.y, 2.0f / size.z);
    }

    public void TestSphere(Vector3 pt, float radius, List<CollisionPoint> outHits) 
    {
        var localPt = pt - m_center;

        var localPtSign = new Vector3(
            localPt.x < 0 ? -1.0f : 1.0f,
            localPt.y < 0 ? -1.0f : 1.0f,
            localPt.z < 0 ? -1.0f : 1.0f);

        var dist = new Vector3(
            m_halfSize.x - (localPt.x * localPtSign.x),
            m_halfSize.y - (localPt.y * localPtSign.y),
            m_halfSize.z - (localPt.z * localPtSign.z));

        if (dist.x < radius) 
        {
            outHits.Add(new CollisionPoint {
                Position = new Vector3(m_center.x + m_halfSize.x * localPtSign.x, pt.y, pt.z),
                Normal = new Vector3(-localPtSign.x, 0, 0),
            });
        } 
        if (dist.y < radius) 
        {
            outHits.Add(new CollisionPoint {
                Position = new Vector3(pt.x, m_center.y + m_halfSize.y * localPtSign.y, pt.z),
                Normal = new Vector3(0, -localPtSign.y, 0),
            });
        }
        if (dist.z < radius) 
        {
            outHits.Add(new CollisionPoint {
                Position = new Vector3(pt.x, pt.y, m_center.z + m_halfSize.z * localPtSign.z),
                Normal = new Vector3(0, 0, -localPtSign.z),
            });
        }
    }
}