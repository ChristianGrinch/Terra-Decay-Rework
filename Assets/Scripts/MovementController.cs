using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Player data")]
    public int speed = 20;
    // ReSharper disable once MemberCanBePrivate.Global
    public static GameObject player = GameManager.Instance.player;
    public Rigidbody playerRb = player.GetComponent<Rigidbody>();

    private void FixedUpdate()
    {
        if(UIManager.Instance.ReturnCurrentMenu() == Menu.Game) MovePlayer();
    }
    private void MovePlayer()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
		
        Vector3 v3 = new(horizontalInput, 0.0f, verticalInput);
        playerRb.AddForce(speed * Time.deltaTime * v3.normalized, ForceMode.Impulse);
    }
}
