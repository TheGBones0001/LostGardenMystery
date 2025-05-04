using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool isMoving = false;
    public float moveSpeed = 5f;
    public LayerMask obstacleLayer; // Layer para obstáculos

    private Vector2 inputDirection;
    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (isMoving || inputDirection == Vector2.zero)
            return;

        Vector2 move;

        if (Mathf.Abs(inputDirection.x) > Mathf.Abs(inputDirection.y))
            move = new Vector2(Mathf.Sign(inputDirection.x), 0);
        else
            move = new Vector2(0, Mathf.Sign(inputDirection.y));

        RaycastHit2D hit = Physics2D.Raycast(transform.position, move, 1f, obstacleLayer);

        if (!hit.collider)
            MovePlayer(move);
        else
        {
            if (hit.collider.TryGetComponent<PushableBox>(out var pushable) && pushable.TryPush(move))
            {
                MovePlayer(move);
            }
        }

        inputDirection = Vector2.zero;
    }

    private System.Collections.IEnumerator MoveToPosition(Vector3 target)
    {
        isMoving = true;

        while ((target - transform.position).sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }

    private void MovePlayer(Vector2 movement)
    {
        targetPosition = transform.position + new Vector3(movement.x, movement.y, 0);
        StartCoroutine(MoveToPosition(targetPosition));
        StepCounterUI.Instance.IncrementSteps();
    }


    public void OnMove(InputValue value)
    {
        inputDirection = value.Get<Vector2>();
    }
}
