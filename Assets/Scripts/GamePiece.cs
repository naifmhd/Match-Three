using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MatchValue
{
    Yellow, Blue, Indigo, Magenta, Green, Cyan, Red, Teal, Wild, None
}

public class GamePiece : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    Board m_board;

    bool m_isMoving = false;

    public InterpType interpolation = InterpType.SmootherStep;

    public MatchValue matchValue;

    public enum InterpType
    {
        Linear,EaseOut,EaseIn,SmoothStep,SmootherStep
    }
    public int scoreValue = 20;

    public AudioClip clearSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(Board board)
    {
        m_board = board;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move((int)transform.position.x + 2, (int)transform.position.y, 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move((int)transform.position.x - 2, (int)transform.position.y, 0.5f);
        }
    }

    public void  SetCoord(int x,int y)
    {
        xIndex = x;
        yIndex = y;
    }

    public void Move(int destX, int destY, float timeToMove)
    {
        if (!m_isMoving)
        { 
            StartCoroutine(MoveRoutine(new Vector3(destX, destY), timeToMove));
        }
    }

    IEnumerator MoveRoutine(Vector3 destination,float timeToMove)
    {
        Vector3 startPosition = transform.position;
        bool reachedDestination = false;
        float elapsedTime = 0f;

        m_isMoving = true;
        while (!reachedDestination)
        {
            if (Vector3.Distance(transform.position, destination) < 0.01f)
            {
                reachedDestination = true;
                if (m_board != null)
                {
                    m_board.PlaceGamePiece(this, (int)destination.x, (int)destination.y);
                }
                break;
            }

            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp(elapsedTime / timeToMove,0f,1f);

            switch (interpolation)
            {
                case InterpType.Linear:
                    break;
                case InterpType.EaseIn:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;
                case InterpType.EaseOut:
                    t = Mathf.Sin(t * Mathf.PI * 0.5f);
                    break;
                case InterpType.SmoothStep:
                    t = t * t * (3 - 2 * t);
                    break;
                case InterpType.SmootherStep:
                    t = t * t * t * (t * (t * 6 - 15) + 10);
                    break;
            }

            transform.position = Vector3.Lerp(startPosition, destination, t);
            yield return null;
        }
        m_isMoving = false;
    }

    public void ChangeColor(GamePiece pieceToMatch)
    {
        SpriteRenderer renderToChange = GetComponent<SpriteRenderer>();
        Color colorToMatch = Color.clear;
        if (pieceToMatch != null)
        {
            SpriteRenderer rendererToMatch = pieceToMatch.GetComponent<SpriteRenderer>();
            if(rendererToMatch!=null && renderToChange != null)
            {
                renderToChange.color = rendererToMatch.color;
            }
            matchValue = pieceToMatch.matchValue;
        }
    }

    public void ScorePoints(int multiplier=1,int bonus=0)
    {
        if(ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue*multiplier+bonus);
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayClipAtPoint(clearSound,Vector3.zero,SoundManager.Instance.fxVolume);
        }
    }
}
