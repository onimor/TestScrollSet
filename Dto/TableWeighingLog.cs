using System;

namespace TestScrollSet.Dto
{
    public class TableWeighingLog
    {
        public int Id { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateTara { get; set; }
        public DateTime DateBrutto { get; set; }
        public DateTime DateRegistration { get; set; }
        public float Tara { get; set; }
        public float Brutto { get; set; }
        public float Netto { get; set; }

        public string TypeScales { get; set; }

        public string Car { get; set; }
        public int CarId { get; set; }

        public string RFID { get; set; }

        public string Driver { get; set; }
        public int DriverId { get; set; }

        public string Trailer { get; set; }
        public int TrailerId { get; set; }

        public string Goods { get; set; }
        public int GoodsId { get; set; }

        public string Sender { get; set; }
        public int SenderId { get; set; }

        public string Payer { get; set; }
        public int PayerId { get; set; }

        public string Recipient { get; set; }
        public int RecipientId { get; set; }
        /// <summary>Перевозчик</summary>
        public string Carrier { get; set; }
        public int CarrierId { get; set; }

        public string PostName { get; set; }
        public int PostId { get; set; }

        public bool IsRemove { get; set; }

        public bool IsFormed { get; set; }

        public string UserName { get; set; }
        public int UserId { get; set; }

        public bool IsBloc { get; set; }
    }
}
