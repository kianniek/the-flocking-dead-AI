using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // movement variables
    [SerializeField] private float movementSpeed;

    // names of the used input axis
    private string horizontalInputAxis = "Horizontal";
    private string verticalInputAxis = "Vertical";

    // reference to unity camera component
    private new Camera camera;

    // keep track of size 
    private float orthographicSize;

    // bounds as defined by the level
    private Vector2 upperBound, lowerBound;

    // whether setup is done
    internal bool initialized = false;

    /// <summary>
    /// Initializes the camera, ready for updating
    /// </summary>
    internal void Initialize()
    {
        // get camera component
        camera = GetComponent<Camera>();

        // define bounds 
        orthographicSize = camera.orthographicSize;
        DetermineBounds();

        // done initializing
        initialized = true;
    }

    /// <summary>
    /// Called once per frame by GameManager
    /// </summary>
    internal void DoUpdate()
    {
        // initialize if first update
        if (!initialized)
            Initialize();

        // re-determine bounds if orthographic sized has been changed
        // which can be caused by the pixel perfect component
        if (orthographicSize != camera.orthographicSize)
            DetermineBounds();

        // change position based on input, speed and delta time
        transform.position += new Vector3(Input.GetAxis(horizontalInputAxis), Input.GetAxis(verticalInputAxis), 0)
            * movementSpeed
            * Time.deltaTime;

        // clamp position to keep within bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, lowerBound.x, upperBound.x),
                                         Mathf.Clamp(transform.position.y, lowerBound.y, upperBound.y),
                                         transform.position.z);
    }

    /// <summary>
    /// Determines the bounds of the level based on the tile map and camera size
    /// </summary>
    private void DetermineBounds()
    {
        // remember the new size
        orthographicSize = camera.orthographicSize;

        // get the extents of the camera
        float verticalExtent = camera.orthographicSize;
        float horizontalExtent = verticalExtent * Screen.width / Screen.height;

        // calculate lower bound based on tile map and extents of the camera
        Vector3Int tempBound = new Vector3Int(GameManager.instance.baseTilemap.cellBounds.xMin,
                                            GameManager.instance.baseTilemap.cellBounds.yMin,
                                            (int)GameManager.instance.baseTilemap.transform.position.y);
        lowerBound = GameManager.instance.baseTilemap.CellToWorld(tempBound) + new Vector3(horizontalExtent, verticalExtent);

        // idem for upperbound
        tempBound = new Vector3Int(GameManager.instance.baseTilemap.cellBounds.xMax,
                                 GameManager.instance.baseTilemap.cellBounds.yMax,
                                 (int)GameManager.instance.baseTilemap.transform.position.y);
        upperBound = GameManager.instance.baseTilemap.CellToWorld(tempBound) + new Vector3(-horizontalExtent, -verticalExtent);

    }
}
