using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody rb;

    private Vector3 moveInput;
    private Vector3 moveVelocity;

    private Camera mainCamera;

    public float movementSpeed = 5f;

    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    private bool isDashing = false;
    public GameObject[] dashBlocks;
    [SerializeField] private int dashCount = 3;
    [SerializeField] private int dashMaximum = 3;
    private float dashRefreshTimer;

    public WeaponType weapon;

    public GunController gun;
    public ShotgunController shotgun;
    public SMGController smg;
    public KnifeAnimator melee;
    public WeaponManager weaponManager;
    private Gamemanager _gameManager;

    public Animator _upperBodyPlayerAnimator;
    public Animator _lowerBodyPlayerAnimator;
    public bool shoot = false;

    void Start() {
        rb = GetComponent<Rigidbody>();
        weaponManager = GetComponent<WeaponManager>();
        mainCamera = FindObjectOfType<Camera>();
        _gameManager = FindObjectOfType<Gamemanager>();

        weapon = weaponManager.currentWeapon;

        for (int i = 0; i < dashBlocks.Length; i++) {
            dashBlocks[i] = GameObject.Find("DashBlock " + (i + 1));
            
            if (dashBlocks[i] == null) {
                Debug.LogError("Could not find DashBlock " + (i + 1));
            }
        }
    }

    void Update() {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput * movementSpeed;
        RunAnimationState();

        // check if the player has any dashes left and is not currently dashing
        if (Input.GetKey(KeyCode.LeftShift) && dashCount > 0 && !isDashing) {
            dashCount--;
            if (dashCount >= 0 && dashCount < dashBlocks.Length) {
                dashBlocks[dashCount].SetActive(false);
            }
            StartCoroutine(Dash());
        }

        // increase the dashCount after 3 second
        if (dashCount < dashMaximum) {
            dashRefreshTimer += Time.deltaTime;
        }

        if (dashRefreshTimer >= 3 && dashCount < dashMaximum) {
            if (dashCount >= 0 && dashCount <= dashBlocks.Length) {
                dashBlocks[dashCount].SetActive(true);
                dashCount++;
            }
            dashRefreshTimer = 0;
        }

        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength)) {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        weapon = weaponManager.currentWeapon;


        if (weapon == WeaponType.Pistol) {
            if (Input.GetMouseButtonDown(0)) {
                gun.isFiring = true;
                _upperBodyPlayerAnimator.SetBool("shoot", true);
            }

            if (Input.GetMouseButtonUp(0)) {
                gun.isFiring = false;
                _upperBodyPlayerAnimator.SetBool("shoot", false);
            }
        }

        if (weapon == WeaponType.Shotgun) {
            if (Input.GetMouseButtonDown(0)) {
                shotgun.isFiring = true;
            }

            if (Input.GetMouseButtonUp(0)) {
                shotgun.isFiring = false;
            }
        }

        if (weapon == WeaponType.SMG) {
            if (Input.GetMouseButtonDown(0)) { 
                smg.isFiring = true;
            }

            if (Input.GetMouseButtonUp(0)) {
                smg.isFiring = false;
            }
        }

        if (weapon == WeaponType.Knife) {
            if (Input.GetMouseButtonDown(0)) {
                melee.isAttacking = true;
                Debug.Log("melee");
            }

            if (Input.GetMouseButtonUp(0)) {
                melee.isAttacking = false;
            }
        }

        if (Input.GetKey(KeyCode.O) && Input.GetKey(KeyCode.P)) {
            _gameManager.InfinityModeActive = true;
        }
    }

    void FixedUpdate() {
        if (!isDashing) {
            rb.velocity = moveVelocity;
        }
    }

    IEnumerator Dash() {
        isDashing = true;

        // Disable the collider during the dash
        //Collider collider = GetComponent<Collider>();
        //collider.enabled = false;

        // Save current velocity
        Vector3 originalVelocity = rb.velocity;

        // Apply dash velocity for a limited time
        rb.velocity = moveInput.normalized * dashSpeed;
        yield return new WaitForSeconds(dashDuration);

        // Re-enable the collider after the dash is finished
        //collider.enabled = true;

        // Revert back to original velocity
        rb.velocity = originalVelocity;
        isDashing = false;
    }

    private void RunAnimationState()
    {
        if (moveInput.z != 0 || moveInput.x != 0)
        {
            //run
            _lowerBodyPlayerAnimator.SetBool("run", true);
        }
        else
        {
            //idle
            _lowerBodyPlayerAnimator.SetBool("run", false);
        }


    }
}
