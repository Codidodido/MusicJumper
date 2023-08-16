using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CommandCenter : MonoBehaviour
{
    
    [SerializeField] MusicNote musicNote;
    [SerializeField] GameObject platform;
    [SerializeField] float screenWidthX;
    List<Color> _colorList = new List<Color>();

    private GameObject _lastObject;
    private bool _isContinuous;
    private int _lastIndex = -4;
    private float _percentPerBpm;

    Jumper _player;
    float _bpm;
    public float GetPercentPerBmp()
    {
        return _percentPerBpm;
    }
    public Color CreateColor(float r, float g, float b)
    {
        return new Color(r / 255, g / 255, b / 255);
    }
    void Start()
    {
        _isContinuous = false;
        _colorList = new List<Color> { Color.blue, CreateColor(163f, 73f, 164f), Color.red, CreateColor(255f, 127f, 39f), Color.yellow, Color.green, CreateColor(34f, 177f, 76f) };
        _bpm = musicNote.GetBpm();
        _percentPerBpm = Mathf.InverseLerp(20f, 120f,_bpm);

        float jumpValue = Mathf.Lerp(28f, 16f, _percentPerBpm);
        float speedValue = Mathf.Lerp(10f, 20f, _percentPerBpm);
        _player = FindObjectOfType<Jumper>();

        _player.SetJumpForce(jumpValue);
        _player.SetMoveSpeed(speedValue);

        DisplayPlatforms();

    }

    void DisplayPlatforms()
    {
        
        for(int i = 0; i < musicNote.musicSheet.Count; i++)
        {
            int duration = musicNote.musicSheet[i].duration;
            
            Notes noteName = musicNote.musicSheet[i].noteName;
            
            if (duration - _lastIndex <= 3f)
            {
                _isContinuous = true;
            }
            else
            {
                _isContinuous = false;
                
            }
            
            Create(noteName,duration);
            
            _lastIndex = duration;



            int listIndex = duration;
            if (listIndex == 0)
            {
                Vector2 position = FindObjectOfType<Platform>().transform.position;
                Jumper player = FindObjectOfType<Jumper>();
                var transform1 = player.transform;
                position.y = transform1.position.y;

                transform1.position = position;
            }

        }
        
    }

    void Create(Notes noteName , int noteDuration)
    {
        var randomX = UnityEngine.Random.Range(-screenWidthX, screenWidthX - 1f);

        if (Camera.main != null)
        {
            float y = (2 * (1 / ( _bpm / 60)) * noteDuration) - (Camera.main.orthographicSize / 2);
            Debug.Log(y);

            if (_isContinuous)
            {
                randomX = _lastObject.transform.position.x;
            }

            GameObject newPlatform = Instantiate(platform, new Vector2(randomX, y), Quaternion.identity);
            _lastObject = newPlatform;
        }

        platform.GetComponent<Platform>().SetNote(noteName);
        platform.GetComponent<SpriteRenderer>().color = _colorList[noteDuration % 7];
    }
    
    
}
