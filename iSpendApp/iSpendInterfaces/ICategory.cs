using System;
using System.Collections.Generic;
using System.Text;

namespace iSpendInterfaces
{
    public interface ICategory
    {
        int Id { get; set; }
        string Name { get; set; }
        int IconId { get; set; }
    }
}
