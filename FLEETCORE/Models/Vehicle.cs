namespace FLEETCORE.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public string Plate { get; private set; }
        public string VinNumber { get; private set; }
        public int ProdYear { get; private set; }
        public int Mileage { get; private set; }
        public string Photo { get; private set; }
        public List<Refueling> Refuelings { get; set; }

        public void Create(string brand, string model, string plate, string vinNumber, int prodYear, int mileage, string photo)
        {
            if(brand.Length > 0 &&
               model.Length > 0 &&
               plate.Length > 0 &&
               plate.Length > 0 &&
               vinNumber.Length > 0 &&
               prodYear > 0 &&
               mileage > 0 &&
               photo.Length > 0)
            {
                Brand = brand;
                Model = model;
                Plate = plate;
                VinNumber = vinNumber;
                ProdYear = prodYear;
                Mileage = mileage;
                Photo = photo;
                Refuelings = new List<Refueling>();
            }
        }
        public void Update(string brand, string model, string plate, string vinNumber, int prodYear, int mileage, string photo)
        {
            if (!Brand.Equals(brand) && brand.Length > 0) Brand = brand;
            if (!Model.Equals(model) && model.Length > 0) Model = model;
            if (!Plate.Equals(plate) && plate.Length > 0) Plate = plate;
            if (!VinNumber.Equals(vinNumber) && vinNumber.Length > 0) VinNumber = vinNumber;
            if (!ProdYear.Equals(prodYear) && prodYear > 0) ProdYear = prodYear;
            if (!Mileage.Equals(mileage) && mileage > 0) Mileage = mileage;
            if (!Photo.Equals(photo) && Photo.Length > 0) Photo = photo;
        }
        public void UpdateMileage(int mileage)
        {
            if (mileage > Mileage) Mileage = mileage;
        }
    }
}
