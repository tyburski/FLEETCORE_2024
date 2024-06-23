using FLEETCORE.Models.Body;

namespace FLEETCORE.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public int? VehicleId { get; set; }
        public Vehicle? Vehicle { get; private set; }
        public string Unique { get; private set; }

        public bool Taken { get; private set; }

        public string Create(string unique)
        {
            if (unique.Length > 7 && unique.Length < 21)
            {
                Unique = unique;
                Vehicle = null;
                Taken = false;
                return "done";
            }
            else return "wrong_unique";
        }
        public void SetVehicle(Vehicle vehicle)
        {
            Vehicle = vehicle;
            if (vehicle != null) Taken = true;
        }
        public void RemoveVehicle()
        {
            Vehicle = null;
            if (Vehicle == null) Taken = false;
        }
    }

}
