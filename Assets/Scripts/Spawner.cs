using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    class Projectile
    {
        public float angle;
        public float velocity;
        public float delay;

        public Projectile(float angle, float velocity, float delay)
        {
            this.angle = angle;
            this.velocity = velocity;
            this.delay = delay;
        }
    }

    class BandPair
    {
        public int index;
        public float value;

        public BandPair(int index, float value)
        {
            this.index = index;
            this.value = value;
        }
    }

    public GameObject prefab;
    public CircularSpectrum spectrum;
    public float minimumBand = 1.0f;
    public float delay = 0.02f;
    public float delayBetweenBands = 1.0f;
    public float velocityMultiplier = 0.5f;
    public float delayBetweenSpawn = 1.0f;


    private float lastUpdate;

    List<Projectile> spawnList;

    List<BandPair> maximum;
    List<BandPair> minimum;

    BandPair maxBand;
    BandPair minBand;

    class BandComparer : Comparer<BandPair>
    {
        public override int Compare(BandPair x, BandPair y)
        {
            if (x.value == y.value)
                return (x.index < y.index) ? 1 : -1;
            return (x.value > y.value) ? 1 : -1;
        }
    }
    BandComparer comparer = new BandComparer();
    

    public void Start()
    {
        spawnList = new List<Projectile>();

        maximum = new List<BandPair>();
        minimum = new List<BandPair>();
        lastUpdate = Time.realtimeSinceStartup;
    }

    public void FixedUpdate()
    {
        this.Spawn();

        if (Time.realtimeSinceStartup - lastUpdate > delayBetweenSpawn)
            lastUpdate = Time.realtimeSinceStartup;
        else 
            return;

        //Get the band spectrum and check if it's empty
        float[] spec = spectrum.BandList;

        bool derivative = (spec[1] - spec[0]) > 0.0f;
        bool nextDerivative;

        //Check if the first band is a maximum or a minimum
        {
            bool lastDerivative = nextDerivative = (spec[0] - spec[spec.Length - 1]) > 0.0f;
            //If they are equal, continue
            if (derivative == lastDerivative) { }
            //If lastDerivative is positive and first derivative is negative -> maximum
            else
            {
                if (lastDerivative && !derivative)
                    maximum.Add(new BandPair(0, spec[0]));
                //Otherwise -> minimum
                else
                    minimum.Add(new BandPair(0, spec[0]));
            }
        }

        for (int i = 1; i < spec.Length - 1; i++)
        {
            if (spec[i] < minimumBand)
                continue;

            nextDerivative = (spec[i + 1] - spec[i]) > 0.0f;

            //If equal, continue
            if (derivative == nextDerivative)
                continue;
            //Positive derivative -> minnimum
            if (derivative == true)
                minimum.Add(new BandPair(i, spec[i]));
            //Negative derivative -> maximum
            else
                maximum.Add(new BandPair(i, spec[i]));
            derivative = nextDerivative;
        }

        BandPair currentMax = null;
        BandPair currentMin = null;

        maximum.Sort(comparer);
        //maximum.Reverse();
        minimum.Sort(comparer);
        minimum.Reverse();
        if(maximum.Count > 0)
            currentMax = maximum[0];
        if (minimum.Count > 0)
            currentMin = minimum[0];

        bool quit = false;
        if (maxBand != null && currentMax != null)
        {
            //if (Mathf.Abs(maxBand.index - currentMax.index) < 1)
            //    quit = true;
            if (Mathf.Abs(maxBand.value - currentMax.value) < 0.5f)
                quit = true;
        }
        if (minBand != null && currentMin != null)
        {
            //if (Mathf.Abs(minBand.index - currentMin.index) < 1)
            //    quit = true;
            if (Mathf.Abs(minBand.value - currentMin.value) < 0.5f)
                quit = true;
        }

        minBand = currentMin;
        maxBand = currentMax;

        if (quit)
        {
            minimum.Clear();
            maximum.Clear();
            return;
        }


        {
            int i = 0;
            //int j = 0;
            //for (; i < minimum.Count || j < maximum.Count; i++, j++)
            //{
            //    if(i < minimum.Count)
            //        spawnList.Add(new Projectile((2.0f * Mathf.PI * minimum[i].index) / (bandList.Count * 1.0f), velocityMultiplier, 0.0f));
            //    if(j < maximum.Count)                                                                            
            //        spawnList.Add(new Projectile((2.0f * Mathf.PI * maximum[i].index) / (bandList.Count * 1.0f), velocityMultiplier, 0.0f));
            //}
            

            //for (i = 0; i < minimum.Count; i++)
            //    spawnList.Add(new Projectile((2.0f * Mathf.PI * minimum[i].index) / (spec.Length * 1.0f), Mathf.Clamp(Random.value * minimum[i].value + Random.value * 0.5f, 0.2f, 1.0f) * velocityMultiplier, 0.0f));
            //for (i = 0; i < maximum.Count; i++)
            //    spawnList.Add(new Projectile((2.0f * Mathf.PI * maximum[i].index) / (spec.Length * 1.0f), Mathf.Clamp(Random.value * maximum[i].value + Random.value * 0.5f, 0.2f, 1.0f) * velocityMultiplier, 0.0f));

            

            //for (i = 0; i < minimum.Count; i++)
            //    spawnList.Add(new Projectile((2.0f * Mathf.PI * minimum[i].index) / (spec.Length * 1.0f), minimum[i].value * velocityMultiplier, 0.0f));
            for (i = 0; i < maximum.Count && (maximum[i].value > 10.0f || i < spectrum.loopCount * 3) && i < spectrum.loopCount * 6; i++)
                spawnList.Add(new Projectile((2.0f * Mathf.PI * maximum[i].index) / (spec.Length * 1.0f), maximum[i].value * velocityMultiplier, 0.0f));
        }
    }

    private void Spawn()
    {
        if (spawnList.Count == 0)
            return;
        for (int i = 0; i < spawnList.Count;)
        {
            Projectile projectile = spawnList[i];
            if (projectile.delay < 0.002f)
            {
                GameObject newEnemy = Instantiate(prefab);
                newEnemy.GetComponent<Enemy>().SetAngleAndVelocity(projectile.angle, projectile.velocity);
            }
            else
            {
                projectile.delay -= Time.deltaTime;
                i++;
                continue;
            }
            spawnList.RemoveAt(i);
        }
    }
}
