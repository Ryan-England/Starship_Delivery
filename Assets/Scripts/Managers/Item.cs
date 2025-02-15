using System.Collections.Generic;

public class Item
{
    public string name;
    public string ItemID;
    public string image_filepath;
    public string description;
    public int value;
    
    //possibly used for item type - eg always use something with the chop tag for chopping minigame
    public List<string> tags;
}
