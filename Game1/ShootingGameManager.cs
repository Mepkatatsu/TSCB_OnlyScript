using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CoreLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShootingGameManager : Singleton<ShootingGameManager>
{
    #region Variables

    [Header("Parents")]
    [SerializeField] private GameObject shootingGameParent;
    [SerializeField] private GameObject bulletParent;
    [SerializeField] private GameObject aliveEnemyParent;
    [SerializeField] private GameObject deadEnemyParent;
    [SerializeField] private GameObject haloParent;
    [SerializeField] private GameObject explosionParent;
    [SerializeField] private GameObject lifeParent;
    
    [Header("Player, Boss")]
    [SerializeField] public GameObject midoriPlane;

    [SerializeField] public GameObject bossEnemy;
    [SerializeField] private GameObject bossMidoriHalo;
    [SerializeField] private GameObject bossMomoiHalo;
    [SerializeField] private GameObject bossYuzuHalo;
    [SerializeField] private GameObject bossArisHalo;
    [SerializeField] private GameObject bossArisLaser;
    [SerializeField] private Slider bossHPBar;
    
    [Header("Skills")]
    [SerializeField] private GameObject skillMessage;
    [SerializeField] private TMP_Text skill1CoolTimeText;
    [SerializeField] private GameObject skill1Background;
    [SerializeField] private TMP_Text skill2CoolTimeText;
    [SerializeField] private GameObject skill2Background;
    
    [Header("Score")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text newHighScoreText;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject midoriBulletPrefab;
    [SerializeField] private GameObject midoriHaloPrefab;
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private GameObject pinkEnemyPrefab;
    [SerializeField] private GameObject greenEnemyPrefab;
    [SerializeField] private GameObject purpleEnemyPrefab;
    [SerializeField] private GameObject yellowEnemyPrefab;
    [SerializeField] private GameObject explosionPrefab;
    
    [Header("Start Settings")]
    [SerializeField] private int startPhaseNum = 0; // 페이즈 (적 등장)

    private readonly Queue<GameObject> _midoriBulletQueue = new Queue<GameObject>();
    private readonly Queue<GameObject> _midoriHaloQueue = new Queue<GameObject>();
    private readonly Queue<GameObject> _enemyBulletQueue = new Queue<GameObject>();

    private readonly Queue<GameObject> _pinkEnemyQueue = new Queue<GameObject>();
    private readonly Queue<GameObject> _greenEnemyQueue = new Queue<GameObject>();
    private readonly Queue<GameObject> _purpleEnemyQueue = new Queue<GameObject>();
    private readonly Queue<GameObject> _yellowEnemyQueue = new Queue<GameObject>();
    private readonly Queue<GameObject> _explosionQueue = new Queue<GameObject>();

    private GameObject _yuzuGrenade;

    private int _phaseNum = 0;
    private int _life = 2; // 목숨
    private int _score = 0; // 점수
    private int _leftEnemy = 0; // 남은 적의 수

    private float _moveSpeed; // 플레이어 이동 속도
    private float _skill1Cooltime = 0; // 공격 쿨타임
    private float _skill2Cooltime = 0; // 스킬 쿨타임
    private float _yuzuBulletDuration = 0; // 유즈 유탄이 날아가는 시간
    private float _joystickInputX = 0;
    private float _joystickInputY = 0;

    private bool _isActivatingSkill = false; // 스킬 발동 중인지?
    private bool _isReadySkill1 = false; // 공격 준비 되었는지?
    private bool _isReadySkill2 = false; // 스킬 준비 되었는지?
    private bool _isAvailableMove = true; // 플레이어 이동 가능한지?
    private bool _isNewHighScore = false; // 최고 점수를 갱신했는지?
    private bool _isSpawnedBoss = false; // 보스가 스폰되었는지?
    private bool _isPointDownAttackButton = false;

    // 총알이 유지되어야 하는 최소 시간: 스킬 사용 시 적에게 총알이 날아가는 최대 시간 + 적 사망 대기시간 1초
    private const int MinimumTime = 2;

    // 스킬1(공격), 스킬2 쿨타임
    private const float Skill1CoolTime = 0.3f;
    private const float Skill2CoolTime = 10f;

    // 점수 관련
    private const int HitEnemyScore = 100;
    private const int KillEnemyScore = 500;
    private const int HighScore = 49000;

    // 화면 범위
    public const int XLeftEnd = -400;
    public const int XRIghtEnd = 400;
    public const int YDownEnd = -570;
    public const int YUpEnd = 570;

    // 적 관련 정보
    private const int BossMaxHP = 50;

    // 총알/적 이동 속도
    private const float MidoriBulletSpeed = 1.0f;
    private const float EnemyBulletSpeed = 1.0f;
    private const float PinkEnemySpeed = 1.0f;
    private const float GreenEnemySpeed = 1.5f;
    private const float YellowEnemySpeed = 1.5f;
    private const float PurpleEnemySpeed = 2f;

    #endregion Variables

    #region Properties

    public bool IsAlivePlane { get; private set; } = true;

    public bool IsShootingLaser { get; private set; }

    #endregion Properties

    // 기즈모
    /*
    [SerializeField] Transform _p1, _p2, _p3, _p4;
    [SerializeField] public float _gizmoDetail;

    List<Vector2> _gizmoPoints = new List<Vector2>();

    private void OnDrawGizmos()
    {
        _gizmoPoints.Clear();

        for (int i=0; i<_gizmoDetail; i++)
        {
            float t = (i / _gizmoDetail);

            Vector2 p5 = Vector2.Lerp(_p1.position, _p2.position, t);
            Vector2 p6 = Vector2.Lerp(_p2.position, _p3.position, t);
            Vector2 p7 = Vector2.Lerp(_p3.position, _p4.position, t);

            Vector2 p8 = Vector2.Lerp(p5, p6, t);
            Vector2 p9 = Vector2.Lerp(p6, p7, t);

            _gizmoPoints.Add(Vector2.Lerp(p8, p9, t));
        }

        for (int i=0; i<_gizmoPoints.Count - 1; i++)
        {
            Gizmos.DrawLine(_gizmoPoints[i], _gizmoPoints[i + 1]);
        }
    }
    */

    #region Unity Methods

    public override void Awake()
    {
        DOTween.SetTweensCapacity(500, 200);
    }

    private void Update()
    {
        if (!_isAvailableMove) return;

        _moveSpeed = 400f * Time.deltaTime;

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector2 moveDirection = new Vector2(inputX, inputY).normalized;

        if (_joystickInputX != 0 && _joystickInputY != 0) moveDirection = new Vector2(_joystickInputX, _joystickInputY);

        if (moveDirection.x > 0 && midoriPlane.GetComponent<RectTransform>().anchoredPosition.x >= 350)
            moveDirection.x = 0;
        else if (moveDirection.x < 0 && midoriPlane.GetComponent<RectTransform>().anchoredPosition.x < -350)
            moveDirection.x = 0;

        if (moveDirection.y > 0 && midoriPlane.GetComponent<RectTransform>().anchoredPosition.y >= 500)
            moveDirection.y = 0;
        else if (moveDirection.y < 0 && midoriPlane.GetComponent<RectTransform>().anchoredPosition.y <= -500)
            moveDirection.y = 0;

        moveDirection = moveDirection.normalized;

        midoriPlane.GetComponent<RectTransform>().anchoredPosition += moveDirection * _moveSpeed;

        // Z키를 눌러 총알을 발사
        if (Input.GetKey(KeyCode.Z))
        {
            OnClickAttackButton();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            OnClickSkillButton();
        }
    }

    #endregion Unity Methods

    public void OnClickAttackButton()
    {
        if (!_isAvailableMove) return;

        // 공격은 기본적으로 연타하기 때문에 경고 표시하지 않도록 함
        if (_skill1Cooltime <= 0)
        {
            _isReadySkill1 = false;
            skill1Background.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f);
            _skill1Cooltime = Skill1CoolTime;
            StartCoroutine(ShootMidoriBullet(new Vector2(midoriPlane.GetComponent<RectTransform>().anchoredPosition.x,
                570)));
        }
    }

    public IEnumerator OnPointDownAttackButtonCoroutine()
    {
        while (_isPointDownAttackButton)
        {
            OnClickAttackButton();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void OnPointerDownAttackButton()
    {
        _isPointDownAttackButton = true;
        StartCoroutine(OnPointDownAttackButtonCoroutine());
    }

    public void OnPointerUpAttackButton()
    {
        _isPointDownAttackButton = false;
    }

    public void OnClickSkillButton()
    {
        if (!_isAvailableMove) return;

        // 쿨타임 중이지 않고, 적이 하나라도 존재할 때 사용 가능
        if (_skill2Cooltime <= 0 && aliveEnemyParent.transform.childCount > 0)
        {
            _isReadySkill2 = false;
            skill2Background.GetComponent<Image>().color = new Color(0, 0, 0, 0.8f);
            _skill2Cooltime = Skill2CoolTime;
            StartCoroutine(UseMidoriSkill());
        }
        // 쿨타임 중에 경고음, 메세지 등 표시
        else if (_skill2Cooltime > 0)
        {
            AudioManager.Instance.PlaySFX("Warning");
            skillMessage.GetComponent<TMP_Text>().text = "재사용 대기 중입니다!";
            skillMessage.GetComponent<Animator>().Play("TextFade");
        }
        // 적이 없을 경우 경고음, 메세지 등 표시
        else if (aliveEnemyParent.transform.childCount == 0)
        {
            AudioManager.Instance.PlaySFX("Warning");
            skillMessage.GetComponent<TMP_Text>().text = "근처에 적이 없습니다!";
            skillMessage.GetComponent<Animator>().Play("TextFade");
        }
    }

    public void SetJoystickInput(float inputX, float inputY)
    {
        _joystickInputX = inputX;
        _joystickInputY = inputY;
    }

    public void StartShootingGame()
    {
        shootingGameParent.SetActive(true);
        InitializeShootingGame();
    }

    public void StopShootingGame()
    {
        shootingGameParent.SetActive(false);
    }

    public void InitializeShootingGame()
    {
        StopAllCoroutines();

        List<GameObject> enemyList = new List<GameObject>();

        AudioManager.Instance.PlayBGM("KurameNoMyojo");

        _phaseNum = startPhaseNum;
        _life = 2;
        _score = 0;
        _leftEnemy = 0;
        _skill1Cooltime = 0;
        _skill2Cooltime = 0;
        _isActivatingSkill = false;
        _isReadySkill1 = false;
        _isReadySkill2 = false;
        _isAvailableMove = true;
        IsAlivePlane = true;
        _isNewHighScore = false;
        IsShootingLaser = false;
        _isSpawnedBoss = false;

        highScoreText.text = HighScore.ToString();

        bossEnemy.GetComponent<ShootingGameEnemy>().SetEnemyMaxHP();
        bossHPBar.value = 1;
        bossEnemy.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 650);

        bossHPBar.transform.GetChild(0).GetComponent<Image>().DOFade(0, 0);
        bossHPBar.transform.GetChild(1).GetChild(0).GetComponent<Image>().DOFade(0, 0);

        midoriPlane.SetActive(true);
        midoriPlane.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -500);
        midoriPlane.GetComponent<Image>().color = Color.white;
        StoryManager.Instance.SetCharacterImage(StoryManager.Instance.midori, 10);

        lifeParent.transform.GetChild(0).gameObject.SetActive(true);
        lifeParent.transform.GetChild(1).gameObject.SetActive(true);

        skillMessage.GetComponent<TMP_Text>().fontSize = 40;
        skillMessage.GetComponent<TMP_Text>().text = "";
        skillMessage.GetComponent<Animator>().Play("Idle");

        AddScore(0);

        for (int i = 0; i < aliveEnemyParent.transform.childCount; i++)
        {
            enemyList.Add(aliveEnemyParent.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].SetActive(false);
            EnqueEnemy(enemyList[i]);
            enemyList[i].transform.SetParent(deadEnemyParent.transform);
            enemyList[i].GetComponent<ShootingGameEnemy>().StopMoving();
        }

        bossMidoriHalo.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        bossMomoiHalo.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        bossYuzuHalo.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        bossArisHalo.GetComponent<Image>().color = new Color(1, 1, 1, 0);

        StoryManager.Instance.midori.SetActive(true);
        StoryManager.Instance.midori.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(-600, 50);
        StoryManager.Instance.midori.transform.parent.GetComponent<RectTransform>().localScale = new Vector2(0.8f, 0.8f);

        StartCoroutine(DoNextPhase());
        StartCoroutine(DoShootingGameTimer());
    }

    private IEnumerator DoShootingGameTimer()
    {
        while (true)
        {
            if (_skill1Cooltime > 0)
            {
                skill1CoolTimeText.text = _skill1Cooltime.ToString("F1");
                _skill1Cooltime -= 0.1f;
            }
            else if (_skill1Cooltime <= 0 && !_isReadySkill1)
            {
                _isReadySkill1 = true;
                skill1CoolTimeText.text = "";
                skill1Background.GetComponent<Image>().color = Color.clear;
            }

            if (_skill2Cooltime > 0)
            {
                skill2CoolTimeText.text = _skill2Cooltime.ToString("F1");
                _skill2Cooltime -= 0.1f;
            }
            else if (_skill2Cooltime <= 0 && !_isReadySkill2)
            {
                _isReadySkill2 = true;
                skill2CoolTimeText.text = "";
                skill2Background.GetComponent<Image>().color = Color.clear;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    // 플레이어가 목숨을 전부 사용하였을 때 동작
    private void DoGameOver()
    {
        bossMomoiHalo.GetComponent<Image>().DOFade(0, 1);
        bossMidoriHalo.GetComponent<Image>().DOFade(0, 1);
        bossYuzuHalo.GetComponent<Image>().DOFade(0, 1);
        bossArisHalo.GetComponent<Image>().DOFade(0, 1);

        AudioManager.Instance.FadeOutMusic();
        AudioManager.Instance.PlaySFX("GameOver");
        skillMessage.GetComponent<TMP_Text>().fontSize = 80;
        skillMessage.GetComponent<TMP_Text>().text = "< GAME OVER >";
        skillMessage.GetComponent<Animator>().Play("GameOverText");

        if (_isNewHighScore) StoryManager.Instance.DoShootingGameEnd("GameClear");
        else StoryManager.Instance.DoShootingGameEnd("GameOver");
    }

    // 플레이어가 보스를 쓰러뜨렸을 때 동작
    private IEnumerator DoGameWin()
    {
        IsAlivePlane = false;
        _isAvailableMove = false;

        List<GameObject> enemyList = new List<GameObject>();

        bossMomoiHalo.GetComponent<Image>().DOFade(0, 1);
        bossMidoriHalo.GetComponent<Image>().DOFade(0, 1);
        bossYuzuHalo.GetComponent<Image>().DOFade(0, 1);
        bossArisHalo.GetComponent<Image>().DOFade(0, 1);

        yield return new WaitForSeconds(1);

        for (int i = 0; i < aliveEnemyParent.transform.childCount; i++)
        {
            enemyList.Add(aliveEnemyParent.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GetComponent<ShootingGameEnemy>().StopMoving();
            if (!enemyList[i].activeSelf) continue;
            enemyList[i].SetActive(false);
            EnqueEnemy(enemyList[i]);
            enemyList[i].transform.SetParent(deadEnemyParent.transform);
            StartCoroutine(ShowExplosion(enemyList[i]));

            yield return new WaitForSeconds(0.2f);
        }

        AudioManager.Instance.FadeOutMusic();
        AudioManager.Instance.PlaySFX("Win");
        skillMessage.GetComponent<TMP_Text>().fontSize = 80;
        skillMessage.GetComponent<TMP_Text>().text = "<color=#FFE400>< VICTORY! ></color>";
        skillMessage.GetComponent<Animator>().Play("GameOverText");

        if (_isNewHighScore) StoryManager.Instance.DoShootingGameEnd("GameClear");
        else StoryManager.Instance.DoShootingGameEnd("GameWin");
    }

    #region For Object Pooling

    private GameObject GetPrefab(string name)
    {
        if (name == "MidoriHalo")
        {
            if (_midoriHaloQueue.Count > 0)
            {
                return _midoriHaloQueue.Dequeue();
            }
            else
            {
                return CreateNewMidoriHalo();
            }
        }
        else if (name == "MidoriBullet")
        {
            if (_midoriBulletQueue.Count > 0)
            {
                return _midoriBulletQueue.Dequeue();
            }
            else
            {
                return CreateNewMidoriBullet();
            }
        }
        else if (name == "EnemyBullet")
        {
            if (_enemyBulletQueue.Count > 0)
            {
                return _enemyBulletQueue.Dequeue();
            }
            else
            {
                return CreateNewEnemyBullet();
            }
        }

        if (name == "PinkEnemy")
        {
            if (_pinkEnemyQueue.Count > 0)
            {
                return _pinkEnemyQueue.Dequeue();
            }
            else
            {
                return CreateNewEnemy(name);
            }
        }
        else if (name == "GreenEnemy")
        {
            if (_greenEnemyQueue.Count > 0)
            {
                return _greenEnemyQueue.Dequeue();
            }
            else
            {
                return CreateNewEnemy(name);
            }
        }
        else if (name == "PurpleEnemy")
        {
            if (_purpleEnemyQueue.Count > 0)
            {
                return _purpleEnemyQueue.Dequeue();
            }
            else
            {
                return CreateNewEnemy(name);
            }
        }
        else if (name == "YellowEnemy")
        {
            if (_yellowEnemyQueue.Count > 0)
            {
                return _yellowEnemyQueue.Dequeue();
            }
            else
            {
                return CreateNewEnemy(name);
            }
        }
        else if (name == "Explosion")
        {
            if (_explosionQueue.Count > 0)
            {
                return _explosionQueue.Dequeue();
            }
            else
            {
                return CreateNewExplosion();
            }
        }
        else
        {
            Debug.Log("GetFrefab 이름 오류");
            return null;
        }
    }

    // 새로운 폭발 이미지를 만듬
    private GameObject CreateNewExplosion()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.SetActive(false);
        explosion.transform.SetParent(explosionParent.transform);
        explosion.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        explosion.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        explosion.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        return explosion;
    }

    // 새로운 미도리 헤일로를 만듬
    private GameObject CreateNewMidoriHalo()
    {
        GameObject halo = Instantiate(midoriHaloPrefab);
        halo.SetActive(false);
        halo.transform.SetParent(haloParent.transform);
        halo.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        halo.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        halo.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        return halo;
    }

    // 새로운 적 총알을 만듬
    private GameObject CreateNewEnemyBullet()
    {
        GameObject bullet = Instantiate(enemyBulletPrefab);
        bullet.SetActive(false);
        bullet.transform.SetParent(bulletParent.transform);
        bullet.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        bullet.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        return bullet;
    }

    // 새로운 미도리의 총알을 만듬
    private GameObject CreateNewMidoriBullet()
    {
        GameObject bullet = Instantiate(midoriBulletPrefab);
        bullet.SetActive(false);
        bullet.transform.SetParent(bulletParent.transform);
        bullet.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        bullet.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        return bullet;
    }

    // 적을 지정된 위치에 소환
    private GameObject SpawnEnemy(string name, Vector2 position)
    {
        GameObject enemy = GetPrefab(name);

        enemy.transform.SetParent(aliveEnemyParent.transform);
        enemy.SetActive(true);
        enemy.GetComponent<RectTransform>().anchoredPosition = position;
        enemy.GetComponent<ShootingGameEnemy>().InitializeEnemy();

        return enemy;
    }

    // 새로운 적을 만듬
    private GameObject CreateNewEnemy(string name)
    {
        GameObject enemy;

        if (name == "PinkEnemy")
        {
            enemy = Instantiate(pinkEnemyPrefab);
        }
        else if (name == "GreenEnemy")
        {
            enemy = Instantiate(greenEnemyPrefab);
        }
        else if (name == "PurpleEnemy")
        {
            enemy = Instantiate(purpleEnemyPrefab);
        }
        else
        {
            enemy = Instantiate(yellowEnemyPrefab);
        }

        enemy.SetActive(false);
        enemy.transform.SetParent(deadEnemyParent.transform);
        enemy.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        enemy.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);

        return enemy;
    }

    private void EnqueEnemy(GameObject enemy)
    {
        if (enemy.CompareTag("PinkEnemy"))
        {
            _pinkEnemyQueue.Enqueue(enemy);
        }
        else if (enemy.CompareTag("GreenEnemy"))
        {
            _greenEnemyQueue.Enqueue(enemy);
        }
        else if (enemy.CompareTag("YellowEnemy"))
        {
            _yellowEnemyQueue.Enqueue(enemy);
        }
        else if (enemy.CompareTag("PurpleEnemy"))
        {
            _purpleEnemyQueue.Enqueue(enemy);
        }
    }

    #endregion For Object Pooling

    #region For Spawn Enemy

    private void SpawnStraightEnemy(string enemyName, int spawnNum)
    {
        GameObject enemy;

        float time = 30;

        if (enemyName == "PinkEnemy")
        {
            time /= PinkEnemySpeed;
        }
        else if (enemyName == "GreenEnemy")
        {
            time /= GreenEnemySpeed;
        }
        else if (enemyName == "YellowEnemy")
        {
            time /= YellowEnemySpeed;
        }
        else if (enemyName == "PurpleEnemy")
        {
            time /= PurpleEnemySpeed;
        }

        enemy = SpawnEnemy(enemyName, new Vector2(-300 + (spawnNum * 150), 600));

        enemy.GetComponent<ShootingGameEnemy>().DoEnemyMove(new Vector2(-300 + (spawnNum * 150), 600),
            new Vector2(-300 + (spawnNum * 150), -600), time);
    }

    // 적이 스폰되는 번호에 따라 위치와 동작 부여
    private void SpawnBezierCurveEnemy(string enemyName, int spawnNum, float speed)
    {
        GameObject enemy;

        if (spawnNum == 0)
        {
            enemy = SpawnEnemy(enemyName, new Vector2(-300, 600));

            enemy.GetComponent<ShootingGameEnemy>().DoBezierCurves2(
                enemy.GetComponent<RectTransform>().anchoredPosition,
                new Vector2(-400, 0), new Vector2(500, 600), new Vector2(300, -600), speed);
        }
        else if (spawnNum == 1)
        {
            enemy = SpawnEnemy(enemyName, new Vector2(-150, 600));

            enemy.GetComponent<ShootingGameEnemy>().DoBezierCurves2(
                enemy.GetComponent<RectTransform>().anchoredPosition,
                new Vector2(800, 0), new Vector2(-800, 0), new Vector2(150, -600), speed);
        }
        else if (spawnNum == 2)
        {
            enemy = SpawnEnemy(enemyName, new Vector2(150, 600));

            enemy.GetComponent<ShootingGameEnemy>().DoBezierCurves2(
                enemy.GetComponent<RectTransform>().anchoredPosition,
                new Vector2(-800, 0), new Vector2(800, 0), new Vector2(-150, -600), speed);
        }
        else if (spawnNum == 3)
        {
            enemy = SpawnEnemy(enemyName, new Vector2(300, 600));

            enemy.GetComponent<ShootingGameEnemy>().DoBezierCurves2(
                enemy.GetComponent<RectTransform>().anchoredPosition,
                new Vector2(400, 0), new Vector2(-500, 600), new Vector2(-300, -600), speed);
        }
    }

    public IEnumerator DoNextPhase()
    {
        yield return new WaitForSeconds(3);

        if (_phaseNum == 0)
        {
            for (int i = 0; i < 5; ++i)
            {
                ++_leftEnemy;
                SpawnStraightEnemy("PinkEnemy", i);
            }
        }
        else if (_phaseNum == 1)
        {
            for (int i = 0; i < 5; ++i)
            {
                ++_leftEnemy;
                SpawnStraightEnemy("GreenEnemy", i);
            }
        }
        else if (_phaseNum == 2)
        {
            for (int i = 0; i < 4; ++i)
            {
                ++_leftEnemy;
                SpawnBezierCurveEnemy("YellowEnemy", i, YellowEnemySpeed);
            }
        }
        else if (_phaseNum == 3)
        {
            for (int i = 0; i < 4; ++i)
            {
                ++_leftEnemy;
                SpawnBezierCurveEnemy("PurpleEnemy", i, PurpleEnemySpeed);
            }
        }
        else if (_phaseNum == 4)
        {
            _leftEnemy = 9;

            for (int i = 0; i < 5; ++i)
            {
                SpawnStraightEnemy("GreenEnemy", i);
            }

            yield return new WaitForSeconds(3);

            for (int i = 0; i < 4; ++i)
            {
                SpawnBezierCurveEnemy("PurpleEnemy", i, PurpleEnemySpeed);
            }
        }
        else if (_phaseNum == 5)
        {
            _leftEnemy = 9;

            for (int i = 0; i < 5; ++i)
            {
                SpawnStraightEnemy("GreenEnemy", i);
            }

            yield return new WaitForSeconds(3);

            for (int i = 0; i < 4; ++i)
            {
                SpawnBezierCurveEnemy("YellowEnemy", i, YellowEnemySpeed);
            }
        }
        else if (_phaseNum == 6)
        {
            _leftEnemy = 9;

            for (int i = 0; i < 5; ++i)
            {
                SpawnStraightEnemy("PinkEnemy", i);
            }

            yield return new WaitForSeconds(3);

            for (int i = 0; i < 4; ++i)
            {
                SpawnBezierCurveEnemy("PurpleEnemy", i, PurpleEnemySpeed);
            }
        }
        else if (_phaseNum == 7)
        {
            _leftEnemy = 9;

            for (int i = 0; i < 5; ++i)
            {
                SpawnStraightEnemy("PinkEnemy", i);
            }

            yield return new WaitForSeconds(3);

            for (int i = 0; i < 4; ++i)
            {
                SpawnBezierCurveEnemy("YellowEnemy", i, YellowEnemySpeed);
            }
        }
        else if (_phaseNum == 8)
        {
            // 보스 등장
            AudioManager.Instance.FadeOutMusic();

            _isSpawnedBoss = true;

            bossEnemy.transform.SetParent(aliveEnemyParent.transform);
            bossEnemy.SetActive(true);
            bossEnemy.GetComponent<ShootingGameEnemy>().InitializeEnemy();

            yield return new WaitForSeconds(1f);

            AudioManager.Instance.PlayBGM("FinalFancyBattle");

            bossHPBar.transform.GetChild(0).GetComponent<Image>().DOFade(1, 1);
            bossHPBar.transform.GetChild(1).GetChild(0).GetComponent<Image>().DOFade(1, 1);
        }
        else
        {
            // 보스 등장 이후 반복
            if (_phaseNum % 4 == 1)
            {
                for (int i = 0; i < 5; ++i)
                {
                    ++_leftEnemy;
                    SpawnStraightEnemy("PinkEnemy", i);
                }
            }
            else if (_phaseNum % 4 == 2)
            {
                for (int i = 0; i < 5; ++i)
                {
                    ++_leftEnemy;
                    SpawnStraightEnemy("GreenEnemy", i);
                }
            }
            else if (_phaseNum % 4 == 3)
            {
                for (int i = 0; i < 4; ++i)
                {
                    ++_leftEnemy;
                    SpawnBezierCurveEnemy("YellowEnemy", i, YellowEnemySpeed);
                }
            }
            else if (_phaseNum % 4 == 0)
            {
                for (int i = 0; i < 4; ++i)
                {
                    ++_leftEnemy;
                    SpawnBezierCurveEnemy("PurpleEnemy", i, PurpleEnemySpeed);
                }
            }
        }

        _phaseNum++;
    }

    #endregion For Spawn Enemy

    private IEnumerator ReviveMidoriPlane()
    {
        yield return new WaitForSeconds(2);
        if (_life <= 0)
        {
            DoGameOver();
            yield break;
        }
        else
        {
            --_life;

            if (_life == 1) lifeParent.transform.GetChild(1).gameObject.SetActive(false);
            else if (_life == 0) lifeParent.transform.GetChild(0).gameObject.SetActive(false);
        }

        midoriPlane.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -600);
        midoriPlane.GetComponent<Image>().color = Color.white;
        midoriPlane.GetComponent<RectTransform>().DOLocalMoveY(-500, 2);

        yield return new WaitForSeconds(2);

        _isAvailableMove = true;

        // 1초간 무적 적용
        yield return new WaitForSeconds(1);

        IsAlivePlane = true;
    }

    private IEnumerator ShowExplosion(GameObject target)
    {
        GameObject explosion = GetPrefab("Explosion");
        explosion.GetComponent<RectTransform>().anchoredPosition =
            target.GetComponent<RectTransform>().anchoredPosition;
        explosion.SetActive(true);
        explosion.GetComponent<Animator>().Play("Explosion");
        AudioManager.Instance.PlaySFX("EnemyDestroy");

        yield return new WaitForSeconds(0.5f);

        _explosionQueue.Enqueue(explosion);
    }

    public IEnumerator HitByEnemy(GameObject midoriPlane, GameObject enemy)
    {
        if (!IsAlivePlane) yield break;
        if (enemy.CompareTag("EnemyLaser") && !IsShootingLaser) yield break;

        _isAvailableMove = false;
        IsAlivePlane = false;


        midoriPlane.GetComponent<Image>().color = Color.clear;
        StartCoroutine(ShowExplosion(midoriPlane));

        if (enemy.CompareTag("EnemyBullet"))
        {
            enemy.GetComponent<Image>().color = Color.clear;
        }

        StartCoroutine(StoryManager.Instance.ShowSadMidori());

        StartCoroutine(ReviveMidoriPlane());
    }

    public IEnumerator HitByBullet(GameObject bullet, GameObject target)
    {
        // 이미 총알이 공격한 상태라면 공격이 작동하지 않도록 함
        if (!bullet.GetComponent<MidoriBullet>().GetIsAvailableAttack()) yield break;

        // 총알이 적중하면 투명하게 이미지를 바꾸고, 적중 상태를 변경 (Active를 false로 하면 yield return 이후 호출이 불가능해짐)
        bullet.GetComponent<Image>().color = Color.clear;
        bullet.GetComponent<MidoriBullet>().SetIsAvailableAttack(false);

        // 아군 총알이 적을 공격했을 떄
        if (target.CompareTag("PinkEnemy") || target.CompareTag("GreenEnemy") || target.CompareTag("YellowEnemy") ||
            target.CompareTag("PurpleEnemy") || target.CompareTag("BossEnemy"))
        {
            var shootingGameEnemy = target.GetComponent<ShootingGameEnemy>();

            if (target.CompareTag("BossEnemy"))
            {
                bossHPBar.value = ((float)shootingGameEnemy.EnemyHP - 1) / BossMaxHP;
            }

            if (shootingGameEnemy.EnemyHP > 1)
            {
                AddScore(HitEnemyScore);

                shootingGameEnemy.SetEnemyHP(shootingGameEnemy.EnemyHP - 1);
                AudioManager.Instance.PlaySFX("EnemyHit");
            }
            else
            {
                target.SetActive(false);

                if (target.CompareTag("BossEnemy"))
                {
                    StartCoroutine(DoGameWin());
                }

                AddScore(KillEnemyScore);

                shootingGameEnemy.SetEnemyHP(shootingGameEnemy.EnemyHP - 1);

                StartCoroutine(StoryManager.Instance.ShowHappyMidori());

                StartCoroutine(ShowExplosion(target));

                _leftEnemy--;
                if (_leftEnemy == 0 && !_isSpawnedBoss) StartCoroutine(DoNextPhase());

                // 스킬 사용 중 적의 Parent가 바뀌어 오류 생기는 것을 방지
                if (_isActivatingSkill) yield return new WaitForSeconds(2);

                target.GetComponent<ShootingGameEnemy>().StopMoving();
                target.transform.SetParent(deadEnemyParent.transform);
                EnqueEnemy(target);
            }
        }
    }

    private void AddScore(int score)
    {
        _score += score;
        scoreText.text = _score.ToString();

        if (_score > HighScore)
        {
            highScoreText.text = _score.ToString();

            if (!_isNewHighScore)
            {
                _isNewHighScore = true;
                DOTween.Sequence().Append(newHighScoreText.DOFade(1, 1)).Append(newHighScoreText.DOFade(0, 1))
                    .Append(newHighScoreText.DOFade(1, 1)).Append(newHighScoreText.DOFade(0, 1));
            }
        }
    }

    #region Helper Method

    // mainObject가 direct을 바라보도록 회전
    private void LookRotation2D(GameObject mainObject, Vector2 direction)
    {
        Vector3 vectorToTarget = direction - mainObject.GetComponent<RectTransform>().anchoredPosition;
        vectorToTarget.z = 0;
        float angle = 0;

        if (mainObject.CompareTag("MidoriBullet"))
        {
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        }
        else if (mainObject.CompareTag("EnemyBullet"))
        {
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg + 90;
        }

        mainObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // 총알의 목적지를 화면 밖까지 연장
    public Vector2 ExtendBulletDirection(Vector2 bulletPosition, Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - bulletPosition;

        // 0으로 나누지 않도록 예외 처리
        if (direction.x == 0)
        {
            direction.x = bulletPosition.x;
            direction.y = (direction.y < 0) ? YDownEnd : YUpEnd;

            return direction;
        }
        else if (direction.y == 0)
        {
            direction.x = (direction.x < 0) ? XLeftEnd : XRIghtEnd;
            direction.y = bulletPosition.y;

            return direction;
        }

        float xToEnd, yToEnd, xToEndRate, yToEndRate;

        // x, y축 기준 화면 밖으로 이동하기 위해 필요한 거리
        xToEnd = (direction.x < 0) ? XLeftEnd - bulletPosition.x : XRIghtEnd - bulletPosition.x;
        yToEnd = (direction.y < 0) ? YDownEnd - bulletPosition.y : YUpEnd - bulletPosition.y;

        // 화면 밖까지 나가려면 얼마나 이동해야 하는지 비율
        xToEndRate = xToEnd / direction.x;
        yToEndRate = yToEnd / direction.y;

        // 비율에 따라 거리 연장
        if (xToEndRate > yToEndRate)
        {
            direction.x *= yToEndRate;
            direction.x += bulletPosition.x;

            direction.y = (direction.y < 0) ? YDownEnd : YUpEnd;
        }
        else
        {
            direction.x = (direction.x < 0) ? XLeftEnd : XRIghtEnd;

            direction.y *= xToEndRate;
            direction.y += bulletPosition.y;
        }

        return direction;
    }

    #endregion Helper Method

    #region Player&Enemy Attack

    public void ShootEnemyBulletToMidoriPlane(GameObject enemy)
    {
        StartCoroutine(ShootEnemyBulletToMidoriPlaneCoroutine(enemy,
            midoriPlane.GetComponent<RectTransform>().anchoredPosition));
    }

    public IEnumerator ShootEnemyBulletToMidoriPlaneCoroutine(GameObject enemy, Vector2 direction,
        string bulletName = "Normal")
    {
        GameObject bullet = GetPrefab("EnemyBullet");
        bullet.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        bullet.SetActive(true);
        bullet.GetComponent<Image>().color = Color.white;

        bullet.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            enemy.GetComponent<RectTransform>().anchoredPosition.x,
            enemy.GetComponent<RectTransform>().anchoredPosition.y - 50);

        // 적들의 통상 공격
        if (bulletName == "Normal")
        {
            direction = ExtendBulletDirection(bullet.GetComponent<RectTransform>().anchoredPosition, direction);
        }
        // 유즈 유탄 공격
        else if (bulletName == "YuzuGrenade")
        {
            bullet.GetComponent<RectTransform>().localScale = new Vector3(6f, 6f, 6f);
            _yuzuGrenade = bullet;

            // 유탄이 헤일로 위치까지 날아가는 시간을 저장
            _yuzuBulletDuration = Vector2.Distance(bullet.GetComponent<RectTransform>().anchoredPosition,
                bossYuzuHalo.GetComponent<RectTransform>().anchoredPosition) / 600 * EnemyBulletSpeed;

            // 유탄의 경로를 연장, UseBossSkill 부분에서 _yuzuBulletDuration이 경과한 이후 체크하기 위함
            direction = ExtendBulletDirection(bullet.GetComponent<RectTransform>().anchoredPosition, direction);
        }
        else if (bulletName == "YuzuBullet")
        {
            // 유즈 헤일로의 중심에서 총알이 나가는 것이기 때문에 y값을 제자리로 돌려줌
            bullet.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                enemy.GetComponent<RectTransform>().anchoredPosition.x,
                enemy.GetComponent<RectTransform>().anchoredPosition.y);

            direction = ExtendBulletDirection(bullet.GetComponent<RectTransform>().anchoredPosition, direction);
        }

        LookRotation2D(bullet, direction);

        float bulletDuration = Vector2.Distance(bullet.GetComponent<RectTransform>().anchoredPosition, direction) /
            600 * EnemyBulletSpeed;

        bullet.GetComponent<RectTransform>().DOLocalMove(direction, bulletDuration).SetEase(Ease.Linear);

        yield return new WaitForSeconds(bulletDuration);
        bullet.GetComponent<Image>().color = Color.clear;
        bullet.SetActive(false);
        _enemyBulletQueue.Enqueue(bullet);
    }

    public IEnumerator ShootMidoriBullet(Vector2 direction)
    {
        GameObject bullet = GetPrefab("MidoriBullet");

        bullet.SetActive(true);
        bullet.GetComponent<Image>().color = Color.white;
        bullet.GetComponent<MidoriBullet>().SetIsAvailableAttack(true);

        bullet.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            midoriPlane.GetComponent<RectTransform>().anchoredPosition.x,
            midoriPlane.GetComponent<RectTransform>().anchoredPosition.y + 50);

        if (direction.y < 570)
        {
            direction = ExtendBulletDirection(bullet.GetComponent<RectTransform>().anchoredPosition, direction);
        }

        LookRotation2D(bullet, direction);

        AudioManager.Instance.PlaySFX("Shoot");

        float bulletDuration = Vector2.Distance(bullet.GetComponent<RectTransform>().anchoredPosition, direction) /
            1200 * MidoriBulletSpeed;
        bullet.GetComponent<RectTransform>().DOLocalMove(direction, bulletDuration).SetEase(Ease.Linear);

        yield return new WaitForSeconds(bulletDuration);
        bullet.GetComponent<Image>().color = Color.clear;
        bullet.GetComponent<MidoriBullet>().SetIsAvailableAttack(false);

        yield return new WaitForSeconds(MinimumTime);

        bullet.SetActive(false);
        _midoriBulletQueue.Enqueue(bullet);
    }

    public IEnumerator UseBossSkill(int skillNum)
    {
        if (skillNum == 0)
        {
            AudioManager.Instance.PlaySFX("UseSkill");
            bossMidoriHalo.GetComponent<Image>().DOFade(1, 1);

            yield return new WaitForSeconds(1);

            bossMidoriHalo.GetComponent<Image>().DOFade(0, 1);

            for (int i = 0; i < 10; ++i)
            {
                ShootEnemyBulletToMidoriPlane(bossEnemy);

                yield return new WaitForSeconds(0.2f);
            }
        }
        else if (skillNum == 1)
        {
            AudioManager.Instance.PlaySFX("UseSkill");
            bossMomoiHalo.GetComponent<Image>().DOFade(1, 1);

            yield return new WaitForSeconds(1f);

            AudioManager.Instance.PlaySFX("MomoiSkill");
            bossMomoiHalo.GetComponent<Image>().DOFade(0, 1);

            for (int i = 0; i < 11; i += 2)
            {
                StartCoroutine(
                    ShootEnemyBulletToMidoriPlaneCoroutine(bossEnemy, new Vector2(-400 + (80 * i), YDownEnd)));
            }

            yield return new WaitForSeconds(0.5f);

            for (int i = 1; i < 11; i += 2)
            {
                StartCoroutine(
                    ShootEnemyBulletToMidoriPlaneCoroutine(bossEnemy, new Vector2(-400 + (80 * i), YDownEnd)));
            }

            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < 11; i += 2)
            {
                StartCoroutine(
                    ShootEnemyBulletToMidoriPlaneCoroutine(bossEnemy, new Vector2(-400 + (80 * i), YDownEnd)));
            }
        }
        else if (skillNum == 2)
        {
            AudioManager.Instance.PlaySFX("UseSkill");

            bossYuzuHalo.transform.SetParent(midoriPlane.transform);
            bossYuzuHalo.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            bossYuzuHalo.GetComponent<Image>().DOFade(1, 1);

            yield return new WaitForSeconds(1f);

            bossYuzuHalo.transform.SetParent(haloParent.transform);

            yield return new WaitForSeconds(1f);

            StartCoroutine(ShootEnemyBulletToMidoriPlaneCoroutine(bossEnemy,
                bossYuzuHalo.GetComponent<RectTransform>().anchoredPosition, "YuzuGrenade"));

            yield return new WaitForSeconds(_yuzuBulletDuration);

            bossYuzuHalo.GetComponent<Image>().DOFade(0, 1);

            // 플레이어가 날아오는 도중에 총알을 맞았을 경우 작동하지 않도록 함
            if (_yuzuGrenade.GetComponent<Image>().color != Color.clear)
            {
                AudioManager.Instance.PlaySFX("8bitBomb");
                _yuzuGrenade.SetActive(false);

                for (int i = -2; i < 3; ++i)
                {
                    for (int j = -2; j < 3; ++j)
                    {
                        if ((i >= -1 && i <= 1) && (j >= -1 && j <= 1)) continue;

                        // 각도를 균일하게 하기 위해 값을 크게 함
                        Vector2 targetPosition = new Vector2(
                            bossYuzuHalo.GetComponent<RectTransform>().anchoredPosition.x + (-10000 * i),
                            bossYuzuHalo.GetComponent<RectTransform>().anchoredPosition.y + (-10000 * j));

                        StartCoroutine(
                            ShootEnemyBulletToMidoriPlaneCoroutine(bossYuzuHalo, targetPosition, "YuzuBullet"));
                    }
                }
            }
        }
        else if (skillNum == 3)
        {
            AudioManager.Instance.PlaySFX("UseSkill");

            bossArisHalo.transform.SetParent(midoriPlane.transform);
            bossArisHalo.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            bossArisLaser.SetActive(true);
            StartCoroutine(bossArisLaser.GetComponent<LaserController>().ChasePlane());

            bossArisHalo.GetComponent<Image>().DOFade(1, 1);

            yield return new WaitForSeconds(1f);

            bossArisHalo.transform.SetParent(haloParent.transform);
            bossArisHalo.GetComponent<Image>().DOFade(0, 1);
            bossArisLaser.GetComponent<LaserController>().SetIsAvailableMoveLaser(false);

            yield return new WaitForSeconds(0.5f);

            AudioManager.Instance.PlaySFX("ArisSkill");
            StartCoroutine(bossArisLaser.GetComponent<LaserController>().ShootLaser());

            yield return new WaitForSeconds(1f);

            bossArisLaser.SetActive(false);
        }
    }

    private IEnumerator UseMidoriSkill()
    {
        _isActivatingSkill = true;

        int aliveEnemyCount = aliveEnemyParent.transform.childCount;
        int haloRepeat;

        List<float> distanceList = new List<float>();
        float distance;

        int[] minIndexArray = new int[5];
        GameObject[] midoriHaloArray = new GameObject[5];

        // 살아있는 적 개체들의 거리를 List에 저장
        for (int i = 0; i < aliveEnemyCount; ++i)
        {
            GameObject enemy = aliveEnemyParent.transform.GetChild(i).gameObject;

            distance = Vector2.Distance(enemy.GetComponent<RectTransform>().anchoredPosition,
                midoriPlane.GetComponent<RectTransform>().anchoredPosition);

            distanceList.Add(distance);
        }


        AudioManager.Instance.PlaySFX("UseSkill");
        // 가장 거리가 가까운 적 5명의 index를 저장
        for (int i = 0; i < 5; ++i)
        {
            minIndexArray[i] = distanceList.IndexOf(distanceList.Min());
            distanceList[minIndexArray[i]] =
                distanceList.Max() + 1; // 제일 가까운 적에게 최댓값 + 1 을 덧씌워서 2, 3번째로 가까운 적을 차례로 찾도록 함
        }

        haloRepeat = (distanceList.Count > 5) ? 5 : distanceList.Count;

        // 가장 거리가 가까운 적 최대 5명에게 헤일로를 씌움
        for (int i = 0; i < haloRepeat; ++i)
        {
            midoriHaloArray[i] = GetPrefab("MidoriHalo");
            midoriHaloArray[i].SetActive(true);
            midoriHaloArray[i].transform.SetParent(aliveEnemyParent.transform.GetChild(minIndexArray[i]));
            midoriHaloArray[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            midoriHaloArray[i].GetComponent<Image>().DOFade(1, 1);
        }

        yield return new WaitForSeconds(1);

        // 가장 거리가 가까운 적 5명에게 총알을 발사하며 씌워진 헤일로 없애기
        for (int i = 0; i < 5; ++i)
        {
            // 씌운 헤일로가 있을 때만 동작
            if (midoriHaloArray[i] != null)
            {
                midoriHaloArray[i].GetComponent<Image>().DOFade(0, 1);
                midoriHaloArray[i].transform.SetParent(aliveEnemyParent.transform.GetChild(minIndexArray[i]));
            }

            // 살아있을 때만 공격
            if (!IsAlivePlane) continue;

            StartCoroutine(ShootMidoriBullet(aliveEnemyParent.transform.GetChild(minIndexArray[i])
                .GetComponent<RectTransform>().anchoredPosition));

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(1);

        // 헤일로 반환
        for (int i = 0; i < haloRepeat; ++i)
        {
            midoriHaloArray[i].transform.SetParent(haloParent.transform);
            _midoriHaloQueue.Enqueue(midoriHaloArray[i]);
        }

        _isActivatingSkill = false;
    }

    #endregion Player&Enemy Attack

    public void SetIsShootingLaser(bool isShootingLaser)
    {
        IsShootingLaser = isShootingLaser;
    }
}