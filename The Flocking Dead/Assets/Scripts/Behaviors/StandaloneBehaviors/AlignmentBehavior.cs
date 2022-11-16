using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")]
public class AlignmentBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent a, List<FlockAgent> context)
    {
        // if there is nothing in the context, return directly
        if (context.Count == 0)
            return Vector2.zero;

        // find vector in direction of average direction of the context
        // aka the alignment direction
        // but only if we're of the same type (both zombie of both not)
        Vector2 direction = Vector2.zero;
        for (int i = 0; i < context.Count; i++)
        {
            // skip over if not the same type
            if (a.isZombie != context[i].isZombie)
                continue;

            // TODO: determine direction of other agent
            direction += context[i].wantedDirection;
        }

        // average out the direction
        direction.Normalize();

        // return resulting direction 
        return direction;
    }
}
