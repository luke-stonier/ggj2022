using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ItemManager : MonoBehaviour
{
    public IItem item;
    public bool enablePickup = false;
    private ICollector lastInvManager;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        item = ItemList.STACK_TEST_ITEM.newItem();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _renderer.color = enablePickup ? Color.green : Color.red;
        if (!enablePickup && lastInvManager != null) return;

        if (Input.GetKeyDown(KeyCode.F)) PickUp(lastInvManager);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        enablePickup = ManageCollision(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enablePickup = ManageCollision(collision);
    }

    private bool ManageCollision(Collider2D collision)
    {
        if (!collision || !collision.gameObject) return false;
        ICollector invManager = collision.gameObject.GetComponent<InventoryManager>();
        if (invManager == null) return false;
        lastInvManager = invManager;
        return true;
    }

    private void PickUp(ICollector invManager)
    {
        if (invManager == null) return;
        invManager.PickUp(item);
        Destroy(gameObject);
    }
}
