using System;
using System.Collections.Generic;

namespace TinyBank.Core.Model
{
    public class Card
    {
        public Guid CardId { get; set; }
        public string CardNumber { get; set; }
        public DateTimeOffset Expiration { get; set; }
        public bool Active { get; set; }
        public Constants.CardType CardType { get; set; }
        public List<Account> Accounts { get; set; }

        public Card()
        {
            CardId = Guid.NewGuid();
            Accounts = new List<Account>();
            Expiration = DateTimeOffset.Now.AddYears(6);
        }
    }
}
