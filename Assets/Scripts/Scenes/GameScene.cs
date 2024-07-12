using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GameObject slimePrefab;
    public GameObject joystickPrefab;

    GameObject slime;
    GameObject joystick;
    // Start is called before the first frame update
    void Start()
    {
        slime = GameObject.Instantiate(slimePrefab);
        joystick = GameObject.Instantiate(joystickPrefab);

        GameObject go = new GameObject() { name = "Monster" };
        slime.transform.parent = go.transform;

        slime.AddComponent<PlayerController>();
        Camera.main.GetComponent<CameraController>().target = slime;
    }

    // Update is called once per frame
    void Update()
    {
        //test
    }
}
