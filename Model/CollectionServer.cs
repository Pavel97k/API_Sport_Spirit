namespace API_Sport_Spirit.Model;

public partial class CollectionServer
{
    public int IdCollectionServer { get; set; }

    public string CollectionServerName { get; set; } = null!;

    public string CollectionServerType { get; set; } = null!;

    public string CollectionServerMultiplicity { get; set; } = null!;

    public bool AvailabilityBasicEquipment { get; set; }

    public bool IsDeleted { get; set; }

    public int GenderId { get; set; }
}
