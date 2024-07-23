using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameResultPopup : UI_Base
{
    #region UI 기능 리스트
    // 정보 갱신
    // ResultStageValueText : 해당 스테이지 수
    // ResultSurvivalTimeValueText : 스테이지 클리어 까지 걸린 시간 ( mm:ss 로 표기)
    // ResultGoldValueText : 죽기전 까지 얻은 골드
    // ResultKillValueText : 죽기전 까지 킬 수
    // ResultRewardScrollContentObject : : 보상으로 얻게될 아이템이 들어갈 부모 개체
    // (골드, 경헌치, 아이템, 캐릭터 강화석 등을 보상으로)

    // 로컬라이징 텍스트
    // GameResultPopupTitleText
    // ResultSurvivalTimeText
    // ConfirmButtonText
    #endregion

    enum GameObjects
    {
        ContentObject,
        ResultRewardScrollContentObject,
        ResultGoldObject,
        ResultKillObject,
    }

    enum Texts
    {
        GameResultPopupTitleText,
        ResultStageValueText,
        ResultSurvivalTimeText,
        ResultSurvivalTimeValueText,
        ResultGoldValueText,
        ResultKillValueText,
        ConfirmButtonText,
    }

    enum Buttons
    {
        StatisticsButton,
        ConfirmButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(GameObjects));
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        //버튼이 눌리면 매개변수의 함수를 실행
        GetButton((int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);

        RefreshUI();
        return true;
    }

    public void SetInfo()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        if (_init == false)
            return;

        // 정보 취합
        GetText((int)Texts.GameResultPopupTitleText).text = "Game Result";
        GetText((int)Texts.ResultStageValueText).text = "4 STAGE";
        GetText((int)Texts.ResultSurvivalTimeText).text = "Survival Time";
        GetText((int)Texts.ResultSurvivalTimeValueText).text = "14:23";
        GetText((int)Texts.ResultGoldValueText).text = "200";
        GetText((int)Texts.ResultKillValueText).text = "100";
        GetText((int)Texts.ConfirmButtonText).text = "OK";
    }

    void OnClickStatisticsButton()
    {
        Debug.Log("OnClickStatisticsButton");
    }

    void OnClickConfirmButton()
    {
        Debug.Log("OnClickConfirmButton");
    }

}
