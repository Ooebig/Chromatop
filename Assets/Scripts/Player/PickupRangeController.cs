using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(SphereCollider))]
public class PickupRangeController : MonoBehaviour
{
    private PlayerStats playerStats;
    private SphereCollider rangeCollider;
    private IPickup pickupReceiver;

    private float previousRange = -1f;

    private void OnEnable()
    {
        FindReferences();

        if (Application.isPlaying &&
            playerStats != null)
        {
            playerStats.StatsChanged += UpdateRange;
        }

        UpdateRange();
    }

    private void OnDisable()
    {
        if (Application.isPlaying &&
            playerStats != null)
        {
            playerStats.StatsChanged -= UpdateRange;
        }
    }

    private void Update()
    {
        // Supports Inspector testing in and outside Play mode.
        UpdateRange();
    }

    private void FindReferences()
    {
        if (playerStats == null)
        {
            playerStats =
                GetComponentInParent<PlayerStats>();
        }

        if (rangeCollider == null)
        {
            rangeCollider =
                GetComponent<SphereCollider>();
        }

        if (pickupReceiver == null)
        {
            pickupReceiver =
                GetComponentInParent<IPickup>();
        }

        if (rangeCollider != null)
        {
            rangeCollider.isTrigger = true;
        }
    }

    private void UpdateRange()
    {
        FindReferences();

        if (playerStats == null ||
            rangeCollider == null)
        {
            return;
        }

        float newRange = playerStats.PickupRange;

        if (Mathf.Approximately(
                newRange,
                previousRange
            ))
        {
            return;
        }

        previousRange = newRange;
        rangeCollider.radius = newRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Application.isPlaying)
            return;

        CurrencyPickup currencyPickup =
            other.GetComponentInParent<CurrencyPickup>();

        if (currencyPickup != null &&
            pickupReceiver != null)
        {
            currencyPickup.Collect(
                pickupReceiver
            );
        }
    }
}