using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;

    private Rigidbody rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Evitar que el objeto rote al chocar con obstaculos
            rb.freezeRotation = true;
        }
    }

    private void Update()
    {
        float x = 0f;
        float y = 0f;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) y += 1f;
            if (Keyboard.current.sKey.isPressed) y -= 1f;
            if (Keyboard.current.aKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed) x += 1f;
        }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
        if (x == 0f && y == 0f)
        {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
        }
#endif

        moveInput = new Vector2(x, y).normalized;
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 targetVelocity = moveDirection * moveSpeed;

        // Mantener la velocidad en Y (gravedad)
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
    }
}
