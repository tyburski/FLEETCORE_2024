namespace FLEETCORE.Models
{
    public class TimeSheet
    {
        public int Id { get; set; }
        public DateTime Date { get; private set; }
        public User User { get; private set; }
        public bool IsPresent { get; private set; }
        public string? Excuse { get; private set; }

        public void Create(User user, bool isPresent)
        {
            Date = DateTime.Now;
            User = user;
            IsPresent= isPresent;
            if (isPresent == false) Excuse = "Nieusprawiedliwiona nieobecność";
        }
        public void Update(bool isPresent)
        {
            if (IsPresent == true && isPresent == false)
            {
                IsPresent = false;
                Excuse = "Nieusprawiedliwiona nieobecność";
            }
            if (IsPresent!= false && isPresent == true) 
            {
                IsPresent = true;
                Excuse = null;
            }

        }
        public void UpdateExcuse(string excuse)
        {
            if (IsPresent == false) Excuse = excuse;
        }
    }
}
