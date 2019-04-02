using System;
using System.Collections.Generic;
using System.Text;
using iSpendInterfaces;

namespace iSpendDAL.Dto
{
    internal class CategoryDto:ICategory
    {
        public CategoryDto(int id,string name,int iconId)
        {
            Id = id;
            Name = name;
            IconId = iconId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int IconId { get; set; }
    }
}
