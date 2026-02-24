
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Entities
{
        public class Chat:BaseEntity
        {
            public Guid Id {get; set;} 
            public string User1Id { get; set; }
            public string User2Id { get; set; }
        }


    }



