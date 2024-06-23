using FLEETCORE.Models.Body;

namespace FLEETCORE.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; private set; }
        public string License { get; private set; }
        public int? TagId { get; set; }
        public Tag? Tag { get; private set; }

        public void Create(CreateVehicleBody body)
        {
            Brand = body.Brand;
            License = body.License.ToUpper().Trim();
            Tag = null;
        }
        public void SetTag(Tag tag)
        {
            Tag = tag;
        }
        public void RemoveTag()
        {
            Tag = null;
        }
    }
}
