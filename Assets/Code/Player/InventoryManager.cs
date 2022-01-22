using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public IInventory inventory;
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

    private void Awake()
    {
        inventory = new Inventory(0);   // default inventory for player is 0
        textStyle.font = inventoryFont;
    }

    private void Update() {
        if (inventory != null) inventoryItems = inventory.Items();

        if (Input.GetKeyDown(KeyCode.M))
            inventory.AddItem(ItemList.STACK_TEST_ITEM.newItem());

        if (Input.GetKeyDown(KeyCode.N))
            inventory.AddItem(ItemList.NO_STACK_TEST_ITEM.newItem());

        for (var i = 0; i < keyCodes.Length; i++)
            if (Input.GetKeyDown(keyCodes[i])) inventory.SetSelectedItem(i);
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
                GUI.DrawTexture(itemPos, item.item?.sprite(), ScaleMode.ScaleToFit);
                if (item.item.count() > 1)
                {
                    textStyle.normal.textColor = Color.white;
                    textStyle.fontSize = 30;
                    textStyle.fontStyle = FontStyle.Normal;
                    DrawOutline(itemCountPos, item.item.count().ToString(), 1, textStyle, Color.grey, Color.white);
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
