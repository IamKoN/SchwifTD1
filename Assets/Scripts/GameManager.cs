using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private int lives = 15 ;/// The player's lives

    private int wave = 0;
    
    //Number of enemies at the start of wave 1
    private int numEnemies = 3;

    //Enemy health
    private int health = 50; 

    private double currency = 100; /// The player's currency

    private bool gameOver = false;

    //a counter for wave 6
    private int wave6 = 0;

    private Tower selectedTower;

    public List<Enemy> activeEnemy = new List<Enemy>();

    [SerializeField]
    private Text sizeText;

    [SerializeField]
    private Text nextWaveText;

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
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject optionsMenu;

    [SerializeField]
    private GameObject towerPanel;
                
    [SerializeField]
    private GameObject statsPanel;

    [SerializeField]
    private GameObject upgradePanel;

    [SerializeField]
    private GameObject waveBtn;

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

    //gets objects from th eobject pool
    private void Awake()
    {
        Pool = GetComponent<ObjectPool>(); 
    }

    //updates every frame
    private void Update()
    {
        HandleEscape();
    }

    //method to put gameover menu visble
    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }

    //Restarts game
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        pauseMenu.SetActive(false);
    }

    //Quits game
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
                pauseMenu.SetActive(true);//show in game menu
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

    public void HideMenu()
    {
        pauseMenu.SetActive(false);
    }

    public void ShowOptionsMenu()
    {
        optionsMenu.SetActive(true);
    }

    public void HideOptionsMenu()
    {
        optionsMenu.SetActive(false);
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

    private void DropTower()
    {
        ClickedBtn = null;

        Hover.Instance.Deactive();
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
        this.Lives = 10;
        Currency = 50;
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
                health += (health * 1) / 4;
            }
            else
            {
                health += (health * 1) / 4 + (health * 4) / 5;
            }
            //doubles the number of enemies after each wave ... we cant fill the map if it's less(we prolly might need more)
            numEnemies += numEnemies;
        }
        else if (wave == 6)
        {
            numEnemies = 4;
            health = 3500;
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
        string type = string.Empty;

        string SFX = string.Empty;
        LevelManager.Instance.GeneratePath ();
        for (int i = 0; i < numEnemies; i++)
        {
          
            if (wave == 1)
            {
                type = "meeseek";
                SFX = "im-mr";
              
            }
            else if (wave == 2)
            {
                type = "duck";
                SFX = "duck";
            }
            else if (wave == 3)
            {
                type = "mrpoopybutthole";
                SFX = "owee_1";
               
            }
            else if (wave == 4)
            {
                type = "tinkles";
                SFX = "tinkles";
            }
            else if (wave == 5)
            {
                int enemyIndex = Random.Range(0, 4);

                switch (enemyIndex)
                {

                    case 0:
                    type = "meeseek";
                    SFX = "im-mr";
                        break;
                    case 1:
                    type = "duck";
                        SFX = "duck";
                    break;
                     case 2:
                    type = "mrpoopybutthole";
                        SFX = "owee_1";
                        break;
                    case 3:
                    type = "tinkles";
                        SFX = "tinkles";
                        break;
                }
            }
            else
            {
                wave6++;
                if(wave6 == 1)
                { 
                        type = "Head1";// + j;
                        SFX = "show-me-what-you-got";
                }
                if (wave6 == 2)
                {
                    type = "Head1";// + j;
                    SFX = "show-me-what-you-got";
                }
                if (wave6 == 3)
                {
                    type = "Head1";// + j;
                    SFX = "show-me-what-you-got";
                }
                if (wave6 > 3) //j = 4 so burris spawns
                {
                        type = "Burris";
                        SFX = "shallnotpass";
                    
                }

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
            SoundManager.Instance.PlaySFX(SFX);

            yield return new WaitForSeconds (2.0f);
   
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
