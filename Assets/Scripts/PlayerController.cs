using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public Rigidbody playerRb;
    public GameObject Player;
    public GameObject Attractor;
    public bool gameStart = false;
    public float groundMoveSpeed = 10f;
    public float downwardForce = 10f;
    public float cooldownTime = 0.2f;

    private Vector3 targetPosition;
    private bool isOnGround;
    private int row = 2;
    private int moveLength = 4;
    private float nextActionTimeA = 0f;
    private float nextActionTimeD = 0f;
    private float speed = 12f;
    private float jumpForce = 6f;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        gameStart = true; // Change later
        isOnGround = true;
        targetPosition = Player.transform.position;
    }

    void Update()
    {
        if (gameStart)
        {
            HandleMovement();
            HandleJump();
            SmoothPositionUpdate();
            HandleDownwardForce();
        }
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.A) && (Time.time >= nextActionTimeA) && (row == 2 || row == 3))
        {
            row -= 1;
            moveLeft();
            nextActionTimeA = Time.time + cooldownTime;
        }
        if (Input.GetKey(KeyCode.D) && (Time.time >= nextActionTimeD) && (row == 1 || row == 2))
        {
            row += 1;
            moveRight();
            nextActionTimeD = Time.time + cooldownTime;
        }
    }

    private void HandleJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && isOnGround)
        {
            jump();
        }
    }

    private void SmoothPositionUpdate()
    {
        Vector3 smoothPosition = Player.transform.position;
        smoothPosition.x = Mathf.Lerp(Player.transform.position.x, targetPosition.x, speed * Time.deltaTime);
        Player.transform.position = smoothPosition;
        Player.transform.Translate(Vector3.forward * Time.deltaTime * speed);
        Attractor.transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void moveLeft()
    {
        Attractor.transform.Translate(Vector3.left * moveLength);
        targetPosition = Attractor.transform.position;
    }

    public void moveRight()
    {
        Attractor.transform.Translate(Vector3.right * moveLength);
        targetPosition = Attractor.transform.position;
    }

    public void jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
    }

    public void MoveToGround()
    {
        if (Physics.Raycast(Player.transform.position, Vector3.down, out RaycastHit hit))
        {
            Vector3 newPosition = new Vector3(
                Player.transform.position.x,
                hit.point.y,
                Player.transform.position.z
            );
            Player.transform.position = Vector3.Lerp(Player.transform.position, newPosition, groundMoveSpeed * Time.deltaTime);
        }
    }

    private void HandleDownwardForce()
    {
        if (!isOnGround)
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                playerRb.AddForce(Vector3.down * downwardForce, ForceMode.Force);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }
    }
}