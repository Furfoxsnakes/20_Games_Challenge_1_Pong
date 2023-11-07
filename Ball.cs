using Godot;
using System;

public partial class Ball : RigidBody2D
{
	[Export] public Vector2 StartingBallPosition = new Vector2(160, 130);

	public Vector2 Movement;

	public override void _Ready()
	{
		GD.Seed((ulong) DateTime.Now.Ticks);
		GD.Randomize();
	}

	public override void _PhysicsProcess(double delta)
	{
		HandleBounce(MoveAndCollide(Movement));
	}

	public void Launch(int startingX)
	{
		Movement = new Vector2(startingX, GD.RandRange(-1, 1));
		Movement = Movement.Normalized();
	}

	public void Reset()
	{
		Movement = Vector2.Zero;
		Position = StartingBallPosition;
	}

	private void HandleBounce(KinematicCollision2D collision)
	{
		var node = collision?.GetCollider() as Node2D;
		
		if (node == null) return;
		
		if (node.IsInGroup("Paddles"))
		{
			Movement.X *= -1;
			Movement.Y += (float)GD.RandRange(-0.15, 0.15);
		} else if (node.IsInGroup("Walls"))
		{
			Movement.Y *= -1;
			Movement.X += (float)GD.RandRange(-0.15, 0.15);
		}
		// Movement = Movement.Normalized();
		Movement *= 1.05f;
	}
}
