using Godot;
using System;

public partial class Game : Node
{
    [Export] private CanvasLayer _startGameOverlay;
    [Export] private CanvasLayer _gameOverScreen;
    
    [Export] public Ball Ball;
    [Export] public GameState State = GameState.Paused;
    [Export] private Paddle _leftPaddle;
    [Export] private Paddle _rightPaddle;
    [Export] private Goal _leftGoal;
    [Export] private Goal _rightGoal;
    [Export] private Label _leftScoreLabel;
    [Export] private Label _rightScoreLabel;

    private int _leftScore = 0;
    private int _rightScore = 0;

    private int _startingBallDirectionX = -1;

    public override void _Ready()
    {
        _leftGoal.BodyEntered += OnLeftGoalBodyEntered;
        _rightGoal.BodyEntered += OnRightGoalBodyEntered;
        Reset();
    }

    private void OnRightGoalBodyEntered(Node2D body)
    {
        if (body is Ball ball)
        {
            ScorePoint(ref _leftScore);
            _startingBallDirectionX = -1;
            // CallDeferred(nameof(EndRound));
        }
    }

    private void OnLeftGoalBodyEntered(Node2D body)
    {
        if (body is Ball ball)
        {
            ScorePoint(ref _rightScore);
            _startingBallDirectionX = 1;
            // CallDeferred(nameof(EndRound));
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (State == GameState.Playing)
        {
            _leftPaddle.HandleMovement(delta);
            _rightPaddle.HandleMovement(delta);
        }
    }
    
    public void StartRound()
    {
        _startGameOverlay.Visible = false;
        State = GameState.Playing;
		Ball.Launch(_startingBallDirectionX);
    }

    public void Reset()
    {
        _gameOverScreen.Visible = false;
        _leftScore = 0;
        _rightScore = 0;
        Ball.Reset();
        EndRound();
    }

    private void _on_start_button_pressed()
    {
        StartRound();
    }

    private void _on_restart_button_pressed()
    {
        Reset();
    }

    public void Pause()
    {
        State = GameState.Paused;
    }

    public void EndRound()
    {
        Ball.Reset();
        State = GameState.Paused;
        UpdateScore();
        _startGameOverlay.Visible = true;
    }
    
    public enum GameState
    {
        Playing,
        GameOver,
        Paused
    }

    private void UpdateScore()
    {
        _leftScoreLabel.Text = _leftScore.ToString();
        _rightScoreLabel.Text = _rightScore.ToString();
    }

    private void ScorePoint(ref int side)
    {
        side++;
        
        if (side == 5)
            GameOver();
        else
            EndRound();
    }

    private void GameOver()
    {
        UpdateScore();
        State = GameState.GameOver;
        _gameOverScreen.Visible = true;
        Ball.Reset();
    }
}
