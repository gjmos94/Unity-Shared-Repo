using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraPan : MonoBehaviour
{
    float EDGE = 0.03f; // % edge width to trigger pan
    float PAN_SPEED = 20.0f;
    Vector2 CAMERA_POS_MAX, CAMERA_POS_MIN;

    void Start()
    {
        // keep mouse inside playspace (esc to release)
        // Cursor.lockState = CursorLockMode.Confined;
        
        // camera limits
        Vector2 halfSize = (Camera.main.ViewportToWorldPoint(new Vector2(1,1)) - Camera.main.ViewportToWorldPoint(Vector2.zero))/2;
        Tilemap world = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        CAMERA_POS_MAX = (Vector2)world.localBounds.center + (Vector2)world.localBounds.extents - halfSize;
        CAMERA_POS_MIN = (Vector2)world.localBounds.center - (Vector2)world.localBounds.extents + halfSize;
    }

    void Update()
    {
        // pan when the cursor goes within any edge
        Vector2 viewportMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

        if (viewportMousePos.x < EDGE) transform.position += Vector3.left * (PAN_SPEED * Time.deltaTime);
        else if (viewportMousePos.x > (1.0f - EDGE)) transform.position += Vector3.right * (PAN_SPEED * Time.deltaTime);

        if (viewportMousePos.y < EDGE) transform.position += Vector3.down * (PAN_SPEED * Time.deltaTime);
        else if (viewportMousePos.y > (1.0f - EDGE)) transform.position += Vector3.up * (PAN_SPEED * Time.deltaTime);

        // keep camera in world
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, CAMERA_POS_MIN.x, CAMERA_POS_MAX.x),
            Mathf.Clamp(transform.position.y, CAMERA_POS_MIN.y, CAMERA_POS_MAX.y),
            transform.position.z);
    }
}
