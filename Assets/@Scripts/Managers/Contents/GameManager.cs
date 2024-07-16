using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public PlayerController Player { get { return Managers.Object?.Player; }  }

    #region 재화
    public int Gold { get; set; }
    public int Gem { get; set; }
    #endregion

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
}
