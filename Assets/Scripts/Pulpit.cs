using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pulpit : MonoBehaviour
{
    public float pulpit_destroy_time;
    public TextMeshPro destroyTimer_txt;
    public Animator anim;
    bool enableScore;
    bool isReadyToDestroy;

    float pulpit_Spawn_Time = 2f;

    public void StartPupilCountDownTimer()
    {
        StartCoroutine(IEStartPulpit_DestroyTime());
    }
    public void StopPupilCountDownTimer()
    {
        StopCoroutine(IEStartPulpit_DestroyTime());
    }
    IEnumerator IEStartPulpit_DestroyTime()
    {
        pulpit_destroy_time = Random.Range(4f, 5f);
        isReadyToDestroy = false;
        enableScore = true;
        yield return null;
        anim.SetTrigger("ScaleOut");

        while (pulpit_destroy_time > 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            if (pulpit_destroy_time >= 0)
            {
                destroyTimer_txt.text = pulpit_destroy_time.ToString("0.00");
                pulpit_destroy_time -= Time.deltaTime;
            }
            if (pulpit_destroy_time <= pulpit_Spawn_Time)
            {
                if (isReadyToDestroy == false)
                {
                    GameManager.instance.SpawnPulpit();
                    isReadyToDestroy = true;
                    GameManager.instance.previousPulpit = this;
                }
            }
        }

        anim.SetTrigger("ScaleIn");
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (enableScore)
            {
                enableScore = false;
                GameManager.instance.Score++;
            }
        }
    }
}
