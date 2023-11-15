using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILine : Graphic
{
    public List<Vector2> _PointsList;

    public RectTransform _Source;
    public RectTransform _Target;

    float _Width;
    float _Height;

    public Canvas _Canvas;
    //float UnitWidth;
    //float UnitHeight;

    public float Thickness = 10f;

    protected override void OnPopulateMesh(VertexHelper _VertexHelper)
    {
        _VertexHelper.Clear();

        

        //_Width = rectTransform.rect.width;
        //_Height = rectTransform.rect.height;

        //var _SourcePoint = _Source.position;
        //DrawVerticesForPoint(_SourcePoint, _VertexHelper);

        //var _TargetPoint = _Target.position;
        //DrawVerticesForPoint(_TargetPoint, _VertexHelper);        

        for (int i = 0; i < _PointsList.Count; i++)
        {
            Vector2 _Point = _PointsList[i];

            DrawVerticesForPoint(_Point, _VertexHelper);
        }

        for (int i = 0; i < _PointsList.Count-1; i++)
        {
            int _Index = i * 2;

            _VertexHelper.AddTriangle(_Index + 0, _Index + 1, _Index + 3);
            _VertexHelper.AddTriangle(_Index + 3, _Index + 2, _Index + 0);
        }
    }

    private void Update()
    {
        _PointsList.Clear();
        _PointsList.Add(_Source.position / _Canvas.scaleFactor);
        _PointsList.Add(_Target.position / _Canvas.scaleFactor);

        SetVerticesDirty();
    }

    void DrawVerticesForPoint(Vector2 _Point, VertexHelper _VertexHelper)
    {
        UIVertex _Vertex = UIVertex.simpleVert;
        _Vertex.color = color;

        _Vertex.position = new Vector3(-Thickness / 2, 0);
        _Vertex.position += new Vector3(_Point.x, _Point.y);
        _VertexHelper.AddVert(_Vertex);

        _Vertex.position = new Vector3(Thickness / 2, 0);
        _Vertex.position += new Vector3(_Point.x, _Point.y);
        _VertexHelper.AddVert(_Vertex);
    }
}
