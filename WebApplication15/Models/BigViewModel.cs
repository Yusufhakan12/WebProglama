using WebApplication15.Data;

namespace WebApplication15.Models
{
    public class BigViewModel
    {
        public BigViewModel()
        {
        }

        public BigViewModel(IEnumerable<FlyName> flyName, Voyage voyage)
        {
            FlyName = flyName;
            Voyage = voyage;
        }

        public IEnumerable<FlyName> FlyName { get; set; }
        public Voyage Voyage { get; set; }
    }
}
