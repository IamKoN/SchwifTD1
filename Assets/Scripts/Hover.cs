using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hover Icon
public class Hover : Singleton<Hover> {

	private SpriteRenderer spriteRenderer;

    private SpriteRenderer rangeSpriteRenderer; 

    //Indicates visibility of hover icon
    public bool Visible { get; set; }

    //float cameraX = 17.68f;

    //float cameraY = -7.37f;

	
	// Update is called once per frame
	void Update () {
        if (spriteRenderer.enabled) //If the hover is enabled
        {
            FollowMouse();
        }
    }

	private void FollowMouse(){
		if (spriteRenderer.enabled) {
            //Camera.main.transform.position = new Vector3(17.68f, -7.37f, -10f);
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3 (transform.position.x, transform.position.y, 0); 
		}
	}

    void Awake()
    {
        //Creates the references
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.rangeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    } 

    //Activates hover icon
    public void Active(Sprite sprite){
        Visible = true;
		spriteRenderer.enabled = true;
        rangeSpriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
	}

    //Deavtivates hover icon
	public void Deactive(){
        Visible = false;
        rangeSpriteRenderer.enabled = false;
        spriteRenderer.enabled = false;
        //GameManager.Instance.ClickedBtn = null;

    }
}
