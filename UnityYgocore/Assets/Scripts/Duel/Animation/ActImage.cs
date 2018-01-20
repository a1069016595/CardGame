using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ActImage : MonoBehaviour
{
    bool isPlay;
    RawImage image;

    float timer;
    float intervals = 0.02f;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > intervals)
        {
            if (isPlay)
            {
                if (image == null)
                {
                    image = this.GetComponent<RawImage>();
                }

                float v = image.rectTransform.localEulerAngles.z;
                image.rectTransform.localEulerAngles = new Vector3(0, 0, v + 2f);

            }
            timer = 0;
        }
    }

    public void StartAnim()
    {
        gameObject.SetActive(true);
        isPlay = true;
    }

    public void EndAnim()
    {
        gameObject.SetActive(false);
        isPlay = false;
    }
}
