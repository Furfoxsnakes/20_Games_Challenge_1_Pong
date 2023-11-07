using Godot;
using System;

public partial class Paddle : StaticBody2D
{
	[Export] private bool _isPlayerControlled;
	[Export] private Game _game;

	private float _x;

	public override void _Ready()
	{
		// Workaround fix for paddle getting pushed by ball
		_x = Position.X;
	}

	public override void _PhysicsProcess(double delta)
	{
		// For some reason the paddle gets pushed by the ball when it's hit by it,
		// Even though the paddle is a StaticBody
		Position = new Vector2(_x, Position.Y);
	}

	private void HandlePlayerMovement(double delta)
	{
		// var pos = GlobalPosition;
		// GlobalPosition = new Vector2(pos.X, GetGlobalMousePosition().Y);
		MoveAndCollide(new Vector2(0, GetLocalMousePosition().Y - Position.Y * (float) delta));
	}

	private void HandleAIMovement(double delta)
	{
		// var vectorToBall = new Vector2(0, Position.Y - _ball.Position.Y);
		// GD.Print(vectorToBall.Y);
		// MoveAndCollide(new Vector2(Position.X, vectorToBall.Y));
		// GlobalPosition = new Vector2( GlobalPosition.X, _game.Ball.GlobalPosition.Y);
		MoveAndCollide(new Vector2(0, _game.Ball.Position.Y - GlobalPosition.Y) * (float)delta * 10);
	}

	public void HandleMovement(double delta)
	{
		if (_isPlayerControlled)
			HandlePlayerMovement(delta);
		else
			HandleAIMovement(delta);
	}
}
