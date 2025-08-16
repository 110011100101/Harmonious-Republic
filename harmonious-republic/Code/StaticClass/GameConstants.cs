namespace HarmoniousRepublic
{
    public static class Constants
    {
        // MapController 常量
        public const float ScaleFactor = 0.85f; // 层缩放系数, 越小缩放越极端
        public const float TileSize = 64f;
        public const float CameraWidth = 2560f;
        public const float BorderWidthFactor = 2f;
        public const float CameraHeight = 1600f;
        public const float cameraSwitchLevelLerp = 0.05f; // 层切换时的插值, 越小切换速度越慢
        public const int LayerCount = 6;
        public const int AirMaterialId = (int)EnumMaterial.Air;
        public const int InvalidSourceId = -1;
        public const int RangePadding = 3;
        public const int MinBorderWidth = 2;
        public const int BaseLayerIndex = LayerCount - 2;
        public const int SpecialLayerThreshold = 3;
    }
}