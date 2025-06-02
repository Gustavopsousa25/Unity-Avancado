using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BeatInputFeedback : MonoBehaviour
{
    [SerializeField] private InputActionReference mouseInput;
    [SerializeField] private Image targetImage;
    [SerializeField] private float flashDurantion;

    private RythmManager manager;
    private float flashTimer = 0f;
    private bool isFlasing = false;
    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<RythmManager>();
        originalColor = targetImage.color;
        mouseInput.action.performed += OnInputperformed;
    }
    private void OnEnable()
    {
        if (mouseInput != null)
            mouseInput.action.Enable();
            mouseInput.action.performed += OnInputperformed;
    }
    private void OnDisable()
    {
        if (mouseInput != null)
            mouseInput.action.performed -= OnInputperformed;
            mouseInput.action.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        if (isFlasing)
        {
            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0f)
            {
                // Revert to original color and stop flashing
                if (targetImage != null)
                    targetImage.color = originalColor;
                    isFlasing = false;
            }
        }
    }
    public void OnInputperformed(InputAction.CallbackContext context)
    {
        Debug.Log("[BeatInput] Input performed at time " + Time.time);
        if (manager == null || targetImage == null)
            return;

        // 1) Check if we’re on the beat right now
        bool onBeat = manager.IsOnBeat();
        Debug.Log($"[BeatInput] IsOnBeat() returned {onBeat}");


        // 2) Tint the image green if on-beat, red if off-beat
        if (onBeat)
            targetImage.color = Color.green;
        else
            targetImage.color = Color.red;

        // 3) Start the flash timer so we can revert later
        isFlasing = true;
        flashTimer = flashDurantion;
    }
}