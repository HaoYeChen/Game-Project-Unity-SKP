using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    float timeOffset;

    [SerializeField]
    Vector2 posOffset;

    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //cameras current position
        Vector3 startPos = transform.position;

        //player current position
        Vector3 endPos = player.transform.position;
        endPos.x += posOffset.x;
        endPos.y += posOffset.y;
        endPos.z = -10;

        //this is how you lerp
        //transform.position = Vector3.Lerp(startPos, endPos, timeOffset * Time.deltaTime);

        transform.position = Vector3.SmoothDamp(startPos, endPos, ref velocity, timeOffset);
        
        
        
        //camera transform position will follow players transform position x & y & -10 on z to keep camera away from player
        //transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
    }
}
