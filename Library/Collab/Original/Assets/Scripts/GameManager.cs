using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public delegate void CurrencyChanged();

public class GameManager : Singleton<GameManager>
{
    private int lives = 30 ;/// The player's lives

    private int wave = 0;
    
    private int numEnemies = 20;//the only new variable (toy with on monday)

    private int health = 50; //changed it to 50 cos it was too low at first(we can toy with this on monday)

    private double currency = 100; /// The player's currency

    private bool gameOver = false;

    public event CurrencyChanged Changed;//triggered when currency is changed

    [SerializeField]
    private Text sizeText;

    [SerializeField]
    private Text statText;


    [SerializeField]
    private Text sellText;

    [SerializeField]
    private Text upgradePriceText;

    [SerializeField]
    private Text waveText;
    
    [SerializeField]
    private Text currencyText;

    [SerializeField]
    private Text livesText;

    [SerializeField]
    private GameObject gameOverMenu;

    [SerializeField]
    private GameObject towerPanel;

                
    [SerializeField]
    private GameObject statsPanel;

    [SerializeField]
    private GameObject upgradePanel;

    [SerializeField]
    private GameObject waveBtn;

    private Tower selectedTower; 

    public List<Enemy> activeEnemy = new List<Enemy>();

    public ObjectPool Pool { get; private set; }

    public TowerBtn ClickedBtn { get; set; }

    public double Currency /// Property for accessing the currency
    {
        get
        {
            return currency;
        }
        set
        {
            this.currency = value;
            currencyText.text = value.ToString() + " <color=lime>$</color>";
            OnCurrencyChanged();
        }
    }
    
    public int Lives /// Property for accessing the lives
    {
        get { return lives; }
        set
        {
            this.lives = value;
            if (lives <= 0)
            {
                lives = 0;
                GameOver();
            }
            livesText.text = value.ToString() +" <color=lime>$</color>";
        }
    }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>(); 
    }

    private void Update()
    {
        HandleEscape();
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void HandleEscape()
    {
        //If we hit esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Hover.Instance.Visible && selectedTower == null)
            {
                //show in game menu
            }

            else if(Hover.Instance.Visible)
            {
                DropTower();

            }
            else if(selectedTower != null){
                DeselectTower();
            }
          
        }
    }

    private void DropTower()
    {
        ClickedBtn = null;

        Hover.Instance.Deactive();
    }

    //towers
    public void PickTower(TowerBtn towerBtn)
    {
        if (Currency >= towerBtn.Price && activeEnemy.Count <= 0)
        {
            ClickedBtn = towerBtn;
            Hover.Instance.Active(ClickedBtn.Sprite);
        }
    }

    public void BuyTower()
    {

        //If we have currency buy tower
        if (Currency >= ClickedBtn.Price)
        {
            //Reduce currency
            Currency -= ClickedBtn.Price;

            ClickedBtn = null;
            //DropTower();
        }
    }

    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            //selects the tower
            selectedTower.Select();
        }

        //sets selected tower
        selectedTower = tower;

        //write sell price on tower
        sellText.text = "+ " + (selectedTower.Price / 2).ToString() + " $";

        //shows upgrade panel
        upgradePanel.SetActive(true);

        //tower has another upgrade?
        if(tower.NextUpgrade != null)
        {
            //Shows upgraded values
            upgradePriceText.text = tower.NextUpgrade.Price.ToString() + " $";
        }

        selectedTower.Select();
    }

    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        upgradePanel.SetActive(false);
        selectedTower = null;

    }

    public void SellTower()
    {   
        //if tower selected
        if (selectedTower != null)
        {
            //sells tower and refunds half price
            Currency += selectedTower.Price / 2;

            //makes tile walkable again
            selectedTower.GetComponentInParent<Tilescript>().IsEmpty = true;

            //removes tower
            Destroy(selectedTower.transform.parent.gameObject);

            //deselcts tower
            DeselectTower();
        }
    }

    public void OnCurrencyChanged()
    {
        if(Changed != null)
        {

            Changed();
        }
    }

    public void Showstats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    public void SetToolTipText(string txt)
    {
        statText.text = txt;
        sizeText.text = txt;
    }

    public void UpdateTooltip()
    {
        //if tower selected
        if(selectedTower != null)
        {
            //updates tooltip text
            sellText.text = "+ " + (selectedTower.Price / 2).ToString() + " $";
            SetToolTipText(selectedTower.GetStats());

            //If  we have a upgrade available, show the upgrade
            if (selectedTower.NextUpgrade != null)
            {
                upgradePriceText.text = selectedTower.NextUpgrade.Price.ToString() + "$";
            }
            else
            {
                upgradePriceText.text = string.Empty;
            }
        }
    }

    private void Start()
    {
        //Sets the lives and currency
        this.Lives = 30;
        Currency = 20;
    }

    //waves
    public bool WaveActive
    {
        get { return activeEnemy.Count > 0; }
    }

    public void StartWave()
    {
        //Increases the wave count
        wave++;

        if (wave > 1 && wave < 6)
        {
            if(wave == 2)
            {
                health = (health * 1) / 4;
            }
            else
            {
                health = (health * 1) / 4 + (health * 4) / 5;
            }
            //doubles the number of enemies after each wave ... we cant fill the map if it's less(we prolly might need more)
            numEnemies += numEnemies;
        }else if (wave == 6)
        {
            numEnemies = 4;
            health = 500;
        }

        print("awarding passive income: "); print(EconManager.Instance.EconLevel);
        //Award passive income
        Currency = Currency + EconManager.Instance.EconLevel;

        //Updates the wave text, so that the player can see it
        waveText.text = string.Format("Wave: <color=lime>{0}</color>", wave);

        //Starts to spawn the wave
        StartCoroutine(SpawnWave());

        //Hides the wavebutton and tower panel

        towerPanel.SetActive(false);
        waveBtn.SetActive(false);
    }

    public void UpgradeTower()
    {
        //If we have selected a tower
        if (selectedTower != null)
        {
            //Upgrades the tower
            if (selectedTower.Level <= selectedTower.Upgrades.Length && Currency >= selectedTower.NextUpgrade.Price)
            {
                selectedTower.Upgrade();
            }

        }
    }

    private IEnumerator SpawnWave()
    {
        LevelManager.Instance.GeneratePath ();
        for (int i = 0; i < numEnemies; i++)
        {
            string type = string.Empty;

            if (wave == 1)
            {
                type = "meeseek";
            }
            else if (wave == 2)
            {
                type = "duck";
            }
            else if (wave == 3)
            {
                type = "mrpoopybutthole";
            }
            else if (wave == 4)
            {
                type = "tinkles";
            }
            else if (wave == 5) {
                //possibly need to add another for wave 6 with different enemies 
                int enemyIndex = Random.Range(0, 4);

            switch (enemyIndex)
            {

                case 0:
                    type = "meeseek";
                    break;
                case 1:
                    type = "duck";
                    break;
                case 2:
                    type = "mrpoopybutthole";
                    break;
                case 3:
                    type = "tinkles";
                    break;
            }
            }
            else
            {
                for (int j = 1; j < 5; j++)
                {
                    if (j < 4)
                    {
                        //Head + 1 = Head1 and so on i think it should work
                        type = "Head" + j;
                    }
                    else //j = 4 so burris spawns
                    {
                        type = "Burris";
                    }
                }

                //the line below is only temporary cos the heads give me a null pointer exception
                //we will just delete it once the heads are fixed
                type = "meeseek";
            }
            Enemy enemy = Pool.GetObject (type).GetComponent<Enemy>(); 
            //get enemy heath from enemy script then assign in next line. 
            enemy.Spawn (health);
            //here is where we alter the health per wave
            //i commented this out cos i alter the hp in the start wave function
            /**if(wave%3 == 0)
            {
                health += 5; 
            }**/
            activeEnemy.Add(enemy);

            //also changed this cos it was too annoying waiting to count the numEnemies(was 2.5 at first if u wanna change it back)
            yield return new WaitForSeconds (1.0f);  
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        activeEnemy.Remove(enemy);
        if(!WaveActive && !gameOver)
        {
            waveBtn.SetActive(true);
            towerPanel.SetActive(true);
        }
    }
}
