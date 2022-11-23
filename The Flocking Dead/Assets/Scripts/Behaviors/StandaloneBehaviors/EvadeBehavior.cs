using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Evade")]
public class EvadeBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent a, List<FlockAgent> context)
    {

        // if there is nothing in the context, return directly
        if (context.Count == 0)
            return Vector2.zero;

        // find vector in average direction away from the context
        // aka the evade direction
        // but only if the other is a zombie
        float closestDistance = float.MaxValue;
        FlockAgent closestAgent = null;
        float distance = 0;
        Vector2 direction = Vector2.zero;

        for (int i = 0; i < context.Count; i++)
        {
            // continue if other isn't zombie
            if (!context[i].isZombie)
                continue;

            // get the distance between the potential prey and myself
            distance = (a.transform.position - context[i].transform.position).magnitude;

            // if he's closer than the prev closest ...
            if (distance < closestDistance)
            {
                // ... update the closest distance and agent 
                closestDistance = distance;
                closestAgent = context[i];
            }
            //determine direction away from the zombie
            direction += (Vector2)(a.transform.position - context[i].transform.position);
        }

        // average out the direction
        direction.Normalize();
 
        // return resulting direction 
        return direction;
    }
}
