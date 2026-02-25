using UnityEngine;

// Script From: https://github.com/pratyushkiran/unity_2d_parallax_background/blob/main/ParallaxEffect.cs
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundParralax : MonoBehaviour
{
    [Header("Parallax Settings")]
    [Tooltip("Speed of the parallax effect on the X-axis. A value of 0 means no parallax, while 1 means full parallax.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float parallaxSpeedX = 0.5f; // Speed of the parallax effect for X-axis

    [Tooltip("Speed of the parallax effect on the Y-axis. A value of 0 means no parallax, while 1 means full parallax.")]
    [Range(0f, 1f)]
    [SerializeField]
    private float parallaxSpeedY = 0.5f; // Speed of the parallax effect for Y-axis

    [Header("Camera Settings")]
    [Tooltip("Reference to the camera's transform. If not set, it will use the main camera.")]
    [SerializeField]
    private Transform cameraTransform; // Reference to the camera's transform

    private float startPositionX; // Initial X position of the sprite
    private float startPositionY; // Initial Y position of the sprite
    private float spriteSizeX; // Width of the sprite
    private float spriteSizeY; // Height of the sprite
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private Vector3 initialCameraPosition; // Initial position of the camera

    private void Awake()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // If no camera is assigned, use the main camera
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        // Store the initial position of the sprite
        startPositionX = transform.position.x;
        startPositionY = transform.position.y;

        // Store the initial camera position
        initialCameraPosition = cameraTransform.position;

        // Calculate the width and height of the sprite
        spriteSizeX = spriteRenderer.bounds.size.x;
        spriteSizeY = spriteRenderer.bounds.size.y;
    }

    private void Update()
    {
        // Calculate how far the camera has moved from its initial position
        float cameraDeltaX = cameraTransform.position.x - initialCameraPosition.x;
        float cameraDeltaY = cameraTransform.position.y - initialCameraPosition.y;

        // Calculate the parallax offset for the sprite
        float parallaxOffsetX = cameraDeltaX * parallaxSpeedX;
        float parallaxOffsetY = cameraDeltaY * parallaxSpeedY;

        // Update the sprite's position
        transform.position = new Vector3(startPositionX + parallaxOffsetX, startPositionY + parallaxOffsetY, transform.position.z);

        // Loop the parallax effect for X-axis (infinite scrolling)
        float relativeCameraDistX = cameraDeltaX * (1 - parallaxSpeedX);
        if (relativeCameraDistX > startPositionX + spriteSizeX)
            startPositionX += spriteSizeX;
        else if (relativeCameraDistX < startPositionX - spriteSizeX)
            startPositionX -= spriteSizeX;

    }
}