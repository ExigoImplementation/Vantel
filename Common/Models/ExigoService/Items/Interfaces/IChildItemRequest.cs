namespace ExigoService
{
    /// <summary>
    /// Used to request population of child items
    /// </summary>
    public interface IChildItemRequest
    {
        bool IncludeDynamicKitChildren { get; set; }
        bool IncludeStaticKitChildren { get; set; }
        bool IncludeGroupMembers { get; set; }
    }
}
