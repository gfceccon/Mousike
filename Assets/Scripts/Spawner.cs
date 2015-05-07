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
    public float maximumDifferenceBetweenBands = 10.0f;
    public float delay = 0.02f;
    public float delayBetweenBands = 1.0f;
    public float velocityMultiplier = 0.5f;
    public float delayBetweenSpawn = 1.0f;


    private float lastUpdate;

    List<Projectile> spawnList;

    List<BandPair> maximum;
    List<BandPair> minimum;
    

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
        int lastIndex = -1;

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
                lastIndex = 0;
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
            lastIndex = i;
        }

        {
            int i = 0;
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
            for (i = 0; i < maximum.Count; i++)
                spawnList.Add(new Projectile((2.0f * Mathf.PI * maximum[i].index) / (spec.Length * 1.0f), maximum[i].value * velocityMultiplier, 0.0f));
        }
        minimum.Clear();
        maximum.Clear();
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
