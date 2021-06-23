using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Magazine
    {
       public string Id { get; set; }
       public string Category { get; set; }

       public string Name { get; set; }
    }

    class Subscriber
    {
        public Subscriber()
        {
            magIds = new List<string>();
        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> magIds { get; set; }
    }
}
