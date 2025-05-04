using UnityEngine;

public class TargetButton : MonoBehaviour
{
    public BoxGroup acceptedGroup;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null)
            return;

        if (!other.TryGetComponent<PushableBox>(out var box))
            return;

        if (box.boxGroup == acceptedGroup)
            GameManager.Instance.ExecuteAction(this);
        else
            GameManager.Instance.Restart();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == null)
            return;

        if (!other.TryGetComponent<PushableBox>(out var box))
            return;

        GameManager.Instance.CancelAction(this);
    }
}
