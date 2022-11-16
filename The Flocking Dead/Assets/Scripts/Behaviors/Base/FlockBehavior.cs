using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehavior : ScriptableObject
{
    /// <summary>
    /// Calculates the desired move by this flock behavior
    /// </summary>
    /// <param name="a">The current agent</param>
    /// <param name="context">The relevant transforms close to the agent</param>
    /// <returns></returns>
    public abstract Vector2 CalculateMove(FlockAgent a, List<FlockAgent> context);
}
