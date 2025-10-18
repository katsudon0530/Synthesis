using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text descriotionText;
    [SerializeField] Text countText;

    private StatusEffectGenerator effectGenerator;
    private EnemyLifeContlloer enemyLifeContlloer;
    private int _life;
    private int _attack;
    private int _defense;
    private int _magicDefense;
    private int _pastLife;
    private int _count;
    private bool _decreaseLife;
    private bool _act;
    
    public EnemyBase Base { get; private set; }
    public StatusEffectGenerator EffectGenerator { get => effectGenerator; set => effectGenerator = value; }
    public bool DecreaseLife { get => _decreaseLife; set => _decreaseLife = value; }
    public int Life { get => _life; set => _life = value; }
    public int Attack { get => _attack; set => _attack = value; }
    public int Defense { get => _defense; set => _defense = value; }
    public int MagicDefense { get => _magicDefense; set => _magicDefense = value; }
    public int Count { get => _count; set => _count = value; }
    public bool Act { get => _act; set => _act = value; }


    //カード内容の定義
    public void SetEnemy(EnemyBase enemyBase)
    {
        Base = enemyBase;
        icon.sprite = Base.Icon;
        descriotionText.text = Base.Description;

        SetState();

        _act = true;
        _decreaseLife = false;
        countText.text = $"{_count}";
        Base.Effects.Clear();

        enemyLifeContlloer = GetComponent<EnemyLifeContlloer>();
        GameObject generator = GameObject.FindWithTag("StatusEffectGenerator");
        effectGenerator = generator.GetComponent<StatusEffectGenerator>();
    }

    private void SetState()
    {
        _life = Base.EnemyLifeMax;
        _pastLife = Base.EnemyLifeMax;
        _attack = Base.EnemyAttack;
        _defense = Base.EnemyDefense;
        _magicDefense = Base.EnemyMagicDefense;
        _count = Base.EnemyCount;
    }


    //HPが減ったら揺れる
    private void Update()
    {
        if (_life < _pastLife)
        {
            enemyLifeContlloer.lifeReflection(this);
            this.GetComponentInChildren<EnemyShaker>().Shake();
            _pastLife = _life;
            DecreaseLife = true;
        }
    }
    //エネミーの強力攻撃までのカウントダウン
    public void EnemyCountDown()
    {
        if (_count == 0)
            _count = Base.EnemyCount;
        else
            _count = _count - 1;

        countText.text = $"{_count}";
    }

    // エネミーの状態をテキストで表示する
    public IEnumerator EnemySituation()
    {
        float RestLife = _life / (float)Base.EnemyLifeMax;
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

        MessageText.TextIn ($"{Base.Name}は{Base.Situation[textInNumber].situationText}");
        yield return new WaitForSeconds(1f);
        yield break;
    }

    public void EnemyReSet()
    {
        GetComponent<EffectCount>().StatusEffectCount(Base.Effects);
        _act = true;
        DecreaseLife = false;
    }
}
