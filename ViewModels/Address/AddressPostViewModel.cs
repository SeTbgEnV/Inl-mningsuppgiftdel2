using System.Text.Json.Serialization;

namespace MormorDagnysDel2.ViewModels.Address;

public enum AddressTypeEnum
{
    Delivery = 1,
    Billing = 2,
    Distribution = 3
}
public class AddressPostViewModel
{
    public string AddressLine { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AddressTypeEnum AddressType { get; set; }
}