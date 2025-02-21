namespace MormorDagnysDel2.Entities;

public class AddressType
{
    public int Id { get; set; }
    public string Value { get; set; }
    public IList<Address> Addresses { get; set; }
}
