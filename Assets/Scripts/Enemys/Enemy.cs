using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text descriotionText;
    [SerializeField] Text CountText;
    GameObject generator;
    StatusEffectGenerator statusEffectGenerator;
    int pastLife;
    bool decreaseLife;
    EnemyLifeContlloer enemyLifeContlloer;
    public EnemyBase Base { get; private set; }
    public Text CountText1 { get => CountText; set => CountText = value; }
    public StatusEffectGenerator StatusEffectGenerator { get => statusEffectGenerator; set => statusEffectGenerator = value; }
    public bool DecreaseLife { get => decreaseLife; set => decreaseLife = value; }


    //カード内容の定義
    public void SetEnemy(EnemyBase enemyBase)
    {
        enemyBase.EnemyLife = enemyBase.EnemyLifeMax;
        enemyBase.Count = enemyBase.EnemyCount;
        pastLife = enemyBase.EnemyLife;
        Base = enemyBase;
        Base.Act = true;
        DecreaseLife = false;
        icon.sprite = enemyBase.Icon;
        descriotionText.text = enemyBase.Description;
        CountText1.text = $"{enemyBase.Count}";
        enemyBase.Effects.Clear();

        enemyLifeContlloer = GetComponent<EnemyLifeContlloer>();
        generator = GameObject.FindWithTag("StatusEffectGenerator");
        statusEffectGenerator = generator.GetComponent<StatusEffectGenerator>();
    }


    //HPが減ったら揺れる
    private void Update()
    {
        if (Base.EnemyLife < pastLife)
        {
            enemyLifeContlloer.lifeReflection(this);
            this.GetComponentInChildren<EnemyShaker>().Shake();
            pastLife = Base.EnemyLife;
            DecreaseLife = true;
        }
    }
    //エネミーの強力攻撃までのカウントダウン
    public void EnemyCountDown()
    {
        if (Base.Count == 0)
            Base.Count = Base.EnemyCount;
        else
            Base.Count = Base.Count - 1;

        CountText1.text = $"{Base.Count}";
    }

    // エネミーの状態をテキストで表示する
    public IEnumerator EnemySituation()
    {
        float RestLife = Base.EnemyLife / (float)Base.EnemyLifeMax;
        int textInNumber = 0;
        float diffResult = 1f;

        for (int i = 0; i < Base.Situation.Count; i++)
        {
            float diff = RestLife - Base.Situation[i].restLife;
            if (diff <= 0 && Mathf.Abs(diff) < diffResult)
            {
                diffResult = Mathf.Abs(diff);
                textInNumber = i;
            }                

        }

        MessageText.TextIn ($"{Base.Name1}は{Base.Situation[textInNumber].situationText}");
        yield return new WaitForSeconds(1f);
        yield break;
    }

    public void EnemyReSet()
    {
        GetComponent<EffectCount>().StatusEffectCount(Base.Effects);
        Base.Act = true;
        DecreaseLife = false;
    }
}
