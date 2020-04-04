using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class WateryLensShift : MonoBehaviour
{

    private VolumeProfile profile;
    private List<VolumeComponent> listComponents;
    private LensDistortion _lensDistortion;

    [SerializeField] private float maxShiftAmount = 0.1f;
    private Volume ppVol;
    private Vector2 xy;
    private float originalCenterX;
    private float originalCenterY;
    private LensDistortion wateryLens;
    // Start is called before the first frame update
    
    // Time taken for the transition.
    float duration = 1.0f;
    float startTime;
    void Start()
    {
        profile = GetComponent<Volume>().profile;
        listComponents = profile.components;
        foreach (VolumeComponent vc in listComponents)
        {
            if (vc.name.Contains("LensDistortion"))
            {
                _lensDistortion = (LensDistortion)vc;
                break;
            }
        }

        if (_lensDistortion != null)
        {
            xy = _lensDistortion.center.value;
            originalCenterX = xy.x;
            originalCenterY = xy.y;
            Debug.Log("Obtained Lens Distortion variables");
        }
        else
        {
            Debug.Log("Did NOT obtain Lens Distortion variables");
        }
            
        startTime = Time.time;        

    }

    // Update is called once per frame
    void LateUpdate()
    {
        float t = (Time.time - startTime) / 2;
        xy.x = originalCenterX + Mathf.SmoothStep(-maxShiftAmount, maxShiftAmount,
                   Mathf.PingPong(t, 1f));
        xy.y = originalCenterY - Mathf.SmoothStep(-maxShiftAmount, maxShiftAmount,
                   Mathf.PingPong(t, 1f));
         _lensDistortion.center.value = xy;
    }
}
