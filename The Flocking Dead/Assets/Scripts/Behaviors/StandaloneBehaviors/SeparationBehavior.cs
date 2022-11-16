using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: sepAration

[CreateAssetMenu(menuName = "Flock/Behavior/Separation")]
public class SeparationBehavior : FlockBehavior
{
    public override Vector2 CalculateMove(FlockAgent a, List<FlockAgent> context)
    {
        // if there is nothing in the context, return directly
        if (context.Count == 0)
            return Vector2.zero;

        // find vector in average direction away from the context
        // aka the separation direction
        // but only if the other is in the 'space' of this agent
        // and only if we're of the same type (both zombie of both not)
        float distance = 0;
        Vector2 direction = Vector2.zero;
        for (int i = 0; i < context.Count; i++)
        {
            // skip over if not the same type
            if (a.isZombie != context[i].isZombie)
                continue;

            // calculate distance between me and the other agent
            distance = (a.transform.position - context[i].transform.position).magnitude;

            // separate if agent is in space
            if (distance <= GameManager.instance.space)
            {
                // TODO: determine direction away from other agent
                direction += (Vector2)(a.transform.position - context[i].transform.position);
            }
        }

        // average out the direction
        direction.Normalize();

        // return resulting direction 
        return direction;
    }
}
