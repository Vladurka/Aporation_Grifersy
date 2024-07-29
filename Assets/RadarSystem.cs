using UnityEngine;

public enum Alignment
{
    None,
    LeftTop,
    RightTop,
    LeftBot,
    RightBot,
    MiddleTop,
    MiddleBot
}
public class RadarSystem : MonoBehaviour
{
    [SerializeField] private float _size = 350;
    [SerializeField] private float _distance = 1500;
    [SerializeField] private float _transparency = 1f;
    [SerializeField] private Texture2D[] _navtexture;
    [SerializeField] private string[] _enemyTag;
    [SerializeField] private Texture2D _navCompass;
    [SerializeField] private Texture2D _compassBackground;
    [SerializeField] private Vector2 _positionOffset = new Vector2(0, 0);
    [SerializeField] private float _scale = 2;
    [SerializeField] private Alignment _positionAlignment = Alignment.None;
    [SerializeField] private bool _mapRotation;
    [SerializeField] private GameObject _player;
    [SerializeField] private bool _show = true;
    [SerializeField] private Color _colorMult = Color.white;

    private Vector2 _inposition;

    private float[] list;

    void Update()
    {
        if (!_player)
        {
            _player = this.gameObject;
        }

        if (_scale <= 0)
        {
            _scale = 100;
        }

        switch (_positionAlignment)
        {
            case Alignment.None:
                _inposition = _positionOffset;
                break;
            case Alignment.LeftTop:
                _inposition = Vector2.zero + _positionOffset;
                break;
            case Alignment.RightTop:
                _inposition = new Vector2(Screen.width - _size, 0) + _positionOffset;
                break;
            case Alignment.LeftBot:
                _inposition = new Vector2(0, Screen.height - _size) + _positionOffset;
                break;
            case Alignment.RightBot:
                _inposition = new Vector2(Screen.width - _size, Screen.height - _size) + _positionOffset;
                break;
            case Alignment.MiddleTop:
                _inposition = new Vector2((Screen.width / 2) - (_size / 2), _size) + _positionOffset;
                break;
            case Alignment.MiddleBot:
                _inposition = new Vector2((Screen.width / 2) - (_size / 2), Screen.height - _size) + _positionOffset;
                break;
        }

    }
    private Vector2 ConvertToNavPosition(Vector3 pos)
    {
        Vector2 res = Vector2.zero;
        if (_player)
        {
            float navRadius = _size / 2f;
            Vector2 radarCenter = _inposition + new Vector2(navRadius, navRadius);

            Vector2 deltaPos = new Vector2(pos.x - _player.transform.position.x, pos.z - _player.transform.position.z);
            float distance = deltaPos.magnitude;

            if (distance > _distance)
            {
                // Clamp the position to the edge of the radar
                deltaPos *= _distance / distance;
            }

            res.x = radarCenter.x + (deltaPos.x / _scale);
            res.y = radarCenter.y - (deltaPos.y / _scale);
        }
        return res;
    }

    private void DrawNav(GameObject[] enemylists, Texture2D navtexture)
    {
        if (_player)
        {
            float navRadius = _size / 2f;
            Vector2 radarCenter = _inposition + new Vector2(navRadius, navRadius);

            for (int i = 0; i < enemylists.Length; i++)
            {
                if (Vector3.Distance(_player.transform.position, enemylists[i].transform.position) <= (_distance * _scale))
                {
                    Vector2 pos = ConvertToNavPosition(enemylists[i].transform.position);

                    if (Vector2.Distance(pos, radarCenter) + (navtexture.width / _scale) < navRadius)
                    {
                        if (_scale < 1)
                        {
                            _scale = 1;
                        }
                        GUI.DrawTexture(new Rect(pos.x - (navtexture.width / _scale) / 2, pos.y - (navtexture.height / _scale) / 2, navtexture.width / _scale, navtexture.height / _scale), navtexture);
                    }
                    else
                    {
                        // Adjust the position to be on the edge
                        Vector2 direction = (pos - radarCenter).normalized;
                        pos = radarCenter + direction * (navRadius - (navtexture.width / _scale) / 2);
                        GUI.DrawTexture(new Rect(pos.x - (navtexture.width / _scale) / 2, pos.y - (navtexture.height / _scale) / 2, navtexture.width / _scale, navtexture.height / _scale), navtexture);
                    }
                }
            }
        }
    }


    private void OnGUI()
    {
        if (!_show)
            return;

        GUI.color = new Color(_colorMult.r, _colorMult.g, _colorMult.b, _transparency);
        if (_mapRotation)
        {
            GUIUtility.RotateAroundPivot(-(this.transform.eulerAngles.y), _inposition + new Vector2(_size / 2f, _size / 2f));
        }

        for (int i = 0; i < _enemyTag.Length; i++)
        {
            DrawNav(GameObject.FindGameObjectsWithTag(_enemyTag[i]), _navtexture[i]);
        }
        if (_compassBackground)
            GUI.DrawTexture(new Rect(_inposition.x, _inposition.y, _size, _size), _compassBackground);
        GUIUtility.RotateAroundPivot((this.transform.eulerAngles.y), _inposition + new Vector2(_size / 2f, _size / 2f));
        if (_navCompass)
            GUI.DrawTexture(new Rect(_inposition.x + (_size / 2f) - (_navCompass.width / 2f), _inposition.y + (_size / 2f) - (_navCompass.height / 2f), _navCompass.width, _navCompass.height), _navCompass);

    }
}
