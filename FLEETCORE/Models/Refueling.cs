namespace FLEETCORE.Models
{
    public class Refueling
    {
        public int Id { get; set; }
        public float Quantity { get; private set; }
        public int Mileage { get; private set; }
        public string ReceiptPhoto { get; private set; }
        public DateTime Date { get; set; }      
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public bool ToFull { get; private set; }
        public Vehicle Vehicle { get; set; }
        public User User { get; set; }

        public void Create (float quantity, int mileage, string receiptPhoto, double latitude, double longitude, bool toFull, Vehicle vehicle, User user)
        {
            if(quantity > 0 &&
                mileage > 0 &&
                receiptPhoto.Length > 0 &&
                latitude > 0 &&
                longitude > 0 &&
                vehicle != null &&
                user != null)
            {
                Quantity = quantity;
                Mileage = mileage;
                ReceiptPhoto = receiptPhoto;
                Date = DateTime.Now;
                Latitude = latitude;
                Longitude = longitude;
                ToFull = toFull;
                Vehicle = vehicle;
                Vehicle.UpdateMileage(mileage);
                User = user;
            }

        }
        public void Update (float quantity, int mileage, string receiptPhoto, DateTime date, bool toFull, Vehicle vehicle, User user)
        {
            if(!Quantity.Equals(quantity) && quantity > 0) Quantity = quantity;
            if(!Mileage.Equals(mileage) && mileage > 0) Mileage = mileage;
            if(!ReceiptPhoto.Equals(receiptPhoto) && receiptPhoto.Length > 0) ReceiptPhoto = receiptPhoto;
            if(!Date.Equals(date)) Date = date;
            if(!ToFull.Equals(toFull)) ToFull = toFull;
            if(!Vehicle.Equals(vehicle)) Vehicle = vehicle;
            if (!User.Equals(user)) User = user;
        }
    }
}
