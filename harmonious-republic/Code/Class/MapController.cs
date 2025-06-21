using Godot;
using System;

public partial class MapController : Node2D
{
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	// TODO: 在这里我需要写一个供外界调用的方法
	// 这个方法的作用是, 当调用时刷新层缩放和可见度等一系列视觉效果
	public void UpdateMap(float level)
	{
		foreach (TileMapLayer layer in GetChildren())
		{
			// 计算层差
			float levelDiff = level - int.Parse(layer.Name);

			// 设置视觉效果
			if (levelDiff <= 5f && levelDiff >= 0f)
			{
				// 设置缩放
				float scaleValue = Mathf.Pow(0.85f, levelDiff);
				layer.Scale = new Vector2(scaleValue, scaleValue);

				// 设置位置(依据相机位置)
				layer.Position = (1f - scaleValue) * GetNode<Camera2D>("../PlayCamera").GlobalPosition;

				// 设置shader
				ShaderMaterial material = layer.Material as ShaderMaterial;
				material.SetShaderParameter("level_diff", levelDiff);
			}

			// 设置是否可见
			if (levelDiff <= 5f && levelDiff >= -0f)
			{
				layer.Visible = true;
			}
			else
			{
				layer.Visible = false;
			}
		}
	}
}
