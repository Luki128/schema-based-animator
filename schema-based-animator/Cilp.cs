using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;



namespace schema_based_animator
{
    public  class Clip
    {
        public Image source;


        public void LoadImage(string str)
        {
            source = Image.FromFile(str);
        }
    }
}
