using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public PlayerController Player { get { return Managers.Object?.Player; }  }

    #region 재화
    public int Gold { get; set; }

    int _gem = 0;
    public event Action<int> OnGemCountChanged;
    public int Gem 
    {
        get { return _gem; }
        set
        {
            _gem = value;
            OnGemCountChanged?.Invoke(value);
        }
    }
    #endregion

    #region 전투
    int _killcount;
    public event Action<int> OnKillCountChanged;

    public int KillCount
    {
        get { return _killcount; }
        set 
        { 
            _killcount = value; OnKillCountChanged?.Invoke(value); 
        }
    }
    #endregion

    #region 이동
    Vector2 _moveDir;
    public event Action<Vector2> OnMoveDirChanged;
    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set
        {
            _moveDir = value;
            //데이터가 변경시 호출됨
            OnMoveDirChanged?.Invoke(_moveDir);
        }

    }
    #endregion
}
