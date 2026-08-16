# CLAUDE.md

이 파일은 이 저장소의 코드로 작업할 때 Claude Code(claude.ai/code)에 대한 지침을 제공합니다.

## 프로젝트 개요

**탕탕(TangTang)**은 **Unity 2022.3.62f2**로 제작된 로그라이크 스타일 액션 게임입니다. 이 프로젝트는 게임 시스템을 위한 매니저 기반 아키텍처와 게임 엔티티를 위한 컨트롤러 기반 아키텍처를 사용합니다.

## 개발 환경

- **Unity 버전**: 2022.3.62f2
- **대상 플랫폼**: 적응형 성능 최적화를 지원하는 모바일(Android/iOS)
- **주요 종속성**: Addressables, Timeline, TextMeshPro, 2D Feature(스프라이트, 애니메이션, 타일맵), Navigation, Visual Scripting, UnityTest Framework

## 빌드 및 실행

### Unity Editor에서 열기
```
Unity Hub에서 프로젝트 루트(C:\Users\jinboem\Desktop\Project\TangTangProject)를 2022.3.62f2 버전으로 열기
```

### 명령줄 빌드
Android용 빌드:
```bash
Unity.exe -projectPath . -executeMethod Build.BuildAndroid -nographics -quit
```

iOS용 빌드:
```bash
Unity.exe -projectPath . -executeMethod Build.BuildiOS -nographics -quit
```

### 테스트 실행
명령줄에서 Unity Test Framework 테스트 실행:
```bash
Unity.exe -projectPath . -runTests -testPlatform playmode -nographics -quit
```

Unity Editor를 통해서도 테스트 실행 가능: Window > TextExecution > Test Runner

## 아키텍처 및 코드 구조

### 핵심 매니저 시스템 (싱글톤 패턴)

모든 매니저는 **Managers** 싱글톤(Assets/@Scripts/Managers/Managers.cs)을 통해 접근됩니다:

**콘텐츠 매니저** (게임별 로직):
- **GameManager**: 핵심 게임 흐름 및 상태 관리
- **ObjectManager**: 게임 엔티티(생물, 발사체, 젬) 생성 및 관리
- **PoolManager**: 성능 최적화를 위한 객체 풀링

**핵심 매니저** (엔진 수준 서비스):
- **DataManager**: 게임 데이터 및 구성
- **ResourceManager**: Addressables를 통한 에셋 로딩
- **SceneManagerEx**: 씬 로딩 및 전환
- **SoundManager**: 오디오 재생
- **UIManager**: UI 시스템 조정

사용 패턴: `Managers.Game`, `Managers.Object`, `Managers.Data` 등

### 컨트롤러 시스템 (상속 계층)

**BaseController**(Assets/@Scripts/Controllers/BaseController.cs)는 모든 게임 엔티티의 루트 클래스입니다:
- 생명주기 훅 제공: `Init()`(Awake에서 호출) 및 `UpdateController()`(Update에서 호출)
- 모든 컨트롤러는 이 베이스를 상속하고 이 메서드들을 재정의합니다
- 엔티티 유형을 분류하기 위해 `objType` enum을 설정합니다

컨트롤러 유형:
- **CreatureController**: 플레이어 및 몬스터의 베이스(체력, 대미지, 스탯 처리)
- **PlayerController**: 플레이어 캐릭터 로직
- **MonsterController / BossController**: 적 AI 및 동작
- **ProjectileController**: 발사체 이동 및 충돌
- **GemController**: 수집 가능한 아이템
- **GridController / MapTileController**: 그리드 기반 레벨 레이아웃
- **CameraController**: 카메라 추적 및 동작

### 스킬 시스템 (다형성 디자인)

**SkillBase**(Assets/@Scripts/Contents/Skills/SkillBase.cs)는 모든 스킬의 루트 클래스입니다:
- `ActivateSkill()`: 스킬 동작을 정의하기 위해 재정의
- `GenerateProjectile()`: 발사체 생성을 위한 헬퍼
- 스킬은 소유자(CreatureController), 유형(Define.SkillType), 데이터(Data.SkillData)를 가집니다

스킬 유형:
- **RepeatSkill**: 수동/반복 스킬(예: EgoSword 근접, FireballSkill 발사체)
- **SequenceSkill**: 순차 행동 스킬(예: Dash, Move)

**SkillBook**(Assets/@Scripts/Contents/Skills/SkillBook.cs)은 사용 가능하고 학습한 스킬을 관리합니다.

스킬은 `Managers.Object.Spawn<SkillType>(position, skillID)`를 통해 생성되며, skillID는 `Data.SkillData`의 스킬 데이터에 매핑됩니다.

### 데이터 및 구성

- **Define.cs**(Assets/@Scripts/Utils/Define.cs): ObjectType, SkillType 및 게임 상수의 Enum(예: `GOBLIN_ID = 1`, `SNAKE_ID = 2`, `BOSS_ID = 3`)
- **Data.Contents.cs**(Assets/@Scripts/Data/Data.Contents.cs): 스킬, 생물, 게임 엔티티의 데이터 구조
- **SpawningPool**(Assets/@Scripts/Contents/SpawningPool.cs): 일반 스테이지 중에 몬스터를 일정 간격으로 생성; 보스 스테이지 중에는 비활성화

### 리소스 로딩 및 Addressables

리소스는 **ResourceManager**를 통해 비동기식으로 로드됩니다:

```csharp
// 캐시 기반 동기식 로딩 (이미 로드된 리소스)
Sprite sprite = Managers.Resource.Load<Sprite>("EXPGem_01.sprite");

// 콜백을 사용한 비동기 로딩
Managers.Resource.LoadAsync<GameObject>(key, (prefab) => {
    var go = Object.Instantiate(prefab);
});

// 라벨을 사용하여 모든 에셋 로드 및 진행률 콜백
Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) => {
    if (count == totalCount) StartLoaded(); // 모든 에셋 로드 시 호출
});

// 자동 풀링으로 인스턴스화
GameObject go = Managers.Resource.Instantiate("Slime_01.prefab", pooling: true);

// 제거 (풀링 가능하면 풀로 반환)
Managers.Resource.Destroy(go);
```

**중요**: `"EXPGem_01.sprite"`와 같은 Addressable 키 사용(스프라이트 에셋의 경우 `[key]` 구문 포함). `PreLoad` 라벨은 GameScene 시작 시 로드될 에셋을 그룹화합니다.

### UI 시스템

- **UI_Base**(Assets/@Scripts/UI/UI_Base.cs): 모든 UI 컴포넌트의 베이스 클래스
- **UI_GameScene**: 메인 게임 HUD
- **UI_SkillSelectPopup / UI_GameResultPopup**: 팝업 대화상자
- **UI_Joystick**: 모바일 가상 조이스틱 입력
- **UIEventHandler**: UI 상호작용을 위한 중앙 집중식 이벤트 처리

### 씬 구조 및 초기화 흐름

**GameScene**(Assets/@Scripts/Scenes/GameScene.cs)은 게임 초기화를 제어합니다:

1. **Start()**: "PreLoad" 라벨이 지정된 모든 에셋을 비동기식으로 로드합니다:
   ```csharp
   Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) => {
       if (count == totalCount) StartLoaded();
   });
   ```

2. **StartLoaded()**: PreLoad 에셋 로드가 완료될 때 호출됩니다:
   - 데이터 초기화: `Managers.Data.Init()`
   - UI 표시: `Managers.UI.ShowSceneUI<UI_GameScene>()`
   - 일정 간격으로 몬스터를 생성하는 SpawningPool 코루틴 생성
   - 플레이어 생성: `Managers.Object.Spawn<PlayerController>(Vector3.zero)`
   - UI/Map 프리팹 생성: `Managers.Resource.Instantiate("UI_Joystick.prefab")`
   - 게임 이벤트 구독: `OnGemCountChanged`, `OnKillCountChanged`

3. **이벤트 핸들러**: 게임 상태 변화 트리거:
   - `HandleOnGemCountChanged()`: 젬 10개 수집 → 스킬 선택 팝업 표시
   - `HandleOnKillCountChanged()`: 몬스터 5마리 처치 → 보스 스테이지로 전환

4. **스테이지 유형**(Define.StageType):
   - **Normal**: SpawningPool이 몬스터를 계속 생성(최대 100마리)
   - **Boss**: SpawningPool 중지; 5마리 처치 후 단일 보스 생성

## 일반적인 개발 작업

### 새로운 몬스터 유형 추가
1. Assets/@Resources/Prefabs/Monsters/에 프리팹 에셋 생성
2. Define.cs에 새로운 상수 추가: `public const int NEW_MONSTER_ID = 4;`
3. ObjectManager.Spawn<MonsterController>()에 프리팹 이름 매핑 추가:
   ```csharp
   case Define.NEW_MONSTER_ID:
       name = "NewMonster_01";
       break;
   ```
4. 사용자 정의 AI가 필요하면 MonsterController 서브클래스 생성, 또는 베이스 MonsterController 재사용
5. 생성하여 테스트: `Managers.Object.Spawn<MonsterController>(position, Define.NEW_MONSTER_ID)`

### 새로운 스킬 추가
1. `RepeatSkill` 또는 `SequenceSkill`을 상속하는 스킬 스크립트 생성
2. 동작을 위해 `ActivateSkill()` 재정의
3. Define.cs에 상수 추가: `public const int NEW_SKILL_ID = 30;`
4. 프리팹 경로 및 스탯과 함께 `Data.Contents.cs`에 스킬 데이터 추가
5. 스킬 컴포넌트를 사용하여 프리팹 생성 및 Addressables에 "PreLoad" 라벨과 함께 추가
6. SkillBook을 통하거나 생성하여 테스트: `Managers.Object.Spawn<YourSkillType>(position, Define.NEW_SKILL_ID)`

### 다양한 엔티티 유형 생성
```csharp
// 몬스터 변형 (templateID를 통해 자동으로 올바른 프리팹 로드)
Managers.Object.Spawn<MonsterController>(position, Define.GOBLIN_ID);
Managers.Object.Spawn<MonsterController>(position, Define.SNAKE_ID);
Managers.Object.Spawn<MonsterController>(position, Define.BOSS_ID);

// 스킬 (templateID = 스킬 ID)
Managers.Object.Spawn<FireballSkill>(position, Define.FRIREBALL_ID);

// 기타 엔티티 (templateID 없음)
Managers.Object.Spawn<GemController>(position);
Managers.Object.Spawn<ProjectileController>(position);
```

### 게임 이벤트 수신
```csharp
// GameScene 또는 모든 컨트롤러에서
void OnEnable() {
    Managers.Game.OnGemCountChanged += OnGemCountChanged;
    Managers.Game.OnKillCountChanged += OnKillCountChanged;
}

void OnDisable() {
    Managers.Game.OnGemCountChanged -= OnGemCountChanged;
    Managers.Game.OnKillCountChanged -= OnKillCountChanged;
}

void OnGemCountChanged(int gemCount) {
    // 젬 개수 변화 처리
}

void OnKillCountChanged(int killCount) {
    // 킬 개수 변화 처리
}
```

### 객체 생명주기
- **초기화**: `Init()`은 컨트롤러의 Awake에서 호출됩니다(한 번만, 반복 호출 시 false 반환)
- **업데이트**: 컨트롤러 클래스에서 `UpdateController()` 재정의
- **정리**: `Managers.Resource.Destroy(gameObject)`를 호출하여 객체를 풀로 반환하거나 제거
- **풀링**: `pooling: true` 매개변수로 생성되면 객체가 자동으로 풀링됩니다

## 중요한 패턴 및 관례

### 매니저 접근 및 null 안전성
매니저가 에디터 모드에서 초기화되지 않을 수 있으므로 항상 null 조건 연산자를 사용하세요:
```csharp
Managers.Game?.SetGemCount(10);  // 안전함
Managers.Data?.Init();
```

### 템플릿 ID를 사용한 객체 생성
`Define.cs` 상수를 사용하여 다양한 엔티티 변형을 생성합니다:
```csharp
Managers.Object.Spawn<MonsterController>(position, Define.GOBLIN_ID);    // 고블린
Managers.Object.Spawn<MonsterController>(position, Define.SNAKE_ID);     // 뱀
Managers.Object.Spawn<MonsterController>(position, Define.BOSS_ID);      // 보스
Managers.Object.Spawn<GemController>(position);  // 템플릿 ID 불필요
```

### 이벤트 기반 상태 관리
GameManager는 상태 변화를 위해 백킹 이벤트가 있는 속성을 사용합니다:
```csharp
// 젬 개수 변화 구독
Managers.Game.OnGemCountChanged -= HandleGemChanged;
Managers.Game.OnGemCountChanged += HandleGemChanged;

// 속성 설정으로 이벤트 트리거
Managers.Game.Gem = 5;  // OnGemCountChanged?.Invoke(5) 호출

// 정리 시 구독 해제 (예: OnDestroy)
Managers.Game.OnGemCountChanged -= HandleGemChanged;
```

사용 가능한 GameManager 이벤트: `OnGemCountChanged`, `OnKillCountChanged`, `OnMoveDirChanged`

### 객체 풀링
ObjectManager는 풀링을 내부적으로 처리합니다 - ObjectManager.Spawn() 및 ResourceManager.Destroy() 사용:
```csharp
PlayerController player = Managers.Object.Spawn<PlayerController>(Vector3.zero);
// 나중에, 작업이 완료될 때:
Managers.Resource.Destroy(player.gameObject);  // 풀로 반환하거나 제거
```

### 컨트롤러의 코루틴
지연된 작업이나 시퀀스에 `StartCoroutine()` 사용:
```csharp
StartCoroutine(DestroyAfterDelay(2f));
```

### 확장 메서드
`GetOrAddComponent<T>()`를 사용하여 안전하게 컴포넌트를 가져오거나 생성합니다:
```csharp
PlayerController pc = go.GetOrAddComponent<PlayerController>();
```

### 이벤트 기반 UI
직접 OnClick 참조 대신 UIEventHandler를 사용합니다.

## 폴더 구조 빠른 참조

```
Assets/
├── @Resources/     # 런타임 에셋 (프리팹, 스프라이트, 오디오)
├── @Scripts/
│   ├── Controllers/          # 게임 엔티티 제어 클래스
│   ├── Managers/             # 핵심 및 콘텐츠 매니저
│   ├── Contents/             # 게임 콘텐츠 (스킬, 생성)
│   ├── UI/                   # UI 컴포넌트 및 팝업
│   ├── Scenes/               # 씬 초기화 스크립트
│   ├── Data/                 # 데이터 정의
│   └── Utils/                # 유틸리티 및 헬퍼
└── ProjectSettings/          # Unity 프로젝트 구성
```

## 모바일 최적화

이 프로젝트는 Android/Samsung 기기용 **적응형 성능(Adaptive Performance)**을 포함합니다. 이는 저사양 기기에서 그래픽 품질을 자동으로 줄입니다:
- 종속성: `com.unity.adaptiveperformance`, `com.unity.adaptiveperformance.google.android`, `com.unity.adaptiveperformance.samsung.android`
- 시스템은 성능 병목을 자동으로 관리합니다 - 대부분의 경우 수동 개입이 필요하지 않습니다
- 테스트 중 저사양 기기에서 성능을 확인합니다

## 배포 및 게시

### Player 설정
- ProjectSettings/PlayerSettings에서 Android/iOS로 구성
- ProjectSettings > Quality의 적응형 성능 설정을 사용하여 모바일 최적화

### Addressables 설정
- 에셋은 Addressable 라벨로 구성되어야 합니다(예: 시작 에셋의 경우 "PreLoad")
- 플랫폼 빌드 전에 Addressables 빌드: Window > Asset Management > Addressables > Build > New Build
- 모든 리소스 로딩은 직접 에셋 경로가 아닌 Addressable 키를 사용하는 ResourceManager를 통해 진행됩니다

### 씬 로딩
- 매니저 시스템과의 일관성을 위해 SceneManager 대신 SceneManagerEx 사용
- 씬은 핵심 매니저를 통해 로드 및 관리됩니다

## 테스트 및 디버깅

### 테스트 실행
```bash
# Unity Editor를 통해
Window > TextExecution > Test Runner > Run All Tests

# 명령줄을 통해
Unity.exe -projectPath . -runTests -testPlatform playmode -nographics -quit
```

### 디버그 로깅
- 매니저 초기화를 위해 콘솔 확인: `Managers.Instance`(안전한 접근은 인스턴스 또는 null을 반환합니다)
- 컨트롤러는 시각적 디버깅을 위해 Inspector 속성을 노출합니다(체력, 위치, 상태)
- 상태 변화를 추적하려면 매니저 및 컨트롤러에서 `Debug.Log()` 사용

### 에디트 모드 vs 플레이 모드
- **에디트 모드**: MonoBehavior Awake의 Init() 로직을 확인하여 개별 컨트롤러/매니저 테스트
- **플레이 모드**: 콘솔 출력을 통해 매니저 초기화 및 엔티티 생성 확인
