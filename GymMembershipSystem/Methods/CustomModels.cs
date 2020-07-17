using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomModels
{
    public partial class ReducedAccount
    {
        public int id { get; set; }
        public System.DateTime PaymentDate { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int MemberId { get; set; }
    }

    public partial class PartialMember
    {
        public long id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public long CardId { get; set; }
        public string TypeId { get; set; }
        public int NumOfEntrances { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> LastEntrance { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int DaysLeft { get; set; }

        public int NumOfDays { get; set; }
    }

    public partial class FullItem
    {
        public int id { get; set; }
        public string ItemName { get; set; }
        public long ItemId { get; set; }
        public decimal ItemPrice { get; set; }
        public int ItemCount { get; set; }
        public string ItemDescription { get; set; }
        public string ItemUrl { get; set; }
    }

    public partial class ShopMember
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string LastEntrance { get; set; }
    }

    public partial class FullExercise
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public byte[] Image { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public int Enrolled { get; set; }
        public int Price { get; set; }
        public string ExerciseUrl { get; set; }
    }
}
