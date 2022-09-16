using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public float slideSpeed;
    public Transform reference;
    public TreeThemeManager themeManager;
    public MIDIControls controls;

    public int themeIndex = -1;

    Material treeMat;

    bool isSliding;

    bool isAnimatingEmission;
    float emissionAnimationDuration = .5f;
    float emissionAnimationTime;

    Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = reference.GetComponent<Renderer>();
        treeMat = renderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (renderer.isVisible && !isSliding)
        {
            StartCoroutine(Slide());
        }

        /*
        if (Input.GetKeyDown("z") && !isAnimatingEmission)
        {
            StartCoroutine(StartEmission());
        }
        */
    }

    IEnumerator StartEmission()
    {
        if (themeIndex == -1)
            yield break;

        isAnimatingEmission = true;

        Color targetColor = themeManager.themes[themeIndex].color;
        float targetIntensity = themeManager.themes[themeIndex].intensity;

        toggleEmission();

        while (emissionAnimationTime < emissionAnimationDuration)
        {
            float intensity = targetIntensity - (targetIntensity * (emissionAnimationTime / emissionAnimationDuration));
            treeMat.SetColor("_EmissionColor", targetColor * intensity);

            emissionAnimationTime += Time.deltaTime;

            print("animation time: " + emissionAnimationTime);

            yield return null;
        }

        emissionAnimationTime = 0f;
        toggleEmission();

        isAnimatingEmission = false;
    }

    IEnumerator Slide()
    {
        isSliding = true;

        while (renderer.isVisible)
        {
            Vector3 hTranslation = Vector3.left * slideSpeed * Time.deltaTime;
            transform.Translate(hTranslation);

            yield return null;
        }

        GameObject.Destroy(gameObject);
    }

    void toggleEmission()
    {
        bool isEmissionEnabled = treeMat.IsKeywordEnabled("_EMISSION");
        if (isEmissionEnabled)
        {
            treeMat.DisableKeyword("_EMISSION");
        }
        else
        {
            treeMat.EnableKeyword("_EMISSION");
        }
    }

    public void ConfigureControls(MIDIControls controls)
    {
        this.controls = controls;
        controls.Gameplay.LightUp1.performed += ctx => StartCoroutine(StartEmission());
    }
}
