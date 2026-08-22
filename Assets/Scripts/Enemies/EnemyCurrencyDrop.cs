using UnityEngine;

public class EnemyCurrencyDrop : MonoBehaviour
{
    [SerializeField] private GameObject currencyItem;

    [Range(0f, 1f)]
    [SerializeField] private float dropChance = 1f;

    public void DropCurrency()
    {
        if (currencyItem == null ||
            currencyItem == null)
        {
            Debug.LogWarning(
                "Enemy currency drop is missing its item or world prefab.",
                this
            );

            return;
        }

        if (Random.value > dropChance)
            return;

        Instantiate(
            currencyItem,
            transform.position,
            Quaternion.identity
        );
    }
}