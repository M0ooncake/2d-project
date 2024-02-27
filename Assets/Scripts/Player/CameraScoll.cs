using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScoll : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Transform player;
    [SerializeField] float minXClamp;
    [SerializeField] float maxXClamp;
    [SerializeField] float minYClamp;
    [SerializeField] float maxYClamp;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        if (!GameManager.Instance || !GameManager.Instance.PlayerInstance) return;
        Vector3 cameraPos = transform.position;

        cameraPos.x  = Mathf.Clamp(GameManager.Instance.PlayerInstance.transform.position.x, minXClamp, maxXClamp);
        cameraPos.y = Mathf.Clamp(GameManager.Instance.PlayerInstance.transform.position.y, minYClamp, maxYClamp);

        //transform.position = Vector3.SmoothDamp(transform.position, cameraPos, player);
        transform.position = cameraPos;
    }
}
