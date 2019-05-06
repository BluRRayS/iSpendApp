using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using iSpendInterfaces;
using Microsoft.AspNetCore.Http;

namespace iSpendWebApp.Models
{
    public class ImportTransactionsViewModel
    {
        public ImportTransactionsViewModel()
        {
            
        }

        public ImportTransactionsViewModel(int accountId)
        {
            AccountId = accountId;
        }

        [Required]
        [DataType(DataType.Upload)]
        //[FileExtensions(Extensions = "csv")]
        [Display (Name = "Upload Transactions")]
        public IFormFile Transactions { get; set; }

        public int AccountId { get; set; }
    }
}
