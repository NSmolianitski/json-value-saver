namespace Domain;

public class SavedValue
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int Code { get; set; }
    public string Value { get; set; } = string.Empty;
}