using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    SpawningPool _spawningPool;
    void Start()
    {
        ////어드레서블의 라벨이 2개이일경우 사용
        //Managers.Resource.LoadAllAsync<GameObject>("Prefabs", (key, count, totalCount) =>
        //{
        //    Debug.Log($"{key} {count}/{totalCount}");

        //    if (count == totalCount)
        //    {
        //        Managers.Resource.LoadAllAsync<TextAsset>("Data", (key3, count3, totalCount3) =>
        //        {
        //            if (count3 == totalCount3)
        //            {
        //                StartLoaded2();
        //            }
        //        });
        //    }
        //});



        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                StartLoaded();
            }
        });
    }

    //어드레서블 사용안할시 호출
    //void StartLoaded()
    //{
    //    GameObject player = Managers.Resource.Instantiate("Slime_01.prefab");
    //    player.AddComponent<PlayerController>();

    //    GameObject snake = Managers.Resource.Instantiate("Snake_01.prefab");
    //    GameObject goblin = Managers.Resource.Instantiate("Goblin_01.prefab");
    //    GameObject joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
    //    joystick.name = "@UI_Joystick";

    //    GameObject map = Managers.Resource.Instantiate("Map.prefab");
    //    map.name = "@Map";
    //    Camera.main.GetComponent<CameraController>().target = player;
    //}

    void StartLoaded()
    {
        //SpawningPool Component를 추가하여 Start문에서 오브젝트를 생성하도록 실행
        _spawningPool = gameObject.AddComponent<SpawningPool>();
        PlayerController player = Managers.Object.Spawn<PlayerController>(Vector3.zero);

        for (int i = 0; i < 10; i++)
        {
            Vector3 randPos = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            MonsterController mc = Managers.Object.Spawn<MonsterController>(randPos, Random.Range(0, 2));
        }
        GameObject joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Joystick";

        GameObject map = Managers.Resource.Instantiate("Map.prefab");
        map.name = "@Map";
        Camera.main.GetComponent<CameraController>().target = player.gameObject;

        //Data Test
        Managers.Data.Init();

        foreach(var playerData in Managers.Data.PlayerDic.Values)
        {
            Debug.Log($"Lv1 : {playerData.level}, HP : {playerData.maxHp}");
        }
    }
}

