using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;

    public Text score;

    public Text winText;

    public Text lives;

    public Text loseText;

    private int scoreValue = 0;

    private int livesValue = 3;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioSource musicSource;

    public Animator anim;

    private bool facingRight = true;

    private bool isOnGround;

    public Transform groundcheck;

    public float checkRadius;

    public LayerMask allGround;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        winText.text = "";
        loseText.text = "";
        musicSource.clip = musicClipOne;
        musicSource.Play();
        anim = GetComponent <Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        if (!isOnGround)
        {
            anim.SetInteger("State", 2);
        }
        else if (hozMovement != 0)
            {
                anim.SetInteger("State", 1);
            } else {
                    anim.SetInteger("State", 0);
                    }
        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if (scoreValue == 4)
            {
                transform.position = new Vector2(80.0f,1);
                livesValue = 3;
                lives.text = "Lives: " + livesValue.ToString();
            }
            
            if (scoreValue == 8)
            {
                winText.text = "You Win! Created by Jared Toavs";
                musicSource.clip = musicClipTwo;
                musicSource.Play();
            }
        }
       if (collision.collider.tag == "Enemy")
       {
           livesValue -= 1;
           lives.text = "Lives: " + livesValue.ToString();
           Destroy(collision.collider.gameObject);

           if (livesValue == 0)
           {
               loseText.text = "You lose! Created by Jared Toavs";
               GetComponent<SpriteRenderer>().enabled = false;
               speed = 0;
               musicSource.Stop();
           }
       }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }
}