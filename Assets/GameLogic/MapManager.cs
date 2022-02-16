using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SKCell;
public sealed class MapManager : MonoSingleton<MapManager>
{
    [SerializeField] Transform cellContainer, emptyCellContainer;
    [SerializeField] SKGridLayer leftGrid, rightGrid;

    [SerializeField] GameObject cellpf;

    private GameObject[,] emptyCellsL = new GameObject[7, 7];
    private GameObject[,] emptyCellsR = new GameObject[7, 7];

    public CellEntity draggingCell;

    public Color selectColor = new Color(.8f, .8f, .1f, .3f);
    public Color defaultColor = new Color(0, 0, 0, .2f);

    float ec, dc;

    private void Start()
    {
        SetEmptyGridColor();

        //CommonUtils.InvokeAction(1f, () => { UpdateCellOccupancy(); });
    }

    public void LoadMap(int level)
    {
        leftGrid = GameObject.Find("LeftGrid").GetComponent<SKGridLayer>();
        rightGrid = GameObject.Find("RightGrid").GetComponent<SKGridLayer>();
        cellContainer = GameObject.Find("CellContainer").transform;
        emptyCellContainer = GameObject.Find("EmptyCellContainer").transform;

        emptyCellsL = new GameObject[10,10];
        emptyCellsR = new GameObject[10,10];

        Vector3 pos = Vector3.zero;
        GameObject go;
        int[,] leftMap = MapData.mapTiles[level * 2];
        int[,] rightMap = MapData.mapTiles[level * 2 + 1];

        for (int i = 0; i < leftMap.GetLength(0); i++)
        {
            for (int j = 0; j < leftMap.GetLength(1); j++)
            {
                int ii = i;
                int jj = j;
                pos = leftGrid.GetWorldPosFromCell(new Vector2Int(i, j));

                if (leftMap[ii, jj] > 0)
                {
                    go = Instantiate(CommonReference.instance.tilePrefabs[leftMap[ii, jj]], pos, Quaternion.identity);
                    go.GetComponent<CellEntity>().SetSortingLayer(100 - j * 10);
                    go.GetComponent<CellEntity>().currentCellPos = pos;
                    go.transform.SetParent(cellContainer);
                }

                pos = rightGrid.GetWorldPosFromCell(new Vector2Int(i, j));
                if (rightMap[ii, jj] > 0)
                {
                    go = Instantiate(CommonReference.instance.tilePrefabs[rightMap[ii, jj]], pos, Quaternion.identity);
                    go.GetComponent<CellEntity>().SetSortingLayer(100 - j * 10);
                    go.GetComponent<CellEntity>().currentCellPos = pos;
                    go.transform.SetParent(cellContainer);
                }
            }
        }

        for (int i = 0; i < leftMap.GetLength(0); i++)
        {
            for (int j = 0; j < leftMap.GetLength(1); j++)
            {
                pos = leftGrid.GetWorldPosFromCell(new Vector2Int(i, j));
                go = Instantiate(CommonReference.instance.tilePrefabs[0], pos, Quaternion.identity);
                go.GetComponent<CellEntity>().currentCellPos = pos;
                go.transform.SetParent(emptyCellContainer, true);
                emptyCellsL[i, j] = go;

                pos = rightGrid.GetWorldPosFromCell(new Vector2Int(i, j));
                go = Instantiate(CommonReference.instance.tilePrefabs[0], pos, Quaternion.identity);
                go.GetComponent<CellEntity>().currentCellPos = pos;
                go.transform.SetParent(emptyCellContainer, true);
                emptyCellsR[i, j] = go;
            }
        }

        CommonUtils.InvokeAction(0.2f, UpdateCellOccupancy);
    }

    public void Test()
    {
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i + j > 2)
                    return;
                pos = leftGrid.GetWorldPosFromCell(new Vector2Int(i, j));
                GameObject go = Instantiate(cellpf, pos, Quaternion.identity);
                go.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 100 - j * 10;
                go.GetComponent<CellEntity>().currentCellPos = pos;
                go.transform.SetParent(cellContainer);
                SetCellOccupancy(0, i, j, true);
            }
        }
    }

    public void OnDragCell()
    {
        if (draggingCell == null)
            return;
        SKGridLayer grid = draggingCell.transform.position.x < CommonReference.instance.center.position.x ? leftGrid : rightGrid;
        Vector2Int cell = grid.GetCellFromWorldPos(draggingCell.transform.position);

        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                if (i == cell.x && j == cell.y)
                    grid.SetCellColor(i, j, selectColor, false);
                else
                    grid.SetCellColor(i, j, defaultColor, false);
            }
        }
        grid.ApplyColorChanges();
        if (cell.x < 0)
        {
            return;
        }

        draggingCell.SetSortingLayer(100 - cell.y * 10);
    }

    public void SetDefaultGridColor()
    {
        CommonUtils.StartProcedure(SKCurve.QuadraticIn, dc, 0.2f, (dc) =>
        {
            Color c = Color.Lerp(Color.clear, defaultColor, dc);
            for (int i = 0; i < leftGrid.width; i++)
            {
                for (int j = 0; j < leftGrid.height; j++)
                {
                    leftGrid.SetCellColor(i, j, c, false);
                    rightGrid.SetCellColor(i, j, c, false);
                }
            }
            leftGrid.ApplyColorChanges();
            rightGrid.ApplyColorChanges();
        });
    }
    public void SetEmptyGridColor()
    {
        CommonUtils.StartProcedure(SKCurve.QuadraticIn, ec, 0.2f, (ec) =>
        {
            Color c = Color.Lerp(defaultColor, Color.clear, ec);
            for (int i = 0; i < leftGrid.width; i++)
            {
                for (int j = 0; j < leftGrid.height; j++)
                {
                    leftGrid.SetCellColor(i, j, c, false);
                    rightGrid.SetCellColor(i, j, c, false);
                }
            }
            leftGrid.ApplyColorChanges();
            rightGrid.ApplyColorChanges();
        });
    }
    public Vector2Int GetGridFromCell()
    {
        if (draggingCell == null)
            return new Vector2Int(-1, -1);
        SKGridLayer grid = draggingCell.transform.position.x < CommonReference.instance.center.position.x ? leftGrid : rightGrid;
        Vector2Int cell = grid.GetCellFromWorldPos(draggingCell.transform.position);
        return cell;
    }
    public Vector2Int GetGridFromCell(Vector3 pos)
    {
        SKGridLayer grid = pos.x < CommonReference.instance.center.position.x ? leftGrid : rightGrid;
        Vector2Int cell = grid.GetCellFromWorldPos(pos);
        return cell;
    }

    public int GetMapIDFromCell()
    {
        if (draggingCell == null)
            return -1;
        return draggingCell.transform.position.x < CommonReference.instance.center.position.x ? 0 : 1;
    }
    public int GetMapIDFromCell(Vector3 pos)
    {
        return pos.x < CommonReference.instance.center.position.x ? 0 : 1;
    }
    public Vector3 GetWorldPosFromCell(Vector2Int x)
    {
        SKGridLayer grid = draggingCell.transform.position.x < CommonReference.instance.center.position.x ? leftGrid : rightGrid;
        return grid.GetWorldPosFromCell(x);
    }
    public void UpdateCellOccupancy()
    {
        for (int i = 0; i < leftGrid.width; i++)
        {
            for (int j = 0; j < leftGrid.height; j++)
            {
                SetCellOccupancy(0, i, j, false);
                SetCellOccupancy(1, i, j, false);

                if(emptyCellsL[i, j])
                    emptyCellsL[i, j].SetActive(true);
                if (emptyCellsR[i, j])
                    emptyCellsR[i, j]?.SetActive(true);
            }
        }

        for (int i = 0; i < cellContainer.childCount; i++)
        {
            Vector3 pos = cellContainer.GetChild(i).position;
            Vector2Int gridcell = GetGridFromCell(pos);
            int mapid = GetMapIDFromCell(pos);
            SKGridLayer grid = pos.x < CommonReference.instance.center.position.x ? leftGrid : rightGrid;
            SetCellOccupancy(grid == leftGrid ? 0 : 1, gridcell.x, gridcell.y, true);

            if (mapid == 0)
                emptyCellsL[gridcell.x, gridcell.y].SetActive(false);
            else
                emptyCellsR[gridcell.x, gridcell.y].SetActive(false);
        }
    }
    public void SetCellOccupancy(int mapid, int x, int y, bool occupied)
    {
        SKGridLayer grid = mapid == 0 ? leftGrid : rightGrid;
        grid.SetCellOccupancy(x, y, occupied);
    }
    public bool GetCellOccupancy(int mapid, int x, int y)
    {
        SKGridLayer grid = mapid == 0 ? leftGrid : rightGrid;
        return grid.GetCellOccupancy(x, y);
    }


    public void PlayPuffEffect(Vector3 pos, int sortorder = 10)
    {
        GameObject efx = CommonUtils.SpawnObject(CommonReference.instance.puffEffect);
        efx.transform.SetParent(CommonReference.instance.effectContainer);
        efx.transform.position = pos;
        CommonUtils.InvokeAction(3f, () => { CommonUtils.ReleaseObject(efx); });
    }
    public void PlayGoalPuffEffect(Vector3 pos, int sortorder = 10)
    {
        GameObject efx = CommonUtils.SpawnObject(CommonReference.instance.goalPuffEffect);
        efx.transform.SetParent(CommonReference.instance.effectContainer);
        efx.transform.position = pos;
        CommonUtils.InvokeAction(3f, () => { CommonUtils.ReleaseObject(efx); });
    }
}
