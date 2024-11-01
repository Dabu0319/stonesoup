using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : Room {
    
    public List<GameObject> enemyPrefabs;
    public List<GameObject> itemPrefabs;
    
    private HashSet<Vector2> itemPlacedPositions = new HashSet<Vector2>();
    private HashSet<Vector2> enemyPlacedPositions = new HashSet<Vector2>();
    	public class SearchVertex {
		public Vector2 gridPos;
		public SearchVertex parent;
		public bool isDeadEnd = false;
	}


	protected static List<SearchVertex> _agenda = new List<SearchVertex>(80);
	protected static List<SearchVertex> _closed = new List<SearchVertex>(80);

	public int extraWallsToRemove = 0;

	protected void SpawnRandomEnemy(Vector2 position) {
		if (enemyPrefabs.Count > 0 && !enemyPlacedPositions.Contains(position)) {
			int index = Random.Range(0, enemyPrefabs.Count);
			GameObject enemyPrefab = enemyPrefabs[index];
			Tile.spawnTile(enemyPrefab, transform, (int)position.x, (int)position.y);
			enemyPlacedPositions.Add(position);
		}
	}
	protected void SpawnRandomItem(Vector2 position) {
		// 检查该位置是否已经放置过物品
		if (itemPrefabs.Count > 0 && !itemPlacedPositions.Contains(position)) {
			int index = Random.Range(0, itemPrefabs.Count);
			GameObject itemPrefab = itemPrefabs[index];
			Tile.spawnTile(itemPrefab, transform, (int)position.x, (int)position.y);
			// 记录已放置物品的位置
			itemPlacedPositions.Add(position);
		}
	}

	protected bool listContainsVertex(List<SearchVertex> list, Vector2 gridPos) {
		foreach (SearchVertex vertex in list) {
			if (vertex.gridPos == gridPos) {
				return true;
			}
		}
		return false;
	}

	protected static List<Vector2> _neighbors = new List<Vector2>(4);

    public override void fillRoom(LevelGenerator ourGenerator, ExitConstraint requiredExits) {
		
		bool[,] wallMap = new bool[LevelGenerator.ROOM_WIDTH, LevelGenerator.ROOM_HEIGHT];

		// Start completely filled with walls. 
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				wallMap[x, y] = true;
			}
		}

        bool foundStartPos = false;
        Vector2 startPos = new Vector2(Random.Range(0, LevelGenerator.ROOM_WIDTH), Random.Range(0, LevelGenerator.ROOM_HEIGHT));

        foreach (Vector2Int exitLocation in requiredExits.requiredExitLocations()) {
			wallMap[exitLocation.x, exitLocation.y] = false;
            if (!foundStartPos) {
                startPos = exitLocation;
                foundStartPos = true;
            }
		}

		_agenda.Clear();
		_closed.Clear();
        
		SearchVertex startVertex = new SearchVertex();
		startVertex.gridPos = startPos;
		startVertex.parent = null;

		_agenda.Add(startVertex);

		while (_agenda.Count > 0) {
			SearchVertex currentVertex = _agenda[_agenda.Count-1];
			_agenda.RemoveAt(_agenda.Count-1);
            if (listContainsVertex(_closed, currentVertex.gridPos)) {
                continue;
            }


			_closed.Add(currentVertex);

			_neighbors.Clear();

			Vector2 neighborPos = currentVertex.gridPos + Vector2.up*2;
			if (inGrid(neighborPos)
				&& !listContainsVertex(_closed, neighborPos)) {
				_neighbors.Add(neighborPos);
			}
			neighborPos = currentVertex.gridPos + Vector2.right*2;
			if (inGrid(neighborPos)
				&& !listContainsVertex(_closed, neighborPos)) {
				_neighbors.Add(neighborPos);
			}
			neighborPos = currentVertex.gridPos - Vector2.up*2;
			if (inGrid(neighborPos)
				&& !listContainsVertex(_closed, neighborPos)) {
				_neighbors.Add(neighborPos);
			}
			neighborPos = currentVertex.gridPos - Vector2.right*2;
			if (inGrid(neighborPos)
				&& !listContainsVertex(_closed, neighborPos)) {
				_neighbors.Add(neighborPos);
			}


			if (_neighbors.Count > 0) {
				GlobalFuncs.shuffle(_neighbors);
			}
			else {
				currentVertex.isDeadEnd = true;
			}
			foreach (Vector2 neighbor in _neighbors) {
				SearchVertex neighborVertex = new SearchVertex();
				neighborVertex.gridPos = neighbor;
				neighborVertex.parent = currentVertex;
				_agenda.Add(neighborVertex);
			}
		}

		// Now go through the closed set and carve out space for all of the neighbors. 
		foreach (SearchVertex vertex in _closed) {
			if (vertex.parent == null) {
				wallMap[(int)vertex.gridPos.x, (int)vertex.gridPos.y] = false;
				continue;
			}

			int currentX = (int)vertex.gridPos.x;
			int currentY = (int)vertex.gridPos.y;
			wallMap[currentX, currentY] = false;


			Vector2 endPos = vertex.parent.gridPos;
			int targetX = (int)endPos.x;
			int targetY = (int)endPos.y;

			while (currentX != targetX || currentY != targetY) {
				if (currentX < targetX) {
					currentX++;
				}
				else if (currentX > targetX) {
					currentX--;
				}

				if (currentY < targetY) {
					currentY++;
				}
				else if (currentY > targetY) {
					currentY--;
				}

				wallMap[currentX, currentY] = false;
			}
			
			if (vertex.isDeadEnd) {
				if (Random.value < 0.3f)
				{
					SpawnRandomItem(vertex.gridPos);
				}
				
			}
		}
		
		foreach (SearchVertex vertex in _closed) {
			// 其他逻辑...
        
			// 检查是否需要在转弯处放置敌人
			if (vertex.parent != null && vertex.parent.parent != null) {
				Vector2 directionToParent = (vertex.gridPos - vertex.parent.gridPos).normalized;
				Vector2 directionToGrandparent = (vertex.parent.gridPos - vertex.parent.parent.gridPos).normalized;
				if (directionToParent != directionToGrandparent) {
					// 方向发生变化，有一定几率放置敌人
					if (Random.value < 0.1f) { // 假设有50%的几率放置敌人
						SpawnRandomEnemy(vertex.gridPos);
					}
				}
			}
        
			
		}

		// Now we remove some extra walls
		List<Vector2> wallLocations = new List<Vector2>();
		for (int i = 0; i < extraWallsToRemove; i++) {
			wallLocations.Clear();
			for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
				for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
					if (wallMap[x, y]) {
						wallLocations.Add(new Vector2(x, y));
					}
				}
			}

			if (wallLocations.Count > 1) {
				Vector2 wallToRemove = GlobalFuncs.randElem(wallLocations);
				wallMap[(int)wallToRemove.x, (int)wallToRemove.y] = false;
			}
		}

		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				if (wallMap[x, y]) {
					Tile.spawnTile(ourGenerator.normalWallPrefab, transform, x, y);
				}
			}
		}

	}

	protected bool inGrid(Vector2 gridPos) {
		return gridPos.x >= 0 && gridPos.x < LevelGenerator.ROOM_WIDTH && gridPos.y >= 0 && gridPos.y < LevelGenerator.ROOM_HEIGHT;
	}

	protected Vector2 getCoordinateFromExit(Dir exitDir) {
		if (exitDir == Dir.Up) {
			return new Vector2(LevelGenerator.ROOM_WIDTH/2, LevelGenerator.ROOM_HEIGHT-1);
		}
		else if (exitDir == Dir.Right) {
			return new Vector2(LevelGenerator.ROOM_WIDTH-1, LevelGenerator.ROOM_HEIGHT/2);
		}
		else if (exitDir == Dir.Down) {
			return new Vector2(LevelGenerator.ROOM_WIDTH/2, 0);
		}
		else if (exitDir == Dir.Left) {
			return new Vector2(0, LevelGenerator.ROOM_HEIGHT/2);
		}
		return Vector2.zero;
	}
}