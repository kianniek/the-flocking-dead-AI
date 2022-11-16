using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Flock/Behavior/Composite")]
public class CompositeBehavior : FlockBehavior
{
    // all behaviors in this composite behavior
    [SerializeField] private CompositePart[] behaviors;

    // struct to define an object in this composite
    // consisting of a behavior and a weight
    [Serializable]
    public struct CompositePart
    {
        public FlockBehavior behavior;
        public float weight;
    }

    public override Vector2 CalculateMove(FlockAgent a, List<FlockAgent> context)
    {
        // if there is nothing in the context, return directly
        if (context.Count == 0)
            return Vector2.zero;

        // set up direction
        Vector2 direction = Vector2.zero;

        // execute each behavior
        for (int i = 0; i < behaviors.Length; i++)
        {
            direction += behaviors[i].behavior.CalculateMove(a, context) * behaviors[i].weight;
        }

        direction.Normalize();

        // return resulting direction 
        return direction;
    }
}
