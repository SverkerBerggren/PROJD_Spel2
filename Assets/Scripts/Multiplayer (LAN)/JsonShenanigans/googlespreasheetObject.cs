using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
public class googlespreasheetObject
{
    public string spreadsheetId { get; set; }
    public List<ValueRange> valueRanges { get; set; }


    public class ValueRange
    {
        public string range { get; set; }
        public string majorDimension { get; set; }

        public List<List<string>> values { get; set; }
    }

    //  public string spreadsheetId { get; set; };
    //  public string spreadsheetId { get; set; };
}
