public class Task
{
    public string description;    // Task description
    public string target;         // Target city element (e.g., road, building)
    public int quantity;          // Quantity involved in the task
    public string detail;         // Additional details

    public Task(string description, string target, int quantity, string detail)
    {
        this.description = description;
        this.target = target;
        this.quantity = quantity;
        this.detail = detail;
    }
}
