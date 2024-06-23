using FLEETCORE.Models;
using FLEETCORE.Models.Body;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace FLEETCORE.Services
{
    public interface ITagService
    {
        string Create(string unique);
        string Delete(int id);
        List<Tag> GetAll();
        Tag Get(int id);
    }
    public class TagService : ITagService
    {
        private readonly FLEETCOREDbContext context;
        public TagService(FLEETCOREDbContext context)
        {
            this.context = context;
        }
        public string Create(string unique)
        {
            var result = "error";

            var u = context.Tags.Where(u => u.Unique == unique).FirstOrDefault();

            if (u != null)
            {
                result = "unique_exist";
                return result;
            } 
            else
            {
                var tag = new Tag();
                result = tag.Create(unique);
                if (result.Equals("done"))
                {
                    context.Tags.Add(tag);
                    context.SaveChanges();
                }
            }
            return result;

        }

        public string Delete(int id)
        {
            var result = "error";
            var t = context.Tags.Where(c => c.Id == id).Include(x=>x.Vehicle).FirstOrDefault();
            if (t is null) result = "not_exist";
            else
            {               
                if(t.Vehicle != null)
                {
                    t.Vehicle.RemoveTag();
                }
                context.Tags.Remove(t);
                context.SaveChanges();
                result = "done";
            }
            return result;
        }
        
        public List<Tag> GetAll()
        {
            return context.Tags.Include(x=>x.Vehicle).ToList();
        }

        public Tag Get(int id)
        {
            var tag = context.Tags.Where(t => t.Id.Equals(id)).Include(x=>x.Vehicle).FirstOrDefault();
            return tag;
        }
    }
}
