using System.Collections.Generic;
using UnityEngine;

public interface IBoidData {
    Vector3 Position { set; get; }
    Vector3 Velocity { set; get; }
}

public static class Boids
{
    public static void AvoidCollisions(IBoidData boid, float viewDist, List<CollisionPoint> collisionPoints) 
    {
        var position = boid.Position;

        for (int i = 0; i < collisionPoints.Count; ++i) 
        {
            var hit = collisionPoints[i];
            var dist = Vector3.Dot(hit.Normal, position - hit.Position);

            if (dist < viewDist) 
            {
                boid.Velocity += hit.Normal * ((viewDist - dist) * 5.0f * Time.deltaTime);
            }
        }
    }

    public static void UpdateCoherence(IBoidData boid, List<IBoidData> neighbours, float coherenceRate, float dt) 
    {
        if (neighbours.Count > 0) 
        {
            Vector3 center = neighbours[0].Position;

            for (int i = 1; i < neighbours.Count; ++i) 
            {
                center += neighbours[i].Position;
            }

            center *= 1.0f / neighbours.Count;
            boid.Velocity += (center - boid.Position) * coherenceRate * dt;
        }
    }

    public static void AvoidOthers(IBoidData boid, float minDist, List<IBoidData> neighbours, float avoidanceRate, float dt) 
    {
        if (neighbours.Count > 0) 
        {
            var myPosition = boid.Position;
            var minDistSqr = minDist * minDist;
            Vector3 step = Vector3.zero;

            for (int i = 0; i < neighbours.Count; ++i) 
            {
                var delta = myPosition - neighbours[i].Position;
                var deltaSqr = delta.sqrMagnitude;

                if (deltaSqr > 0 && deltaSqr < minDistSqr) 
                {
                    step += delta / Mathf.Sqrt(deltaSqr);
                }
            }

            boid.Velocity += step * avoidanceRate * dt;
        }
    }

    public static void MatchVelocity(IBoidData boid, List<IBoidData> neighbours, float matchRate, float dt) 
    {
        if (neighbours.Count > 0) 
        {
            Vector3 velocity = Vector3.zero;

            for (int i = 0; i < neighbours.Count; ++i) 
            {
                velocity += neighbours[i].Velocity;
            }

            velocity /= neighbours.Count;
            boid.Velocity += (velocity - boid.Velocity) * matchRate * dt;
        }
    }
}