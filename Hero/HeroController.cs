using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour, ITargetCombat
{
    [Header("Power Up")]
    [SerializeField] private PowerUpId currentPowerUp;
    [SerializeField] private int powerUpAmount;
    [SerializeField] SpellLauncherController bluePotionLauncher;
    [SerializeField] SpellLauncherController redPotionLauncher;

    [Header("Health Variables")]
    [SerializeField] int health=10;
    [SerializeField] DamageFeedbackEffect damageFeedbackEffect;

    [Header("Attack Variables")]
    [SerializeField] SwordController swordController;

    [Header("Animation Variables")]
    [SerializeField] AnimatorController animatorController;

    [Header("Checker Variables")]
    [SerializeField] LayerChecker footA;
    [SerializeField] LayerChecker footB;
    [Header("Rigid Variables")]
    [SerializeField] private float damageForce;
    [SerializeField] private float damageForceUp;

    [Header("Boolean Variables")]
    public bool playerIsAttacking;
    public bool playerIsUsingPowerUp;
    public bool playerIsRecovering;
    public bool canDoubleJump;
    public bool isLookingRight;

    [Header("Interruption Variables")]
    public bool canCheckGround;
    public bool canMove;
    public bool canFlip;

    [SerializeField] private float jumpForce;
    [SerializeField] private float doublejumpForce;
    [SerializeField] private float speed;

    [Header("Audio")]
    [SerializeField] AudioClip attackSfx;

//Control variables
    [SerializeField] private Vector2 movementDirection;
    private bool jumpPressed = false;
    private bool attackPressed = false;
    private bool usePowerUpPressed = false;
    private int coins;

    private bool playerIsOnGround;

    private Rigidbody2D rigidbody2D;
    
    public static HeroController instance;

    private void Awake(){
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }else{
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        canCheckGround = true;
        canMove = true;
        rigidbody2D = GetComponent<Rigidbody2D>();
        animatorController.Play(AnimationId.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        HandleIsGrounding();
        HandleControls();
        HandleMovements();
        HandleFlip();
        HandleJump();
        HandleAttack();
        HandleUsePowerUp();
    }

    public void GiveHealthPoint(){
        health=Mathf.Clamp(health + 1, 0, 10);
    }

    public void GiveCoin(){
        coins=Mathf.Clamp(coins + 1, 0, 10000000);
    }

    public void ChangePowerUp(PowerUpId powerUpId, int amount){
        currentPowerUp = powerUpId;
        powerUpAmount = amount;
        Debug.Log(currentPowerUp);
    }

    void HandleControls() {
        movementDirection=new Vector2( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        jumpPressed = Input.GetButtonDown("Jump");
        attackPressed = Input.GetButtonDown("Attack");
        usePowerUpPressed = Input.GetButtonDown("UsePowerUp");

    }
    void HandleIsGrounding(){
        if(!canCheckGround) return;
        playerIsOnGround=footA.isTouching || footB.isTouching;

    }
    void HandleMovements() {
        if(!canMove) return;
        rigidbody2D.velocity = new Vector2(movementDirection.x* speed, rigidbody2D.velocity.y);

        if (playerIsOnGround){ 

            if (Mathf.Abs(rigidbody2D.velocity.x) > 0)
            {
                animatorController.Play(AnimationId.Run);
            }
            else
            {
                animatorController.Play(AnimationId.Idle);
            }
        }
    }
    void HandleFlip() {
        if (!canFlip) return;

        if (rigidbody2D.velocity.magnitude > 0 ){ 
            animatorController.Play(AnimationId.Run);
            
            if (rigidbody2D.velocity.x >= 0){
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                isLookingRight = true;
            }
            else{
                this.transform.rotation = Quaternion.Euler(0, 180, 0);
                isLookingRight = false;
            }


        }
    }
    void HandleJump(){
        if(canDoubleJump && jumpPressed&&!playerIsOnGround){
           this.rigidbody2D.velocity=Vector2.zero;
           this.rigidbody2D.AddForce(Vector2.up*doublejumpForce, ForceMode2D.Impulse);
           canDoubleJump = false;
        }

        if(jumpPressed && playerIsOnGround){
           this.rigidbody2D.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
           StartCoroutine(HandleJumpAnimation());
           canDoubleJump=true;
        }
    }
    void HandleAttack(){
         if (attackPressed&&!playerIsAttacking){
            if (playerIsOnGround) { 
                rigidbody2D.velocity = Vector2.zero;
            }
            AudioManager.instance.PlaySfx(attackSfx);
            animatorController.Play(AnimationId.Attack);
            playerIsAttacking =  true;
            swordController.Attack(0.1f,0.31f);
            StartCoroutine(RestoreAttack());
         }
    }

    IEnumerator RestoreAttack(){
        if(playerIsOnGround)
           canMove = false;
        yield return new WaitForSeconds(0.3f);
        playerIsAttacking = false;
        if(!playerIsOnGround)
           animatorController.Play(AnimationId.Jump);
        canMove = true;
    }
    //
    void HandleUsePowerUp(){
         if (attackPressed&&!playerIsUsingPowerUp && currentPowerUp!=PowerUpId.Nothing){
            if (playerIsOnGround) { 
                rigidbody2D.velocity = Vector2.zero;
            }
            AudioManager.instance.PlaySfx(attackSfx);
            animatorController.Play(AnimationId.UsePowerUp);
            playerIsUsingPowerUp =  true;

            //swordController.Attack(0.1f,0.31f);

            if(currentPowerUp==PowerUpId.BluePotion){ 
                bluePotionLauncher.Launch((Vector2)transform.right + Vector2.up*0.3f);
            }
            if(currentPowerUp==PowerUpId.RedPotion){ 
                redPotionLauncher.Launch(transform.right);
            }
            

            StartCoroutine(RestoreUsePowerUp());

            powerUpAmount--;

            if(powerUpAmount <= 0){
               currentPowerUp = PowerUpId.Nothing;
            }
         }
    }

    IEnumerator RestoreUsePowerUp(){
        if(playerIsOnGround)
           canMove = false;
        yield return new WaitForSeconds(0.3f);
        playerIsUsingPowerUp = false;
        if(!playerIsOnGround)
           animatorController.Play(AnimationId.Jump);
        canMove = true;
    }

    //

    IEnumerator HandleJumpAnimation(){
        canCheckGround = false;
        playerIsOnGround = false;
       if(!playerIsAttacking)
            animatorController.Play(AnimationId.PrepareJump);
        yield return new WaitForSeconds(0.3f);
        if(!playerIsAttacking)
           animatorController.Play(AnimationId.Jump);
        canCheckGround = true;
    }

    public void TakeDamage(int damagePoints){
        if (!playerIsRecovering){ 
            health=Mathf.Clamp(health - damagePoints, 0, 10);
            StartCoroutine(StartPlayerRecover());
            if (isLookingRight){
                rigidbody2D.AddForce(Vector2.left* damageForce + Vector2.up* damageForceUp, ForceMode2D.Impulse);

            }else{
                rigidbody2D.AddForce(Vector2.right* damageForce + Vector2.up* damageForceUp, ForceMode2D.Impulse);
            }
        }

    }

    IEnumerator StartPlayerRecover(){
        canMove = false;
        canFlip = true;
        animatorController.Play(AnimationId.Hurt);
        yield return new WaitForSeconds(0.2f);
        canMove = true;
        canFlip = true;
        rigidbody2D.velocity = Vector2.zero;

        playerIsRecovering = true;
        damageFeedbackEffect.PlayBlinkDamageEffect();
        yield return new WaitForSeconds(2);
        damageFeedbackEffect.StopBlinkDamageEffect();
        playerIsRecovering = false;
    }
}
    
