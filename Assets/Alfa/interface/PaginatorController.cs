using UnityEngine;
using UnityEngine.UI;

public class PaginatorController : MonoBehaviour
{
    [SerializeField] private Image image;

    public void Enable()
    {
        image.color = ButtonManager.ins.activePaginationColor;
    }

    internal void Disable()
    {
        image.color = ButtonManager.ins.passivePaginationColor;
    }
}
