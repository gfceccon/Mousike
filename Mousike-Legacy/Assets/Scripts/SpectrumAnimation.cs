using UnityEngine;
using System.Collections;

class SpectrumAnimation : MonoBehaviour
{
    Color start, end;
    float startColorTime, changeColorTime;
    float startRotateTime, changeRotateTime;
    float colorAnimTime, rotateAnimTime;
    bool direction = true;
    float rotate;


    #region Animation Paramters
    [Header("Paramters")]
    public float minColorTime = 2.0f;
    public float colorTime = 2.0f;
    public float minRotateAngle = 0.7853f;
    public float rotateAngle = 0.7853f;
    public float minRotateTime = 5.0f;
    public float rotateTime = 2.0f;
    #endregion

    public void Start()
    {
        start = new Color(Random.value, Random.value, Random.value);
        end = new Color(Random.value, Random.value, Random.value);
    }

    public void UpdateAnimation(LineRenderer line, ref float multiplier, ref float phase, ref float radius)
    {
        changeColorTime += Time.deltaTime;
        changeRotateTime += Time.deltaTime;

        if (changeColorTime - startColorTime >= colorAnimTime)
        {
            start.r = end.r;
            start.g = end.g;
            start.b = end.b;
            end.r = Random.value;
            end.g = Random.value;
            end.b = Random.value;

            startColorTime = Time.realtimeSinceStartup;
            changeColorTime = Time.realtimeSinceStartup;
            colorAnimTime = Random.value * colorTime + minColorTime;
        }
        if (changeRotateTime - startRotateTime >= rotateAnimTime)
        {
            direction = !direction;
            startRotateTime = Time.realtimeSinceStartup;
            changeRotateTime = Time.realtimeSinceStartup;
            rotate = Random.value * rotateAngle + minRotateAngle;
            rotateAnimTime = Random.value * rotateTime + minRotateTime;
        }

        Color c = Color.Lerp(start, end, (changeColorTime - startColorTime) / colorAnimTime);
        line.SetColors(c, c);
        if (direction)
            phase += rotate * Time.deltaTime / rotateAnimTime;
        else
            phase -= rotate * Time.deltaTime / rotateAnimTime;
    }
}