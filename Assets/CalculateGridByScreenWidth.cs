using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalculateGridByScreenWidth : MonoBehaviour
{
    [SerializeField] private RectTransform canvas;

    private GridLayoutGroup grid;

    [SerializeField] private bool affectImagePagination;
    [SerializeField] private ImagePagination imagePagination;

   
    void Start()
    {
        grid = GetComponent<GridLayoutGroup>();

        //������ ������������� �������
        var canvasWidth = canvas.rect.width;       

        //����� �������
        var cellCountX = grid.constraintCount;

        //������� �������
        var offsetX = grid.padding.left + grid.padding.right;

        //���������� ����� ��������
        var spacingX = grid.spacing.x * (cellCountX - 1);

        //Debug.Log($"canvasWidth {canvasWidth} cellCountX {cellCountX} offsetX {offsetX} spacingX {spacingX}");

        var cellSize = grid.cellSize;

        cellSize = new Vector2((canvasWidth / cellCountX) - spacingX - (offsetX/4), cellSize.y);

        grid.cellSize = cellSize;

        if (affectImagePagination)
        {
            imagePagination.cellSizeX = (int)cellSize.x;
        }
    }

   
}
