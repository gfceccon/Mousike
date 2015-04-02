using UnityEngine;
using System.Collections;

public class AudioSpectrum
{
    #region Band type definition
    public enum BandType
    {
        FourBand,
        FourBandVisual,
        EightBand,
        TenBand,
        TwentySixBand,
        ThirtyOneBand
    };
    public static int[] bandTypeSizes = {
        4,
        4,
        8,
        10,
        26,
        31
    };

    static float[][] middleFrequenciesForBands = {
        new float[]{ 125.0f, 500, 1000, 2000 },
        new float[]{ 250.0f, 400, 600, 800 },
        new float[]{ 63.0f, 125, 500, 1000, 2000, 4000, 6000, 8000 },
        new float[]{ 31.5f, 63, 125, 250, 500, 1000, 2000, 4000, 8000, 16000 },
        new float[]{ 25.0f, 31.5f, 40, 50, 63, 80, 100, 125, 160, 200, 250, 315, 400, 500, 630, 800, 1000, 1250, 1600, 2000, 2500, 3150, 4000, 5000, 6300, 8000 },
        new float[]{ 20.0f, 25, 31.5f, 40, 50, 63, 80, 100, 125, 160, 200, 250, 315, 400, 500, 630, 800, 1000, 1250, 1600, 2000, 2500, 3150, 4000, 5000, 6300, 8000, 10000, 12500, 16000, 20000 },
    };
    static float[] bandwidthForBands = {
        1.414f, // 2^(1/2)
        1.260f, // 2^(1/3)
        1.414f, // 2^(1/2)
        1.414f, // 2^(1/2)
        1.122f, // 2^(1/6)
        1.260f, // 2^(1/3)
    };
    #endregion

    #region Public variables
    public int numberOfSamples = 1024;
    public BandType bandType = BandType.ThirtyOneBand;
    #endregion

    #region Private variables
    int channel;
    AudioSource source;
    float[] rawSpectrum;
    float[] levels;
    #endregion

    #region Private functions
    private AudioSpectrum() { }
    
    void CheckBuffers()
    {
        if (rawSpectrum == null || rawSpectrum.Length != numberOfSamples)
        {
            rawSpectrum = new float[numberOfSamples];
        }
        var bandCount = middleFrequenciesForBands[(int)bandType].Length;
        if (levels == null || levels.Length != bandCount)
        {
            levels = new float[bandCount];
        }
    }

    int FrequencyToSpectrumIndex(float f)
    {
        var i = Mathf.FloorToInt(f / AudioSettings.outputSampleRate * 2.0f * rawSpectrum.Length);
        return Mathf.Clamp(i, 0, rawSpectrum.Length - 1);
    }
    #endregion

    public AudioSpectrum(AudioSource source, int channel)
    {
        this.source = source;
        this.channel = channel;
    }

    public float[] GetBand(out float bandMax)
    {
        CheckBuffers();
        if(source == null)
            AudioListener.GetSpectrumData(rawSpectrum, channel, FFTWindow.Blackman);
        else
            source.GetSpectrumData(rawSpectrum, channel, FFTWindow.Blackman);

        float[] middlefrequencies = middleFrequenciesForBands[(int)bandType];
        float bandwidth = bandwidthForBands[(int)bandType];

        bandMax = 0.0f;
        for (var bi = 0; bi < levels.Length; bi++)
        {
            int imin = FrequencyToSpectrumIndex(middlefrequencies[bi] / bandwidth);
            int imax = FrequencyToSpectrumIndex(middlefrequencies[bi] * bandwidth);

            float band = 0.0f;
            for (var fi = imin; fi <= imax; fi++)
            {
                band += rawSpectrum[fi];//Mathf.Max(bandMax, rawSpectrum[fi]);
            }
            if (band > bandMax)
                bandMax = band;
            levels[bi] = band;
        }
        return levels;
    }
}