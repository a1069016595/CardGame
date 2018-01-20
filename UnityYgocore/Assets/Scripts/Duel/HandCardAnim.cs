using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 用于显示手牌虚线框动画的显示
/// </summary>
public class HandCardAnim : MonoBehaviour
{
    RawImage image;


    private Texture texture1;
    private Texture texture2;

    public void Init()
    {
        texture1 = Resources.Load("Texture/dashed1") as Texture;
        texture2 = Resources.Load("Texture/dashed2") as Texture;

        image = this.GetComponent<RawImage>();

        gameObject.SetActive(false);
        image.texture = texture1;
    }

    public void ShowAnim()
    {
        gameObject.SetActive(true);
        StartCoroutine(anim(texture2));
    }

    public void HideAnim()
    {
        gameObject.SetActive(false);
        EndAnim();
        image.texture = texture1;
        
    }

    IEnumerator anim(Texture tex)
    {
        yield return new WaitForSeconds(0.1f);
        image.texture = tex;
        if (image.texture == texture1)
            StartCoroutine(anim(texture2));
        else
            StartCoroutine(anim(texture1));
    }

    private void EndAnim()
    {
        StopAllCoroutines();
    }
}
