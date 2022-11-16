using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    [Header("Movement settings")]
    [SerializeField] private float movementSpeed = 75f;
    [SerializeField] private float zombieSpeedModifier = 0.3f;

    [Header("Debug settings")]
    [SerializeField] private GameObject sightVisualization;
    [SerializeField] private GameObject spaceVisualization;

    // animator references and variables
    private Animator animator;
    private static string isZombieParam = "isZombie";
    private static string xMoveParam = "XMove";
    private static string yMoveParam = "YMove";

    // mark zombies to differentiate between them
    internal bool isZombie = false;

    // physics and movement references and variables
    internal Vector2 wantedDirection;

    // flocking variables
    private List<FlockAgent> context;

    // whether setup is done
    private bool initialized = false;

    /// <summary>
    /// Initializes the agent, ready it for updating
    /// </summary>
    /// <param name="isZombie">Whether or not this agent is a zombie</param>
    /// <param name="position">Position to spawn on</param>
    internal void Initialize(bool isZombie, Vector2 position)
    {
        // get components from this gameobject
        animator = GetComponent<Animator>();

        // initialize variables
        context = new List<FlockAgent>();

        // set settings for zombie / regular seagull
        if (isZombie)
            BecomeZombie();

        // set position
        transform.position = position;

        // done initializing
        initialized = true;
    }

    /// <summary>
    /// Called once per frame by GameManager
    /// </summary>
    internal void DoUpdate()
    {
        // wait until agent is initialized
        if (!initialized)
            return;

        // reset context and direction for this update
        context.Clear();
        wantedDirection = Vector2.zero;

        // re-find the context: nearby agents
        float distance = 0;
        foreach (FlockAgent a in GameManager.instance.agents)
        {
            // skip over self
            if (a == this)
                continue;

            // calculate distance between me and the other agent
            distance = (transform.position - a.transform.position).magnitude;

            // add if agent is in sight
            if (distance <= GameManager.instance.sight)
                context.Add(a);
        }

        // calculate move based on context
        if (isZombie)
            wantedDirection = GameManager.instance.zombieBehavior.CalculateMove(this, context);
        else
            wantedDirection = GameManager.instance.regularBehavior.CalculateMove(this, context);

        // set animator variables for movement animations
        // but smooth out the differences to avoid jittery behavior
        animator.SetFloat(xMoveParam, wantedDirection.x, 1f, Time.deltaTime);
        animator.SetFloat(yMoveParam, wantedDirection.y, 1f, Time.deltaTime);
    }

    private void LateUpdate()
    {
        // wait until agent is initialized
        if (!initialized)
            return;

        // move agent
        if (isZombie)
            transform.position += (Vector3)wantedDirection * movementSpeed * zombieSpeedModifier * Time.deltaTime;
        else
            transform.position += (Vector3)wantedDirection * movementSpeed * Time.deltaTime;
    }

    /// <summary>
    /// Become a zombie
    /// </summary>
    private void BecomeZombie()
    {
        // nothing to do if we're already a zombie
        if (isZombie)
            return;

        // now a zombie
        isZombie = true;
        animator.SetBool(isZombieParam, true);

        // reset velocity
        wantedDirection = Vector2.zero;
    }

    /// <summary>
    /// Resolve trigger collision
    /// </summary>
    /// <param name="other">The other collider</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // is this another agent? get a reference
        FlockAgent otherAgent = other.gameObject.GetComponent<FlockAgent>();

        // resolve collision if it's an agent
        if (otherAgent != null)
        {
            // if other is a zombie and i'm not (yet), become one
            if (otherAgent.isZombie && !isZombie)
                BecomeZombie();
        }
    }

    /// <summary>
    /// Turns on/off debugging visuals
    /// </summary>
    /// <param name="on">Visuals on or off</param>
    internal void ActivateDebug(bool on)
    {
        sightVisualization.SetActive(on);
        spaceVisualization.SetActive(on);

        if (on)
        {
            sightVisualization.transform.localScale = new Vector3(GameManager.instance.sight, GameManager.instance.sight, 1);
            spaceVisualization.transform.localScale = new Vector3(GameManager.instance.space, GameManager.instance.space, 1);
        }
    }
}
