using System;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    public GameObject gridSlotPrefab;
    public Transform gridParent;
    public Array slotArray;

    public int gridSize = 11;

    private SpellGridSlot[,] gridSlots;
    public void BuildGrid()
    {
        foreach (Transform child in gridParent) Destroy(child.gameObject);
        gridSlots = new SpellGridSlot[gridSize, gridSize];
        int center = gridSize / 2;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                GameObject slotObj = Instantiate(gridSlotPrefab, gridParent);
                SpellGridSlot slot = slotObj.GetComponent<SpellGridSlot>();

                bool isUnlocked = (x >= center - 1 && x <= center + 1 && y >= center - 1 && y <= center + 1); // will be replaced once the method to expand the grid is created

                slot.Initialize(x, y, isUnlocked);
                gridSlots[x, y] = slot;
            }
        }
    }
    void Start()
    {
        BuildGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
