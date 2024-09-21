using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Services.BankAccountServices
{
    public record BankAccountDTOs(int Id, [Required] string FullName, [Required] string PhoneNumber,
       [Required] string AccountNumber, [Required, DataType(DataType.Currency)] decimal Balance);
}
