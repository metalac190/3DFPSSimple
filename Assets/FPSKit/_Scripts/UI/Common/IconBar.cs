using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconBar : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] 
    private Image _iconPrefab = null;
    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup;

    [Header("Settings")]
    [SerializeField]
    private int _iconSize = 25;
    [SerializeField]
    private int _iconSpacing = 3;

    private List<Image> _icons = new List<Image>();
    public int Count => _icons.Count;

    private Coroutine _refreshRoutine;

    public void CreateIcons(int count)
    {
        _icons.Clear();
        // resize
        ConstrainGridPadding();
        // create all possible icons
        for (int i = 0; i < count; i++)
        {
            CreateIcon();
        }
    }

    public void FillIcons(int fillValue)
    {
        for (int i = 0; i < _icons.Count; i++)
        {
            // fill the requested number of icons
            if(i < fillValue)
                EnableIcon(_icons[i]);
            // clear the rest
            else
                DisableIcon(_icons[i]); 
        }

        if (_refreshRoutine != null)
            StopCoroutine(_refreshRoutine);
        _refreshRoutine = StartCoroutine(RefreshLayoutGroup());
    }

    private Image CreateIcon()
    {
        Image icon = Instantiate(_iconPrefab, _gridLayoutGroup.transform);
        _icons.Add(icon);
        DisableIcon(icon);
        return icon;
    }

    private void EnableIcon(Image image)
    {
        image.gameObject.SetActive(true);
    }

    private void DisableIcon(Image image)
    {
        image.gameObject.SetActive(false);
    }

    private void ConstrainGridPadding()
    {
        // constrain grid padding to size of our entire UI object
        // note this currently only supports square icons
        _gridLayoutGroup.cellSize = new Vector2(_iconSize, _iconSize);
        _gridLayoutGroup.spacing = new Vector2(_iconSpacing, _iconSpacing);

        if (_refreshRoutine != null)
            StopCoroutine(_refreshRoutine);
        _refreshRoutine = StartCoroutine(RefreshLayoutGroup());
    }

    private IEnumerator RefreshLayoutGroup()
    {
        _gridLayoutGroup.enabled = true;
        yield return null;
        _gridLayoutGroup.enabled = false;
    }
}
