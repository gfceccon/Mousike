using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SpectrumAnimation))]
public class CircularSpectrum : MonoBehaviour
{
    public enum LoopType
    {
        Continuous,
        Mirrored
    }

    public enum GraphType
    {
        Linear,
        Log
    }

    #region Spectrum Options
    [Header("Spectrum")]
    public AudioSource source;
    public LoopType loopType = LoopType.Continuous;
    public GraphType graphType = GraphType.Log;
    [Range(1, 16)]
    public int loopCount = 1;
    public AudioSpectrum.BandType bandType = AudioSpectrum.BandType.ThirtyOneBand;
    #endregion

    #region Paramters
    [Header("Initial Values")]
    public float initialMultiplier = 2.0f;
    public float initialRadius = 5.0f;
    public float initialPhase = 0.0f;
    #endregion

    #region Private Classes
    LineRenderer lineRenderer;
    AudioSpectrum spectrumAnalyzerLeft;
    AudioSpectrum spectrumAnalyzerRight;
    int vertexCount;
    Vector3[] points;
    float[] bandList;
    SpectrumAnimation spectrumAnim;
    #endregion

    #region Private Attributes
    float multiplier;
    float radius;
    float phase;
    #endregion

    #region Properties
    public float[] BandList
    {
        get
        {
            if (bandList == null)
                this.GetPoints();
            return bandList;
        }
    }
    #endregion

    void Start()
    {
        spectrumAnalyzerLeft = new AudioSpectrum(source, 0);
        spectrumAnalyzerLeft.bandType = bandType;

        spectrumAnalyzerRight = new AudioSpectrum(source, 1);
        spectrumAnalyzerRight.bandType = bandType;

        vertexCount = loopCount * AudioSpectrum.bandTypeSizes[(int)bandType];

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetVertexCount(vertexCount + 1);
        lineRenderer.SetWidth(0.05f, 0.05f);

        spectrumAnim = GetComponent<SpectrumAnimation>();

        multiplier = initialMultiplier;
        radius = initialRadius;
        phase = initialPhase;
    }

    void Update()
    {
        float animMultiplier = 0.0f, animRadius = 0.0f;
        spectrumAnim.UpdateAnimation(lineRenderer, ref animMultiplier, ref phase, ref animRadius);

        multiplier = initialMultiplier + animMultiplier;
        radius = initialRadius + animRadius;

        this.GetPoints();
        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }

    float GetValueFromBand(float band, float maxBand)
    {
        switch (graphType)
        {
            case GraphType.Linear:
                return multiplier * band;
            default:
            case GraphType.Log:
                return multiplier * Mathf.Log(band + 1);
        }
    }

    public Vector3[] GetPoints()
    {
        bool mirror = false;
        float maxLeft, maxRight;
        int counter = 0;
        float[] bandsLeft = spectrumAnalyzerLeft.GetBand(out maxLeft);
        float[] bandsRight = spectrumAnalyzerRight.GetBand(out maxRight);
        float outputBand;
        float maxBand = (maxLeft + maxRight) / 2f;

        CheckArray();
        for (int i = 0; i < loopCount; i++)
        {
            int j = 0;
            if (mirror)
                j = bandsLeft.Length - 1;

            while (j < bandsLeft.Length && j > -1)
            {
                outputBand = (bandsLeft[j] + bandsRight[j]) / 2.0f;
                bandList[counter] = GetValueFromBand(outputBand, maxBand);
                points[counter].x = Mathf.Cos(phase - 2.0f * Mathf.PI * counter / (float)vertexCount) * (bandList[counter] + radius);
                points[counter].y = Mathf.Sin(phase - 2.0f * Mathf.PI * counter / (float)vertexCount) * (bandList[counter] + radius);

                if (mirror)
                    j--;
                else
                    j++;
                counter++;
            }

            if (loopType == LoopType.Mirrored)
                mirror = !mirror;
        }

        outputBand = (bandsLeft[0] + bandsRight[0]) / 2.0f;
        points[counter].x = Mathf.Cos(phase) * (GetValueFromBand(outputBand, maxBand) + radius);
        points[counter].y = Mathf.Sin(phase) * (GetValueFromBand(outputBand, maxBand) + radius);
        return points;
    }

    private void CheckArray()
    {
        if (points == null)
            points = new Vector3[vertexCount + 1];
        if (bandList == null)
            bandList = new float[vertexCount];
    }
}
