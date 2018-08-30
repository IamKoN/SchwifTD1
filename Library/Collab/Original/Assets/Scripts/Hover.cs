using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover> {

	private SpriteRenderer spriteRenderer;

    private SpriteRenderer rangeSpriteRenderer; 

	// Use this for initialization
	void Start () {
		this.spriteRenderer = GetComponent<SpriteRenderer> ();
        this.rangeSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (spriteRenderer.enabled) //If the hover is enabled
        {
            FollowMouse();
        }
    }

	private void FollowMouse(){
		if (spriteRenderer.enabled) {
			transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0); 
		}
	}

	public void Active(Sprite sprite){
		spriteRenderer.enabled = true;
        rangeSpriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
	}

	public void Deactive(){
        rangeSpriteRenderer.enabled = false;
        spriteRenderer.enabled = false;
        GameManager.Instance.ClickedBtn = null;
	}
}
