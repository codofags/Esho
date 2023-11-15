using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathController : MonoBehaviour
{

    public static PathController ins;

    [SerializeField]
    private Transform imageContainer;

    private float cellSize;

    private float imageContainerDesirePosition;

    [SerializeField]
    private float SmoothSpeed = 1;

    public bool canMove = false;

    void Start()
    {
        ins = this;

        Setup();

        List<GameObject> dsfg = new List<GameObject>();

        Destroy(dsfg[0].gameObject);
    }

    private void Setup()
    {
        var imageCount = imageContainer.childCount;

        var gridLayout = imageContainer.GetComponent<GridLayoutGroup>();

        var gridSpacing = gridLayout.spacing;
        var gridCellSize = gridLayout.cellSize;
        var gridPadding = gridLayout.padding;

        var desireWidth = imageCount * gridCellSize.x + (imageCount - 1) * gridSpacing.x + gridPadding.left + gridPadding.right;

        imageContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(desireWidth, 852);

        cellSize = (imageCount * gridCellSize.x + (imageCount) * gridSpacing.x) / imageCount;

        Debug.Log($"cellSize {cellSize}");
    }

    public void OnImageContainerDrop()
    {
        canMove = true;

        var x = imageContainer.localPosition.x;

        x = x - 12.5f;

        var cell = (int)(x / cellSize);

        imageContainerDesirePosition = cell * cellSize+165;
    }

    void Update()
    {
        //if (canMove)
        //{
        //    var currentPosition = imageContainer.localPosition;
        //    var desirePosition = new Vector3(imageContainerDesirePosition, 0, 0);

        //    imageContainer.localPosition = Vector3.Lerp(currentPosition, desirePosition, Time.deltaTime * SmoothSpeed);
        //}
    }
}
