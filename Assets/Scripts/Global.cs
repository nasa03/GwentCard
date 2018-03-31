﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global {
    public enum Group { northern, nilfgaardian, monster, scoiatael }
    public enum List { leader, special, monster, neutral }
    public enum Line { melee, ranged, siege, empty, agile }
    public enum Effect { empty, clear_sky, dummy, fog, frost, improve_neighbours, muster, nurse, rain, same_type_morale, scorch, spy, warhorn }
    public static readonly string path = string.Format("{0}/PlayerCards.xml", Application.persistentDataPath);
    public static readonly string enemyPath = string.Format("{0}/EnemyCards.xml", Application.persistentDataPath);

    public static void SetParent(this Transform grid, int index, Transform targetGrid)
    {
        grid.GetChild(index).SetParent(targetGrid);
        UIGrid grid1 = grid.GetComponent<UIGrid>();
        UIGrid grid2 = targetGrid.GetComponent<UIGrid>();
        if (grid1 != null) grid1.Reposition();
        if (grid2 != null) grid2.Reposition();
    }

    public static int GetItemsInt(this UIPopupList popupList)
    {
        int index = 0;
        for (int i = 0; i < popupList.items.Count; i++)
        {
            if (popupList.value == popupList.items[i]) index = i;
        }
        return index;
    }
}