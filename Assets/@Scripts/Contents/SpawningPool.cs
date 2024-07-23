using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    float _spawnInterval = 2.0f;
    int _maxMonsterCount = 100;
    Coroutine _coUpdateSpawningPool;

    public bool Stopped { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        _coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
    }

    IEnumerator CoUpdateSpawningPool()
    {
        while(true)
        {
            TrySpawn();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    void TrySpawn()
    {
        if(Stopped)
        {
            return;
        }
        int monsterCount = Managers.Object.Monsters.Count;
        if(monsterCount >= _maxMonsterCount)
        {
            return;
        }
        //위치를 일정영역 안으로 받을시 사용
        //Vector3 randPos = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));

        //몬스터 위치를 캐릭터 주변의 원형태로 받을시 사용
        Vector3 randPos = Utils.GenerateMonsterSpawnPosition(Managers.Game.Player.transform.position, 10, 15);
        MonsterController mc = Managers.Object.Spawn<MonsterController>(randPos, 1 + Random.Range(0, 2));
    }


}
