using UnityEngine;

public class UIUpgradeButton : MonoBehaviour
{
    [SerializeField] CircleCollider2D _collider;

    public bool HasTouchedUpgradeButton(Vector2 position)
    {
        Vector2 centerCircle = gameObject.transform.position;
        float distance = Vector2.Distance(centerCircle, position);
        return distance <= (_collider.radius / 2);
    }
}
