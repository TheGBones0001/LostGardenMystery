using UnityEngine;

public class PushableBox : MonoBehaviour
{
    public LayerMask obstacleLayer; // Layer para obstáculos
    public float moveSpeed = 5f;
    public BoxGroup boxGroup;

    private bool isMoving = false;
    private Vector2 targetPosition;

    private void Start()
    {
        targetPosition = transform.position;
    }

    public bool TryPush(Vector2 direction)
    {
        if (isMoving)
            return false;

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(obstacleLayer);
        filter.useTriggers = false;

        RaycastHit2D[] hits = new RaycastHit2D[5];
        int hitCount = Physics2D.Raycast(transform.position, direction, filter, hits, 1f);

        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider != null && hits[i].collider.gameObject != gameObject)
                return false;
        }

        targetPosition = transform.position + new Vector3(direction.x, direction.y, 0);
        StartCoroutine(MoveToPosition(targetPosition));

        return true;
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
}
