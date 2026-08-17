using UnityEngine;

public class CurrencyPickup : MonoBehaviour
{
    [SerializeField] private int currencyAmount = 10;

    private bool collected;

    private void OnTriggerEnter(Collider other)
    {
        IPickup receiver =
            other.GetComponentInParent<IPickup>();

        if (receiver != null)
        {
            Collect(receiver);
        }
    }

    public void Collect(IPickup receiver)
    {
        if (collected || receiver == null)
            return;

        collected = true;

        receiver.AddCurrency(currencyAmount);
        Destroy(gameObject);
    }
}