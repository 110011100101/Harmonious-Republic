using Godot;
using HarmoniousRepublic.Code.Class;
using HarmoniousRepublic.Code.Enum;
using HarmoniousRepublic.Code.StaticClass;

namespace HarmoniousRepublic.Code.CustomNode;

public partial class MapController : CanvasGroup
{
    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }

    public void UpdateMap(float cameraLevel)
    {
        Camera2D camera = GetNode<Camera2D>("../PlayCamera"); // 获取相机节点
        Data data = GetNode<Data>("/root/Data"); // 获取数据节点

        if (camera == null || data == null || data.gameMap == null) { return; } // 安全性检测

        Vector2 cameraPos = camera.GlobalPosition; // 获取相机位置
        Vector2 cameraZoom = camera.Zoom; // 获取相机缩放

        float floorBaseLevel = Mathf.Floor(cameraLevel); // 计算整数基准层

        for (int index = 0; index < Constants.LayerCount; index++)
        {
            TileMapLayer tileMapLayer = GetChild<TileMapLayer>(index);
            if (tileMapLayer == null) continue;

            ShaderMaterial material = tileMapLayer.Material as ShaderMaterial;
            if (material == null) continue;

            float realLevel = index + floorBaseLevel - Constants.BaseLayerIndex;
            float levelDiffer = cameraLevel - realLevel;
            float scale = Mathf.Pow(Constants.ScaleFactor, levelDiffer);
            int xBaseIndex = (int)Mathf.Floor(cameraPos.X / Constants.TileSize);
            int yBaseIndex = (int)Mathf.Floor(cameraPos.Y / Constants.TileSize);
            int xRange = (int)(Constants.CameraWidth / (Constants.TileSize * cameraZoom.X * scale)) + Constants.RangePadding;
            int yRange = (int)(Constants.CameraHeight / (Constants.TileSize * cameraZoom.Y * scale)) + Constants.RangePadding;
            int xStart = xBaseIndex - xRange / 2;
            int yStart = yBaseIndex - yRange / 2;
            int xEnd = xBaseIndex + xRange / 2;
            int yEnd = yBaseIndex + yRange / 2;
            int levelIndex = (int)Mathf.Floor(realLevel);
            int borderWidth = Mathf.Max(Constants.MinBorderWidth, Mathf.CeilToInt(Constants.BorderWidthFactor / ((cameraZoom.X + cameraZoom.Y) / 2f)));
            int minX = xStart - borderWidth;
            int maxX = xEnd + borderWidth;
            int minY = yStart - borderWidth;
            int maxY = yEnd + borderWidth;
            bool isSpecialLayer = scale >= 0.85f;

            for (int x = minX; x < maxX; x++)
            for (int y = minY; y < maxY; y++)
            {
                bool isInView = x >= xStart && x < xEnd && y >= yStart && y < yEnd;

                if (IsInRange(data.gameMap, new Vector3I(x, y, levelIndex)) && isInView)
                    HandleTileRendering(data, tileMapLayer, x, y, levelIndex, isSpecialLayer);
                else if (tileMapLayer.GetCellSourceId(new Vector2I(x, y)) != Constants.InvalidSourceId)
                    tileMapLayer.EraseCell(new Vector2I(x, y));
            }

            tileMapLayer.Scale = new Vector2(scale, scale);
            tileMapLayer.Position = (1f - scale) * cameraPos;

            material.SetShaderParameter("index", index);
            material.SetShaderParameter("alpha", 1 + levelDiffer);
        }
    }

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

        if (index.Z >= map.GetLength(2) - 1)
            return false;

        return map[index.X, index.Y, index.Z + 1].material != EnumMaterial.Air;
    }

    /// <summary>
    /// </summary>
    /// <param name="data">数据节点</param>
    /// <param name="tileMapLayer"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="levelIndex"></param>
    /// <param name="isSpecialLayer"></param>
    private void HandleTileRendering(Data data, TileMapLayer tileMapLayer, int x, int y, int levelIndex, bool isSpecialLayer)
    {
        bool shouldRender = isSpecialLayer || !IsCovered(data.gameMap, new Vector3I(x, y, levelIndex));

        if (shouldRender)
        {
            if (data.gameMap[x, y, levelIndex].material != EnumMaterial.Air)
            {
                int targetMaterial = (int)data.gameMap[x, y, levelIndex].material;
                int currentSourceId = tileMapLayer.GetCellSourceId(new Vector2I(x, y));

                if (targetMaterial != currentSourceId)
                {
                    if (targetMaterial != Constants.AirMaterialId)
                    {
                        tileMapLayer.SetCell(new Vector2I(x, y), targetMaterial, new Vector2I(0, 0));
                    }
                    else if (currentSourceId != Constants.InvalidSourceId)
                    {
                        tileMapLayer.EraseCell(new Vector2I(x, y));
                    }
                }
            }
            else if (tileMapLayer.GetCellSourceId(new Vector2I(x, y)) != Constants.InvalidSourceId)
            {
                tileMapLayer.EraseCell(new Vector2I(x, y));
            }
        }
        else if (tileMapLayer.GetCellSourceId(new Vector2I(x, y)) != Constants.InvalidSourceId)
        {
            tileMapLayer.EraseCell(new Vector2I(x, y));
        }
    }
}