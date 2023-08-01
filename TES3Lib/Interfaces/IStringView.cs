using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TES3Lib.Interfaces
{
    //public interface IStringView
    //{
    //    public string Text { get; set; }
    //}

    //public interface IIntegerView
    //{
    //    public int Value { get; set; }
    //}
    
    //public interface IFloatView
    //{
    //    public float Value { get; set; }
    //}
    
    public interface IDataView
    {
        public Dictionary<string, object> GetData();
    }
}
