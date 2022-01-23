using UnityEngine;

public class InventoryManager : MonoBehaviour, ICollector
{
    private IInventory _inventory;
    public IInventory inventory {
        get {
            if (_inventory == null)
                _inventory = new Inventory(0);

            return _inventory;
        }
    }
    public InventoryRenderItem[] inventoryItems;
    
    [Header("UI")]
    public Vector2 itemFontOffset;
    public int itemSpriteSize;
    public int inventoryBoxSize;
    public int inventoryBoxPadding = 5;
    public Font inventoryFont;
    public Texture2D inventoryBox;
    private GUIStyle textStyle = new GUIStyle();

    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };

    // Inventory handling

    public void PickUp(IItem item)
    {
        print($"PICK UP {item.entityResourceName} with count {item.Count}");
        EntityService.Instantiate(inventory.AddItem(item));
    }

    // Unity handling

    private void Awake()
    {
        textStyle.font = inventoryFont;

        inventory.AddItem(ItemList.STACK_TEST_ITEM);
    }

    private void Update() {
        inventoryItems = inventory.Items();

        for (var i = 0; i < keyCodes.Length; i++)
            if (Input.GetKeyDown(keyCodes[i])) inventory.SetSelectedItem(i);

        // Keybinds for inv interaction
        if (Input.GetKeyDown(KeyCode.Q))
        {
            var itemToDrop = inventory.DropItem();
            print($"DROP {itemToDrop.entityResourceName} with count {itemToDrop.Count}");
            EntityService.Instantiate(itemToDrop, transform.position);
        }
    }

    private void OnApplicationQuit()
    {
        if (inventory != null) inventory.SaveInventory(0);
    }

    private void OnGUI()
    {
        DrawInventory();
    }

    private void DrawInventory()
    {
        if (inventoryItems == null) return;
        var i = 0;
        var xStartOffset = (inventoryItems.Length * inventoryBoxSize) + (inventoryItems.Length * inventoryBoxPadding);
        xStartOffset = (Screen.width / 2) - (xStartOffset / 2);
        foreach (InventoryRenderItem item in inventoryItems)
        {
            var x = xStartOffset + ((inventoryBoxSize * i) + (inventoryBoxPadding * i));
            var y = Screen.height - inventoryBoxSize - inventoryBoxPadding;
            var boxPos = new Rect(x, y, inventoryBoxSize, inventoryBoxSize);

            var itemPos = new Rect(x + ((inventoryBoxSize - itemSpriteSize) / 2),
                y + ((inventoryBoxSize - itemSpriteSize) / 2),
                itemSpriteSize, itemSpriteSize);

            var itemCountPos = new Rect(x + itemFontOffset.x,
                y + itemFontOffset.y,
                itemSpriteSize, itemSpriteSize);

            GUI.DrawTexture(boxPos, inventoryBox, ScaleMode.ScaleToFit, true, 0, (item.selected ? Color.yellow : Color.white), 0, 0);
            if (item.item != null)
            {
                GUI.DrawTexture(itemPos, item.item?.Sprite, ScaleMode.ScaleToFit);
                if (item.item.Count > 1)
                {
                    textStyle.normal.textColor = Color.white;
                    textStyle.fontSize = 30;
                    textStyle.fontStyle = FontStyle.Normal;
                    DrawOutline(itemCountPos, item.item.Count.ToString(), 1, textStyle, Color.grey, Color.white);
                }
            }
            i++;
        }
    }

    void DrawOutline(Rect r, string t, int strength, GUIStyle style, Color outlineColour, Color fontColour)
    {
        GUI.color = outlineColour;
        int i;
        for (i = -strength; i <= strength; i++)
        {
            GUI.Label(new Rect(r.x - strength, r.y + i, r.width, r.height), t, style);
            GUI.Label(new Rect(r.x + strength, r.y + i, r.width, r.height), t, style);
        }
        for (i = -strength + 1; i <= strength - 1; i++)
        {
            GUI.Label(new Rect(r.x + i, r.y - strength, r.width, r.height), t, style);
            GUI.Label(new Rect(r.x + i, r.y + strength, r.width, r.height), t, style);
        }
        GUI.color = fontColour;
        GUI.Label(r, t, style);
    }
}
