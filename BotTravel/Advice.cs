using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotTravel
{
    class Advice
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Photo { get; set; }
       
        public Advice (string title, string text,string photo) {
            Title = title;
            Text = text;
            Photo = photo;
        }
        public Advice()
        {
        }
    }
}
