using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 moveDir = Vector2.zero;
    float speed = 5.0f;

    public Vector2 MoveDir
    {
        get { return moveDir; }
        set { moveDir = value.normalized; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        
        
    }
}
