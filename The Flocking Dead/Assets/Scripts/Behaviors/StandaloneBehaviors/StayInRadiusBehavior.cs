using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/StayInRadius")]
public class StayInRadiusBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent a, List<FlockAgent> context)
    {
        // we're not interested in the context in this behavior, 
        // only in how far the agent is from the center 
        // find out how far that is
        Vector2 distanceFromCenter = GameManager.instance.centerMap - (Vector2)a.transform.position;

        // calculate how far from the center that is percentage-wise
        // t = 0, at center. t = 1, at radius. t > 1, beyond radius
        float t = distanceFromCenter.magnitude / GameManager.instance.radiusMap;

        // if we're still nicely within the radius, 
        // return no direction
        if (t < 0.8)
            return Vector2.zero;

        // if we do get in the last 20% of the radius
        // return a direction back to the center
        return distanceFromCenter.normalized;
    }
}
