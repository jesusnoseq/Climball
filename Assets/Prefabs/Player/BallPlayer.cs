using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using com.jesusnoseq.util;

public class BallPlayer : MonoBehaviourSingleton<BallPlayer>
{
    private float maxPowerIndicatorSize=2.5f;
    private Vector3 powerOffset=Vector3.right;
    public LineRenderer powerBar;
    public GameObject powerIndicator;
    public SpriteRenderer playerSpriteRenderer;

    public SoundManager sm;
    public AudioClip jumpSound;
    public AudioClip hitSound;
    private PlayerInput pi;

    private bool isShooting;

    private float maxPower=100;
    private float powerIncrement=90;
    private float power;
    private bool executed;
    private Rigidbody2D rb;
    private bool isMoving;
    private Vector3 oldPosition;
    private int moveChecks;
    private int moveChecksNeeded=3;
    private float forceMultiplier=0.16f;

    void Awake() {
        pi=new PlayerInput();
    }

    void OnEnable() {
        pi.Enable();
    }

    void OnDisable() {
        pi.Disable();
    }
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        oldPosition=transform.position;
    }

    void Update()
    {
        if (isMoving){
            return;
        }
        
        Vector2 cursorScreen=pi.PlayerMap.Move.ReadValue<Vector2>();
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreen);
        Vector3 relativePos = cursorPosition - powerIndicator.transform.position;
        powerIndicator.transform.rotation = Quaternion.LookRotation(Vector3.forward, relativePos);
        Vector2 shootDirection = powerIndicator.transform.up;

        float shoot=pi.PlayerMap.Shoot.ReadValue<float>();
        
        if (pi.PlayerMap.Shoot.WasPressedThisFrame() && pi.PlayerMap.Shoot.IsPressed()){
            isShooting=true;
            powerBar.enabled=true;
            power=0;
        }

        if(isShooting){
            power+=powerIncrement*Time.deltaTime;
            Vector3 start = transform.position;
            Vector3 end = start+new Vector3(maxPowerIndicatorSize,0,0);
            Vector3 current = Vector3.Lerp(start, end, power / maxPower);
            powerBar.SetPosition(0, start-transform.position+powerOffset);
            powerBar.SetPosition(1, current-transform.position+powerOffset);
            if (power>=maxPower){
                Shoot(shootDirection, maxPower);
            }
        }
            
        if (pi.PlayerMap.Shoot.WasReleasedThisFrame() && isShooting){
            Shoot(shootDirection, power);
        }
    }


    private void FixedUpdate() {
        Vector3 newPos =  transform.position;
        if(newPos==oldPosition){
            moveChecks++;
            if(moveChecks>=moveChecksNeeded){
                playerSpriteRenderer.color=Color.red;
                moveChecks=0;
                isMoving=false;
                powerIndicator.SetActive(true);
            }
        }else{
            playerSpriteRenderer.color=Color.blue;
            powerIndicator.SetActive(false);
            moveChecks=0;
        }
        oldPosition=newPos;
    }

    private void Shoot(Vector2 dir, float power){
        rb.AddForce(dir * power * forceMultiplier, ForceMode2D.Impulse);
        power=0;
        isShooting=false;
        powerBar.enabled=false;
        isMoving=true;
        sm.PlayRandomizeSfx(jumpSound);
    }


    public bool IsMoving(){
        return isMoving;
    }


    void OnCollisionEnter2D(Collision2D collision){
        sm.PlayRandomizeSfx(hitSound);
    }
}
