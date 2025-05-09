using Godot;

// interface IBoss
// {
// 	void Attack();
// }

public interface I人类
{
	public void Walk();
	public void Eat();
}
public interface I程序员
{
	public abstract void Coding();
}

class 板蓝根() : I人类, I程序员
{
	public void Walk()
	{
		GD.Print("板蓝根走路");
	}

	public void Eat()
	{
		GD.Print("板蓝根吃饭");
	}

	public void Coding()
	{
		GD.Print("我写GDScript");
	}
}

class 谢成烨() : I人类, I程序员
{
	public void Walk()
	{
		GD.Print("谢成烨走路");
	}

	public void Eat()
	{
		GD.Print("谢成烨吃饭");
	}

	public void Coding()
	{
		GD.Print("我写C#");
	}
}


partial class Test : Node
{
	public override void _Ready()
	{
		板蓝根 板蓝根 = new();

		板蓝根.Walk(); // 输出:"板蓝根走路"
		板蓝根.Eat(); // 输出:"板蓝根吃饭"

		谢成烨 谢成烨 = new();

		谢成烨.Walk(); // 输出:"谢成烨走路"
		谢成烨.Eat(); // 输出:"谢成烨吃饭"
	}	 
}