using FLEETCORE.Models;
using FLEETCORE.Models.Body;
using Microsoft.EntityFrameworkCore;

namespace FLEETCORE.Services
{
    public interface IVehicleService
    {
        string Create(CreateVehicleBody body);
        string Delete(int id);
        List<Vehicle> GetAll();
        Vehicle Get(int id);
        string SetTag(int tagId, int vehicleId);
        string RemoveTag(int id);
    }
    public class VehicleService : IVehicleService
    {
        private readonly FLEETCOREDbContext context;
        public VehicleService(FLEETCOREDbContext context)
        {
            this.context = context;
        }

        public string Create(CreateVehicleBody body)
        {
            var veh = context.Vehicles.Where(v=>v.License.Equals(body.License)).FirstOrDefault();
            if (veh is null)
            {
                if(body.License.Length > 4 && body.License.Length < 9)
                {
                    var vehicle = new Vehicle();
                    vehicle.Create(body);
                    context.Vehicles.Add(vehicle);
                    context.SaveChanges();
                    return "done";
                }
                return "license_length";
            }
            else return "vehicle_exist";
        }
        public string Delete(int id)
        {
            var veh = context.Vehicles.Where(v => v.Id.Equals(id)).Include(x=>x.Tag).FirstOrDefault();
            if (veh != null)
            {
                if (veh.Tag != null)
                {
                    var tag = context.Tags.Where(t => t.Vehicle.Equals(veh)).Include(x=>x.Vehicle).FirstOrDefault();
                    tag.RemoveVehicle();
                }
                context.Vehicles.Remove(veh);
                context.SaveChanges();
                return "done";

            }
            else return "not_exist";
        }

        public List<Vehicle> GetAll()
        {
            return context.Vehicles.Include(x=>x.Tag).ToList();
        }
        public Vehicle Get(int id)
        {
            var tag = context.Vehicles.Where(t => t.Id.Equals(id)).Include(x => x.Tag).FirstOrDefault();
            return tag;
        }
        public string SetTag(int tagId, int vehicleId)
        {
            var vehicle = context.Vehicles.Where(v => v.Id.Equals(vehicleId)).FirstOrDefault();
            var tag = context.Tags.Where(t => t.Id.Equals(tagId)).FirstOrDefault();

            if (vehicle is null)
            {
                return "vehicle_not_exist";
            }
            if(tag is null)
            {
                return "tag_not_exist";
            }
            if (vehicle != null && tag != null)
            {
                var veh = context.Vehicles.Where(t => t.Tag.Equals(tag)).Include(x => x.Tag).FirstOrDefault();
                if(veh is not null) veh.RemoveTag();

                vehicle.SetTag(tag);
                tag.SetVehicle(vehicle);
                context.SaveChanges();
                return "done";
            }
            else return "error";
        }
        public string RemoveTag(int id)
        {
            var vehicle = context.Vehicles.Where(v => v.Id.Equals(id)).Include(x=>x.Tag).FirstOrDefault();
            if (vehicle is null)
            {
                return "vehicle_not_exist";
            }
            if(vehicle.Tag is null)
            {
                return "vehicle_doesnt_have_tag";
            }
            if (vehicle != null && vehicle.Tag != null)
            {
                vehicle.Tag.RemoveVehicle();
                vehicle.RemoveTag();
                

                context.SaveChanges();
                return "done";
            }
            else return "error";
        }
    }
}
