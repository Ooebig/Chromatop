using UnityEngine;

public class CurrencyPickup : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    private bool collected;

    public ItemData ItemData => itemData;

    private void Awake()
    {
        if (itemData == null)
        {
            Debug.LogError(
                "CurrencyPickup needs an ItemData asset.",
                this
            );
        }
    }

    public void Collect(IPickup receiver)
    {
        if (collected ||
            receiver == null ||
            itemData == null)
        {
            return;
        }

        if (itemData.pocketType != PocketType.Currency)
        {
            Debug.LogError(
                $"{itemData.itemName} is not a Currency item.",
                this
            );

            return;
        }

        collected = true;

        receiver.AddCurrency(
            itemData.currencyValue
        );

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        IPickup receiver =
            other.GetComponentInParent<IPickup>();

        if (receiver != null)
        {
            Collect(receiver);
        }
    }
}