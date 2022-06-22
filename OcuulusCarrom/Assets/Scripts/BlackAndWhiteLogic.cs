using System.Collections.Generic;

public  class BlackAndWhiteLogic
{
    // Start is called before the first frame update
    public  static int ExecuteLogic(int id,List<string> coins)
    {
        bool isWhite = false, isBlack = false, isRed = false;
        int returnid = id;
        foreach(string c in coins)
        {
            if(c =="White")
            {
                isWhite = true;
            }
            if (c == "Black")
            {
                isBlack = true;
            }
            if (c == "Red")
            {
                isRed = true;
            }
        }
        if(id == 1)
        {
            if(isWhite || isRed)
            {
                returnid = 1;
            }
            else
            {
                returnid = 2;
            }
        }
        else
        {
            if (isBlack || isRed)
            {
                returnid = 2;
            }
            else
            {
                returnid = 1;
            }
        }
        return returnid;
    }
}
