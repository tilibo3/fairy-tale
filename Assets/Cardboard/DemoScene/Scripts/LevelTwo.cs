using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelTwo : MonoBehaviour {

    public static bool hitSuccess = false;//子弹是否打中水晶
    private static int partNum = 0;//收集水晶碎片的数量

    public static int gameState = 1;//偶数暂停时间状态  奇数恢复时间状态
    public static int lifeValue = 1;//生命值  
    private float timeLeft = 5;//死亡后的倒计时
    private float timeFinish = 96;//行至终点的倒计时

    private float[] timers = new float[4];//敌人生成的定时器
    public Sprite[] sprites;//指定两张图片
    private Vector3[] paths = new Vector3[3];//设置抛物线   

    protected GameObject bullet;
    protected GameObject crystalredpart;
    public AudioClip sGatherPart;

    protected GameObject startCanvas;
    protected GameObject startObjects;
    protected GameObject instructCanvas;
    protected GameObject finishCanvas;
    protected GameObject dieCanvas;

    public static GameObject UIimage;//场景中的UI组件
    protected GameObject UItext;//场景中的UI组件
    protected GameObject UIbuttonF;//场景中的UI组件
    protected GameObject UIbuttonV;//场景中的UI组件

    protected Text instruction;
    protected Text DIEtext;
    protected Text FINISHtext;
    public static Image image;

    private GameObject fairy_now;
    protected GameObject fairy;
    protected GameObject[] fairys = new GameObject[4];
    protected GameObject[] fairys_body = new GameObject[4];

    private float AlphaValue = 0;//透明通道的值

    void Start()
    {

        /*变量的初始赋值*/
        hitSuccess = false;
        partNum = 0;
        gameState = 1;
        AlphaValue = 0;

        timeLeft = 5;
        timeFinish = 96;

        timers = new float[4];
        paths = new Vector3[3];
        fairys = new GameObject[4];
        fairys_body = new GameObject[4];

        /*读取canvas*/
        instructCanvas = GameObject.FindGameObjectWithTag("instructcanvas");
        finishCanvas = GameObject.FindGameObjectWithTag("finishcanvas");
        dieCanvas = GameObject.FindGameObjectWithTag("diecanvas");

        /*读取UI*/
        UIimage = GameObject.FindGameObjectWithTag("pcimage");
        UItext = GameObject.FindGameObjectWithTag("instruction");
        UIbuttonF = GameObject.FindGameObjectWithTag("buttonf");
        UIbuttonV = GameObject.FindGameObjectWithTag("buttonv");

        instruction = UItext.GetComponent<Text>();
        image = UIimage.GetComponent<Image>();
        DIEtext = GameObject.FindGameObjectWithTag("dietext").GetComponent<Text>();
        FINISHtext = GameObject.FindGameObjectWithTag("finishtext").GetComponent<Text>();

        /*读取场景物体*/
        fairy = GameObject.FindGameObjectWithTag("fairy");
        fairys[0] = GameObject.FindGameObjectWithTag("fairy1");
        fairys[1] = GameObject.FindGameObjectWithTag("fairy2");
        fairys[2] = GameObject.FindGameObjectWithTag("fairy3");
        fairys[3] = GameObject.FindGameObjectWithTag("fairy4");

        /*为包含敌人的数组赋值*/
        for (int i = 0; i < 4; i++)
        {
            foreach (Transform child in fairys[i].transform)
            {
                if (child.name == "jingling")
                    fairys_body[i] = child.gameObject;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            foreach (Transform child in fairys_body[i].transform)
            {
                Color c = child.gameObject.renderer.material.color;
                c.a = AlphaValue;
                child.gameObject.renderer.material.color = c;
            }
        }

        /*为控制敌人生成的定时器赋值*/
        timers[0] = 3f;
        timers[1] = 28f;
        timers[2] = 46f;
        timers[3] = 76f;

        /*初始状态为时间暂停状态*/
    }

    void Update()
    {
        /*在不同的时间生成敌人*/
        for (int i = 0; i < 4; i++)
            timers[i] -= Time.deltaTime;

        if (timers[0] < 0)
        {
            timers[0] = 1000f;
            GenerateEnermy(0);
        }
        if (timers[1] < 0)
        {
            timers[1] = 1000f;
            GenerateEnermy(1);
        }
        if (timers[2] < 0)
        {
            timers[2] = 1000f;
            GenerateEnermy(2);
        }
        if (timers[3] < 0)
        {
            timers[3] = 1000f;
            GenerateEnermy(3);
        }

        /*更新时判断游戏状态，控制相应物体的显隐性*/
        if (gameState % 2 == 0)//暂停时间状态
        {
            Time.timeScale = 0;
            fairy.SetActive(false);//暂停时间才会出现准心点标志，同时小精灵消失，使画面简洁，任务明确
            UIimage.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            fairy.SetActive(true);
            UIimage.SetActive(false);
        }
        
        if (gameState == -1)//角色死亡状态
        {
            instructCanvas.SetActive(false);
            dieCanvas.SetActive(true);
        }
        else
        {
            instructCanvas.SetActive(true);
            dieCanvas.SetActive(false);
        }

        /*当主角被子弹击中时，游戏结束*/
        if (lifeValue == 0)
        {
            iTween.Stop(GameObject.FindGameObjectWithTag("capsule"));

            gameState = -1;

            Die();

            lifeValue = 1;
        }


        timeFinish -= Time.deltaTime;

        /*行进过程中*/
        if (timeFinish > 94 && timeFinish < 96)
        {
            instruction.text = "欢迎来到精灵童话！";
        }
        if (timeFinish > 90 && timeFinish < 94)
        {
            instruction.text = "拉下磁铁，可以暂停/恢复时间，试试看吧！";
        }
        if (timeFinish > 88 && timeFinish < 90)
        {
            instruction.text = "邪恶小精灵发射子弹了，被打中，你会消失的！";
        }
        if (partNum == 1 && timeFinish > 74)
        {
            instruction.text = "恭喜你，成功收集到一块水晶碎片！";
        }
        if (hitSuccess == true && partNum == 0 && timeFinish > 80)
        {
            instruction.text = "水晶碎片只会短暂出现，要及时收集哟！";
        }
        if (partNum == 0 && timeFinish < 80 && timeFinish > 74)
        {
            instruction.text = "很遗憾，你没有收集到这块水晶碎片";
        }
        if (timeFinish < 74 && partNum == 0)
        {
            instruction.text = "不要灰心，接下来的探险途中，要加油！";
        }
        if (timeFinish < 74 && partNum == 1)
        {
            instruction.text = "你真棒！再接再厉，继续你的探险吧！";
        }
        if (timeFinish < 64)
        {
            instruction.text = "拥有的水晶碎片：" + partNum;
        }

        /*走到终点时*/
        if (timeFinish < 0)
        {
            instructCanvas.SetActive(false);
            finishCanvas.SetActive(true);
            iTween.Stop(GameObject.FindGameObjectWithTag("capsule"));

            if (partNum < 4)
            {
                FINISHtext.text = "很遗憾，您的水晶碎片只有" + partNum + "块，无法解救小精灵，没有小精灵的带路，您无法继续向前，也找不到回去的路……";
                FINISHtext.color = Color.blue;
                UIbuttonF.SetActive(true);
                UIbuttonV.SetActive(false);
            }
            else
            {
                FINISHtext.text = "恭喜你，您成功收集了" + partNum + "块水晶碎片，解救了小精灵，接下来，小精灵会带您去蓝色关卡(正在开发中)";
                FINISHtext.color = Color.red;
                UIbuttonF.SetActive(false);
                UIbuttonV.SetActive(true);
            }
        }
        else
        {
            finishCanvas.SetActive(false);
        }
    }

    public void Die()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 5 && timeLeft >= 4)
        {
            DIEtext.text = "您被子弹击中，解救任务失败. 5秒后将重新开始游戏";
        }
        else if (timeLeft < 4 && timeLeft >= 3)
        {
            DIEtext.text = "您被子弹击中，解救任务失败. 4秒后将重新开始游戏";
        }
        else if (timeLeft < 3 && timeLeft >= 2)
        {
            DIEtext.text = "您被子弹击中，解救任务失败. 3秒后将重新开始游戏";
        }
        else if (timeLeft < 2 && timeLeft >= 1)
        {
            DIEtext.text = "您被子弹击中，解救任务失败. 2秒后将重新开始游戏";
        }
        else if (timeLeft < 1 && timeLeft >= 0)
        {
            DIEtext.text = "您被子弹击中，解救任务失败. 1秒后将重新开始游戏";
        }
        else
        {
            Restart();
        }
        if (timeLeft > 0)
            Invoke("Die", Time.deltaTime);
    }

    public void Restart()
    {
        Application.LoadLevel("Next");
    }

    public void FrozeTime()//暂停时间和恢复时间的函数
    {
        gameState++;
        image.sprite = sprites[0];//准心点图片为红色       
    }

    public void ChangeImage(bool gazedAt)//如果注视水晶或者水晶碎片，则准心点图片变成绿色
    {
        if (gameState % 2 == 0)
        {
            if (gazedAt)
                image.sprite = sprites[1];
            else
                image.sprite = sprites[0];
        }
    }

    public void HitCrystal(int num)//击碎水晶函数，int 参数，选择第几块水晶被击中
    {
        if (gameState % 2 == 0 && Time.time > 5.5f)//只有当游戏暂停时且出现子弹时，水晶才能被击碎 
        /*bug
         *前一个子弹销毁和后一个子弹生成之间的空闲时间，场景内无法获得子弹
         */
        {
            gameState++;//更新游戏状态

            bullet = GameObject.FindGameObjectWithTag("bullet");//获得子弹
            hitSuccess = true;//打中水晶的标志

            paths[0] = bullet.transform.position;
            switch (num)
            {
                case 1://第1块水晶被击碎
                    iTween.Stop(GameObject.FindGameObjectWithTag("crystal1"));
                    paths[2] = GameObject.FindGameObjectWithTag("crystal1").transform.position;
                    DestroyEnermy(0);
                    break;
                case 2://第2块水晶被击碎
                    iTween.Stop(GameObject.FindGameObjectWithTag("crystal2"));
                    paths[2] = GameObject.FindGameObjectWithTag("crystal2").transform.position;
                    DestroyEnermy(1);
                    break;
                case 3://第3块水晶被击碎
                    iTween.Stop(GameObject.FindGameObjectWithTag("crystal3"));
                    paths[2] = GameObject.FindGameObjectWithTag("crystal3").transform.position;
                    DestroyEnermy(2);
                    break;
                case 4://第4块水晶被击碎
                    iTween.Stop(GameObject.FindGameObjectWithTag("crystal4"));
                    paths[2] = GameObject.FindGameObjectWithTag("crystal4").transform.position;
                    DestroyEnermy(3);
                    break;
            }
            paths[1] = new Vector3(paths[0].x / 2 + paths[2].x / 2, paths[0].y + paths[2].y , paths[0].z / 2 + paths[2].z / 2);

            iTween.MoveTo(bullet, iTween.Hash("path", paths, "movetopath", false, "orienttopath", true, "time", 4f, "easytype", iTween.EaseType.linear));//使子弹呈抛物线射向水晶
        }
        else
        {
            gameState++;//更新游戏状态
        }
    }

    public void GatherPart()//收集碎片函数
    {
        if (gameState % 2 == 0)//只有时间暂停时才能收集碎片
        {
            gameState++;//更新游戏状态

            partNum++;//碎片数量增加1

            crystalredpart = GameObject.FindGameObjectWithTag("crystalredpart");
            crystalredpart.renderer.enabled = false;

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            player.audio.clip = sGatherPart;
            Debug.Log(player.audio.clip);
            player.audio.Play(); //播放收集碎片的音效
        }
        else
        {
            gameState++;//只更新游戏状态
        }
    }

    public void GenerateEnermy(int num)
    {
        AlphaValue = 0;
        fairy_now = fairys_body[num];
        appear();
        Invoke("shoot", 2.5f);
    }

    public void DestroyEnermy(int num)
    {
        AlphaValue = 1;
        fairy_now = fairys_body[num];
        disappear();
    }

    private float timeforFairy = 0;
    private bool state = false;
    public Transform m_rocket;

    void appear()
    {
        timeforFairy += Time.deltaTime;

        if (timeforFairy >= 0.2f)
        {
            state = true;
            timeforFairy = 0;
        }
        if (state == true)
        {
            AlphaValue += 0.1f;
            state = false;
        }
        if (AlphaValue >= 1)
        {
            AlphaValue = 1;
            return;
        }
        foreach (Transform child in fairy_now.transform)
        {
            Color c = child.gameObject.renderer.material.color;
            c.a = AlphaValue;
            child.gameObject.renderer.material.color = c;
        }
        Invoke("appear", Time.deltaTime);
    }

    public void disappear()
    {
        timeforFairy += Time.deltaTime;

        if (timeforFairy >= 0.1f)
        {
            state = true;
            timeforFairy = 0;
        }
        if (state == true)
        {
            AlphaValue -= 0.2f;
            state = false;
        }
        if (AlphaValue <= -0.2)
        {
            AlphaValue = 0;
            return;
        }
        foreach (Transform child in fairy_now.transform)
        {
            Color c = child.gameObject.renderer.material.color;
            c.a = AlphaValue;
            child.gameObject.renderer.material.color = c;
        }
        Invoke("disappear", Time.deltaTime);
    }

    void shoot()
    {
        Instantiate(m_rocket, fairy_now.transform.position, fairy_now.transform.rotation);//在精灵位置处发射子弹
    }
}
