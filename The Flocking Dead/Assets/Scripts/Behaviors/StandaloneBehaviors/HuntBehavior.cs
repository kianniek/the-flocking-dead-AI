using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Hunt")]
public class HuntBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent a, List<FlockAgent> context)
    {
        // if there is nothing in the context, return directly
        if (context.Count == 0)
            return Vector2.zero;

        // find vector in direction of closed not zombie agent in the context
        // aka the hunt direction

        // keep track of the closest distance and agent
        float closestDistance = float.MaxValue;
        FlockAgent closestAgent = null;
        float distance = 0;
        Vector2 direction = Vector2.zero;

        foreach (FlockAgent other in context)
        {
            // continue if other is zombie
            if (other.isZombie)
                continue;

            // get the distance between the potential prey and myself
            distance = (a.transform.position - other.transform.position).magnitude;

            // if he's closer than the prev closest ...
            if (distance < closestDistance)
            {
                // ... update the closest distance and agent 
                closestDistance = distance;
                closestAgent = other;
            }
        }    

        // if we found a closest agent
        if (closestAgent != null)
        {
            // TODO: determine direction towards prey
            // direction += ???;
        }

        // average out the direction
        direction.Normalize();

        // return resulting direction 
        return direction;
    }
}
