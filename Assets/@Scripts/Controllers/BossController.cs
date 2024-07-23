using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonsterController
{
    public override bool Init()
    {
        base.Init();

        _animator = GetComponent<Animator>();
        CreatureState = Define.CreatureState.Moving;
        Hp = 10000;

        CreatureState = Define.CreatureState.Skill;
        Skills.AddSkill<Move>(transform.position);
        Skills.AddSkill<Dash>(transform.position);
        Skills.AddSkill<Dash>(transform.position);
        Skills.AddSkill<Dash>(transform.position);
        Skills.StartNextSequenceSkill();

        return true;
    }

    public override void UpdateAnimation()
    {
        switch(CreatureState)
        {
            case Define.CreatureState.Idle:
                _animator.Play("Idle");
                break;
            case Define.CreatureState.Moving:
                _animator.Play("Moving");
                break;
            case Define.CreatureState.Skill:
                //_animator.Play("Attack");
                break;
            case Define.CreatureState.Dead:
                _animator.Play("Death");
                break;
        }
    }

    //Boss Collider + Player Collider
    //float _range = 2.0f;
    //protected override void UpdateMoving()
    //{
    //    PlayerController pc = Managers.Object.Player;
    //    if(pc.IsValid() == false)
    //    {
    //        return;
    //    }

    //    Vector3 dir = pc.transform.position - transform.position;

    //    if(dir.magnitude<_range)
    //    {
    //        //상태를 스킬로 변경
    //        CreatureState = Define.CreatureState.Skill;

    //        // _animtor.runtimeAnimatorController.animationClips;
    //        float animLength = 0.41f;
    //        Wait(animLength);
    //    }
    //}

    //protected override void UpdateSkill()
    //{
    //    if(_coWait == null)
    //    {
    //        CreatureState = Define.CreatureState.Moving;
    //    }
    //}

    protected override void UpdateDead()
    {
        if(_coWait == null)
        {
            Managers.Object.Despawn(this);
        }
    }

    #region Wait Coroutine
    Coroutine _coWait;

    void Wait(float waitSeconds)
    {
        if(_coWait != null)
        {
            StopCoroutine(_coWait);
        }
        _coWait = StartCoroutine(CoStartWait(waitSeconds));
    }

    IEnumerator CoStartWait(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        _coWait = null;
        
    }
    #endregion

    public override void OnDamaged(BaseController attacker, int damage)
    {
        base.OnDamaged(attacker, 1);
    }

    protected override void OnDead()
    {
        CreatureState = Define.CreatureState.Dead;
        Wait(2.0f);
    }
}
