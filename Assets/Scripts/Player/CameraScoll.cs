using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    [SerializeField] float minXClamp;
    [SerializeField] float maxXClamp;
    [SerializeField] float minYClamp;
    [SerializeField] float maxYClamp;
    [SerializeField] float minYThreshold = 1; // Y threshold to start auto-scrolling
    [SerializeField] float autoScrollSpeed = 0.0f; // Speed of auto-scrolling

    Vector3 velocity = Vector3.zero;
    //private float initialYPosition = -0.92f; // Initial Y position of the player
    private bool isAutoScrolling = false; // Flag to indicate auto-scrolling state

    void LateUpdate()
    {
        // Ensure player reference is set and GameManager is available
        if (!GameManager.Instance || !GameManager.Instance.PlayerInstance)
            return;

        Vector3 cameraPos = transform.position;

        // Clamp camera position within specified bounds
        //cameraPos.x = Mathf.Clamp(GameManager.Instance.PlayerInstance.transform.position.x, minXClamp, maxXClamp);
        //cameraPos.y = Mathf.Clamp(GameManager.Instance.PlayerInstance.transform.position.y, minYClamp, maxYClamp);

        // Calculate the difference between current player's Y position and initial Y position
        float yDifference = GameManager.Instance.PlayerInstance.transform.position.y - transform.position.y;

        // Check if the difference exceeds the threshold
        if (yDifference > minYThreshold && cameraPos.y < maxYClamp)
        {
            // Trigger auto-scrolling
            isAutoScrolling = true;
        }

        // If auto-scrolling is enabled, move the camera upward
        if (isAutoScrolling)
        {
            cameraPos.y += autoScrollSpeed;
        }
        if (cameraPos.y >= maxYClamp) 
        {
            isAutoScrolling = false;
        }

        // Update camera position
        transform.position = Vector3.SmoothDamp(transform.position, cameraPos, ref velocity, 1.9f);
    }

    void Start()
    {

    }
}
