namespace TicTacToeStudent.Server.Entities;

public class PlayerEntity
{
    public Guid ID { get; set; }
    public string? ConnectionID { get; set; }
    public string Name { get; set; }
    public int Wons { get; set; }
    public int Loses { get; set; }
    public int Draws { get; set; }
}