using UnityEngine;
using System.Collections;

public class Bullet1 : MonoBehaviour
{

    public float bullet_speed = 0.2f;

    public GameObject Explosion;
    public GameObject CrystalParts;

    protected AudioSource bulletAudio;
    public AudioClip sBullet;

    void Start()
    {
        bulletAudio = this.audio;
        bulletAudio.clip = sBullet;
        bulletAudio.Play();
    }

    void Update()
    {
        if (GameManager.gameState % 2 == 1 && GameManager.hitSuccess == false)
        {
            this.transform.Translate(new Vector3(0, 0, bullet_speed * Time.deltaTime));//当恢复时间而且并未打中水晶时，子弹向前运动
        }
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "crystal1":
            case "crystal2":
            case "crystal3":
            case "crystal4":
            case "crystal5"://如果和水晶碰撞

                Destroy(this.gameObject);//摧毁子弹和水晶
                Destroy(other.gameObject);

                Instantiate(Explosion, this.transform.position, Quaternion.identity);//播放子弹碰撞特效 
                GameObject cp = (GameObject)Instantiate(CrystalParts, this.transform.position, Quaternion.identity);//创建一组水晶碎片
                cp.transform.Rotate(new Vector3(1, 0, 0), 90);
                break;

            case "Player":
                GameManager.lifeValue = 0;
                Destroy(this.gameObject);
                break;

            case "fairy":
                break;//待优化
        }

        GameManager.hitSuccess = false;
    }
}
