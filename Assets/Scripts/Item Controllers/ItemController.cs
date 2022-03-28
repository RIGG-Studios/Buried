using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemController : MonoBehaviour
{
    public Vector3 startingPosition;
    public Vector3 startingRotation;
    public ItemProperties properties;
    protected Player player;

    private UIElementGroup ammoNeededGroup = null;
    private UIElement ammoNeededText = null;

    public virtual void SetupController(Player player, ItemProperties properties)
    {
        ammoNeededGroup = CanvasManager.instance.FindElementGroupByID("AmmoNeeded");
        ammoNeededText = ammoNeededGroup.FindElement("text");

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        this.player = player;
        this.properties = properties;
    }

    public virtual void UseItem() { }
    public virtual void ResetItem() { }
    public virtual void ActivateItem() { }

    public IEnumerator ShowAmmoNeededUI(string text)
    {
        ammoNeededGroup.UpdateElements(0, 0, true);
        ammoNeededText.OverrideValue(text);
        yield return new WaitForSeconds(0.25f);
        ammoNeededText.OverrideValue(string.Empty);
        ammoNeededGroup.UpdateElements(0, 0, false);
    }
}
