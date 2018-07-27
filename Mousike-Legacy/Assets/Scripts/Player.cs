using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float sensitivity = Mathf.PI;
    public float distance;


    float angle = 0.0f;
    Vector3 position = Vector3.zero;
    Vector3 zAxis = new Vector3(0, 0, 1);


	// Use this for initialization
	void Start ()
    {
        distance = Camera.main.orthographicSize - 0.3f;
        Rotate(0.0f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        float difference = 0.0f;
        if (Input.touchSupported == true)
        {
            if (Input.touchCount > 0)
            {
                if (Input.touches[0].position.x > Display.main.renderingWidth / 2)
                    difference += Time.deltaTime * sensitivity;
                else
                    difference -= Time.deltaTime * sensitivity;
            }
        }
        else
        {
            if (Input.GetAxisRaw("Horizontal") > 0.02f)
                difference += Time.deltaTime * sensitivity;
            else if (Input.GetAxisRaw("Horizontal") < -0.02f)
                difference -= Time.deltaTime * sensitivity;

        }
        if (difference != 0.0f)
        {
            Rotate(difference);
            angle += difference;
        }
	}

    private void Rotate(float difference)
    {
        position.x = Mathf.Cos(angle) * distance;
        position.y = Mathf.Sin(angle) * distance;
        transform.Rotate(zAxis, difference * Mathf.Rad2Deg);
        transform.position = position;
    }
}
