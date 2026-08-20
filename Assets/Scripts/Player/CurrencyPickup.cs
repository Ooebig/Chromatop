using UnityEngine;

public class CurrencyPickup : MonoBehaviour
{
    [SerializeField] private int currencyValue = 1;

    public void Collect()
    {
        gameManager.instance.inventory.AddCurrency(currencyValue);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        IPickup receiver =
            other.GetComponentInParent<IPickup>();

        if (receiver != null)
        {
            Collect();
        }
    }
}