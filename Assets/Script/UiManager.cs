using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _aiPointText;
    [SerializeField] private TextMeshProUGUI _playerPointText;

    private int _aiPoint;
    private int _playerPoint;
    private StringBuilder _stringBuilder = new StringBuilder();

    #region Singleton

    public static UiManager Instance { get; private set; }
    
    #endregion
    
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUi();
    }

    void UpdateUi()
    {
        _stringBuilder.Append(_aiPoint);
        _aiPointText.text = _stringBuilder.ToString();
        _stringBuilder.Clear();

        _stringBuilder.Append(_playerPoint);
        _playerPointText.text = _stringBuilder.ToString();
        _stringBuilder.Clear();
    }

    public void GetPoint(bool isPlayer)
    {
        if (isPlayer)
            _playerPoint += 10;
        else
        {
            _aiPoint += 10;
        }
        UpdateUi();
    }
}
