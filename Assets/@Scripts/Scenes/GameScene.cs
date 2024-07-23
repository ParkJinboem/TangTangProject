using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
 
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

    SpawningPool _spawningPool;
    Define.StageType _stageType;
    public Define.StageType StageType
    {
        get { return _stageType; }
        set
        {
            _stageType = value;
            if(_spawningPool != null)
            {
                switch(value)
                {
                    case Define.StageType.Normal:
                        _spawningPool.Stopped = false;
                        break;
                    case Define.StageType.Boss:
                        _spawningPool.Stopped = true;
                        break;
                }
            }
        }
    }

    void StartLoaded()
    {
        Managers.Data.Init();

        Managers.UI.ShowSceneUI<UI_GameScene>();
        //SpawningPool Component를 추가하여 Start문에서 오브젝트를 생성하도록 실행
        _spawningPool = gameObject.AddComponent<SpawningPool>();
        PlayerController player = Managers.Object.Spawn<PlayerController>(Vector3.zero);

        GameObject joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Joystick";

        GameObject map = Managers.Resource.Instantiate("Map_01.prefab");
        map.name = "@Map";
        Camera.main.GetComponent<CameraController>().target = player.gameObject;

        foreach(var playerData in Managers.Data.PlayerDic.Values)
        {
            Debug.Log($"Lv1 : {playerData.level}, HP : {playerData.maxHp}");
        }

        Managers.Game.OnGemCountChanged -= HandleOnGemCountChanged;
        Managers.Game.OnGemCountChanged += HandleOnGemCountChanged;

        Managers.Game.OnKillCountChanged -= HandleOnKillCountChanged;
        Managers.Game.OnKillCountChanged += HandleOnKillCountChanged;
    }

    int _collectedGemCount = 0;
    int _remainingTotalGemCount = 10;
    public void HandleOnGemCountChanged(int gemCount)
    {
        _collectedGemCount++;

        if (_collectedGemCount == _remainingTotalGemCount)
        {
            Managers.UI.ShowPopup<UI_SkillSelectPopup>();
            _collectedGemCount = 0;
            _remainingTotalGemCount *= 2;
        }

        Managers.UI.GetSceneUI<UI_GameScene>().SetGemCountRatio((float)_collectedGemCount / _remainingTotalGemCount);
    }

    public void HandleOnKillCountChanged(int killCount)
    {
        Managers.UI.GetSceneUI<UI_GameScene>().SetKillCount(killCount);

        if(killCount == 5)
        {
            StageType = Define.StageType.Boss;

            Managers.Object.DespawnAllMonsters();

            Vector2 spawnPos = Utils.GenerateMonsterSpawnPosition(Managers.Game.Player.transform.position, 5, 10);

            Managers.Object.Spawn<MonsterController>(spawnPos, Define.BOSS_ID);
        }
    }

    private void OnDestroy()
    {
        if(Managers.Game != null)
        {
            Managers.Game.OnGemCountChanged -= HandleOnGemCountChanged;
        }       
    }
}

