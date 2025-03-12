using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollDiceController : MonoBehaviour
{
    public GameObject dice;
    public bool isRollable = true;
    public float spawnHeight = 20f;
    public float torqueRange = 25f;
    private Camera _mainCamera;


    // Blinking outline settings
    public float blinkSpeed = 2.0f;
    public float blinkDuration = 3.0f;
    private bool isBlinking = false;

    private Outline outline;
    public Color outlineColor;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseClick();
    }

    void ShowOutline(bool show)
    {
        if (show)
        {
            if (!isBlinking)
            {
                StartCoroutine(BlinkOutline(blinkDuration));
            }
        }
    }

    IEnumerator BlinkOutline(float duration)
    {
        if (outline == null)
        {
            outline = gameObject.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineWidth = 5f;
        }
        isBlinking = true;
        float endTime = Time.time + duration;

        Color newColor = outlineColor;
        while (Time.time < endTime)
        {
            float alpha = (Mathf.Sin(Time.time * blinkSpeed * Mathf.PI) + 1) / 2;
            newColor.a = alpha;
            outline.OutlineColor = newColor;

            yield return null;
        }
        newColor.a = 0;
        outline.OutlineColor = newColor;
        isBlinking = false;
    }

    void HandleMouseClick()
    {
        if (isRollable && Input.GetMouseButtonDown(0))
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                Vector3 spawnPosition = hit.point; // World coordinates
                spawnPosition.y = spawnHeight;
                isRollable = false;
                GameObject spawnedDice = Instantiate(dice, spawnPosition, Random.rotation);
                Rigidbody rb = spawnedDice.GetComponent<Rigidbody>();

                Vector3 randomTorque = new Vector3(
                    Random.Range(-torqueRange, torqueRange),
                    Random.Range(-torqueRange, torqueRange),
                    Random.Range(-torqueRange, torqueRange)
                );

                rb.angularVelocity = randomTorque;
            }
            else
            {
                ShowOutline(true);
            }
        }
    }
}
