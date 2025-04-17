using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropItemPanel : MonoBehaviour
{
    public Slider Slider;
    public TMP_Text DropButtonText;
    public ItemSlot SelectedItemSlot;
    public Button DropButton;

    public event Action<ItemSlot, int> DropEvent; // Event to be invoked when the drop button is clicked

    /// <summary>
    /// The maximum number of items that can be dropped at once
    /// </summary>
    public int maxDropItems;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UiController.SubmitEvent += OnSubmit;
        UiController.CancelEvent += OnCancel;

        Slider.maxValue = maxDropItems;
        Slider.value = maxDropItems; // Set the default value to the maximum
        DropButton.onClick.AddListener(OnSubmit);
        Slider.onValueChanged.AddListener(OnSliderValueChanged);

        // Customize navigation settings
        CustomizeNavigation();

        // Start coroutine to set the selected GameObject
        StartCoroutine(SetSelectedGameObjectNextFrame());
    }

    private void OnDestroy()
    {
        DropButton.onClick.RemoveListener(OnSubmit);
        Slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        UiController.SubmitEvent -= OnSubmit;
        UiController.CancelEvent -= OnCancel;
    }

    private IEnumerator SetSelectedGameObjectNextFrame()
    {
        // Wait until the next frame
        yield return null;

        // Set the slider as the selected GameObject
        EventSystem.current.SetSelectedGameObject(Slider.gameObject);
    }

    public void OnSliderValueChanged(float value)
    {
        // Update the drop button text to show the number of items to drop
        DropButtonText.text = "Drop " + (int)value;
    }

    private void OnSubmit()
    {
        // Invoke the drop event with the selected item slot and quantity
        DropEvent?.Invoke(SelectedItemSlot, (int)Slider.value);
    }

    private void OnCancel()
    {
        Destroy(gameObject);
    }

    private void CustomizeNavigation()
    {
        // Set navigation mode to Explicit for the slider
        var sliderNav = new Navigation
        {
            mode = Navigation.Mode.Explicit,
            selectOnUp = DropButton,
            selectOnDown = DropButton
        };
        Slider.navigation = sliderNav;

        // Set navigation mode to Explicit for the drop button
        var buttonNav = new Navigation
        {
            mode = Navigation.Mode.Explicit,
            selectOnUp = Slider,
            selectOnDown = Slider
        };
        DropButton.navigation = buttonNav;
    }
}
