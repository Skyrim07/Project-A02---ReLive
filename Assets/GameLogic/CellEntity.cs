using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;

public sealed class CellEntity : MonoBehaviour
{
    public bool canDrag = true;
    private bool oCanDrag;

    private Vector3 mouseOffset = new Vector3(2.5f, 0f, 0f);
    public int statusID = 0;

    [SerializeField] GameObject status0, status1;
    [SerializeField] SpriteRenderer[] sr;

    public Vector3 currentCellPos;

    private Animator anim;
    private Collider2D cld;
    public GameObject[] cldGO;
    float f = 0;

    private void Awake()
    {
        oCanDrag = canDrag;
        cld = GetComponent<Collider2D>();
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        UpdateStatus();

        if (!canDrag)
        {
            Vector2Int cell = MapManager.instance.GetGridFromCell(transform.position);
            int mapid = MapManager.instance.GetMapIDFromCell(transform.position);
            MapManager.instance.SetCellOccupancy(mapid, cell.x, cell.y, true);
        }
    }
    public void SetSortingLayer(int layer)
    {
        foreach (var s in sr)
        {
            s.sortingOrder = layer;
        }
    }
    public void UpdateStatus()
    {
        int mapid = MapManager.instance.GetMapIDFromCell(transform.position);
        status0.SetActive(mapid == 0);
        status1.SetActive(mapid == 1);
    }

    private void OnMouseDown()
    {
        if (!canDrag||UIManager.instance.inUI)
            return;
        SKAudioManager.instance.PlaySound("border");
        MapManager.instance.draggingCell = this;

        Vector2Int cell = MapManager.instance.GetGridFromCell();
        int mapid = MapManager.instance.GetMapIDFromCell();
        if (mapid < 0)
            return;
        MapManager.instance.SetCellOccupancy(mapid, cell.x, cell.y, false);
        anim.SetBool("Up", true);

        MapManager.instance.SetDefaultGridColor();
        if (cld)
        {
            cld.enabled = false;
        }
        foreach (var go in cldGO)
        {
            go.SetActive(false);
        }
    }
    private void OnMouseUp()
    {
        if (!canDrag || UIManager.instance.inUI)
            return;

        Vector2Int cell = MapManager.instance.GetGridFromCell();
        int mapid = MapManager.instance.GetMapIDFromCell();
        if (mapid < 0)
            return;
        if (MapManager.instance.GetCellOccupancy(mapid, cell.x, cell.y))
            cell.x = -1;
        Vector3 oPos = transform.position;

        if (cell.x < 0)
        {
            CommonUtils.StartProcedure(SKCurve.CubicIn, f, 0.15f, (f) =>
            {
                transform.position = oPos + (currentCellPos - oPos) * f;
            }, (f) =>
            {
                cell = MapManager.instance.GetGridFromCell();
                this.SetSortingLayer(100 - cell.y * 10);
                MapManager.instance.UpdateCellOccupancy();
                MapManager.instance.draggingCell = null;
                SKAudioManager.instance.PlaySound("tile");
                if (cld)
                {
                    cld.enabled = true;
                }
                foreach(var go in cldGO)
                {
                    go.SetActive(true);
                }

            });
        }
        else
        {
            Vector3 tPos = MapManager.instance.GetWorldPosFromCell(cell);
            CommonUtils.StartProcedure(SKCurve.CubicIn, f, 0.15f, (f) =>
            {
                transform.position = oPos + (tPos - oPos) * f;
            }, (f) =>
            {
                MapManager.instance.PlayPuffEffect(transform.position);

                cell = MapManager.instance.GetGridFromCell();
                this.SetSortingLayer(100 - cell.y * 10);
                MapManager.instance.UpdateCellOccupancy();
                MapManager.instance.draggingCell = null;
                SKAudioManager.instance.PlaySound("tile");
                if (cld)
                {
                    cld.enabled = true;
                }
                foreach (var go in cldGO)
                {
                    go.SetActive(true);
                }
                UpdateStatus();


            });
            if(currentCellPos!=tPos)
                OnMoveSuccess(mapid);
            currentCellPos = tPos;
        }
        anim.SetBool("Up", false);
        MapManager.instance.SetEmptyGridColor();
    }
    private void OnMouseDrag()
    {
        if (!canDrag || UIManager.instance.inUI)
            return;

        FollowMouse();
        MapManager.instance.OnDragCell();
    }

    public void OnMoveSuccess(int mapid)
    {
        if (mapid == 0)
            FlowManager.instance.currentMoveCountL--;
        if (mapid == 1)
            FlowManager.instance.currentMoveCountR--;
        UIManager.instance.UpdateMoveText();

        if(FlowManager.instance.currentMoveCountL<=0|| FlowManager.instance.currentMoveCountR <= 0)
        {
            UIManager.instance.OnMoveDepleted();
        }
    }
    private void FollowMouse()
    {
        Vector3 worldPos = CommonReference.cam.ScreenToWorldPoint(Input.mousePosition);
        worldPos = new Vector3(worldPos.x, worldPos.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, worldPos, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && cld && cld.enabled)
        {
            canDrag = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && cld && cld.enabled)
        {
            if(oCanDrag)
            canDrag = true;
        }
    }
}
