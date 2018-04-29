using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

	// grid stuff
	[SerializeField] 
	private int rows;
	[SerializeField] 
	private int cols;
	[SerializeField] 
	private Vector2 gridSize;
	[SerializeField] 
	private Vector2 gridOffset;
	
	// cell stuff
	[SerializeField]
	private Sprite cellSprite;
	private Vector2 cellSize;
	private Vector2 cellScale;
	
	// Use this for initialization
	void Start()
	{
		InitCells();
	}

	void InitCells()
	{
		GameObject cellObject = new GameObject();
		cellObject.AddComponent<SpriteRenderer>().sprite = cellSprite;
		cellSize = cellSprite.bounds.size;
		Vector2 newCellSize = new Vector2(gridSize.x / (float) cols, gridSize.y / (float) rows);
		cellScale.x = newCellSize.x / cellSize.x;
		cellScale.y = newCellSize.y / cellSize.y;
		cellSize = newCellSize;
		cellObject.transform.localScale = new Vector2(cellScale.x, cellScale.y);

		for (int row = 0; row < rows; row++)
		{
			for (int col = 0; col < cols; col++)
			{
				Vector2 pos = new Vector2(col * cellSize.x + gridOffset.x + transform.position.x,
					row * cellSize.y + gridOffset.y + transform.position.y);
				GameObject c0 = Instantiate(cellObject, pos, Quaternion.identity);
				c0.transform.parent = transform;
			}
		}
	}

	void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, gridSize);
	}
}
