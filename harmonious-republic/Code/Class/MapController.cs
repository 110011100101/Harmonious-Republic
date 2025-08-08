using Godot;
using System.Threading.Tasks;

public partial class MapController : Node2D
{
	private const float ScaleFactor = 0.85f;
	private const float TileSize = 64f;
	private const float CameraWidth = 2560f;
	private const float CameraHeight = 1600f;
	private const int LayerCount = 5;
	private const int AirMaterialId = (int)EnumMaterial.Air;
	private const int InvalidSourceId = -1;
	private const int RangePadding = 3;
	private const int MinBorderWidth = 2;
	private const float BorderWidthFactor = 2f;
	private const int LevelOffset = 4;
	private const int SpecialLayerThreshold = 2; // LayerCount - SpecialLayerThreshold为特殊层

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	// 更新地图层的缩放、位置和可见性
	public void UpdateMap(float baseLevel)
	{
		// 获取相机和地图数据引用
		Camera2D camera = GetNode<Camera2D>("../PlayCamera");
		Data data = GetNode<Data>("/root/Data");
		
		// 空值检查确保必要组件存在
		if (camera == null || data == null || data.gameMap == null)
		{
			return;
		}
		
		Vector2 cameraPos = camera.GlobalPosition;
		Vector2 cameraZoom = camera.Zoom;
		
		// 计算基础层级值
		float floorBaseLevel = Mathf.Floor(baseLevel);
		
		// 遍历所有地图层进行更新
		for (int index = 0; index < LayerCount; index++)
		{
			// 计算当前层的级别和缩放值
			float level = index + baseLevel - LevelOffset;
			float scale = Mathf.Pow(ScaleFactor, floorBaseLevel - level);

			// 获取当前层和材质
			TileMapLayer tileMapLayer = GetChild<TileMapLayer>(index);
			if (tileMapLayer == null) continue;

			ShaderMaterial material = tileMapLayer.Material as ShaderMaterial;
			if (material == null) continue;

			// 应用缩放和位置变换
			tileMapLayer.Scale = new Vector2(scale, scale);
			tileMapLayer.Position = (1f - scale) * cameraPos;

			// 计算视口内的渲染范围
			int xBaseIndex = (int)Mathf.Floor(cameraPos.X / TileSize);
			int yBaseIndex = (int)Mathf.Floor(cameraPos.Y / TileSize);
			int xRange = (int)(CameraWidth / (TileSize * cameraZoom.X * scale)) + RangePadding;
			int yRange = (int)(CameraHeight / (TileSize * cameraZoom.Y * scale)) + RangePadding;
			int xStart = xBaseIndex - xRange / 2;
			int yStart = yBaseIndex - yRange / 2;
			int xEnd = xBaseIndex + xRange / 2;
			int yEnd = yBaseIndex + yRange / 2;

			// 计算当前层的整数级别索引
			int levelIndex = (int)Mathf.Floor(level);

			// 计算边缘宽度
			int borderWidth = Mathf.Max(MinBorderWidth, Mathf.CeilToInt(BorderWidthFactor / ((cameraZoom.X + cameraZoom.Y) / 2f)));
			
			// 合并视口内部和边缘区域的处理
			int minX = xStart - borderWidth;
			int maxX = xEnd + borderWidth;
			int minY = yStart - borderWidth;
			int maxY = yEnd + borderWidth;
			
			// 提取是否为顶层或次顶层的判断
			bool isSpecialLayer = index >= LayerCount - SpecialLayerThreshold; // 简化判断，包括顶层和次顶层
			
			// 遍历所有图块位置进行渲染处理
			for (int x = minX; x < maxX; x++)
			{
				for (int y = minY; y < maxY; y++)
				{
					// 判断是否在视口内部区域
					bool isInView = x >= xStart && x < xEnd && y >= yStart && y < yEnd;
					
					if (IsInRange(data.gameMap, new Vector3I(x, y, levelIndex)))
					{
						if (isInView)
						{
							// 统一处理图块渲染逻辑
							HandleTileRendering(data, tileMapLayer, x, y, levelIndex, isSpecialLayer);
						}
						else
						{
							// 处理边缘区域：清除图块以优化性能
							if (tileMapLayer.GetCellSourceId(new Vector2I(x, y)) != InvalidSourceId)
							{
								tileMapLayer.EraseCell(new Vector2I(x, y));
							}
						}
					}
				}
			}

			// 设置着色器参数，使用与层级计算一致的公式
			material.SetShaderParameter("levelDiff", floorBaseLevel - level);
		}
	}

	// 检查指定坐标是否在地图范围内
	public bool IsInRange<T>(T[,,] map, Vector3I index)
	{
		if (map == null) return false;
		
		return index.X >= 0 && index.X < map.GetLength(0) && 
			   index.Y >= 0 && index.Y < map.GetLength(1) && 
			   index.Z >= 0 && index.Z < map.GetLength(2);
	}

	// 检查指定位置的图块是否被上一层的图块遮挡
	public bool IsCovered(Block[,,] map, Vector3I index)
	{
		if (map == null) return false;
		
		// 顶层图块不会被遮挡
		if (index.Z >= map.GetLength(2) - 1)
			return false;

		// 如果上层图块不为空气，则当前图块被遮挡
		return map[index.X, index.Y, index.Z + 1].material != EnumMaterial.Air;
	}
	
	// 新增方法：统一处理图块渲染逻辑
	private void HandleTileRendering(Data data, TileMapLayer tileMapLayer, int x, int y, int levelIndex, bool isSpecialLayer)
	{
		// 特殊层总是渲染，其他层需要检查是否被遮挡
		bool shouldRender = isSpecialLayer || !IsCovered(data.gameMap, new Vector3I(x, y, levelIndex));

		if (shouldRender)
		{
			// 只渲染非空气材质的图块
			if (data.gameMap[x, y, levelIndex].material != EnumMaterial.Air)
			{
				// 检查图块是否需要更新
				int targetMaterial = (int)data.gameMap[x, y, levelIndex].material;
				int currentSourceId = tileMapLayer.GetCellSourceId(new Vector2I(x, y));

				if (targetMaterial != currentSourceId)
				{
					// 设置或清除图块
					if (targetMaterial != AirMaterialId)
					{
						tileMapLayer.SetCell(new Vector2I(x, y), targetMaterial, new Vector2I(4, 1));
					}
					else if (currentSourceId != InvalidSourceId)
					{
						tileMapLayer.EraseCell(new Vector2I(x, y));
					}
				}
			}
			else if (tileMapLayer.GetCellSourceId(new Vector2I(x, y)) != InvalidSourceId)
			{
				// 清除空气图块的显示
				tileMapLayer.EraseCell(new Vector2I(x, y));
			}
		}
		else if (tileMapLayer.GetCellSourceId(new Vector2I(x, y)) != InvalidSourceId)
		{
			// 遮挡剔除：如果当前块被上层块遮挡，则清除当前块的显示
			tileMapLayer.EraseCell(new Vector2I(x, y));
		}
	}
}