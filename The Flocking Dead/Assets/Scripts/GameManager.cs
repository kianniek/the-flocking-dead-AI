using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [Header("Swarm settings")]
    [SerializeField] private int swarmCount = 10;
    [SerializeField] private float percentZombie = 0.7f;
    [SerializeField] private GameObject agentPrefab;

    [Header("Flocking behaviour settings")]
    [SerializeField] internal float sight = 7f;
    [SerializeField] internal float space = 2f;
    [SerializeField] internal FlockBehavior regularBehavior;
    [SerializeField] internal FlockBehavior zombieBehavior;

    [Header("Level settings")]
    [SerializeField] internal Tilemap baseTilemap;
    [SerializeField] private Tilemap collisionTilemap;
    [SerializeField] private CameraController cameraController;

    [Header("Debug settings")]
    [SerializeField] private bool visualizeSightAndSpace;
    private bool previousvisualizeSightAndSpace;

    // semi singleton
    internal static GameManager instance;

    // all agents in the swarm
    internal List<FlockAgent> agents;

    // valid places to spawn in the level
    private List<Vector3> spawnPoints;
    private List<Vector3> availableSpawnPoints;

    // most center point of map
    internal Vector2 centerMap;

    // radius from center to outer tiles of map
    internal float radiusMap;

    /// <summary>
    /// Start is called before the first frame update 
    /// </summary>
    private void Start()
    {
        // setup singleton
        instance = this;

        // find all valid spawn points in the tile map
        // by looking at each tile in the base tile map
        // and if that tile doesn't exist in the collision tile map
        // it's an open space to spawn, so add it to the spawn points
        spawnPoints = new List<Vector3>();
        for (int x = baseTilemap.cellBounds.xMin; x < baseTilemap.cellBounds.xMax; x++)
        {
            for (int y = baseTilemap.cellBounds.yMin; y < baseTilemap.cellBounds.yMax; y++)
            {
                Vector3Int localPosition = new Vector3Int(x, y, (int)baseTilemap.transform.position.y);

                if (!collisionTilemap.HasTile(localPosition))
                    spawnPoints.Add(baseTilemap.CellToWorld(localPosition) + baseTilemap.cellSize * 0.5f);
            }
        }

        // for starters, all spawn points are available
        availableSpawnPoints = spawnPoints;

        // determine center and radius of map
        centerMap = new Vector2(baseTilemap.cellBounds.center.x, baseTilemap.cellBounds.center.y);
        radiusMap = (centerMap - new Vector2(baseTilemap.cellBounds.xMin, baseTilemap.cellBounds.center.y)).magnitude;

        // initialize list
        agents = new List<FlockAgent>();

        // create as many agents as given count
        FlockAgent agent = null;
        for (int i = 0; i < swarmCount; i++)
        {
            // create new agent
            agent = GameObject.Instantiate(agentPrefab, this.transform).GetComponent<FlockAgent>();

            // give it a name!
            agent.gameObject.name = "Agent" + i;

            // initialize agent
            agent.Initialize(i < swarmCount * percentZombie, GetUniqueSpawnPosition());

            // and add it to our collection
            agents.Add(agent);
        }

        // set debug settings
        UpdateDebug();
    }

    /// <summary>
    /// Called once per frame
    /// </summary>
    private void Update()
    {
        // update debug settings when change in inspector is made
        if (previousvisualizeSightAndSpace != visualizeSightAndSpace) 
            UpdateDebug();

        // update each agent
        agents.ForEach(a => a.DoUpdate());

        // update camera
        cameraController.DoUpdate();
    }

    /// <summary>
    /// Called when debug settings have to be updated
    /// </summary>
    private void UpdateDebug()
    {
        agents.ForEach(a => a.ActivateDebug(visualizeSightAndSpace));
        previousvisualizeSightAndSpace = visualizeSightAndSpace;
    }

    /// <summary>
    /// Get a valid and unique spawn position on the tile map 
    /// </summary>
    /// <returns>Returns a valid and unique spawn position</returns>
    private Vector2 GetUniqueSpawnPosition()
    {
        // get a spawn position from the available list 
        Vector2 spawnPosition = availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count)];

        // remove it, since it's not available anymore
        availableSpawnPoints.Remove(spawnPosition);

        // return unique spawn point
        return spawnPosition;
    }

    /// <summary>
    /// Get a valid spawn position on the tile map 
    /// </summary>
    /// <returns>Returns a valid spawn position</returns>
    private Vector2 GetSpawnPosition()
    {
        // get a random spawn point and return it
        return spawnPoints[Random.Range(0, spawnPoints.Count)];
    }
}
