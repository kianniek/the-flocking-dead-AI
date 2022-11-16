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
        Vector2 direction = Vector2.zero;
        for (int i = 0; i < context.Count; i++)
        {
            // continue if other isn't zombie
            if (!context[i].isZombie)
                continue;

            // TODO: determine direction away from the zombie
            // direction += ???;
        }

        // average out the direction
        direction.Normalize();

        // return resulting direction 
        return direction;
    }
}
