﻿using SQLite;

namespace TCSA.MathGame.Maui.Models;
public class Game
{
    [PrimaryKey, AutoIncrement, Column("Id")]
    public int Id { get; set; }
    public GameOperation Type {  get; set; }
    public int Score {  get; set; }
    public DateTime DatePlayed { get; set; }

}

public enum GameOperation
{
    Addition,
    Subtraction,
    Multiplication,
    Division
}

