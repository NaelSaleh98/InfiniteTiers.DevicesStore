using System;
using System.ComponentModel.DataAnnotations;


namespace InfiniteTiers.DevicesStore.Data.Models
{
    /// <summary>
    /// Each device transaction are stored the date, the owner, the requester and the device it self.
    /// </summary>
    public class UserDevice
    {
        #region Basic
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }
        #endregion

        #region Relationships
        public ApplicationUser FromUser { get; set; }
        public ApplicationUser ToUser { get; set; }
        public Device Device { get; set; } 
        #endregion
    }
}
