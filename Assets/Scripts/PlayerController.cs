using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool IsAlive
    {
        private set { }
        get { return livesLeft > 0f; }
    }

    [SerializeField] bool activeOnStart;
    [SerializeField] CharacterController controller;
    [SerializeField] Animator anim;
    [SerializeField] Transform cam;
    [SerializeField] int enemyLayer = 8;
    [SerializeField] int maxLives = 3;
    [SerializeField] float explosionRadius = 3;
    [SerializeField] float explosionForce = 1000f;
    [SerializeField] ForceFieldController forceFieldPrefab;
    [SerializeField] Transform forcieFieldSpawn;
    [SerializeField] float forceFieldSpeed = 6;
    [SerializeField] float forceFieldLifetime = 5;
    [SerializeField] float movementThreshold = 0.1f;
    [SerializeField] float speed;
    [SerializeField] float smoothSpeedTime;
    [SerializeField] float turnSmoothTime;
    [SerializeField] GameObject arcAttack;
    [SerializeField] Transform arcSpawnPosition;
    [SerializeField] float waitTillInstantiateArc;
    [SerializeField] float arcSpeed = 10f;
    [SerializeField] float arcExplosionRadius = 3f;
    [SerializeField] float arcLifetime = 3f;
    [SerializeField] public float arcCastTime = 2f;
    [SerializeField] float arcCastMin = 0.2f;
    [SerializeField] GameObject hammerAttack;
    [SerializeField] Transform hammerSpawnPosition;
    [SerializeField] float hammerDamage = 10f;
    [SerializeField] float waitTillInstantiateHammer;
    [SerializeField] float arcMovementBlockedTime;
    [SerializeField] int lightningAmount = 100;
    [SerializeField] float lightningDistance = 0.1f;
    [SerializeField] float lightningNextTime = 0.01f;
    [SerializeField] public float hammerCastTime = 2f;
    [SerializeField] float hammerCastMin = 0.2f;
    [SerializeField] float hammerMovementBlockedTime = 1f;
    public int livesLeft = 0;
    public int score = 0;
    bool isActive = false;
    float turnSmoothVelocity;
    Vector3 lastMoveDir;
    bool movementBlocked = false;
    bool isHammerCasting = false;
    bool isArcCasting = false;
    public float actualHammerCastTime = 0f;
    public float actualArcCastTime = 0f;

    void Start()
    {
        isActive = activeOnStart;
        livesLeft = maxLives;
    }

    public void StartGame()
    {
        isActive = true;
    }

    void Update()
    {
        if (!isActive || !IsAlive) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        bool castHammerInput = Input.GetMouseButton(1);
        bool castArcInput = Input.GetMouseButton(0);

        anim.SetFloat("hammerCastTime", actualHammerCastTime);
        anim.SetFloat("arcCastTime", actualArcCastTime);
        

        Vector3 dir = new Vector3(horizontal, 0f, vertical);

        if (castHammerInput)
        {
            HammerCast();
            return;
        }
        else if (isHammerCasting)
        {
            HammerAttack();
            return;
        }

        if (castArcInput)
        {
            ArcCast();
            return;
        }
        else if (isArcCasting)
        {
            ArcAttack();
            return;
        }

        if (movementBlocked) { dir = Vector3.zero; }
        Move(dir);

        controller.Move(Physics.gravity * Time.deltaTime);


    }

    public void AddScore(int _amount)
    {
        if (!IsAlive) return;

        score += _amount;
    }

    public void Move(Vector3 _dir)
    {
        Vector3 desiredMoveDir = _dir;

        if (_dir.magnitude >= movementThreshold)
        {
            float targetAngle = Mathf.Atan2(_dir.x, _dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            desiredMoveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        }
        desiredMoveDir = Vector3.Lerp(lastMoveDir, desiredMoveDir, smoothSpeedTime);

        controller.Move(transform.forward * desiredMoveDir.magnitude * speed * Time.deltaTime);
        anim.SetFloat("velocity", desiredMoveDir.magnitude * speed);

        lastMoveDir = desiredMoveDir;
    }

    public void Rotate()
    {
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    public void GetHit()
    {
        if (!IsAlive) return;

        livesLeft--;

        if (livesLeft <= 0)
        {
            GameManager.instance.EndGame();
            anim.SetTrigger("die");
            return;
        }

        if(forceFieldPrefab) {
            ForceFieldController force = Instantiate(forceFieldPrefab, forcieFieldSpawn.position, Quaternion.identity);
            force.Init(forceFieldSpeed, forceFieldLifetime);
        }


    }
    public void HammerCast()
    {
        movementBlocked = true;
        isHammerCasting = true;
        actualHammerCastTime += Time.deltaTime;


        if (actualHammerCastTime > hammerCastTime)
            actualHammerCastTime = hammerCastTime;

        Rotate();
    }

    public void HammerAttack()
    {
        isHammerCasting = false;

        if (actualHammerCastTime > hammerCastMin)
        {
            float multiplier = actualHammerCastTime / hammerCastTime;
            anim.SetTrigger("hammerAttack");
            StartCoroutine(WaitTillInstantiateHammerAttack(multiplier, waitTillInstantiateHammer));
            StartCoroutine(ResetMovementBlockedIn(hammerMovementBlockedTime));
        }
        else
        {
            movementBlocked = false;
            anim.SetTrigger("hammerCanceled");
        }

        actualHammerCastTime = 0f;
    }

    public void ArcCast()
    {
        movementBlocked = true;
        isArcCasting = true;
        actualArcCastTime += Time.deltaTime;


        if (actualArcCastTime > arcCastTime)
            actualArcCastTime = arcCastTime;

        Rotate();
    }

    public void ArcAttack()
    {
        isArcCasting = false;

        if (actualArcCastTime > arcCastMin)
        {
            float multiplier = actualArcCastTime / arcCastTime;
            anim.SetTrigger("arcAttack");
            StartCoroutine(WaitTillInstantiateArcAttack(multiplier, waitTillInstantiateArc));
            StartCoroutine(ResetMovementBlockedIn(arcMovementBlockedTime));
        }
        else
        {
            movementBlocked = false;
            anim.SetTrigger("arcCanceled");
        }

        actualArcCastTime = 0f;
    }

    IEnumerator ResetMovementBlockedIn(float _time)
    {
        yield return new WaitForSeconds(_time);
        movementBlocked = false;
    }

    IEnumerator WaitTillInstantiateArcAttack(float _multiplier = 1f, float _time = 0.1f)
    {
        yield return new WaitForSeconds(_time);
        ArcController attack = Instantiate(arcAttack, arcSpawnPosition.position, transform.rotation).GetComponent<ArcController>();
        attack.Init(arcSpeed, arcLifetime * _multiplier, arcExplosionRadius);
    }

    IEnumerator WaitTillInstantiateHammerAttack(float _multiplier = 1f, float _time = 0.1f)
    {
        yield return new WaitForSeconds(_time);
        LightningController attack = Instantiate(hammerAttack, hammerSpawnPosition.position, transform.rotation).GetComponent<LightningController>();
        attack.Init(hammerDamage, Mathf.FloorToInt(lightningAmount * _multiplier), lightningDistance, lightningNextTime);
    }
}
